


namespace Interpret;

public class CallableClass(string Name) : Callable
{
    public string Name = Name;

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