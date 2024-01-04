using Lex;
using Interpret;

namespace Parse;

class ClassDeclrStmt(Token Name, List<FuncStmt> Methods) : Stmt
{
    public Token Name = Name;
    public List<FuncStmt> Methods = Methods;

    public override void Execute(Env? env = null)
    {
        env!.Define(Name.Lexeme, new CallableClass(Name.Lexeme));
    }

    public override void Resolve(Resolver resolver)
    {
    }
}