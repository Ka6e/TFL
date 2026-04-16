using System.Diagnostics;

using Ast.Statements;

using Semantics.Exceptions;

namespace Semantics.Symbols;

public sealed class SymbolsTable
{
    private readonly Dictionary<string, Statement> _variables;

    public SymbolsTable()
    {
        _variables = [];
    }

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
}