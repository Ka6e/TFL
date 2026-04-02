using Lexemes;

namespace Lexer.UnitTests.LexerTests;

public class KeywordsTests
{
    [Theory]
    [MemberData(nameof(GetKeywords))]
    public void Can_tokenize_keywords(string code, List<Token> expected)
    {
        List<Token> actual = LexerTest.Tokenize(code);
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
            {
                "read print",
                [
                    new Token(TokenType.Read),
                    new Token(TokenType.Print),
                ]
            },
        };
    }
}
