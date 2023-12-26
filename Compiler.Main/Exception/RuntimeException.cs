namespace Exception;

using Lex;

class RuntimeException(Token token, string message) : System.Exception
{
    public Token Token = token;
    new public string Message = message;

    public override string ToString()
    {
        return Message + " at line " + Token.Line;
    }

}