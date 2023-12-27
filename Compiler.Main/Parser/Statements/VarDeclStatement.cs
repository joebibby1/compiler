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
}