using Lex;
using Interpret;


namespace Parse;

/// <summary>
/// Expression for variable assignment. This expression does not create a new variable binding,
/// it can only update the value of an existing variable.
/// </summary>
public class VarAssignExpr(Token identifier, Expr value) : Expr
{
    public int ScopeDistance { get; set; } = -1;
    public override object Evaluate(Env? env)
    {
        return env!.AssignAt(ScopeDistance, identifier, value.Evaluate(env));
    }

    public override void Resolve(Resolver resolver)
    {
        value.Resolve(resolver);
        ScopeDistance = resolver.ResolveLocal(this, identifier);
    }
}