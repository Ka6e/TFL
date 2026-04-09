using System.ComponentModel;
using System.Globalization;

using VirtualMachine;

namespace Interpreter;

public class ConsoleEnvironment : IEnvironment
{
    public void PrintInt(int value)
    {
        Console.Write(value.ToString(CultureInfo.InvariantCulture));
    }

    public int ReadInt()
    {
        return Console.Read();
    }
}