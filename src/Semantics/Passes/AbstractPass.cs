using Ast;
using Ast.Statement;

namespace Semantics.Passes;

public abstract class AbstractPass : IAstVisitor
{
    public void Visit(AssignmentStatement s)
    {
        throw new NotImplementedException();
    }
}