namespace Interpret;


interface Callable
{
    object Call(List<object> arguments);
    int Arity();
    string ToString();
}