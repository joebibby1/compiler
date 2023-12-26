namespace Parse;

using Interpret;
using Lex;
class LiteralExpr(Token literal) : Expr
{

    public string Lexeme => literal.Lexeme;

    public int? Value => literal.Value;

    public override string Parenthesise()
    {
        return "(" + literal.Lexeme + ")";
    }

    public override Object Evaluate(Env? env = null)
    {
        return Value!;
    }
}
