


using System.ComponentModel;

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
        ClassInstance instance = new ClassInstance(this);
        CallableFunc constructor = Methods["init"];
        if (constructor != null)
        {
            constructor.Bind(instance).Call(arguments);
        }
        return instance;
    }

    public int Arity()
    {
        CallableFunc initializer = Methods["init"];
        if (initializer == null) return 0;
        return initializer.Arity();
    }
}