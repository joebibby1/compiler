using Lex;

namespace Exception;


class SyntaxException(Token token, string message) : System.Exception
{
    public override string ToString()
    {
        return "Syntax error at line " + token.Line + ": " + message;
    }
}