using Ast.Statement;

namespace Ast;

public interface IAstVisitor
{
    void Visit(AssignmentStatement s);
}