using Interpret;

namespace Parse;



public class Expr
{
    public virtual string Parenthesise()
    {
        return "";
    }

    public virtual object Evaluate(Env? env = null)
    {
        return new object();
    }

    public virtual string PrintFormat()
    {
        return "";
    }
}