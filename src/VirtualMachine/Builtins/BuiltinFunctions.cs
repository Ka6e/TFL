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
        _environment.Print(value);
    }

    public Value ReadI()
    {
        int v = _environment.ReadInt();
        return new Value(v);
    }

    public Value ReadF()
    {
        double v = _environment.ReadFloat();
        return new Value(v);
    }

    public Value ReadS()
    {
        string v = _environment.ReadString();
        return new Value(v);
    }

    public Value Length(Value value)
    {
        if (!value.IsString())
        {
            throw new InvalidOperationException("length expects string");
        }

        return new Value(value.AsString().Length);
    }
}