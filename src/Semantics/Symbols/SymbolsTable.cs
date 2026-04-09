using Ast.Statements;

namespace Semantics.Symbols;

public sealed class SymbolsTable
{
    private readonly SymbolsTable? _parent;
    private readonly Dictionary<string, Statement> _variables;
    private readonly Dictionary<string, Statement> _types;

    public SymbolsTable(SymbolsTable? parent)
    {
        _parent = parent;
        _variables = [];
        _types = [];
    }

    public SymbolsTable? Parent => _parent;

    //public void DeclareVariable()
}