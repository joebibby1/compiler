
using Lex;

namespace Parse;

// Have left this at this point:
// - it checks variable decarations and adds them to the stack of scopes
// - for variable usages it checks the scope distance and adds it to the variable expr
// - i have not done the evaluation/execution part yet
// - this involves using the scope distance when traversing the linked list of envrionments during execution
// need to do function declarations/calls and write tests for those also

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

    public void ResolveFunction(FuncStmt function)
    {
        BeginScope();
        foreach (var param in function.Args)
        {
            Declare(param);
            Define(param);
        }
        function.Body.Resolve(this);
        EndScope();
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