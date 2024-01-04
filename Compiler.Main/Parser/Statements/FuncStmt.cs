using Lex;
using Interpret;

namespace Parse;

public class FuncStmt(Token name, List<Token> args, BlockStmt body) : Stmt
{
    public readonly Token Name = name;
    public readonly List<Token> Args = args;
    public readonly BlockStmt Body = body;
    public override void Execute(Env? env)
    {
        // define the new function in the current environment
        env!.Define(Name.Lexeme, new CallableFunc(this, env));
    }

    public override void Resolve(Resolver resolver)
    {
        resolver.Declare(Name);
        resolver.Define(Name);
        // resolver.ResolveFunction(this);
    }
}