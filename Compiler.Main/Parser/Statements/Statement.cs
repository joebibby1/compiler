using Interpret;

namespace Parse;

public abstract class Stmt
{
    // public virtual void Execute()
    // {

    // }

    // /// <summary>
    // /// Execute the statement in the given environment, for variable declarations.
    // /// </summary>
    // public virtual void Execute(Env env)
    // {

    // }

    public abstract void Execute(Env? env = null);
};