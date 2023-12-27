using Lex;
using Interpret;
namespace Parse;


public class LogicalExpr(Expr left, Token logicalOp, Expr right) : Expr
{
    public override object Evaluate(Env? env = null)
    {
        var leftValue = left.Evaluate(env);

        if (logicalOp.Type == TokenType.OR)
        {
            // In an OR expression, if the left side is truthy, we don't need to evaluate the right side.
            if (InterpretHelpers.IsTruthy(leftValue))
            {
                return leftValue;
            }
        }
        else
        {
            // In an AND expression, if the left side is falsy, we don't need to evaluate the right side.
            if (!InterpretHelpers.IsTruthy(leftValue))
            {
                return leftValue;
            }
        }

        // If we get to this point, then the truthiness of the whole expression depends on the truthiness of the right side.
        return right.Evaluate(env);
    }
}
