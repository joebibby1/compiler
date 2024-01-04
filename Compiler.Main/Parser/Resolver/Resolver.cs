
using Lex;

namespace Parse;



public class Resolver
{
    public Stack<Dictionary<string, bool>> Scopes = new Stack<Dictionary<string, bool>>();

    public Resolver()
    {
        // push the global scope on to start
        BeginScope();
    }

    public void BeginScope()
    {
        Scopes.Push(new Dictionary<string, bool>());
    }

    public void EndScope()
    {
        Scopes.Pop();
    }

    /// <summary>
    /// Add the variable name to the current scope, setting its value to false to indicate that it has not yet been defined.
    /// </summary>
    public void Declare(Token name)
    {
        if (Scopes.Count == 0)
        {
            return;
        }
        var scope = Scopes.Peek();
        scope.Add(name.Lexeme, false);
    }

    public void Define(Token name)
    {
        if (Scopes.Count == 0)
        {
            return;
        }
        var scope = Scopes.Peek();
        scope[name.Lexeme] = true;
    }



    public int ResolveLocal(Expr expr, Token name)
    {
        // when converting to array, the item on top of stack (current scope)
        var listScopes = Scopes.ToArray();
        for (int i = 0; i < listScopes.Length; i++)
        {
            if (listScopes[i].ContainsKey(name.Lexeme))
            {
                // here there is interpreter.resolve(expr, Scopes.Count - 1 - i)
                return i;
            }
        }
        return -1;
    }



}