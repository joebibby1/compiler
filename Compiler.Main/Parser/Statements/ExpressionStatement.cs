using Interpret;

namespace Parse;



public class ExprStmt(Expr expr) : Stmt
{
    public Expr expr = expr;

    public override void Execute(Env? env = null)
    {
        expr.Evaluate(env);
    }

    public override void Resolve(Resolver resolver)
    {
        expr.Resolve(resolver);
    }
}