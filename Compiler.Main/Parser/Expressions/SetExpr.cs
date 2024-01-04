using Lex;
using Interpret;
using Exception;

namespace Parse;


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
}