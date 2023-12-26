using Exception;
using Lex;

namespace Interpret;


class Env
{
    private Dictionary<string, Object> variables = new Dictionary<string, Object>();

    public void Define(string name, Object value)
    {
        variables[name] = value;
    }

    public Object Assign(Token identifier, Object value)
    {
        if (!variables.ContainsKey(identifier.Lexeme))
        {
            throw new RuntimeException(identifier, "Variable name does not exist");
        }

        variables[identifier.Lexeme] = value;

        return value;
    }

    public Object Get(Token identifier)
    {
        if (variables.ContainsKey(identifier.Lexeme))
        {
            return variables[identifier.Lexeme];
        }
        throw new SyntaxException(identifier, "Undefined variable " + identifier.Lexeme + '.');
    }
}