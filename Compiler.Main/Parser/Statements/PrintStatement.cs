using Interpret;

namespace Parse;


public class PrintStmt(Expr expr) : Stmt
{
    public override void Execute(Env? env = null)
    {
        Console.WriteLine(expr.Evaluate(env).ToString());
    }

    public override void Resolve(Resolver resolver)
    {
        expr.Resolve(resolver);
    }
}