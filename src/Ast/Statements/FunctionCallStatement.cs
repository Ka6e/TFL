using Ast.Expressions;

namespace Ast.Statements;

public sealed class FunctionCallStatement : Statement
{
    public FunctionCallStatement(FunctionCallExpression call)
    {
        Call = call;
    }

    public FunctionCallExpression Call { get; }

    public override void Accept(IAstVisitor visitor)
    {
        visitor.Visit(this);
    }
}
