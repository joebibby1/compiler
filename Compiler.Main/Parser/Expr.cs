using Interpret;

namespace Parse;



class Expr
{
    public virtual string Parenthesise()
    {
        return "";
    }

    public virtual Object Evaluate(Env? env = null)
    {
        return new Object();
    }

    public virtual string PrintFormat()
    {
        return "";
    }
}