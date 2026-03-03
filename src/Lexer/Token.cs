namespace Lexer;

public class Token(
        TokenType type,
        TokenValue? value = null
    )
{
    public TokenType Type { get; } = type;

    public TokenValue? Value { get; } = value;

    public override bool Equals(object? obj)
    {
        if (obj is Token other)
        {
            return Type == other.Type && Value == other.Value;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int)Type, Value);
    }

}