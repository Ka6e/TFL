using Interpreter;

using Tests.TestLibrary.TestDoubles;

using Xunit;

namespace Interpreter.IntegrationTests;

public class FunctionsTest
{
    [Fact]
    public void Can_call_void_function()
    {
        const string code = """
            func greet(): void {
                print("hello");
            }

            main {
                greet();
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("hello", environment.Output);
    }

    [Fact]
    public void Can_call_function_with_return_value_from_print()
    {
        const string code = """
            func add(a: int, b: int): int {
                return a + b;
            }

            main {
                print(add(3, 4));
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("7", environment.Output);
    }

    [Fact]
    public void Function_parameters_are_passed_by_value()
    {
        const string code = """
            func addOne(n: int): void {
                n = n + 1;
            }

            main {
                var x: int = 5;
                addOne(x);
                print(x);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("5", environment.Output);
    }

    [Fact]
    public void Can_call_function_multiple_times()
    {
        const string code = """
            func square(n: int): int {
                return n * n;
            }

            main {
                print(square(2));
                print(square(3));
                print(square(4));
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("4916", environment.Output);
    }

    [Fact]
    public void Can_call_iterative_factorial()
    {
        const string code = """
            func factorial(n: int): int {
                var result: int = 1;
                while (n > 1) {
                    result = result * n;
                    n = n - 1;
                }
                return result;
            }

            main {
                print(factorial(5));
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("120", environment.Output);
    }

    [Fact]
    public void Can_use_gcd_function()
    {
        const string code = """
            func gcd(a: int, b: int): int {
                while (b != 0) {
                    var tmp: int = b;
                    b = a % b;
                    a = tmp;
                }
                return a;
            }

            main {
                print(gcd(12, 8));
                print(gcd(100, 75));
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("425", environment.Output);
    }

    [Fact]
    public void Function_with_string_parameter_and_return()
    {
        const string code = """
            func greet(name: string): string {
                return "Hello, " + name;
            }

            main {
                print(greet("World"));
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("Hello, World", environment.Output);
    }

    [Fact]
    public void Function_with_bool_return()
    {
        const string code = """
            func isEven(n: int): bool {
                return n % 2 == 0;
            }

            main {
                print(isEven(4));
                print(isEven(3));
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("10", environment.Output);
    }

    [Fact]
    public void Function_can_call_another_function()
    {
        const string code = """
            func double(n: int): int {
                return n * 2;
            }

            func quadruple(n: int): int {
                var d: int = double(n);
                return double(d);
            }

            main {
                print(quadruple(3));
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("12", environment.Output);
    }

    [Fact]
    public void Function_visible_before_declaration()
    {
        const string code = """
            func b(): void {
                print("b");
            }

            func a(): void {
                print("a");
                b();
            }

            main {
                a();
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("ab", environment.Output);
    }

    [Fact]
    public void Void_function_return_value_is_discarded_when_called_as_statement()
    {
        const string code = """
            func increment(n: int): int {
                return n + 1;
            }

            main {
                increment(5);
                print(1);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("1", environment.Output);
    }

    [Fact]
    public void Function_with_if_else_returns_correct_value()
    {
        const string code = """
            func abs(n: int): int {
                if (n < 0) {
                    return n * -1;
                } else {
                    return n;
                }
            }

            main {
                print(abs(-5));
                print(abs(3));
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("53", environment.Output);
    }
}