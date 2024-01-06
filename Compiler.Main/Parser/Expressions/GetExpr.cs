using Lex;
using Interpret;
using Exception;

namespace Parse;

/// <summary>
/// Expression for getting a property on an instance.
/// </summary>
class GetExpr(Expr Object, Token Name) : Expr
{
    public Expr Object = Object;
    public Token Name = Name;
    public override object Evaluate(Env env)
    {
        var obj = Object.Evaluate(env);
        if (obj is ClassInstance classInstance)
        {
            return classInstance.Get(Name);
        }
        throw new RuntimeException(Name, $"Cannot access property {Name.Lexeme} on non-class instance");
    }

    public override void Resolve(Resolver resolver)
    {
        Object.Resolve(resolver);
    }
}