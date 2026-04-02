using Lexemes;

namespace Lexer.UnitTests.LexerTests;

public class ProgramStructureTests
{
    [Theory]
    [MemberData(nameof(GetSeparators))]
    public void Can_tokenize_separators(string code, List<Token> expected)
    {
        List<Token> actual = LexerTest.Tokenize(code);
        Assert.Equal(expected, actual);
    }

    public static TheoryData<string, List<Token>> GetSeparators()
    {
        return new TheoryData<string, List<Token>>()
        {
            { ",", [new Token(TokenType.Comma)] },
            { ";", [new Token(TokenType.Semicolon)] },
            { ":", [new Token(TokenType.Colon)] },
            { "(", [new Token(TokenType.OpenParenthesis)] },
            { ")", [new Token(TokenType.CloseParenthesis)] },
            { "{", [new Token(TokenType.OpenBrace)] },
            { "}", [new Token(TokenType.CloseBrace)] },
            {
                "( )",
                [
                    new Token(TokenType.OpenParenthesis),
                    new Token(TokenType.CloseParenthesis),
                ]
            },
            {
                "{ }",
                [
                    new Token(TokenType.OpenBrace),
                    new Token(TokenType.CloseBrace),
                ]
            },
        };
    }

    [Theory]
    [MemberData(nameof(GetVariableDeclarations))]
    public void Can_tokenize_variable_declarations(string code, List<Token> expected)
    {
        List<Token> actual = LexerTest.Tokenize(code);
        Assert.Equal(expected, actual);
    }

    public static TheoryData<string, List<Token>> GetVariableDeclarations()
    {
        return new TheoryData<string, List<Token>>()
        {
            {
                "var x : int = 42;",
                [
                    new Token(TokenType.Var),
                    new Token(TokenType.Identifier, new TokenValue("x")),
                    new Token(TokenType.Colon),
                    new Token(TokenType.IntegerType),
                    new Token(TokenType.Assign),
                    new Token(TokenType.IntLiteral, new TokenValue(42)),
                    new Token(TokenType.Semicolon),
                ]
            },
            {
                "const y : int = 0;",
                [
                    new Token(TokenType.Const),
                    new Token(TokenType.Identifier, new TokenValue("y")),
                    new Token(TokenType.Colon),
                    new Token(TokenType.IntegerType),
                    new Token(TokenType.Assign),
                    new Token(TokenType.IntLiteral, new TokenValue(0)),
                    new Token(TokenType.Semicolon),
                ]
            },
        };
    }

    [Theory]
    [MemberData(nameof(GetMainBlock))]
    public void Can_tokenize_main_block(string code, List<Token> expected)
    {
        List<Token> actual = LexerTest.Tokenize(code);
        Assert.Equal(expected, actual);
    }

    public static TheoryData<string, List<Token>> GetMainBlock()
    {
        return new TheoryData<string, List<Token>>()
        {
            {
                """
                main {
                    var a : int = 10;
                    print(a);
                }
                """,
                [
                    new Token(TokenType.Main),
                    new Token(TokenType.OpenBrace),
                    new Token(TokenType.Var),
                    new Token(TokenType.Identifier, new TokenValue("a")),
                    new Token(TokenType.Colon),
                    new Token(TokenType.IntegerType),
                    new Token(TokenType.Assign),
                    new Token(TokenType.IntLiteral, new TokenValue(10)),
                    new Token(TokenType.Semicolon),
                    new Token(TokenType.Print),
                    new Token(TokenType.OpenParenthesis),
                    new Token(TokenType.Identifier, new TokenValue("a")),
                    new Token(TokenType.CloseParenthesis),
                    new Token(TokenType.Semicolon),
                    new Token(TokenType.CloseBrace),
                ]
            },
        };
    }

    [Fact]
    public void Can_tokenize_read_statement()
    {
        List<Token> actual = LexerTest.Tokenize("read(x);");
        List<Token> expected =
        [
            new Token(TokenType.Read),
            new Token(TokenType.OpenParenthesis),
            new Token(TokenType.Identifier, new TokenValue("x")),
            new Token(TokenType.CloseParenthesis),
            new Token(TokenType.Semicolon),
        ];
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Can_tokenize_arithmetic_expression()
    {
        List<Token> actual = LexerTest.Tokenize("a + b * c - d / e % f");
        List<Token> expected =
        [
            new Token(TokenType.Identifier, new TokenValue("a")),
            new Token(TokenType.Plus),
            new Token(TokenType.Identifier, new TokenValue("b")),
            new Token(TokenType.Multiply),
            new Token(TokenType.Identifier, new TokenValue("c")),
            new Token(TokenType.Minus),
            new Token(TokenType.Identifier, new TokenValue("d")),
            new Token(TokenType.Divide),
            new Token(TokenType.Identifier, new TokenValue("e")),
            new Token(TokenType.Module),
            new Token(TokenType.Identifier, new TokenValue("f")),
        ];
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Can_tokenize_comparison_expression()
    {
        List<Token> actual = LexerTest.Tokenize("a == b != c");
        List<Token> expected =
        [
            new Token(TokenType.Identifier, new TokenValue("a")),
            new Token(TokenType.Equal),
            new Token(TokenType.Identifier, new TokenValue("b")),
            new Token(TokenType.NotEqual),
            new Token(TokenType.Identifier, new TokenValue("c")),
        ];
        Assert.Equal(expected, actual);
    }
}
