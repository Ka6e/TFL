namespace Lexer;

public class Lexer
{
    private static readonly Dictionary<string, TokenType> Keywords = new()
    {
        { "main", TokenType.Main },
        { "var", TokenType.Var },
        { "const", TokenType.Const },
        { "read", TokenType.Read },
        { "int", TokenType.IntegerType },
        { "float", TokenType.FloatType },
        { "string", TokenType.StringType },
    };

    private readonly TextScanner _scanner;

    public Lexer(string str)
    {
        _scanner = new TextScanner(str);
    }

    public Token ParseToken()
    {
        SkipWhiteSpace();

        if (_scanner.IsEnd())
        {
            return new Token(TokenType.EndOfFile);
        }

        char c = _scanner.Peek();

        if (char.IsLetter(c) || c == '_')
        {
            return ParseIdentifierOrKeyword();
        }

        if (char.IsAsciiDigit(c))
        {
            return ParseNumericLiteral();
        }

        if (c == '"')
        {
            return ParseStringLiteral();
        }

        switch (c)
        {
            case '{':
                _scanner.Advance();
                return new Token(TokenType.OpenBrace);
            case '}':
                _scanner.Advance();
                return new Token(TokenType.CloseBrace);
            case '(':
                _scanner.Advance();
                return new Token(TokenType.OpenParenthesis);
            case ')':
                _scanner.Advance();
                return new Token(TokenType.CloseParenthesis);
        }

        _scanner.Advance();
        return new Token(TokenType.Error, new TokenValue(c.ToString()));
    }

    private Token ParseIdentifierOrKeyword()
    {
        string value = _scanner.Peek().ToString();
        _scanner.Advance();

        for (char c = _scanner.Peek(); char.IsLetter(c) || c == '_' || char.IsAsciiDigit(c); c = _scanner.Peek())
        {
            value += c;
            _scanner.Advance();
        }

        if (Keywords.TryGetValue(value, out TokenType type))
        {
            return new Token(type);
        }

        return new Token(TokenType.Identifier, new TokenValue(value));
    }

    private Token ParseNumericLiteral()
    {
        decimal value = GetDigitValue(_scanner.Peek());
        _scanner.Advance();

        for (char c = _scanner.Peek(); char.IsAsciiDigit(c); c = _scanner.Peek())
        {
            value = value * 10 + GetDigitValue(c);
            _scanner.Advance();
        }

        if (_scanner.Peek() == '.')
        {
            _scanner.Advance();
            return ParseFloatLiteral(value);
        }

        return new Token(TokenType.IntLiteral, new TokenValue(value));
    }

    private Token ParseFloatLiteral(decimal intPart)
    {
        decimal value = intPart;
        decimal factor = 0.1m;

        for (char c = _scanner.Peek(); char.IsAsciiDigit(c); c = _scanner.Peek())
        {
            _scanner.Advance();
            value += factor * GetDigitValue(c);
            factor *= 0.1m;
        }

        return new Token(TokenType.FloatLiteral, new TokenValue(value));
    }

    private Token ParseStringLiteral()
    {
        _scanner.Advance();

        string contents = "";

        while (_scanner.Peek() != '"' && !_scanner.IsEnd())
        {
            if (TryParseStringLiteralEscapeSequence(out char unescaped))
            {
                contents += unescaped;
            }
            else
            {
                contents += _scanner.Peek();
                _scanner.Advance();
            }
        }

        if (_scanner.Peek() == '"')
        {
            _scanner.Advance();
            return new Token(TokenType.StringLiteral, new TokenValue(contents));
        }

        return new Token(TokenType.Error, new TokenValue(contents));
    }

    private bool TryParseStringLiteralEscapeSequence(out char unescaped)
    {
        if (_scanner.Peek() != '\\')
        {
            unescaped = '\0';
            return false;
        }

        _scanner.Advance();

        unescaped = _scanner.Peek() switch
        {
            '\\' => '\\',
            '\'' => '\'',
            '\"' => '\"',
            'n' => '\n',
            't' => '\t',
            'r' => '\r',
            _ => '\0',
        };

        if (unescaped != '\0')
        {
            _scanner.Advance();
            return true;
        }

        return false;
    }

    private void SkipWhiteSpacesAndComments()
    {
        do
        {
            SkipWhiteSpace();
        }
        while (TryParseMultiLineComment() || TryParseSingleLineComment());
    }

    private void SkipWhiteSpace()
    {
        while (char.IsWhiteSpace(_scanner.Peek()))
        {
            _scanner.Advance();
        }
    }

    private bool TryParseMultiLineComment()
    {
        if (_scanner.Peek() == '/' && _scanner.Peek(1) == '#')
        {
            do
            {
                _scanner.Advance();
            }
            while (!(_scanner.Peek() == '#' && _scanner.Peek() == '/'));

            _scanner.Advance();
            _scanner.Advance();
            return true;
        }

        return false;
    }

    private bool TryParseSingleLineComment()
    {
        if (_scanner.Peek() == '#')
        {
            do
            {
                _scanner.Advance();
            }
            while (!_scanner.IsEnd() && _scanner.Peek() != '\n' && _scanner.Peek() != '\r');
            return true;
        }

        return false;
    }

    private int GetDigitValue(char c)
    {
        return c - '0';
    }
}