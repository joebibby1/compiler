namespace Parse;

using Lex;
using Interpret;

public class ReturnStmt(Token keyword, Expr? value) : Stmt
{
    public readonly Token Keyword = keyword;
    public readonly Expr? Value = value;

    public override void Execute(Env? env)
    {
        object? returnValue = null;
        if (Value != null)
        {
            returnValue = Value.Evaluate(env);
        }
        throw new ReturnValue(returnValue);
    }
}