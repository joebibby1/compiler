using Interpret;

namespace Parse;


public class IfStmt(Expr condition, Stmt thenBranch, Stmt? elseBranch) : Stmt
{
    public override void Execute(Env? env = null)
    {
        if (InterpretHelpers.IsTruthy(condition.Evaluate(env)))
        {
            thenBranch.Execute(env);
        }
        else if (elseBranch != null)
        {
            elseBranch.Execute(env);
        }
    }
}