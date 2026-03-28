using Runtime;

namespace VirtualMachine;

internal class VariablesTable
{
    private readonly Dictionary<string, Value> _variables = new();

    /// <summary>
    /// Объявляет переменную с указанным именем и начальным значением.
    /// </summary>
    public void DefineVariable(string name, Value value)
    {
        if (_variables.ContainsKey(name))
        {
            throw new InvalidOperationException($"Variable {name} already defined");
        }

        _variables[name] = value;
    }

    /// <summary>
    /// Присваивает значение переменной с указанным именем.
    /// </summary>
    public void AssignVariable(string name, Value value)
    {
        if (!_variables.ContainsKey(name))
        {
            throw new InvalidOperationException($"Variable {name} not defined");
        }

        _variables[name] = value;
    }

    /// <summary>
    /// Получает значение переменной по имени.
    /// </summary>
    public Value GetVariable(string name)
    {
        if (!_variables.TryGetValue(name, out Value? value))
        {
            throw new InvalidOperationException($"Variable {name} not found");
        }

        return value;
    }
}