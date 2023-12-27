using Interpret;

namespace Parse;


public class BlockStmt(List<Stmt> statements) : Stmt
{

    public override void Execute(Env? env = null)
    {
        // create a new environment for the block and pass in the enclosing environment
        var localEnv = new Env(env);

        foreach (var statement in statements)
        {
            statement.Execute(localEnv);
        }
    }
}