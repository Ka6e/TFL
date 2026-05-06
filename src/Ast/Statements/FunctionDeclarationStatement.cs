using ValueType = Runtime.ValueType;

namespace Ast.Statements;

public sealed class FunctionDeclarationStatement : AbstractFunctionDeclarationStatement
{
    public FunctionDeclarationStatement(
        string name,
        IReadOnlyList<AbstractParametrStatement> parameters,
        ValueType returnType,
        BlockStatement body
    )
        : base(name, parameters)
    {
        ReturnType = returnType;
        Body = body;
    }

    public ValueType ReturnType { get; }

    public BlockStatement Body { get; }

    public override void Accept(IAstVisitor visitor)
    {
        visitor.Visit(this);
    }
}