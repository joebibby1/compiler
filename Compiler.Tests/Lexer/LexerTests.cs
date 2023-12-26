using Lex;

namespace Compiler.Tests;

public class UnitTest1
{
    [Fact]
    public void Lexer_ScansIdentifierWithoutInitializer()
    {
        var input = "var x;";
        var lexer = new Lexer(input);
        var tokens = lexer.ScanTokens();


        Assert.Equal(4, tokens.Count);
        Assert.Equal(TokenType.VAR, tokens[0].Type);
        Assert.Equal(TokenType.IDENTIFIER, tokens[1].Type);
    }

    [Fact]
    public void Lexer_AddsEOFToken()
    {
        var input = "var x;";
        var lexer = new Lexer(input);
        var tokens = lexer.ScanTokens();

        Assert.Equal(TokenType.EOF, tokens[^1].Type);
    }
}