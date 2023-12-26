using Interpret;

namespace Parse;


class PrintStmt(Expr expr) : Stmt
{
    public override void Execute(Env? env = null)
    {
        Console.WriteLine(expr.Evaluate(env).ToString());
    }
}