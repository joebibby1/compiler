using Interpret;

namespace Parse;

public abstract class Stmt
{
    public abstract void Resolve(Resolver resolver);

    public abstract void Execute(Env? env = null);
};