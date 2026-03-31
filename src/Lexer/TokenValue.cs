using System.Globalization;

namespace Lexemes;

public class TokenValue
{
    private readonly object _value;

    public TokenValue(string value)
    {
        _value = value;
    }

    public TokenValue(int value)
    {
        _value = value;
    }

    public int ToInt()
    {
        return _value switch
        {
            int i => i,
            string s => int.Parse(s, CultureInfo.InvariantCulture),
            _ => throw new NotImplementedException(),
        };
    }

    public override string ToString()
    {
        return _value switch
        {
            string s => s,
            int i => i.ToString(),
            _ => throw new NotImplementedException(),
        };
    }

    public override bool Equals(object? obj)
    {
        if (obj is not TokenValue other)
        {
            return false;
        }

        return _value.Equals(other._value);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }
}