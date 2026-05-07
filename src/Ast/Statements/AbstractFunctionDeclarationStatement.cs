namespace Ast.Statements;

public abstract class AbstractFunctionDeclarationStatement : Statement
{
    protected AbstractFunctionDeclarationStatement(
        string name,
        IReadOnlyList<VariableDeclarationStatement> parameters)
    {
        Name = name;
        Parameters = parameters;
    }

    public string Name { get; }

    public IReadOnlyList<VariableDeclarationStatement> Parameters { get; }
}