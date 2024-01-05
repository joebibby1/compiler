
using Lex;
using Exception;

namespace Interpret;


public class ClassInstance(CallableClass c)
{
    private CallableClass c = c;
    private Dictionary<string, object> fields = new Dictionary<string, object>();

    public override string ToString()
    {
        return c.Name + " instance";
    }

    public object Get(Token name)
    {
        // this method is used to both access properties and methods
        if (fields.ContainsKey(name.Lexeme))
        {
            return fields[name.Lexeme];
        }

        if (c.Methods.ContainsKey(name.Lexeme))
        {
            return c.Methods[name.Lexeme].Bind(this);
        }

        // check for inherited methods
        if (c.Super != null)
        {
            return c.Super.Methods[name.Lexeme].Bind(this);
        }



        throw new RuntimeException(name, $"Undefined property '{name.Lexeme}'");
    }

    public void Set(Token name, object value)
    {
        fields[name.Lexeme] = value;
    }
}