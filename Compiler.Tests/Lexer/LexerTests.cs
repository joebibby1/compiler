using Lex;

// throws correct errors in different situations:
// trying to use a keyword as an identifier
// scans keywords correclty
// scans identifiers correctly
// scans numbers correctly

namespace Compiler.Tests;

public class UnitTest1
{
    [Fact]
    public void Lexer_ScansIdentifier()
    {
        var input = "var x;";
        var lexer = new Lexer(input);
        var tokens = lexer.ScanTokens();


        Assert.Equal(4, tokens.Count);
        Assert.Equal(TokenType.IDENTIFIER, tokens[1].Type);
        Assert.Equal("x", tokens[1].Lexeme);
    }

    [Fact]
    public void Lexer_AddsEOFToken()
    {
        var input = "var x;";
        var lexer = new Lexer(input);
        var tokens = lexer.ScanTokens();

        Assert.Equal(TokenType.EOF, tokens[^1].Type);
    }

    [Fact]
    public void ScansKeywordsCorrectly()
    {
        var input = "var x;";
        var lexer = new Lexer(input);
        var tokens = lexer.ScanTokens();

        Assert.Equal(TokenType.VAR, tokens[0].Type);
    }
}