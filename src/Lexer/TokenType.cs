namespace Lexemes;

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
    /// Ключевое слово read
    /// </summary>
    Read,

    /// <summary>
    /// Ключевое слово print
    /// </summary>
    Print,

    /// <summary>
    /// Ключевое слово length
    /// </summary>
    Length,

    /// <summary>
    /// Ключевое слово substr
    /// </summary>
    Substr,

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
    /// Вечественный тип
    /// </summary>
    FloatType,

    /// <summary>
    /// Строковый тип
    /// </summary>
    StringType,

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
    Module,

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