using Exception;
using Lex;

namespace Interpret;


public class Env(Env? enclosing = null)
{
    private Dictionary<string, object> variables = new Dictionary<string, object>();

    public void Define(string name, object value)
    {
        variables[name] = value;
    }

    public object Assign(Token identifier, object value)
    {
        // here we want to assign the variable if it exists in local scope. Otherwise we want to assign it to enclosing scope
        // If it does not exist in any enclosing scope we want to throw a runtime exception
        if (variables.ContainsKey(identifier.Lexeme))
        {
            variables[identifier.Lexeme] = value;
            return value;
        }

        if (enclosing != null)
        {
            return enclosing.Assign(identifier, value);
        }

        throw new RuntimeException(identifier, "Undefined variable: " + identifier.Lexeme + '.');
    }

    public object Get(Token identifier)
    {
        if (variables.ContainsKey(identifier.Lexeme))
        {
            return variables[identifier.Lexeme];
        }

        // we recursively search the outer enclosing scopes to see if the variable is in any of them
        if (enclosing != null)
        {
            return enclosing.Get(identifier);
        }

        throw new SyntaxException(identifier, "Undefined variable " + identifier.Lexeme + '.');
    }
}