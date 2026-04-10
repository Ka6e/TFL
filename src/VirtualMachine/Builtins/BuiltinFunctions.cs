using Runtime;

namespace VirtualMachine.Builtins;

public class BuiltinFunctions
{
    private readonly IEnvironment _environment;

    public BuiltinFunctions(IEnvironment environment)
    {
        _environment = environment;
    }

    public void Print(Value value)
    {
        _environment.PrintInt(value.AsInt());
    }

    public Value ReadI()
    {
        int v = _environment.ReadInt();
        return new Value(v);
    }
}