namespace Semantics.Exceptions;

#pragma warning disable RCS1194
/// <summary>
/// Исключение при использовании break или continue вне тела цикла while.
/// </summary>
public class BreakContinueOutsideLoopException : Exception
{
    public BreakContinueOutsideLoopException(string statement)
        : base($"'{statement}' is not allowed outside a while loop")
    {
    }
}
#pragma warning restore RCS1194
