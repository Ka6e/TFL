using Ast.Expressions;
using Ast.Statements;

using Semantics.Exceptions;
using Semantics.Symbols;

namespace Semantics.Passes;

public sealed class ResolveNamePass : AbstractPass
{
    private readonly SymbolsTable _symbols;

    public ResolveNamePass(SymbolsTable globalSymbols)
    {
        _symbols = globalSymbols;
    }

    public override void Visit(VariableDeclarationStatement s)
    {
        _symbols.DeclareVariable(s);
        base.Visit(s);
    }

    public override void Visit(ConstDeclarationStatement s)
    {
        _symbols.DeclareVariable(s);
        base.Visit(s);
    }

    public override void Visit(AssignmentStatement s)
    {
        Statement decl = _symbols.GetVariableDeclaration(s.Name);

        if (decl is ConstDeclarationStatement)
        {
            throw new InvalidAssignmentException($"Cannot assign to constant '{s.Name}'");
        }

        base.Visit(s);
    }

    public override void Visit(ReadStatement s)
    {
        Statement decl = _symbols.GetVariableDeclaration(s.Name);

        if (decl is ConstDeclarationStatement)
        {
            throw new InvalidAssignmentException("Cannot read into const variable");
        }
    }

    public override void Visit(VariableExpression e)
    {
        base.Visit(e);

        e.Variable = _symbols.GetVariableDeclaration(e.Name);
    }
}