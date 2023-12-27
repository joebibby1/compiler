using Interpret;

namespace Parse;



public class ExprStmt(Expr expr) : Stmt
{

    public override void Execute(Env? env = null)
    {
        expr.Evaluate(env);
    }
}