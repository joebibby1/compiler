using Lex;
using Interpret;

namespace Parse;

/// <summary>
/// Statement for variable declarations. Stores the result of the initializer expression in the env table. This creates a new variable binding.
/// </summary>
public class VarDecl(Token identififer, Expr initializer) : Stmt
{

    public override void Execute(Env? env)
    {
        env!.Define(identififer.Lexeme, initializer.Evaluate(env));
    }

    public override void Resolve(Resolver resolver)
    {
        // we declare the variable in the current scope in the resolver
        resolver.Declare(identififer);
        if (initializer != null)
        {
            initializer.Resolve(resolver);
        }
        resolver.Define(identififer);
    }
}