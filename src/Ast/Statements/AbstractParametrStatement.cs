namespace Ast.Statements;

/// <summary>
/// Абстрактный класс с информацией о функции пользовательской.
/// </summary>
public abstract class AbstractParametrStatement : AbstractVariableDeclarationStatemnt
{
    protected AbstractParametrStatement(string name)
        : base(name)
    {
    }
}