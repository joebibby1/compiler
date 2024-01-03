using Xunit;
using Parse;
using Lex;
using Interpret;


namespace Compiler.Tests;

public class FunctionTests
{
    private List<Stmt> LexAndParseInput(string input)
    {
        var lexer = new Lexer(input);
        var tokens = lexer.ScanTokens();
        return new Parser(tokens).Parse();
    }

    [Fact]
    public void DeclareFuncWithoutArgs()
    {
        var input = @"
            func test() {
                return 1;
            }
        ";
        var statements = LexAndParseInput(input);
        var env = new Env();

        foreach (var statement in statements)
        {
            statement.Execute(env);
        }

        Assert.IsType<CallableFunc>(env.Get(new Token(TokenType.IDENTIFIER, "test", 1, 1)));
    }

    [Fact]
    public void DeclareFuncWithArgs()
    {
        var input = @"
            func test(a, b) {
                return a + b;
            }
        ";
        var statements = LexAndParseInput(input);
        var env = new Env();

        foreach (var statement in statements)
        {
            statement.Execute(env);
        }

        Assert.IsType<CallableFunc>(env.Get(new Token(TokenType.IDENTIFIER, "test", 1, 1)));
        Assert.Equal(2, ((CallableFunc)env.Get(new Token(TokenType.IDENTIFIER, "test", 1, 1))).Arity());
    }

    [Fact]
    public void CallFuncWithoutArgs()
    {
        var input = @"
            var a = 0;
            func test() {
                a = 4;
            }
            test();
        ";
        var statements = LexAndParseInput(input);
        var env = new Env();

        foreach (var statement in statements)
        {
            statement.Execute(env);
        }

        Assert.Equal(4, env.Get(new Token(TokenType.IDENTIFIER, "a", 1, 1)));
    }

    [Fact]
    public void CallFuncWithArgs()
    {
        var input = @"
            var a = 0;
            func test(x, y) {
                a = x + y;
            }
            test(1, 2);
        ";
        var statements = LexAndParseInput(input);
        var env = new Env();

        foreach (var statement in statements)
        {
            statement.Execute(env);
        }

        Assert.Equal(3, env.Get(new Token(TokenType.IDENTIFIER, "a", 1, 1)));
    }

    [Fact]
    public void ReturnAValue()
    {
        var input = @"
            func test() {
                return 1;
            }
            var a = test();
        ";
        var statements = LexAndParseInput(input);
        var env = new Env();

        foreach (var statement in statements)
        {
            statement.Execute(env);
        }

        Assert.Equal(1, env.Get(new Token(TokenType.IDENTIFIER, "a", 1, 1)));
    }

    [Fact]
    public void ReturnHandsControlBackToParent()
    {
        var input = @"
            var a = 0;  
            func test() {
                a = 5;
                return;
                a = 10;
            }
            test();
        ";
        var statements = LexAndParseInput(input);
        var env = new Env();

        foreach (var statement in statements)
        {
            statement.Execute(env);
        }

        Assert.Equal(5, env.Get(new Token(TokenType.IDENTIFIER, "a", 1, 1)));

    }

    [Fact]
    public void AccessClosureScopedVariables()
    {
        var input = @"
            func test() {
                var a = 1;
                func testTwo() {
                    return a;
                }
                return testTwo;
            }
            var a = test()();
        ";
        var statements = LexAndParseInput(input);
        var env = new Env();

        foreach (var statement in statements)
        {
            statement.Execute(env);
        }

        Assert.Equal(1, env.Get(new Token(TokenType.IDENTIFIER, "a", 1, 1)));
    }
}