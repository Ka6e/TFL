using Interpreter;

using Runtime;

using Semantics.Exceptions;

using Tests.TestLibrary.TestDoubles;

using Xunit;

namespace Interpreter.IntegrationTests;

public class ExpressionsTest
{
    [Theory]
    [MemberData(nameof(GetEvaluateExpressionsData))]
    public void Can_evaluate_expressions(string code, int expected)
    {
        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal(expected.ToString(), environment.Output);
    }

    public static TheoryData<string, int> GetEvaluateExpressionsData()
    {
        return new TheoryData<string, int>
        {
            // Арифметика с приоритетом
            { "main { print(1 + 2 * 8 / 4 - 1); }", 4 },
            { "main { print(10 + 20 * 2); }", 50 },
            { "main { print(100 - 30 / 3); }", 90 },

            // Арифметика со скобками
            { "main { print((1 + 2) * (8 / (3 - 1))); }", 12 },
            { "main { print((10 - 5) * (2 + 3)); }", 25 },

            // Левоассоциативность
            { "main { print(10 - 3 - 2); }", 5 },
            { "main { print(10 / 2 / 5); }", 1 },
            { "main { print(10 - 3 + 2); }", 9 },
            { "main { print(10 / 5 * 2); }", 4 },

            // Унарный минус
            { "main { print(-4); }", -4 },
            { "main { print(2 * 2 * (-5)); }", -20 },
            { "main { print(-10 + 5); }", -5 },
            { "main { print(10 + -5); }", 5 },

            // Оператор модуль
            { "main { print(10 % 3); }", 1 },
            { "main { print(17 % 5); }", 2 },
            { "main { print(20 % 4); }", 0 },

            // Сложные выражения
            { "main { print(2 + 3 * 4 - 10 / 2); }", 9 },
            { "main { print((2 + 3) * (4 - 10) / 2); }", -15 },
        };
    }
}