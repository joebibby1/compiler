using Lex;
using Interpret;

namespace Parse;

class ClassDeclrStmt(Token Name, List<FuncStmt> Methods) : Stmt
{
    public Token Name = Name;
    public List<FuncStmt> Methods = Methods;

    public override void Execute(Env? env = null)
    {
        // store all the methods in a hashmap to go into the class
        Dictionary<string, CallableFunc> methods = new Dictionary<string, CallableFunc>();
        foreach (var method in Methods)
        {
            methods.Add(method.Name.Lexeme, new CallableFunc(method, env, method.Name.Lexeme == "init"));
        }

        // store the class in the current scope
        env!.Define(Name.Lexeme, new CallableClass(Name.Lexeme, methods));
    }

    public override void Resolve(Resolver resolver)
    {
    }
}