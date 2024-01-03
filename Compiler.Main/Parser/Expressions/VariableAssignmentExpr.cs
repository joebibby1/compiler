using Lex;
using Interpret;


namespace Parse;

/// <summary>
/// Expression for variable assignment. This expression does not create a new variable binding,
/// it can only update the value of an existing variable.
/// </summary>
class VarAssignExpr(Token identifier, Expr value) : Expr
{
    public override object Evaluate(Env? env)
    {
        return env!.Assign(identifier, value.Evaluate(env));
    }
}