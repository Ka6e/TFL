namespace Lexemes.UnitTests;

public class ErrorTests
{
    [Theory]
    [MemberData(nameof(GetUnknownCharacters))]
    public void Returns_error_token_for_unknown_characters(string code, List<Token> expected)
    {
        List<Token> actual = LexerHelper.Tokenize(code);
        Assert.Equal(expected, actual);
    }

    public static TheoryData<string, List<Token>> GetUnknownCharacters()
    {
        return new TheoryData<string, List<Token>>()
        {
            { "@", [new Token(TokenType.Error, new TokenValue("@"))] },
            { "$", [new Token(TokenType.Error, new TokenValue("$"))] },
            { "?", [new Token(TokenType.Error, new TokenValue("?"))] },

            // ошибка среди валидных токенов
            {
                "@ x",
                [
                    new Token(TokenType.Error, new TokenValue("@")),
                    new Token(TokenType.Identifier, new TokenValue("x")),
                ]
            },
        };
    }
}