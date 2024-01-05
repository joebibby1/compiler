using Lex;
using Interpret;

namespace Parse;

/// <summary>
/// Expression for variable references. Evaluates to the value of the variable in the environment. This is the expression 
/// used when we want to access the value of a variable.
/// </summary>
public class VarExpr(Token identifier) : Expr
{

    public Token Identifier = identifier;
    public int ScopeDistance = -1;

    public override string Parenthesise()
    {
        return "(" + identifier.Lexeme + ")";
    }

    public override object Evaluate(Env? env)
    {
        return env!.GetAt(ScopeDistance, identifier);
    }

    public override string PrintFormat()
    {
        // print the formatted value here
        return "";
    }

    public override void Resolve(Resolver resolver)
    {
        ScopeDistance = resolver.ResolveLocal(this, identifier);
    }
}