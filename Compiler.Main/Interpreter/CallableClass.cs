


namespace Interpret;

/// <summary>
/// This is the runtime representation of the class syntax node.
/// </summary>
public class CallableClass(string Name, Dictionary<string, CallableFunc> methods) : Callable
{
    public string Name = Name;
    public Dictionary<string, CallableFunc> Methods = methods;

    public override string ToString()
    {
        return Name;
    }

    public object Call(List<object> arguments)
    {
        return new ClassInstance(this);
    }

    public int Arity()
    {
        return 0;
    }
}