using System.Runtime.InteropServices;
using Parse;

namespace Interpret;

// closure is the environment in which the function was defined, so we pass it in to the constructor, so that it can be used when the function is called
public class CallableFunc(FuncStmt funcDecl, Env closure) : Callable
{
    public int Arity()
    {
        return funcDecl.Args.Count;
    }

    public object Call(List<object> arguments)
    {
        // create a new environment for the function
        var localEnv = new Env(closure);

        // define the values of the parameters as they are passed in to this environment
        for (var i = 0; i < Arity(); i++)
        {
            localEnv.Define(funcDecl.Args[i].Lexeme, arguments[i]);
        }

        try
        {
            funcDecl.Body.Execute(localEnv);
        }
        catch (ReturnValue returnValue)
        {
            return returnValue.Value;
        }

        return true;
    }

    public override string ToString()
    {
        return $"<fn {funcDecl.Name.Lexeme}>";
    }
}