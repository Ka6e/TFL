using Ast.Statements;

using Semantics.Exceptions;

namespace Semantics.Passes;

/// <summary>
/// Проход по AST для проверки, что инструкции break и continue используются только внутри тела цикла while.
/// </summary>
/// <exception cref="InvalidBreakContinueException">Бросается при использовании break или continue вне цикла while.</exception>
public class CheckBreakContinuePass : AbstractPass
{
    /// <summary>
    /// Счётчик вложенных циклов while. Если равен 0, значит мы не внутри цикла.
    /// </summary>
    private int _loopDepth;

    public override void Visit(WhileStatement s)
    {
        _loopDepth++;
        base.Visit(s);
        _loopDepth--;
    }

    public override void Visit(BreakStatement s)
    {
        if (_loopDepth == 0)
        {
            throw new InvalidBreakContinueException("break statement is only allowed inside while loop body");
        }
    }

    public override void Visit(ContinueStatement s)
    {
        if (_loopDepth == 0)
        {
            throw new InvalidBreakContinueException("continue statement is only allowed inside while loop body");
        }
    }
}
