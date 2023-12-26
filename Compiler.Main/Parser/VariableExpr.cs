using Lex;
using Interpret;

namespace Parse;


class VarExpr(Token identifier) : Expr
{

    public Token Identifier = identifier;

    public override string Parenthesise()
    {
        return "(" + identifier.Lexeme + ")";
    }

    public override Object Evaluate(Env? env)
    {
        return env!.Get(identifier);
    }

    public override string PrintFormat()
    {
        // print the formatted value here
        return "";
    }
}