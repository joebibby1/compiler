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
}



