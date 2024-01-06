using Lex;
using Interpret;
using Exception;

namespace Parse;

/// <summary>
/// Expression for setting a property on an instance. 
/// </summary>
public class SetExpr(Expr Object, Token Name, Expr Value) : Expr
{
    public override object Evaluate(Env? env)
    {
        var obj = Object.Evaluate(env);

        if (obj is not ClassInstance)
        {
            throw new RuntimeException(Name, "Only instances have fields.");
        }

        var value = Value.Evaluate(env);
        ((ClassInstance)obj).Set(Name, value);

        return value;
    }

    public override void Resolve(Resolver resolver)
    {
        Object.Resolve(resolver);
        Value.Resolve(resolver);
    }
}