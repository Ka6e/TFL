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
        // Pre-register all functions so they are visible everywhere in the program.
        foreach (Statement stmt in p.Block.Statements)
        {
            if (stmt is FunctionDeclarationStatement func)
            {
                if (_functions.ContainsKey(func.Name))
                    throw DuplicateSymbolException.DuplicateVariableOrFunction(func.Name);

                if (func.Name == "length" || func.Name == "substr")
                    throw DuplicateSymbolException.DuplicateVariableOrFunction(func.Name);

                _functions[func.Name] = func;
            }
        }

        base.Visit(p);
    }

    public override void Visit(BlockStatement s)
    {
        // Each block creates a new variable scope.
        SymbolsTable saved = _symbols;
        _symbols = new SymbolsTable(_symbols);

        foreach (Statement stmt in s.Statements)
        {
            stmt.Accept(this);
        }

        _symbols = saved;
    }

    public override void Visit(VariableDeclarationStatement s)
    {
        // Visit initializer before declaring so the variable is not yet in scope.
        s.Value?.Accept(this);
        _symbols.DeclareVariable(s);
    }

    public override void Visit(ConstDeclarationStatement s)
    {
        s.Value.Accept(this);
        _symbols.DeclareVariable(s);
    }

    public override void Visit(AssignmentStatement s)
    {
        AbstractVariableDeclarationStatemnt decl = _symbols.GetVariableDeclaration(s.Name);

        if (decl is ConstDeclarationStatement)
            throw new InvalidAssignmentException($"Cannot assign to constant '{s.Name}'");

        s.Variable = decl;
        s.Expression.Accept(this);
    }

    public override void Visit(ReadStatement s)
    {
        AbstractVariableDeclarationStatemnt decl = _symbols.GetVariableDeclaration(s.Name);

        if (decl is ConstDeclarationStatement)
            throw new InvalidAssignmentException("Cannot read into const variable");

        s.Variable = decl;
    }

    public override void Visit(VariableExpression e)
    {
        e.Variable = _symbols.GetVariableDeclaration(e.Name);
    }

    public override void Visit(FunctionCallExpression e)
    {
        foreach (Expression arg in e.Arguments)
        {
            arg.Accept(this);
        }

        if (!_functions.TryGetValue(e.Name, out FunctionDeclarationStatement? func))
            throw UnknownSymbolException.UndefinedVariableOrFunction(e.Name);

        e.Function = func;
    }

    public override void Visit(FunctionDeclarationStatement s)
    {
        // Function body is isolated: parameters are the only visible names from outside.
        SymbolsTable saved = _symbols;
        _symbols = new SymbolsTable(null);

        foreach (AbstractParametrStatement param in s.Parameters)
        {
            _symbols.DeclareVariable(param);
        }

        s.Body.Accept(this);

        _symbols = saved;
    }

    public override void Visit(FunctionCallStatement s)
    {
        s.Call.Accept(this);
    }
}
