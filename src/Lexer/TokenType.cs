namespace Lexer;

public enum TokenType
{
    /// <summary>
    /// Ключевое слово main
    /// </summary>
    Main,

    /// <summary>
    /// Ключевое слово var
    /// </summary>
    Var,

    /// <summary>
    /// Ключевое слово const
    /// </summary>
    Const,

    /// <summary>
    /// Идентификатор
    /// </summary>
    Identifier,

    /// <summary>
    /// Литерал целого числа
    /// </summary>
    IntLiteral,

    /// <summary>
    /// Целочисленный тип
    /// </summary>
    IntegerType,

    /// <summary>
    /// Запятая
    /// </summary>
    Comma,

    /// <summary>
    /// Точка с запятой
    /// </summary>
    Semicolon,

    /// <summary>
    /// Двоеточие
    /// </summary>
    Colon,

    /// <summary>
    /// Открываюшая круглая скобка
    /// </summary>
    OpenParenthesis,

    /// <summary>
    /// Закрывающая круглая скобка
    /// </summary>
    CloseParenthesis,

    /// <summary>
    /// Открвающая фигурная скобка
    /// </summary>
    OpenBrace,

    /// <summary>
    /// Закрывающая фигурная скобка
    /// </summary>
    CloseBrace,

    /// <summary>
    /// Оператор присваивания =
    /// </summary>
    Assign,

    /// <summary>
    /// Оператор сложения/конкатенации +
    /// </summary>
    Plus,

    /// <summary>
    /// Оператор вычитания -
    /// </summary>
    Minus,

    /// <summary>
    /// Оператор умножения *
    /// </summary>
    Multiply,

    /// <summary>
    /// Оператор деления /
    /// </summary>
    Divide,

    /// <summary>
    /// Оператор остатка от деления %
    /// </summary>
    Modulus,

    /// <summary>
    /// Оператор равенства ==
    /// </summary>
    Equal,

    /// <summary>
    /// Оператор неравенства !=
    /// </summary>
    NotEqual,

    /// <summary>
    /// Конец потока токенов
    /// </summary>
    EndOfFile,

    /// <summary>
    /// Недопустимая лексемма
    /// </summary>
    Error,
}