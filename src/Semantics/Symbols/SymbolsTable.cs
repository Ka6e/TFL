using System.Diagnostics;

using Ast.Statements;

using Semantics.Exceptions;

namespace Semantics.Symbols;

public sealed class SymbolsTable
{
    private readonly SymbolsTable? _parent;
    private readonly Dictionary<string, Statement> _variables;

    public SymbolsTable(SymbolsTable? parent)
    {
        _parent = parent;
        _variables = [];
    }

    public SymbolsTable? Parent => _parent;

    public AbstractVariableDeclarationStatemnt GetVariableDeclaration(string name)
    {
        if (_variables.TryGetValue(name, out Statement? declaration))
        {
            return declaration switch
            {
                AbstractVariableDeclarationStatemnt v => v,
                _ => throw new UnreachableException(),
            };
        }

        if (_parent != null)
        {
            return _parent.GetVariableDeclaration(name);
        }

        throw UnknownSymbolException.UndefinedVariableOrFunction(name);
    }

    public void DeclareVariable(Statement symbol)
    {
        string name = symbol switch
        {
            AbstractVariableDeclarationStatemnt v => v.Name,
            _ => throw new UnreachableException(),
        };

        if (!_variables.TryAdd(name, symbol))
        {
            throw DuplicateSymbolException.DuplicateVariableOrFunction(name);
        }
    }
}
