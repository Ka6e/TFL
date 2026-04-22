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
            { "main { print(1 + 2 * 8 / 4 - 1); }", 4 },
            { "main { print(10 + 20 * 2); }", 50 },
            { "main { print(100 - 30 / 3); }", 90 },
            { "main { print((1 + 2) * (8 / (3 - 1))); }", 12 },
            { "main { print((10 - 5) * (2 + 3)); }", 25 },
            { "main { print(10 - 3 - 2); }", 5 },
            { "main { print(10 / 2 / 5); }", 1 },
            { "main { print(10 - 3 + 2); }", 9 },
            { "main { print(10 / 5 * 2); }", 4 },
            { "main { print(-4); }", -4 },
            { "main { print(2 * 2 * (-5)); }", -20 },
            { "main { print(-10 + 5); }", -5 },
            { "main { print(10 + -5); }", 5 },
            { "main { print(10 % 3); }", 1 },
            { "main { print(17 % 5); }", 2 },
            { "main { print(20 % 4); }", 0 },
            { "main { print(2 + 3 * 4 - 10 / 2); }", 9 },
            { "main { print((2 + 3) * (4 - 10) / 2); }", -15 },
            { "main { print(1 != 2); }", 1 },
            { "main { print(1 != 1); }", 0 },
            { "main { print(1 == 1); }", 1 },
            { "main { print(1 == 2); }", 0 },
        };
    }

    [Theory]
    [MemberData(nameof(GetEvaluateFloatExpressionsData))]
    public void Can_evaluate_float_expressions(string code, string expected)
    {
        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal(expected, environment.Output);
    }

    public static TheoryData<string, string> GetEvaluateFloatExpressionsData()
    {
        return new TheoryData<string, string>
        {
            { "main { print(1.5 + 2.0); }", "3.5" },
            { "main { print(10.0 - 3.5); }", "6.5" },
            { "main { print(3.0 * 2.5); }", "7.5" },
            { "main { print(10.0 / 4.0); }", "2.5" },
            { "main { print(-1.5); }", "-1.5" },
            { "main { print(3.5 % 2.0); }", "1.5" },
            { "main { print(1.5 + 2.0 * 3.0); }", "7.5" },
            { "main { print((1.5 + 2.5) * 0.5); }", "2" },
            { "main { print(1.5 == 1.5); }", "1" },
            { "main { print(1.5 != 2.5); }", "1" },
            { "main { print(1.5 == 2.5); }", "0" },
            { "main { print(1.5 != 1.5); }", "0" },
        };
    }

    [Theory]
    [MemberData(nameof(GetEvaluateStringExpressionsData))]
    public void Can_evaluate_string_expressions(string code, string expected)
    {
        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal(expected, environment.Output);
    }

    public static TheoryData<string, string> GetEvaluateStringExpressionsData()
    {
        return new TheoryData<string, string>
        {
            { """main { print("hello" + " world"); }""", "hello world" },
            { """main { print("foo" + "bar"); }""", "foobar" },
            { """main { print("a" + "b" + "c"); }""", "abc" },
            { """main { print("hello" == "hello"); }""", "1" },
            { """main { print("hello" == "world"); }""", "0" },
            { """main { print("hello" != "world"); }""", "1" },
            { """main { print("hello" != "hello"); }""", "0" },
        };
    }
}