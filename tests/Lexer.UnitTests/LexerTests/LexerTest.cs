namespace Lexer.UnitTests.LexerTests;

public static class LexerTest
{
    public static List<Lexemes.Token> Tokenize(string code)
    {
        List<Lexemes.Token> results = new();
        Lexemes.Lexer lexer = new(code);

        for (Lexemes.Token t = lexer.ParseToken(); t.Type != Lexemes.TokenType.EndOfFile; t = lexer.ParseToken())
        {
            results.Add(t);
        }

        return results;
    }
}
