using Runtime;

namespace VirtualMachine;

public class VariablesTable
{
    private readonly VariablesTable? _parent;
    private readonly Dictionary<string, Value> _variables = new();

    public VariablesTable(VariablesTable? parent = null)
    {
        _parent = parent;
    }

    public VariablesTable? Parent => _parent;

    /// <summary>
    /// Объявляет переменную с указанным именем и начальным значением.
    /// </summary>
    public void DefineVariable(string name, Value value)
    {
        if (!_variables.TryAdd(name, value))
        {
            throw new InvalidOperationException($"Variable {name} already defined");
        }
    }

    /// <summary>
    /// Присваивает значение переменной с указанным именем.
    /// Ищет переменную в текущей таблице и всех родительских.
    /// </summary>
    public void AssignVariable(string name, Value value)
    {
        if (_variables.ContainsKey(name))
        {
            _variables[name] = value;
            return;
        }

        if (_parent != null)
        {
            _parent.AssignVariable(name, value);
            return;
        }

        throw new InvalidOperationException($"Variable {name} not defined");
    }

    /// <summary>
    /// Получает значение переменной по имени.
    /// Ищет переменную в текущей таблице и всех родительских.
    /// </summary>
    public Value GetVariable(string name)
    {
        if (_variables.TryGetValue(name, out Value? value))
        {
            return value;
        }

        if (_parent != null)
        {
            return _parent.GetVariable(name);
        }

        throw new InvalidOperationException($"Variable {name} not found");
    }
}