using Ast.Expressions;

namespace Ast.Statements;

public sealed class AssignmentStatement : Statement
{

    public AssignmentStatement(string name, Expression expression)
    {
        Name = name;
        Expression = expression;
    }

    public string Name { get; }

    public Expression Expression { get; }

    public override void Accept(IAstVisitor visitor)
    {
        visitor.Visit(this);
    }
}