namespace Ast.Statements;

public sealed class ReadStatement : Statement
{
    public ReadStatement(string name)
    {
        Name = name;
    }

    public string Name { get; }

    public override void Accept(IAstVisitor visitor)
    {
        visitor.Visit(this);
    }
}