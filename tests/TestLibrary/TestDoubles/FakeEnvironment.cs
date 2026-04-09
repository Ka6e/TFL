using System.Globalization;
using System.Text;

using VirtualMachine;

namespace Tests.TestLibrary.TestDoubles;

/// <summary>
/// Имитирует средства ввода-вывода для тестов.
/// </summary>
public class FakeEnvironment : IEnvironment
{
    private readonly Queue<int> _input = new();
    private readonly StringBuilder _output = new();

    public string Output => _output.ToString();

    public void AddInput(params int[] values)
    {
        foreach (int val in values)
        {
            _input.Enqueue(val);
        }
    }

    public int ReadInt()
    {
        if (_input.TryDequeue(out int value))
        {
            return value;
        }

        throw new InvalidOperationException("No input available");
    }

    public void PrintInt(int value)
    {
        _output.Append(value.ToString(CultureInfo.InvariantCulture));
    }
}