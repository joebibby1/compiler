

namespace Interpret;

/// <summary>
/// This is a special exception that is thrown when a return statement is executed. It does not indicate an error, but is used as a control flow construct.
/// </summary>
public class ReturnValue(object? value) : System.Exception
{
    public readonly object? Value = value;
}