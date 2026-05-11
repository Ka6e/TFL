using System.Globalization;

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
        if (value.IsBool())
        {
            if (value.AsBool())
            {
                _environment.Print(new Value(1));
            }
            else
            {
                _environment.Print(new Value(0));
            }
        }
        else
        {
            _environment.Print(value);
        }
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

        return new Value(new StringInfo(value.AsString()).LengthInTextElements);
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

        StringInfo stringInfo = new StringInfo(str);
        int stringLength = stringInfo.LengthInTextElements;

        if (startIndex < 0 || len < 0 || startIndex + len > stringLength)
        {
            throw new InvalidOperationException(
                $"substr: out of range (start={startIndex}, length={len}, string_length={stringLength})");
        }

        if (len == 0)
        {
            return new Value(string.Empty);
        }

        return new Value(stringInfo.SubstringByTextElements(startIndex, len));
    }
}