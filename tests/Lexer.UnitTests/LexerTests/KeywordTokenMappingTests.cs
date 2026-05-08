namespace Lexemes.UnitTests;

public class KeywordTokenMappingTests
{
    [Theory]
    [MemberData(nameof(GetControlFlowKeywords))]
    [MemberData(nameof(GetExtendedControlFlowKeywords))]
    [MemberData(nameof(GetTypeKeywords))]
    [MemberData(nameof(GetBuiltinFunctionKeywords))]
    public void Keyword_maps_to_expected_token(string keyword, TokenType expectedType)
    {
        List<Token> tokens = LexerHelper.Tokenize(keyword);

        Assert.Single(tokens);
        Assert.Equal(expectedType, tokens[0].Type);
    }

    [Theory]
    [MemberData(nameof(GetKeywordsWithIdentifierPrefix))]
    public void Keyword_prefix_is_tokenized_as_identifier(string identifier, string expectedValue)
    {
        List<Token> tokens = LexerHelper.Tokenize(identifier);

        Assert.Single(tokens);
        Assert.Equal(TokenType.Identifier, tokens[0].Type);
        Assert.Equal(new TokenValue(expectedValue), tokens[0].Value);
    }

    public static TheoryData<string, TokenType> GetControlFlowKeywords()
    {
        return new TheoryData<string, TokenType>
        {
            { "main",  TokenType.Main },
            { "var",   TokenType.Var },
            { "const", TokenType.Const },
            { "read",  TokenType.Read },
            { "print", TokenType.Print },
        };
    }

    public static TheoryData<string, TokenType> GetExtendedControlFlowKeywords()
    {
        return new TheoryData<string, TokenType>
        {
            { "if",       TokenType.If },
            { "else",     TokenType.Else },
            { "while",    TokenType.While },
            { "break",    TokenType.Break },
            { "continue", TokenType.Continue },
            { "return",   TokenType.Return },
            { "func",     TokenType.Func },
            { "void",     TokenType.Void },
        };
    }

    public static TheoryData<string, TokenType> GetTypeKeywords()
    {
        return new TheoryData<string, TokenType>
        {
            { "int",    TokenType.IntegerType },
            { "float",  TokenType.FloatType },
            { "string", TokenType.StringType },
            { "bool",   TokenType.BooleanType },
        };
    }

    public static TheoryData<string, TokenType> GetBuiltinFunctionKeywords()
    {
        return new TheoryData<string, TokenType>
        {
            { "length", TokenType.Length },
            { "substr", TokenType.Substr },
        };
    }

    public static TheoryData<string, string> GetKeywordsWithIdentifierPrefix()
    {
        return new TheoryData<string, string>
        {
            { "lengths",  "lengths" },
            { "length2",  "length2" },
            { "length_x", "length_x" },
            { "main2",    "main2" },
            { "printer",  "printer" },
            { "reading",  "reading" },
            { "strings",  "strings" },
            { "floater",  "floater" },
            { "integer",  "integer" },
            { "substrings", "substrings" },
            { "substr2",    "substr2" },
            { "substr_x",   "substr_x" },
            { "iff",        "iff" },
            { "elsewhere",  "elsewhere" },
            { "whileTrue",  "whileTrue" },
            { "breaker",    "breaker" },
            { "continues",  "continues" },
            { "returned",   "returned" },
            { "functional", "functional" },
            { "voidable",   "voidable" },
            { "boolean",    "boolean" },
        };
    }
}