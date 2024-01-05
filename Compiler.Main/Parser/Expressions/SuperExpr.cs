
using Interpret;
using Lex;

namespace Parse;


public class SuperExpr(Token keyword, Token method) : Expr
{

    /// <summary>
    /// Finds the correct superclass, the correct instance and returns the method from the superclass bound to the context of the instance.
    /// </summary>
    public override object Evaluate(Env? env = null)
    {
        // this souldnt actually be a token, but i havent implemented resolving properly yet. At which point is will be getAt with a distance and a string
        CallableClass super = (CallableClass)env!.Get(new Token(TokenType.IDENTIFIER, "super", 0, keyword.Line));

        ClassInstance instance = (ClassInstance)env.Get(new Token(TokenType.IDENTIFIER, "this", 0, -1));

        CallableFunc func = super.Methods[method.Lexeme].Bind(instance);

        return func;
    }

}