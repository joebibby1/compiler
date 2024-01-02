using Parse;
using Lex;
using Interpret;
using Xunit;

namespace Compiler.Tests;
// we'll use the lexer to generate tokens, just for ease, and assume it is correct. We test the lexer separately.
// It may be a good idea to hardcode the tokens in the future, but for now, this is fine.

public class ParseStatementTests
{
    [Fact]
    public void ParsesVarDeclaration()
    {
        var input = "var a = 1;";
        var tokens = new Lexer(input).ScanTokens();
        var statements = new Parser(tokens).Parse();

        Assert.Single(statements);
        Assert.IsType<VarDecl>(statements[0]);
    }

    [Fact]
    public void ParsesBlockStatement()
    {
        var input = "{ var a = 1; }";
        var tokens = new Lexer(input).ScanTokens();
        var statements = new Parser(tokens).Parse();

        Assert.Single(statements);
        Assert.IsType<BlockStmt>(statements[0]);
    }

    [Fact]
    public void ParsesPrintStatement()
    {
        var input = "print 1;";
        var tokens = new Lexer(input).ScanTokens();
        var statements = new Parser(tokens).Parse();

        Assert.Single(statements);
        Assert.IsType<PrintStmt>(statements[0]);
    }

    [Fact]
    public void ParsesExpressionStatement()
    {
        var input = "1 + 1;";
        var tokens = new Lexer(input).ScanTokens();
        var statements = new Parser(tokens).Parse();

        Assert.Single(statements);
        Assert.IsType<ExprStmt>(statements[0]);
    }

    [Fact]
    public void ParsesIfStatement()
    {
        var input = "if (true) { print 1; } else { print 2; }";
        var tokens = new Lexer(input).ScanTokens();
        var statements = new Parser(tokens).Parse();

        Assert.Single(statements);
        Assert.IsType<IfStmt>(statements[0]);
    }

    [Fact]
    public void ParsesWhileStatement()
    {
        var input = "while (true) { print 1; }";
        var tokens = new Lexer(input).ScanTokens();
        var statements = new Parser(tokens).Parse();

        Assert.Single(statements);
        Assert.IsType<WhileStmt>(statements[0]);
    }

    [Fact]
    public void ParsesForStatementWithoutInitializer()
    {
        var input = "for (; i < 10; i = i + 1) { print i; }";
        var tokens = new Lexer(input).ScanTokens();
        var statements = new Parser(tokens).Parse();

        Assert.Single(statements);
        Assert.IsType<WhileStmt>(statements[0]);
    }

    [Fact]
    public void ParsesForStatementWithInitializer()
    {
        var input = "for (var i = 0; i < 10; i = i + 1) { print i; }";
        var tokens = new Lexer(input).ScanTokens();
        var statements = new Parser(tokens).Parse();

        Assert.Single(statements);
        // if there is an initializer then we have a block containing the declaration and then the while statement
        Assert.IsType<BlockStmt>(statements[0]);
    }
}



