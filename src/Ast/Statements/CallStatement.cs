using Ast.Expressions;

namespace Ast.Statements;

public sealed class CallStatement : Statement
{
    public CallStatement(FunctionCallExpression call)
    {
        Call = call;
    }

    public FunctionCallExpression Call { get; }

    public override void Accept(IAstVisitor visitor)
    {
        visitor.Visit(this);
    }
}
