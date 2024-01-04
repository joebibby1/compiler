using Lex;
using Interpret;

namespace Parse;

public class ThisExpr(Token keyword) : Expr
{
    public override object Evaluate(Env? env)
    {
        // for now we just search up the environmnets for the nearest class instance (not necessarily the correct one, where the method is accessed, if that method is then passed somewhere else)
        // we need to finish the resolver to implement method binding properly 
        return env!.Get(keyword);
    }
}