using Lex;
using Interpret;

namespace Parse;

public class ThisExpr(Token keyword) : Expr
{
    public override object Evaluate(Env? env)
    {
        // dont need to resolve this expressions. This happends dynmically at runtime via method binding. As so:
        // - i create an instance of a class
        // - the instance "inherits" methods from class (the class is passed in as an arg, and we look up methods on the class)
        // - when i access a method on the instance it returns the method 'bound' to the instance
        // - binding involves creating a new env just inside the function closure, and defining 'this' in that env. And then setting that env as the closure to the bound method

        return env!.Get(keyword);
    }


}