using Lexemes;

namespace Lexer.UnitTests.LexerTests;

public class CommentTests
{
    [Theory]
    [MemberData(nameof(GetSingleLineCommentData))]
    public void Can_skip_single_line_comments(string code, List<Token> expected)
    {
        List<Token> actual = LexerTest.Tokenize(code);
        Assert.Equal(expected, actual);
    }

    public static TheoryData<string, List<Token>> GetSingleLineCommentData()
    {
        return new TheoryData<string, List<Token>>()
        {
            {
                "# это комментарий",
                []
            },
            {
                """
                # комментарий в начале
                var x
                """,
                [
                    new Token(TokenType.Var),
                    new Token(TokenType.Identifier, new TokenValue("x")),
                ]
            },
            {
                """
                var x # комментарий в конце строки
                var y
                """,
                [
                    new Token(TokenType.Var),
                    new Token(TokenType.Identifier, new TokenValue("x")),
                    new Token(TokenType.Var),
                    new Token(TokenType.Identifier, new TokenValue("y")),
                ]
            },
            {
                """
                # первый комментарий
                # второй комментарий
                main
                """,
                [
                    new Token(TokenType.Main),
                ]
            },
        };
    }
}
