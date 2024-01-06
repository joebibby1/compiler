


namespace Interpret;

/// <summary>
/// This is the runtime representation of the class syntax node.
/// </summary>
public class CallableClass(string Name, Dictionary<string, CallableFunc> methods, CallableClass? super) : Callable
{
    public string Name = Name;
    public Dictionary<string, CallableFunc> Methods = methods;
    public CallableClass? Super = super;

    public override string ToString()
    {
        return Name;
    }

    public object Call(List<object> arguments)
    {
        ClassInstance instance = new ClassInstance(this);
        Methods.TryGetValue("init", out CallableFunc? constructor);
        if (constructor != null)
        {
            constructor.Bind(instance).Call(arguments);
        }
        return instance;
    }

    public int Arity()
    {
        Methods.TryGetValue("init", out CallableFunc? constructor);
        if (constructor == null) return 0;
        return constructor.Arity();
    }
}