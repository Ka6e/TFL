using Ast.Expressions;
using Ast.Program;
using Ast.Statements;

using Lexemes;

using Value = Runtime.Value;
using ValueType = Runtime.ValueType;

namespace Parsing;

public class Parser
{
    private readonly TokenStream _tokens;

    public Parser(string code)
    {
        _tokens = new TokenStream(code);
    }

    /// <summary>
    /// program =
    ///     "main",
    ///     block ;
    /// </summary>
    public ProgramNode ParseProgram()
    {
        Match(TokenType.Main);
        return new ProgramNode(ParseBlock());
    }

    /// <summary>
    /// block =
    ///     "{",
    ///     { statement },
    ///     "}" ;
    /// </summary>
    private BlockStatement ParseBlock()
    {
        Match(TokenType.OpenBrace);
        List<Statement> statements = new();

        while (_tokens.Peek().Type != TokenType.CloseBrace)
        {
            statements.Add(ParseStatement());
        }

        Match(TokenType.CloseBrace);

        return new BlockStatement(statements);
    }

    /// <summary>
    /// statement =
    ///     var_decl
    ///     | const_decl
    ///     | assignment
    ///     | print_stmt
    ///     | read_stmt
    ///     | block ;
    /// </summary>
    private Statement ParseStatement()
    {
        Token t = _tokens.Peek();
        return t.Type switch
        {
            TokenType.Var => ParseVarDecl(),
            TokenType.Const => ParseConstDecl(),
            TokenType.Identifier => ParseAssignment(),
            TokenType.Print => ParsePrintStatement(),
            TokenType.Read => ParseReadStatement(),
            _ => ParseBlock(),
        };
    }

    /// <summary>
    /// var_decl =
    ///     "var",
    ///     identifier,
    ///     ":",
    ///     type,
    ///     ["=", expression],
    ///     ";" ;
    /// </summary>
    private VariableDeclarationStatement ParseVarDecl()
    {
        Match(TokenType.Var);
        string name = Match(TokenType.Identifier).Value!.ToString();
        Match(TokenType.Colon);
        ValueType type = ParseType();
        if (_tokens.Peek().Type == TokenType.Assign)
        {
            Match(TokenType.Assign);
            Expression expr = ParseExpression();
            Match(TokenType.Semicolon);
            return new VariableDeclarationStatement(name, type, expr);
        }

        Match(TokenType.Semicolon);
        return new VariableDeclarationStatement(name, type, null);
    }

    /// <summary>
    /// const_decl =
    ///     "const",
    ///     identifier,
    ///     ":",
    ///     type,
    ///     "=",
    ///     expression,
    ///     ";" ;
    /// </summary>
    private ConstDeclarationStatement ParseConstDecl()
    {
        Match(TokenType.Const);
        string name = Match(TokenType.Identifier).Value!.ToString();
        Match(TokenType.Colon);
        ValueType type = ParseType();
        Match(TokenType.Assign);
        Expression expr = ParseExpression();
        Match(TokenType.Semicolon);

        return new ConstDeclarationStatement(name, type, expr);
    }

    /// <summary>
    /// assignment =
    ///     identifier,
    ///     "=",
    ///     expression,
    ///     ";" ;
    /// </summary>
    private AssignmentStatement ParseAssignment()
    {
        string name = Match(TokenType.Identifier).Value!.ToString();
        Match(TokenType.Assign);
        Expression expr = ParseExpression();
        Match(TokenType.Semicolon);

        return new AssignmentStatement(name, expr);
    }

    /// <summary>
    /// read_stmt =
    ///     "read",
    ///     "(",
    ///     identifier,
    ///     ")",
    ///     ";" ;
    /// </summary>
    private ReadStatement ParseReadStatement()
    {
        Match(TokenType.Read);
        Match(TokenType.OpenParenthesis);
        string name = Match(TokenType.Identifier).Value!.ToString();
        Match(TokenType.CloseParenthesis);
        Match(TokenType.Semicolon);

        return new ReadStatement(name);
    }

    /// <summary>
    /// print_stmt =
    ///     "print",
    ///     "(",
    ///     expression,
    ///     ")",
    ///     ";" ;
    /// </summary>
    private PrintStatement ParsePrintStatement()
    {
        Match(TokenType.Print);
        Match(TokenType.OpenParenthesis);
        Expression expression = ParseExpression();
        Match(TokenType.CloseParenthesis);
        Match(TokenType.Semicolon);

        return new PrintStatement(expression);
    }

    /// <summary>
    /// expression = equality ;
    /// </summary>
    private Expression ParseExpression()
    {
        return ParseEquality();
    }

    /// <summary>
    /// equality =
    ///     additive,
    ///     { ( "==" | "!=" ), additive };
    /// </summary>
    private Expression ParseEquality()
    {
        Expression expr = ParseAdditive();
        while (true)
        {
            switch (_tokens.Peek().Type)
            {
                case TokenType.Equal:
                    _tokens.Advance();
                    expr = new BinaryOperationExpression(expr, BinaryOperation.Equal, ParseAdditive());
                    break;
                case TokenType.NotEqual:
                    _tokens.Advance();
                    expr = new BinaryOperationExpression(expr, BinaryOperation.NotEqual, ParseAdditive());
                    break;
                default:
                    return expr;
            }
        }
    }

    /// <summary>
    /// additive =
    ///     multiplicative,
    ///     { ( "+" | "-" ), multiplicative };
    /// </summary>
    private Expression ParseAdditive()
    {
        Expression expr = ParseMultiplicative();
        while (true)
        {
            switch (_tokens.Peek().Type)
            {
                case TokenType.Plus:
                    _tokens.Advance();
                    expr = new BinaryOperationExpression(expr, BinaryOperation.Add, ParseMultiplicative());
                    break;
                case TokenType.Minus:
                    _tokens.Advance();
                    expr = new BinaryOperationExpression(expr, BinaryOperation.Subtract, ParseMultiplicative());
                    break;
                default:
                    return expr;
            }
        }
    }

    /// <summary>
    /// multiplicative =
    ///     unary,
    ///     { ( "*" | "/" | "%" ), unary };
    /// </summary>
    private Expression ParseMultiplicative()
    {
        Expression expr = ParseUnary();
        while (true)
        {
            switch (_tokens.Peek().Type)
            {
                case TokenType.Multiply:
                    _tokens.Advance();
                    expr = new BinaryOperationExpression(expr, BinaryOperation.Multiply, ParseUnary());
                    break;
                case TokenType.Divide:
                    _tokens.Advance();
                    expr = new BinaryOperationExpression(expr, BinaryOperation.Divide, ParseUnary());
                    break;
                case TokenType.Module:
                    _tokens.Advance();
                    expr = new BinaryOperationExpression(expr, BinaryOperation.Module, ParseUnary());
                    break;
                default:
                    return expr;
            }
        }
    }

    /// <summary>
    /// unary =
    ///     "-", unary
    ///     | primary ;
    /// </summary>
    private Expression ParseUnary()
    {
        if (_tokens.Peek().Type == TokenType.Minus)
        {
            Match(TokenType.Minus);
            return new UnaryOperationExpression(UnaryOperation.Minus, ParseUnary());
        }

        return ParsePrimary();
    }

    /// <summary>
    /// primary =
    ///     literal
    ///     | identifier
    ///     | "(",
    ///     expression,
    ///     ")" ;
    /// </summary>
    private Expression ParsePrimary()
    {
        Token t = _tokens.Peek();
        switch (t.Type)
        {
            case TokenType.IntLiteral:
                _tokens.Advance();
                return new LiteralExpression(new Value(t.Value!.ToInt()));
            case TokenType.FloatLiteral:
                _tokens.Advance();
                return new LiteralExpression(new Value(t.Value!.ToFloat()));
            case TokenType.StringLiteral:
                _tokens.Advance();
                return new LiteralExpression(new Value(t.Value!.ToString()));
            case TokenType.Identifier:
                string name = Match(TokenType.Identifier).Value!.ToString();
                return new VariableExpression(name);
            case TokenType.OpenParenthesis:
                _tokens.Advance();
                Expression expression = ParseExpression();
                Match(TokenType.CloseParenthesis);
                return expression;
            default:
                throw new UnexpectedLexemeException(t, expected: [
                        TokenType.IntLiteral,
                        TokenType.FloatLiteral,
                        TokenType.StringLiteral,
                        TokenType.Identifier,
                        TokenType.OpenParenthesis,
                    ]
                );
        }
    }

    private Token Match(TokenType expected)
    {
        Token t = _tokens.Peek();

        if (t.Type != expected)
        {
            throw new UnexpectedLexemeException(t, expected);
        }

        _tokens.Advance();

        return t;
    }

    private ValueType ParseType()
    {
        Token t = _tokens.Peek();

        ValueType result = t.Type switch
        {
            TokenType.IntegerType => ValueType.Int,
            TokenType.FloatType => ValueType.Float,
            TokenType.StringType => ValueType.String,
            _ => throw new Exception($"Expected types 'int', 'float', 'string', got {t.Type}"),
        };

        _tokens.Advance();

        return result;
    }
}