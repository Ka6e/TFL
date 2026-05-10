using ValueType = Runtime.ValueType;

namespace Ast.Statements;

public sealed class ParameterStatement : AbstractParametrStatement
{
    public ParameterStatement(string name, ValueType type) : base(name)
    {
        Type = type;
    }

    public ValueType Type { get; }

    public override void Accept(IAstVisitor visitor)
    {
        // Parameters are resolved directly in Visit(FunctionDeclarationStatement)
    }
}
