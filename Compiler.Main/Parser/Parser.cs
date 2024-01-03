using System.Security.AccessControl;
using Exception;
using Lex;

namespace Parse;


// STATEMENT GRAMMAR
// program        → declaration* EOF ;
// declaration    → varDecl | funcDecl | statement ;
// funcDecl       → "func" function ;
// function       → IDENTIFIER "(" arguments? ")" block ;
// statement      → exprStmt | printStmt | block | ifStmt | whileStmt | forStmt | returnStmt ;
// returnStmt     → "return" expression? ";"
// exprStmt       → expression ";" ;
// printStmt      → "print" expression ";" ;
// whileStmt      → "while" "(" expression ")" statement ;
// forStmt        → "for" "(" ( varDecl | exprStmt | ";" ) expression? ";" expression? ")" statement ;
// if statement   → "if" "(" expression ")" statement ( "else" statement )? ;
// block          → "{" declaration* "}" ;
// varDecl        → "var" IDENTIFIER ( "=" expression )? ";" ;



// EXPRESSION GRAMMAR
// expression     → assignment ;
// assignment     → IDENTIFIER "=" assignment | equality ;
// logic_or       → logic_and ( "or" logic_and )* ;
// logic_and      → equality ( "and" equality )* ;
// equality       → comparison ( ( "!=" | "==" ) comparison )* ;
// comparison     → term ( ( ">" | ">=" | "<" | "<=" ) term )* ;
// term           → factor ( ( "-" | "+" ) factor )* ;
// factor         → primary ( ( "/" | "*" ) primary )* ;
// call           → primary ( "(" arguments? ")" )* ;
// primary        → NUMBER | "(" expression ")" | IDENTIFIER | "true" | "false" ;

// need to add: comparison(including equality), false/null tokens, strings
// todo: put the parsing methods in order of precedence in the grammar rules


public class Parser(List<Token> tokens)
{
    private int current = 0;

    public List<Stmt> Parse()
    {
        List<Stmt> statements = new List<Stmt>();
        while (!IsAtEnd())
        {
            statements.Add(Declaration());
        }
        return statements;
    }

    // --- UTILITIES ---

    /// <summary>
    /// Return the current token without consuming it 
    /// </summary>
    private Token Peek()
    {
        return tokens[current];
    }

    private bool IsAtEnd()
    {
        return Peek().Type == TokenType.EOF;
    }

    /// <summary>
    /// Consume the current token and return it
    /// </summary>
    private Token Advance()
    {
        if (!IsAtEnd())
        {
            current++;
        }
        return Previous();
    }

    /// <summary>
    /// Throws error if the current token is not of the expected type
    /// </summary>
    private Token Consume(TokenType type, string message)
    {
        if (Check(type))
        {
            return Advance();
        }
        throw new SyntaxException(Peek(), message);
    }

    private Token Previous()
    {
        return tokens[current - 1];
    }

    /// <summary>
    /// Check the type of the current token
    /// </summary>
    private bool Check(TokenType type)
    {
        if (IsAtEnd())
        {
            return false;
        }
        return Peek().Type == type;
    }

    /// <summary>
    /// Consume a token if it matches one of the passed types
    /// </summary>
    private bool Match(TokenType[] types)
    {
        foreach (TokenType type in types)
        {
            if (Check(type))
            {
                Advance();
                return true;
            }
        }
        return false;
    }

    private void Synchronise()
    {
        Advance();
        while (!IsAtEnd())
        {
            if (Previous().Type == TokenType.SEMICOLON)
            {
                return;
            }
            switch (Peek().Type)
            {
                // If we find one of these keywords, we know we are at the start of a new statement
                case TokenType.PRINT:
                case TokenType.VAR:
                    return;
            }
            Advance();
        }
    }

    // --- STATEMENTS ---

    private Stmt? Declaration()
    {
        try
        {
            if (Match([TokenType.VAR]))
            {
                return VarDeclaration();
            }
            return Statement();
        }
        catch (ParseException)
        {
            Synchronise();
            return null;
        }
    }

    private Stmt VarDeclaration()
    {
        Token identifier = Consume(TokenType.IDENTIFIER, "Expected identifier.");
        Expr initializer = null!;
        // If we see an equals we know the variable is being initialised, otherwise its value will be set to null.
        if (Match([TokenType.EQUAL]))
        {
            initializer = Expression();
        }
        Consume(TokenType.SEMICOLON, "Expected ';' after variable declaration.");
        return new VarDecl(identifier, initializer!);
    }

    private Stmt Statement()
    {
        // These statements can be identified by their first token
        if (Match([TokenType.PRINT]))
        {
            return PrintStatement();
        }
        if (Match([TokenType.FUNC]))
        {
            return FuncDecl();
        }
        if (Match([TokenType.RETURN]))
        {
            return ReturnStatement();
        }
        if (Match([TokenType.IF]))
        {
            return IfStatement();
        }
        if (Match([TokenType.WHILE]))
        {
            return WhileStatement();
        }
        if (Match([TokenType.FOR]))
        {
            return ForStatement();
        }
        if (Match([TokenType.LEFT_BRACE]))
        {
            return Block();
        }
        // The default case is expression statement, this is more difficult to ascertain from the first token
        return ExpressionStatement();
    }

    private Stmt ReturnStatement()
    {
        Token keyword = Previous();
        Expr? value = null;
        if (!Check(TokenType.SEMICOLON))
        {
            value = Expression();
        }
        Consume(TokenType.SEMICOLON, "Expected ';' after return value.");
        return new ReturnStmt(keyword, value);
    }

    private Stmt FuncDecl()
    {
        Token name = Consume(TokenType.IDENTIFIER, "Expected function name.");
        Consume(TokenType.LEFT_PAREN, "Expected '(' after function name.");
        List<Token> args = new List<Token>();
        if (!Check(TokenType.RIGHT_PAREN))
        {
            do
            {
                if (args.Count >= 255)
                {
                    Logger.LogException(Peek(), "Cannot have more than 255 arguments.");
                }
                args.Add(Consume(TokenType.IDENTIFIER, "Expected argument name."));
            } while (Match([TokenType.COMMA]));
        }
        Consume(TokenType.RIGHT_PAREN, "Expected ')' after arguments.");
        Consume(TokenType.LEFT_BRACE, "Expected '{' before function body.");
        BlockStmt body = Block();
        return new FuncStmt(name, args, body);
    }

    private WhileStmt WhileStatement()
    {
        Consume(TokenType.LEFT_PAREN, "Expected '(' after 'while'.");
        Expr condition = Expression();
        Consume(TokenType.RIGHT_PAREN, "Expected ')' after while condition.");
        Stmt body = Statement();
        return new WhileStmt(condition, body);
    }

    private Stmt ForStatement()
    {
        Consume(TokenType.LEFT_PAREN, "Expected '(' after 'for'.");
        Stmt? initializer;
        if (Match([TokenType.SEMICOLON]))
        {
            // If we immediatly hit a semicolon, the initializer has been omitted.
            initializer = null;
        }
        else if (Match([TokenType.VAR]))
        {
            initializer = VarDeclaration();
        }
        else
        {
            initializer = ExpressionStatement();
        }

        Expr? condition = null;
        if (!Check(TokenType.SEMICOLON))
        {
            condition = Expression();
        }
        Consume(TokenType.SEMICOLON, "Expected ';' after loop condition.");

        Expr increment = null!;
        if (!Check(TokenType.RIGHT_PAREN))
        {
            increment = Expression();
        }
        Consume(TokenType.RIGHT_PAREN, "Expected ')' after for clauses.");

        Stmt body = Statement();

        // if an increment is given, we need to add it to the end of the body
        if (increment != null)
        {
            body = new BlockStmt(new List<Stmt> { body, new ExprStmt(increment) });
        }

        // if no condition is given the loop will run untill mnaually broken
        if (condition == null)
        {
            condition = new LiteralExpr(new Token(TokenType.TRUE, "true", -1, -1));
        }


        // The for statement gets DESUGARED into a while statement
        if (initializer != null)
        {
            body = new BlockStmt(new List<Stmt> { initializer, new WhileStmt(condition, body) });
        }
        else
        {
            body = new WhileStmt(condition, body);
        }

        return body;
    }

    private PrintStmt PrintStatement()
    {
        var value = Expression();
        Consume(TokenType.SEMICOLON, "Expected ';' after value.");
        return new PrintStmt(value);
    }

    private ExprStmt ExpressionStatement()
    {
        // Creates syntax tree for the expression before the semicolon and returns it
        var value = Expression();
        // Consume the semicolon when done parsing, so that we can parse the next statement
        Consume(TokenType.SEMICOLON, "Expected ';' after value.");
        return new ExprStmt(value);
    }

    private IfStmt IfStatement()
    {
        Consume(TokenType.LEFT_PAREN, "Expected '(' after 'if'.");
        Expr condition = Expression();
        Consume(TokenType.RIGHT_PAREN, "Expected ')' after if condition.");
        // We dont allow variable declarations inside control flow statements. So this is Statement() not Declaration()
        Stmt thenBranch = Statement();
        Stmt? elseBranch = null;
        if (Match([TokenType.ELSE]))
        {
            elseBranch = Statement();
        }
        return new IfStmt(condition, thenBranch, elseBranch);
    }

    private BlockStmt Block()
    {
        List<Stmt> statements = new List<Stmt>();
        while (!Check(TokenType.RIGHT_BRACE) && !IsAtEnd())
        {
            statements.Add(Declaration());
        }
        Consume(TokenType.RIGHT_BRACE, "Expected '}' after block.");
        return new BlockStmt(statements);
    }

    // --- EXPRESSIONS ---

    // These parsing methods are in order of precedence in the grammar rules. 
    // This call stack goes from the top to the bottom of the syntax tree.
    // When it reaches the bottom (the terminals of the language) it will return a value and the call frames will begin popping off the stack.
    private Expr Expression()
    {
        return Assignment();
    }

    private Expr Assignment()
    {
        Expr expr = Or();
        if (Match([TokenType.EQUAL]))
        {
            Token equals = Previous();
            Expr value = Assignment();
            if (expr is VarExpr)
            {
                Token identifier = ((VarExpr)expr).Identifier;
                return new VarAssignExpr(identifier, value);
            }
            throw new ParseException("Invalid assignment target.");
        }
        return expr;

    }

    private Expr Or()
    {
        Expr expr = And();
        while (Match([TokenType.OR]))
        {
            var op = Previous();
            var right = And();
            expr = new LogicalExpr(expr, op, right);
        }
        return expr;
    }

    private Expr And()
    {
        Expr expr = Equality();
        // If there is no AND token here, this just falls through to equality.
        while (Match([TokenType.AND]))
        {
            var op = Previous();
            var right = Equality();
            expr = new LogicalExpr(expr, op, right);
        }
        return expr;
    }

    private Expr Equality()
    {
        Expr expr = Comparison();

        while (Match([TokenType.EQUAL_EQUAL, TokenType.BANG_EQUAL]))
        {
            var op = Previous();
            var right = Term();
            expr = new BinaryExpr(expr, op, right);
        }

        return expr;
    }

    // need  to add comparison in here
    private Expr Comparison()
    {
        var expr = Term();
        while (Match([TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL]))
        {
            var op = Previous();
            var right = Term();
            expr = new BinaryExpr(expr, op, right);
        }

        return expr;
    }


    private Expr Term()
    {
        var expr = Factor();
        // we loop here as a term can consist of factor + factor - factor + factor ... etc
        // this will keep updating the expr variable and adding new left associative terms to the right hand side
        while (Match([TokenType.PLUS, TokenType.MINUS]))
        {
            var op = Previous();
            var right = Factor();
            expr = new BinaryExpr(expr, op, right);
        }

        return expr;
    }

    private Expr Factor()
    {
        var expr = Call();
        while (Match([TokenType.MULT, TokenType.DIV]))
        {
            var op = Previous();
            var right = Primary();
            expr = new BinaryExpr(expr, op, right);
        }

        return expr;

    }

    private Expr Call()
    {
        var expr = Primary();
        while (Match([TokenType.LEFT_PAREN]))
        {
            List<Expr> arguments = new List<Expr>();
            if (!Check(TokenType.RIGHT_PAREN))
            {
                do
                {
                    if (arguments.Count >= 255)
                    {
                        Logger.LogException(Peek(), "Cannot have more than 255 arguments.");
                    }
                    arguments.Add(Expression());
                } while (Match([TokenType.COMMA]));
            }
            Token rightParen = Consume(TokenType.RIGHT_PAREN, "Expected ')' after arguments.");
            expr = new CallExpr(expr, rightParen, arguments);
        }
        return expr;
    }

    private Expr Primary()
    {
        if (Match([TokenType.NUM, TokenType.TRUE, TokenType.FALSE]))
        {
            return new LiteralExpr(Previous());
        }
        if (Match([TokenType.IDENTIFIER]))
        {
            return new VarExpr(Previous());
        }
        throw new ParseException("Expected expression.");
    }
}