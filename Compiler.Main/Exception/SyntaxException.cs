using Lex;

namespace Exception;


class SyntaxException(Token token, string message) : System.Exception
{

}