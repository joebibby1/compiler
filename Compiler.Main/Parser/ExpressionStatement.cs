using Interpret;

namespace Parse;



class ExpressionStmt(Expr expr) : Stmt
{

    public override void Execute(Env? env = null)
    {
        expr.Evaluate(env);
    }
}