using Parse;

namespace Interpret;

public class CallableFunc(FuncStmt funcDecl) : Callable
{
    public int Arity()
    {
        return funcDecl.Args.Count;
    }

    public object Call(List<object> arguments)
    {
        // create a new environment for the function
        var localEnv = new Env();

        // define the values of the parameters as they are passed in to this environment
        for (var i = 0; i < Arity(); i++)
        {
            localEnv.Define(funcDecl.Args[i].Lexeme, arguments[i]);
        }

        // execute the function body
        funcDecl.Body.Execute(localEnv);

        return true;
    }

    public override string ToString()
    {
        return $"<fn {funcDecl.Name.Lexeme}>";
    }
}