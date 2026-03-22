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
    /// Ключевое слово
    /// </summary>
    Read,

    /// <summary>
    /// 
    /// </summary>
    Print,

    /// <summary>
    /// Идентификатор 
    /// </summary>
    Identifier,

    /// <summary>
    /// Литерал целого числа
    /// </summary>
    IntLiteral,

    /// <summary>
    /// Литерал вещественного числа
    /// </summary>
    FloatLiteral,

    /// <summary>
    /// Литерал строки
    /// </summary>
    StringLiteral,

    /// <summary>
    /// Целочисленный тип
    /// </summary>
    IntegerType,

    /// <summary>
    /// Вещщественный тип
    /// </summary>
    FloatType,

    /// <summary>
    /// Строковый тип
    /// </summary>
    StringType,

    /// <summary>
    /// Точка с запятой
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
    /// Конец потока токенов
    /// </summary>
    EndOfFile,

    /// <summary>
    /// Недопустимая лексемма
    /// </summary>
    Error,
}