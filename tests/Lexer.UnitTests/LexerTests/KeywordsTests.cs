namespace Lexer.UnitTests.LexerTests;

public class KeywordsTests
{
    [Theory]
    [MemberData(nameof(GetKeywords))]
    public void Can_tokenize_keywords(string code, List<Token> expected)
    {
        List<Token> actual = LexerHelper.Tokenize(code);
        Assert.Equal(expected, actual);
    }

    public static TheoryData<string, List<Token>> GetKeywords()
    {
        return new TheoryData<string, List<Token>>()
        {
            { "main",  [new Token(TokenType.Main)] },
            { "var",   [new Token(TokenType.Var)] },
            { "const", [new Token(TokenType.Const)] },
            { "int",   [new Token(TokenType.IntegerType)] },
            { "read",  [new Token(TokenType.Read)] },
            { "print", [new Token(TokenType.Print)] },
            {
                "var const int",
                [
                    new Token(TokenType.Var),
                    new Token(TokenType.Const),
                    new Token(TokenType.IntegerType),
                ]
            },
            // слова, начинающиеся как ключевые — должны быть Identifier
            { "mainX",    [new Token(TokenType.Identifier, new TokenValue("mainX"))] },
            { "variable", [new Token(TokenType.Identifier, new TokenValue("variable"))] },
            { "consts",   [new Token(TokenType.Identifier, new TokenValue("consts"))] },
            { "integer",  [new Token(TokenType.Identifier, new TokenValue("integer"))] },
            { "reading",  [new Token(TokenType.Identifier, new TokenValue("reading"))] },
        };
    }
}
