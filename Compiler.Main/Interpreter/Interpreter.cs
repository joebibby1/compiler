using Lex;

namespace Interpret;


class Interpreter
{
    private Env env = new Env();


    public void DefineVar(string lexeme, Object value)
    {
        env.Define(lexeme, value);
    }

    public Object GetVar(Token token)
    {
        return env.Get(token);
    }
}