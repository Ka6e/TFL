using System.Globalization;

namespace VirtualMachine;

public class ConsoleEnvironment : IEnvironment
{
    public int ReadInt()
    {
        string? input = Console.ReadLine();

        if (int.TryParse(input, NumberStyles.Integer, CultureInfo.InvariantCulture, out int value))
        {
            return value;
        }

        throw new InvalidOperationException("Invalid integer input");
    }

    public void PrintInt(int value)
    {
        Console.Write(value.ToString(CultureInfo.InvariantCulture));
    }
}