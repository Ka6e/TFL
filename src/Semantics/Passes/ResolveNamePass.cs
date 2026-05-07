using Ast.Expressions;
using Ast.Program;
using Ast.Statements;

using Semantics.Exceptions;
using Semantics.Symbols;

namespace Semantics.Passes;

public sealed class ResolveNamePass : AbstractPass
{
    private SymbolsTable _symbols;
    private readonly Dictionary<string, FunctionDeclarationStatement> _functions = new();

    public ResolveNamePass(SymbolsTable globalSymbols)
    {
        _symbols = globalSymbols;
    }

    public override void Visit(ProgramNode p)
    {
        foreach (Statement stmt in p.Block.Statements)
        {
            if (stmt is FunctionDeclarationStatement func)
            {
                _functions[func.Name] = func;
            }
        }

        base.Visit(p);
    }

    public override void Visit(FunctionDeclarationStatement s)
    {
        SymbolsTable outerScope = _symbols;
        _symbols = new SymbolsTable(_symbols);

        foreach (VariableDeclarationStatement param in s.Parameters)
        {
            param.Accept(this);
        }

        s.Body.Accept(this);

        _symbols = outerScope;
    }

    public override void Visit(FunctionCallExpression e)
    {
        base.Visit(e);

        if (!_functions.TryGetValue(e.Name, out FunctionDeclarationStatement? func))
        {
            throw UnknownSymbolException.UndefinedVariableOrFunction(e.Name);
        }

        e.Function = func;
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

        s.Variable = (AbstractVariableDeclarationStatemnt)decl;

        base.Visit(s);
    }

    public override void Visit(ReadStatement s)
    {
        Statement decl = _symbols.GetVariableDeclaration(s.Name);

        if (decl is ConstDeclarationStatement)
        {
            throw new InvalidAssignmentException("Cannot read into const variable");
        }

        s.Variable = (AbstractVariableDeclarationStatemnt)decl;
    }

    public override void Visit(VariableExpression e)
    {
        base.Visit(e);

        e.Variable = _symbols.GetVariableDeclaration(e.Name);
    }
}