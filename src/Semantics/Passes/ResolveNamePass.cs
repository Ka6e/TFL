using Semantics.Symbols;

namespace Semantics.Passes;

/// <summary>
/// Проход по AST, устанавливающий соответствие имён и символов (объявлений).
/// </summary>
public sealed class ResolveNamePass : AbstractPass
{
    /// <summary>
    /// В таблицу символов складываются объявления.
    /// </summary>
    private SymbolsTable _symbols;

    public ResolveNamePass(SymbolsTable globalSymbols)
    {
        _symbols = globalSymbols;
    }


}