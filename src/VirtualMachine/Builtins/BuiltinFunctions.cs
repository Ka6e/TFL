using System.Text;

using Runtime;

namespace VirtualMachine.Builtins;

public class BuiltinFunctions
{
    private readonly IEnvironment _environment;

    public BuiltinFunctions(IEnvironment environment)
    {
        _environment = environment;
    }

    public void Print(Value value)
    {
        _environment.Print(value);
    }

    public Value ReadI()
    {
        int v = _environment.ReadInt();
        return new Value(v);
    }

    public Value ReadF()
    {
        double v = _environment.ReadFloat();
        return new Value(v);
    }

    public Value ReadS()
    {
        string v = _environment.ReadString();
        return new Value(v);
    }

    public Value Length(Value value)
    {
        if (!value.IsString())
        {
            throw new InvalidOperationException("length expects string");
        }

        return new Value(value.AsString().Length);
    }

    public Value Substr(Value source, Value start, Value length)
    {
        if (!source.IsString())
        {
            throw new InvalidOperationException("substr: first argument must be string");
        }

        string str = source.AsString();
        int startIndex = start.AsInt();
        int len = length.AsInt();

        Rune[] runes = str.EnumerateRunes().ToArray();

        if (startIndex < 0 || startIndex >= runes.Length || len <= 0)
        {
            return new Value("");
        }

        int actualLen = Math.Min(len, runes.Length - startIndex);

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < actualLen; i++)
        {
            sb.Append(runes[startIndex + i].ToString());
        }

        return new Value(sb.ToString());
    }
}