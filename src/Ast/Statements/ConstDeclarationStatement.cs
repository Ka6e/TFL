namespace Ast.Statements;

using Ast.Expressions;

using ValueType = Runtime.ValueType;

public class ConstDeclarationStatement : Statement
{
    public ConstDeclarationStatement(string name, ValueType type, Expression value)
    {
        Name = name;
        Type = type;
        Value = value;
    }

    public string Name { get; }

    public ValueType Type { get; }

    public Expression Value { get; }

    public override void Accept(IAstVisitor visitor)
    {
        visitor.Visit(this);
    }
}