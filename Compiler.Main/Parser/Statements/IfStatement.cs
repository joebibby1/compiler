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

    public override void Resolve(Resolver resolver)
    {
        condition.Resolve(resolver);
        thenBranch.Resolve(resolver);
        if (elseBranch != null)
        {
            elseBranch.Resolve(resolver);
        }
    }
}