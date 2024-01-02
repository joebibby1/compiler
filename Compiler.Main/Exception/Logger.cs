namespace Exception;
using Lex;

public static class Logger
{
    public static void LogException(Token token, string message)
    {
        Console.WriteLine($"Error: {message}, on line {token.Line}");
    }
}