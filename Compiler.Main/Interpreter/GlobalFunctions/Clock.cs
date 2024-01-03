namespace Interpret;

public class Clock : Callable
{
    public object Call(List<object> arguments)
    {
        return (double)DateTime.Now.Ticks / TimeSpan.TicksPerSecond;
    }

    public int Arity()
    {
        return 0;
    }

    public override string ToString()
    {
        return "<native fn>";
    }
}