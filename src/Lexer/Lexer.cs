namespace Lexer;

public class Lexer
{
    private static readonly Dictionary<string, TokenType> Keywords = new()
    {
        { "main", TokenType.Main },
        { "var", TokenType.Var },
        { "const", TokenType.Const },
        { "int", TokenType.IntegerType },
    };

    private readonly TextScanner _scanner;

    public Lexer(string str)
    {
        _scanner = new TextScanner(str);
    }

    public Token ParseToken()
    {
        SkipWhiteSpacesAndComments();

        if (_scanner.IsEnd())
        {
            return new Token(TokenType.EndOfFile);
        }

        char c = _scanner.Peek();

        if (char.IsLetter(c) || c == '_')
        {
            return ParseIdentifierOrKeyword();
        }

        if (char.IsDigit(c) || (c == '-' && char.IsDigit(_scanner.Peek(1))))
        {
            return ParseNumericLiteral();
        }

        switch (c)
        {
            case '+':
                _scanner.Advance();
                return new Token(TokenType.Plus);

            case '-':
                _scanner.Advance();
                return new Token(TokenType.Minus);

            case '*':
                _scanner.Advance();
                return new Token(TokenType.Multiply);

            case '/':
                _scanner.Advance();
                return new Token(TokenType.Divide);

            case '%':
                _scanner.Advance();
                return new Token(TokenType.Modulus);

            case '=':
                if (_scanner.Peek(1) == '=')
                {
                    _scanner.Advance();
                    _scanner.Advance();
                    return new Token(TokenType.Equal);
                }

                _scanner.Advance();
                return new Token(TokenType.Assign);

            case '!':
                if (_scanner.Peek(1) == '=')
                {
                    _scanner.Advance();
                    _scanner.Advance();
                    return new Token(TokenType.NotEqual);
                }

                break;

            case ',':
                _scanner.Advance();
                return new Token(TokenType.Comma);

            case ';':
                _scanner.Advance();
                return new Token(TokenType.Semicolon);

            case ':':
                _scanner.Advance();
                return new Token(TokenType.Colon);

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
        string value = "";

        while (char.IsLetterOrDigit(_scanner.Peek()) || _scanner.Peek() == '_')
        {
            value += _scanner.Peek();
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
        bool isNegative = false;

        if (_scanner.Peek() == '-')
        {
            isNegative = true;
            _scanner.Advance();
        }

        decimal value = 0;

        while (char.IsDigit(_scanner.Peek()))
        {
            value = value * 10 + GetDigitValue(_scanner.Peek());
            _scanner.Advance();
        }

        if (isNegative)
        {
            value = -value;
        }

        return new Token(TokenType.IntLiteral, new TokenValue(value));
    }

    private void SkipWhiteSpacesAndComments()
    {
        while (true)
        {
            SkipWhiteSpace();

            if (!TryParseSingleLineComment())
            {
                break;
            }
        }
    }

    private void SkipWhiteSpace()
    {
        while (char.IsWhiteSpace(_scanner.Peek()))
        {
            _scanner.Advance();
        }
    }

    private bool TryParseSingleLineComment()
    {
        if (_scanner.Peek() == '#')
        {
            while (!_scanner.IsEnd() && _scanner.Peek() != '\n')
            {
                _scanner.Advance();
            }

            return true;
        }

        return false;
    }

    private int GetDigitValue(char c)
    {
        return c - '0';
    }
}