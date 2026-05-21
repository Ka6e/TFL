using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.Runtime.InteropServices.JavaScript;

using Ast.Statements;

using Semantics.Exceptions;

namespace Semantics.Symbols;

public sealed class SymbolsTable
{
    private readonly SymbolsTable? _parent;
    private readonly Dictionary<string, Statement> _variables;
    private readonly Dictionary<string, AbstractFunctionDeclarationStatement?> _functions;

    public SymbolsTable(SymbolsTable? parent)
    {
        _parent = parent;
        _variables = [];
    }

    public SymbolsTable? Parent => _parent;

    public AbstractVariableDeclarationStatemnt GetVariableDeclaration(string name)
    {
        if (!_variables.TryGetValue(name, out Statement? declaration))
        {
            throw UnknownSymbolException.UndefinedVariableOrFunction(name);
        }

        return declaration switch
        {
            AbstractVariableDeclarationStatemnt varible => varible,
            _ => throw new UnreachableException(),
        };
    }

    public AbstractFunctionDeclarationStatement GetFunctionDeclaration(string name)
    {
        if (!_functions.TryGetValue(name, out AbstractFunctionDeclarationStatement? declaration))
        {
            throw DuplicateSymbolException.DuplicateVariableOrFunction(name);
        }

        return declaration switch
        {
            AbstractFunctionDeclarationStatement function => function,
            _ => throw new UnreachableException(),
        };
    }

    public void DeclareVariable(Statement symbol)
    {
        string name = symbol switch
        {
            VariableDeclarationStatement v => v.Name,
            ConstDeclarationStatement c => c.Name,
            _ => throw new UnreachableException(),
        };

        if (!_variables.TryAdd(name, symbol))
        {
            throw DuplicateSymbolException.DuplicateVariableOrFunction(name);
        }
    }

    public void DeclareFunction(AbstractFunctionDeclarationStatement symbol)
    {
        if (!_functions.TryAdd(symbol.Name, symbol))
        {
            throw DuplicateSymbolException.DuplicateVariableOrFunction(symbol.Name);
        }
    }
}