using Interpret;

namespace Parse;

public class WhileStmt(Expr condition, Stmt body) : Stmt
{
    public override void Execute(Env? env = null)
    {
        while (InterpretHelpers.IsTruthy(condition.Evaluate(env)))
        {
            body.Execute(env);
        }
    }
}