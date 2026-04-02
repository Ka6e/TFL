using Lexemes;

namespace Lexer.UnitTests.LexerTests;

public class OperatorTests
{
    [Theory]
    [MemberData(nameof(GetArithmeticOperators))]
    public void Can_tokenize_arithmetic_operators(string code, List<Token> expected)
    {
        List<Token> actual = LexerTest.Tokenize(code);
        Assert.Equal(expected, actual);
    }

    public static TheoryData<string, List<Token>> GetArithmeticOperators()
    {
        return new TheoryData<string, List<Token>>()
        {
            {
                "+ - * / %",
                [
                    new Token(TokenType.Plus),
                    new Token(TokenType.Minus),
                    new Token(TokenType.Multiply),
                    new Token(TokenType.Divide),
                    new Token(TokenType.Module),
                ]
            },
        };
    }

    [Theory]
    [MemberData(nameof(GetComparisonOperators))]
    public void Can_tokenize_comparison_operators(string code, List<Token> expected)
    {
        List<Token> actual = LexerTest.Tokenize(code);
        Assert.Equal(expected, actual);
    }

    public static TheoryData<string, List<Token>> GetComparisonOperators()
    {
        return new TheoryData<string, List<Token>>()
        {
            {
                "== !=",
                [
                    new Token(TokenType.Equal),
                    new Token(TokenType.NotEqual),
                ]
            },
        };
    }

    [Theory]
    [MemberData(nameof(GetAssignOperator))]
    public void Can_tokenize_assign_operator(string code, List<Token> expected)
    {
        List<Token> actual = LexerTest.Tokenize(code);
        Assert.Equal(expected, actual);
    }

    public static TheoryData<string, List<Token>> GetAssignOperator()
    {
        return new TheoryData<string, List<Token>>()
        {
            { "=",  [new Token(TokenType.Assign)] },
            // = должен отличаться от ==
            {
                "= ==",
                [
                    new Token(TokenType.Assign),
                    new Token(TokenType.Equal),
                ]
            },
        };
    }
}
