using Ast.Statements;

namespace Ast.Program;

public class ProgramNode : AstNode
{
    public ProgramNode(BlockStatement block)
    {
        Block = block;
    }

    public BlockStatement Block { get; }

    public override void Accept(IAstVisitor visitor)
    {
        visitor.Visit(this);
    }
}