namespace Lex;


public class Token(TokenType type, string lexeme, int line, int? value = 0)
{
    public TokenType Type = type;
    public string Lexeme = lexeme;
    public int Line = line;
    public int? Value = value;

}
