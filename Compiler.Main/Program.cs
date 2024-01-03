using Lex;
using Parse;
using Interpret;


class Program
{
    static void Main(string[] args)
    {
        var input = @"func foo(x, y) {
            print x + y;
        }
        foo(1, 2);";
        var lexer = new Lexer(input);

        var tokens = lexer.ScanTokens();
        var statements = new Parser(tokens).Parse();
        var env = new Env();
        foreach (var statement in statements)
        {
            statement.Execute(env);
        }

    }
}
