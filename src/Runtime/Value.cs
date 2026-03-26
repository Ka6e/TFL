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
}