using Lex;
using Interpret;
using Exception;

namespace Parse;

public class CallExpr(Expr callee, Token rightParen, List<Expr> arguments) : Expr
{
    public Expr callee = callee;
    public override object Evaluate(Env? env = null)
    {
        var func = callee.Evaluate(env);

        var args = new List<object>();
        foreach (var argument in arguments)
        {
            args.Add(argument.Evaluate(env));
        }

        if (!(func is Callable))
        {
            throw new RuntimeException(rightParen, "Can only call functions and classes.");
        }

        Callable callable = (Callable)func;

        if (args.Count != callable.Arity())
        {
            throw new RuntimeException(rightParen, "Expected " + callable.Arity() + " arguments but got " + args.Count + ".");
        }

        return callable.Call(args);
    }

    public override void Resolve(Resolver resolver)
    {
        callee.Resolve(resolver);
        foreach (var argument in arguments)
        {
            argument.Resolve(resolver);
        }
    }

}