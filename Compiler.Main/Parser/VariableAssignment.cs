using Lex;
using Interpret;


namespace Parse;

class VarAssignExpr(Token identifier, Object value) : Expr
{
    public void Evaluate(Env? env)
    {
        env!.Assign(identifier, value);
    }
}