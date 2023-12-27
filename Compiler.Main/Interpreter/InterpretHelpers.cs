namespace Interpret;

public static class InterpretHelpers
{
    /// <summary>
    /// Check if the given object is truthy. Null and false are falsey, everything else is truthy.
    /// </summary>
    public static bool IsTruthy(object? obj)
    {
        if (obj == null)
        {
            return false;
        }
        else if (obj is bool)
        {
            return (bool)obj;
        }
        else
        {
            return true;
        }
    }
}