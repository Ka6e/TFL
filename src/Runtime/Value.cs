using System.Globalization;

namespace Runtime;

public class Value
{
    private readonly object _value;

    public Value(int value)
    {
        _value = value;
    }

    /// <summary>
    /// Возвращает тип значения.
    /// </summary>
    public ValueType GetValueType()
    {
        return _value switch
        {
            int => ValueType.Int,
            _ => throw new InvalidOperationException($"Unexpected value {_value} of  type {_value.GetType()}"),
        };
    }

    /// <summary>
    /// Определяет, является ли значение целым числом.
    /// </summary>
    public bool IsInt()
    {
        return _value switch
        {
            int => true,
            _ => false,
        };
    }

    /// <summary>
    /// Возвращает значение как целое число либо бросает исключение.
    /// </summary>
    public int AsInt()
    {
        return _value switch
        {
            int i => i,
            _ => throw new InvalidOperationException($"Value {_value} is not an integer"),
        };
    }

    /// <summary>
    /// Печатает значение для отладки.
    /// </summary>
    public override string ToString()
    {
        return _value switch
        {
            int i => i.ToString(CultureInfo.InvariantCulture),
            _ => throw new InvalidOperationException($"Unexpected value {_value} of type {_value.GetType()}"),
        };
    }

    public bool Equal(Value? other)
    {
        if (other == null)
        {
            return false;
        }

        if (GetValueType() != other.GetValueType())
        {
            return false;
        }

        return _value switch
        {
            int i => other.AsInt() == i,
            _ => throw new NotImplementedException(),
        };
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Value);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }
}