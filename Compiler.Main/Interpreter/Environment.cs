using Exception;
using Lex;

namespace Interpret;


public class Env(Env? enclosing = null)
{
    private Dictionary<string, object> variables = new Dictionary<string, object>();
    public Env? Enclosing = enclosing;

    // we define a static dictionary of global functions which is shared by all environments and does not change. This is not the same as the top level scope, which can be written to
    private static readonly Dictionary<string, object> globals = new Dictionary<string, object>()
    {
        {"clock", new Clock()},
    };

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

        if (globals.ContainsKey(identifier.Lexeme))
        {
            return globals[identifier.Lexeme];
        }

        throw new SyntaxException(identifier, "Undefined variable " + identifier.Lexeme + '.');
    }

    private Env Ancestor(int distance)
    {
        Env environment = this;
        for (int i = 0; i < distance; i++)
        {
            environment = environment.Enclosing!;
        }

        return environment;
    }

    public object GetAt(int distance, Token name)
    {
        // -1 is the scope distance we set as default on the syntax nodes, if it is still -1 then the node has not been resolved properly
        // may need to change this to account for global variables
        if (distance == -1)
        {
            throw new RuntimeException(name, "Variable has not been resolved properly.");
        }
        return Ancestor(distance).variables[name.Lexeme];
    }

    public object AssignAt(int distance, Token name, object value)
    {
        Ancestor(distance).variables[name.Lexeme] = value;
        return value;
    }
}