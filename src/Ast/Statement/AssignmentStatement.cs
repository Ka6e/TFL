namespace Ast.Statement;

public class AssignmentStatement : Statement
{
    public override void Accept(IAstVisitor visitor)
    {
        visitor.Visit(this);
    }
}