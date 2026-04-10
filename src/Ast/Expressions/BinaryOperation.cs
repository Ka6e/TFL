namespace Ast.Expressions;

public enum BinaryOperation
{
    /// <summary>
    /// Сложение чисел.
    /// </summary>
    Add,

    /// <summary>
    /// Вычитание чисел.
    /// </summary>
    Subtract,

    /// <summary>
    /// Умножение чисел.
    /// </summary>
    Multiply,

    /// <summary>
    /// Деление чисел.
    /// </summary>
    Divide,

    /// <summary>
    /// Деление чисел с остатком.
    /// </summary>
    Module,

    /// <summary>
    /// Оператор равенства.
    /// </summary>
    Equal,

    /// <summary>
    /// Оператор неравенства.
    /// </summary>
    NotEqual,
}