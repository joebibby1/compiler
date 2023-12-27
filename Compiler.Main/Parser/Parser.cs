using Exception;
using Lex;

namespace Parse;


// STATEMENT GRAMMAR
// program        → declaration* EOF ;
// declaration    → varDecl | statement ;
// statement      → exprStmt | printStmt | block | ifStmt ;
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
// primary        → NUMBER | "(" expression ")" | IDENTIFIER ;

// need to add: comparison, false/null tokens
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
        // If we see an equals we know the variable is being initialise, otherwise its value will be set to null.
        if (Match([TokenType.EQUAL]))
        {
            initializer = Expression();
        }
        Consume(TokenType.SEMICOLON, "Expected ';' after variable declaration.");
        return new VarDecl(identifier, initializer!);
    }

    private Stmt Statement()
    {
        // If the first token is PRINT, we know this is a print statement
        if (Match([TokenType.PRINT]))
        {
            return PrintStatement();
        }
        if (Match([TokenType.IF]))
        {
            return IfStatement();
        }
        if (Match([TokenType.LEFT_BRACE]))
        {
            return Block();
        }
        // The default case is expression statement, this is more difficult to ascertain from the first token
        return ExpressionStatement();
    }

    private Stmt PrintStatement()
    {
        var value = Expression();
        Consume(TokenType.SEMICOLON, "Expected ';' after value.");
        return new PrintStmt(value);
    }

    private Stmt ExpressionStatement()
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

    private Stmt Block()
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
        Expr expr = Term();

        while (Match([TokenType.EQUAL_EQUAL, TokenType.BANG_EQUAL]))
        {
            var op = Previous();
            var right = Term();
            expr = new BinaryExpr(expr, op, right);
        }

        return expr;
    }

    // need  to add comparison in here


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
        var expr = Primary();
        while (Match([TokenType.MULT, TokenType.DIV]))
        {
            var op = Previous();
            var right = Primary();
            expr = new BinaryExpr(expr, op, right);
        }

        return expr;

    }

    private Expr Primary()
    {
        if (Match([TokenType.NUM]))
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