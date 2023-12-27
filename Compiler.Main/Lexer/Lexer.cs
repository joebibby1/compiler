namespace Lex;

//  To add: parentheses

public class Lexer(string input)
{
    private List<Token> tokens = new List<Token>();
    private int start = 0;
    private int current = 0;
    private int line = 1;

    private readonly Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType>()
    {
        {"print", TokenType.PRINT},
        {"var", TokenType.VAR}
    };

    public List<Token> ScanTokens()
    {
        while (!IsAtEnd())
        {
            start = current;
            ScanToken();
        }
        // indicate end of token stream
        tokens.Add(new Token(TokenType.EOF, "", line, null));
        return tokens;
    }

    private char Advance()
    {
        return input[current++];
    }

    private char Peek()
    {
        if (IsAtEnd())
        {
            return '\0';
        }
        return input[current];
    }

    private void ScanToken()
    {
        // discard all white space
        // switch on the current character
        // if it is a digit, call scan number method
        // if it is a + or - call scan operator method
        var c = Advance();
        switch (c)
        {
            case ' ':
            case '\r':
            case '\t':
                // Ignore whitespace.
                break;
            case '\n':
                line++;
                break;
            case '+':
                AddToken(TokenType.PLUS, "+", line, null);
                break;
            case '-':
                AddToken(TokenType.MINUS, "-", line, null);
                break;
            case '*':
                AddToken(TokenType.MULT, "*", line, null);
                break;
            case '/':
                AddToken(TokenType.DIV, "/", line, null);
                break;
            case '=':
                AddToken(TokenType.EQUAL, "=", line, null);
                break;
            case ';':
                AddToken(TokenType.SEMICOLON, ";", line, null);
                break;
            case '{':
                AddToken(TokenType.LEFT_BRACE, "{", line, null);
                break;
            case '}':
                AddToken(TokenType.RIGHT_BRACE, "}", line, null);
                break;
            default:
                if (IsDigit(c))
                {
                    ScanNum();
                }
                else if (IsAlpha(c))
                {
                    ScanIdentifier();
                }
                else
                {
                    // Throw new ParseException(line, "Unexpected character.");
                    // ErrorHandler.HandleError(line, "Unexpected character.");
                }
                break;
        }
    }

    private void ScanNum()
    {
        while (IsDigit(Peek()))
        {
            Advance();
        }
        AddToken(TokenType.NUM, input.Substring(start, current - start), line, int.Parse(input.Substring(start, current - start)));
    }

    private void ScanIdentifier()
    {
        while (IsAlpha(Peek()))
        {
            Advance();
        }
        var text = input.Substring(start, current - start);
        TokenType type;
        if (keywords.ContainsKey(text))
        {
            type = keywords[text];
        }
        else
        {
            type = TokenType.IDENTIFIER;
        }
        AddToken(type, text, line, null);
    }

    private bool IsDigit(char c)
    {
        return c >= '0' && c <= '9';
    }

    private bool IsAlpha(char c)
    {
        return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
    }

    private void AddToken(TokenType type, string lexeme, int line, int? value)
    {
        tokens.Add(new Token(type, lexeme, line, value));
    }

    private bool IsAtEnd()
    {
        return current >= input.Length;
    }

}