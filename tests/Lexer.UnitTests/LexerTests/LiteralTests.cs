namespace Lexemes.UnitTests;

public class LiteralTests
{
    [Theory]
    [MemberData(nameof(GetIntLiterals))]
    public void Can_tokenize_int_literals(string code, List<Token> expected)
    {
        List<Token> actual = LexerHelper.Tokenize(code);
        Assert.Equal(expected, actual);
    }

    public static TheoryData<string, List<Token>> GetIntLiterals()
    {
        return new TheoryData<string, List<Token>>()
        {
            { "0",    [new Token(TokenType.IntLiteral, new TokenValue(0))] },
            { "1",    [new Token(TokenType.IntLiteral, new TokenValue(1))] },
            { "42",   [new Token(TokenType.IntLiteral, new TokenValue(42))] },
            { "1000", [new Token(TokenType.IntLiteral, new TokenValue(1000))] },
            {
                "1 2 3",
                [
                    new Token(TokenType.IntLiteral, new TokenValue(1)),
                    new Token(TokenType.IntLiteral, new TokenValue(2)),
                    new Token(TokenType.IntLiteral, new TokenValue(3)),
                ]
            },
        };
    }
}
