namespace Semantics.Exceptions;

#pragma warning disable RCS1194 // Конструкторы исключения не нужны, т.к. это не класс общего назначения.
/// <summary>
/// Исключение, бросаемое при использовании break или continue вне тела цикла while.
/// </summary>
public class InvalidBreakContinueException : Exception
{
    public InvalidBreakContinueException(string message)
        : base(message)
    {
    }
}
#pragma warning restore RCS1194
