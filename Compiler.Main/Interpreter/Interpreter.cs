using Lex;

namespace Interpret;


class Interpreter
{
    private Env env = new Env();


    public void DefineVar(string lexeme, object value)
    {
        env.Define(lexeme, value);
    }

    public object GetVar(Token token)
    {
        return env.Get(token);
    }
}