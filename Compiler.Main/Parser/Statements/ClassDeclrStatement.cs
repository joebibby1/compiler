using Lex;
using Interpret;
using Exception;

namespace Parse;

class ClassDeclrStmt(Token Name, List<FuncStmt> Methods, VarExpr? Super) : Stmt
{
    public Token Name = Name;
    public List<FuncStmt> Methods = Methods;

    public VarExpr? Super = Super;

    public override void Execute(Env? env = null)
    {
        // store all the methods in a hashmap to go into the class
        Dictionary<string, CallableFunc> methods = new Dictionary<string, CallableFunc>();
        foreach (var method in Methods)
        {
            methods.Add(method.Name.Lexeme, new CallableFunc(method, env, method.Name.Lexeme == "init"));
        }

        object? super = null;
        if (Super != null)
        {
            // store the superclass in the current scope
            super = Super.Evaluate(env);
            if (super is not CallableClass)
            {
                throw new RuntimeException(Name, "Superclass must be a class.");
            }
        }

        // store the class in the current scope
        env!.Define(Name.Lexeme, new CallableClass(Name.Lexeme, methods, super as CallableClass));

        if (super != null)
        {
            // create a closure for the superclass methods
            Env superEnv = new Env(env);
            superEnv.Define("super", super);
        }
    }

    public override void Resolve(Resolver resolver)
    {
        resolver.Declare(Name);
        resolver.Define(Name);

        if (Super != null)
        {
            Super.Resolve(resolver);
        }

        // here we start a new scope, define 'this' in that scope, resolve all of the methods, and then pop that scope
        resolver.BeginScope();
        resolver.Define(new Token(TokenType.THIS, "this", 0, Name.Line));
        foreach (var method in Methods)
        {
            method.Resolve(resolver);
        }
        resolver.EndScope();
    }
}