using Lex;
using Exception;
using Interpret;

namespace Parse;


class BinaryExpr(Expr left, Token op, Expr right) : Expr
{
    public Expr Left = left;
    public Token Op = op;
    public Expr Right = right;

    public override string Parenthesise()
    {
        return "(" + Op.Lexeme + " " + Left.Parenthesise() + " " + Right.Parenthesise() + ")";
    }

    private void CheckNumberOperands(Token op, Object operand, Object operand2)
    {
        if (operand is int && operand2 is int)
        {
            return;
        }
        throw new RuntimeException(op, "Operands must be numbers.");
    }

    public override Object Evaluate(Env? env = null)
    {
        // Check for type errors at runtime
        var left = Left.Evaluate(env);
        var right = Right.Evaluate(env);
        CheckNumberOperands(Op, left, right);

        switch (Op.Type)
        {
            case TokenType.PLUS:
                return (int)left + (int)right;
            case TokenType.MINUS:
                return (int)left - (int)right;
            case TokenType.MULT:
                return (int)left * (int)right;
            case TokenType.DIV:
                return (int)left / (int)right;
            default:
                return new Object();
        }
    }
}