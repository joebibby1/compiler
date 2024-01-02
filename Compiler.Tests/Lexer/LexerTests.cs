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
    public void Lexer_ScansKeywordsCorrectly()
    {
        var input = "var x;";
        var lexer = new Lexer(input);
        var tokens = lexer.ScanTokens();

        Assert.Equal(TokenType.VAR, tokens[0].Type);
    }

    // [Fact]
    // public void Lexer_ScansForLoop()
    // {
    //     var input = "for (var i = 0; i < 10; i = i + 1) { print i; }";
    //     var lexer = new Lexer(input);
    //     var tokens = lexer.ScanTokens();

    //     Assert.Equal(TokenType.FOR, tokens[0].Type);
    //     Assert.Equal(TokenType.LEFT_PAREN, tokens[1].Type);
    //     Assert.Equal(TokenType.VAR, tokens[2].Type);
    //     Assert.Equal(TokenType.IDENTIFIER, tokens[3].Type);
    //     Assert.Equal(TokenType.EQUAL, tokens[5].Type);
    //     Assert.Equal(TokenType.NUM, tokens[6].Type);
    //     Assert.Equal(TokenType.SEMICOLON, tokens[7].Type);
    //     Assert.Equal(TokenType.IDENTIFIER, tokens[9].Type);
    //     Assert.Equal(TokenType.LESS, tokens[11].Type);
    //     Assert.Equal(TokenType.NUM, tokens[12].Type);
    //     Assert.Equal(TokenType.SEMICOLON, tokens[13].Type);
    //     Assert.Equal(TokenType.IDENTIFIER, tokens[15].Type);
    //     Assert.Equal(TokenType.EQUAL, tokens[17].Type);
    //     Assert.Equal(TokenType.IDENTIFIER, tokens[19].Type);
    //     Assert.Equal(TokenType.PLUS, tokens[21].Type);
    //     Assert.Equal(TokenType.NUM, tokens[22].Type);
    //     Assert.Equal(TokenType.RIGHT_PAREN, tokens[23].Type);
    //     Assert.Equal(TokenType.LEFT_BRACE, tokens[25].Type);
    //     Assert.Equal(TokenType.PRINT, tokens[27].Type);
    //     Assert.Equal(TokenType.IDENTIFIER, tokens[29].Type);
    //     Assert.Equal(TokenType.SEMICOLON, tokens[30].Type);
    //     Assert.Equal(TokenType.RIGHT_BRACE, tokens[31].Type);
    // }

    [Fact]
    public void Lexer_ScansWhileLoop()
    { }
}