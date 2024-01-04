
namespace Interpret;

class ClassInstance(CallableClass c)
{
    private CallableClass c = c;

    public override string ToString()
    {
        return c.Name + " instance";
    }
}