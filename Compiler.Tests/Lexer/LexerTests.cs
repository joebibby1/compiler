using Lex;

// throws correct errors in different situations:
// trying to use a keyword as an identifier
// scans keywords correclty
// scans identifiers correctly
// scans numbers correctly

namespace Compiler.Tests;

public class LexerTests
{
    [Fact]
    public void ScansIdentifier()
    {
        var input = "var x;";
        var lexer = new Lexer(input);
        var tokens = lexer.ScanTokens();


        Assert.Equal(4, tokens.Count);
        Assert.Equal(TokenType.IDENTIFIER, tokens[1].Type);
        Assert.Equal("x", tokens[1].Lexeme);
    }

    [Fact]
    public void AddsEOFToken()
    {
        var input = "var x;";
        var lexer = new Lexer(input);
        var tokens = lexer.ScanTokens();

        Assert.Equal(TokenType.EOF, tokens[^1].Type);
    }

    [Fact]
    public void ScansKeywords()
    {
        var input = "var x;";
        var lexer = new Lexer(input);
        var tokens = lexer.ScanTokens();

        Assert.Equal(TokenType.VAR, tokens[0].Type);
    }

    [Fact]
    public void ScansNumbers()
    {
        var input = "2";
        var lexer = new Lexer(input);
        var tokens = lexer.ScanTokens();

        Assert.Equal(TokenType.NUM, tokens[0].Type);
        Assert.Equal("2", tokens[0].Lexeme);
        Assert.Equal(2, tokens[0].Value);
    }
}