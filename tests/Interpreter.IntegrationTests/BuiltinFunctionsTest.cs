using Interpreter;

using Runtime;

using Semantics.Exceptions;

using Tests.TestLibrary.TestDoubles;

using Xunit;

namespace Interpreter.IntegrationTests;

public class BuiltinFunctionsTest
{
    [Fact]
    public void Can_print_numbers()
    {
        const string code = """
            main {
                print(42);
                print(-10);
                print(0);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("42-100", environment.Output);
    }

    [Fact]
    public void Can_print_expressions()
    {
        const string code = """
            main {
                print(10 + 20);
                print(100 - 30);
                print(15 * 4);
                print(100 / 20);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("3070605", environment.Output);
    }

    [Fact]
    public void Can_read_numbers()
    {
        const string code = """
            main {
                var x: int = 0;
                read(x);
                print(x);
            }
            """;

        FakeEnvironment environment = new();
        environment.AddInput(42);

        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("42", environment.Output);
    }

    [Fact]
    public void Can_read_multiple_numbers()
    {
        const string code = """
            main {
                var a: int = 0;
                var b: int = 0;
                read(a);
                read(b);
                print(a + b);
                print(a - b);
            }
            """;

        FakeEnvironment environment = new();
        environment.AddInput(100, 30);

        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("13070", environment.Output);
    }

    [Fact]
    public void Can_read_and_process_in_expression()
    {
        const string code = """
            main {
                var x: int = 0;
                var y: int = 0;
                read(x);
                read(y);
                print(x * y + x - y);
            }
            """;

        FakeEnvironment environment = new();
        environment.AddInput(10, 5);

        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        // 10 * 5 + 10 - 5 = 50 + 10 - 5 = 55
        Assert.Equal("55", environment.Output);
    }

    [Fact]
    public void Throws_on_read_to_const()
    {
        const string code = """
            main {
                const x: int = 10;
                read(x);
                print(x);
            }
            """;

        FakeEnvironment environment = new();
        environment.AddInput(42);

        Interpreter interpreter = new(environment);

        Assert.Throws<InvalidAssignmentException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Print_can_output_variables()
    {
        const string code = """
            main {
                var a: int = 5;
                var b: int = 3;
                print(a);
                print(b);
                print(a + b);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("538", environment.Output);
    }

    [Fact]
    public void Can_chain_print_and_read()
    {
        const string code = """
            main {
                var x: int = 0;
                print(1);
                read(x);
                print(x);
                print(2);
            }
            """;

        FakeEnvironment environment = new();
        environment.AddInput(99);

        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("1992", environment.Output);
    }

    [Fact]
    public void Throws_when_no_input_available()
    {
        const string code = """
            main {
                var x: int = 0;
                read(x);
                print(x);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<InvalidOperationException>(() => interpreter.Execute(code));
    }
}