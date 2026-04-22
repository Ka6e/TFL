using Interpreter;

using Semantics.Exceptions;

using Tests.TestLibrary.TestDoubles;

using Xunit;

namespace Interpreter.IntegrationTests;

public class StringFunctionsTest
{
    [Fact]
    public void Substr_returns_middle_of_string()
    {
        const string code = """
            main {
                print(substr("hello", 1, 3));
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("ell", environment.Output);
    }

    [Fact]
    public void Substr_returns_prefix()
    {
        const string code = """
            main {
                print(substr("hello", 0, 3));
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("hel", environment.Output);
    }

    [Fact]
    public void Substr_returns_suffix()
    {
        const string code = """
            main {
                print(substr("hello", 2, 3));
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("llo", environment.Output);
    }

    [Fact]
    public void Substr_returns_empty_string_when_length_is_zero()
    {
        const string code = """
            main {
                print(substr("hello", 2, 0));
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("", environment.Output);
    }

    [Fact]
    public void Substr_works_with_string_variable()
    {
        const string code = """
            main {
                var s: string = "world";
                print(substr(s, 1, 3));
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("orl", environment.Output);
    }

    [Fact]
    public void Substr_works_with_int_variables_for_start_and_length()
    {
        const string code = """
            main {
                var s: string = "abcdef";
                var start: int = 2;
                var len: int = 3;
                print(substr(s, start, len));
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("cde", environment.Output);
    }

    [Fact]
    public void Substr_combined_with_length()
    {
        const string code = """
            main {
                var s: string = "hello";
                print(substr(s, 0, length(s) - 2));
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("hel", environment.Output);
    }

    [Fact]
    public void Substr_result_can_be_assigned_to_variable()
    {
        const string code = """
            main {
                var s: string = "programming";
                var sub: string = substr(s, 0, 7);
                print(sub);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("program", environment.Output);
    }

    [Fact]
    public void Substr_result_can_be_concatenated()
    {
        const string code = """
            main {
                var s: string = "hello world";
                print(substr(s, 0, 5) + "!");
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("hello!", environment.Output);
    }

    [Fact]
    public void Substr_throws_on_non_string_source()
    {
        const string code = """
            main {
                print(substr(42, 0, 1));
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Substr_throws_on_non_int_start()
    {
        const string code = """
            main {
                print(substr("hello", "a", 1));
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Substr_throws_on_non_int_length()
    {
        const string code = """
            main {
                print(substr("hello", 0, "bad"));
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Length_returns_string_length()
    {
        const string code = """
            main {
                print(length("hello"));
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("5", environment.Output);
    }

    [Fact]
    public void Length_of_empty_string_is_zero()
    {
        const string code = """
            main {
                print(length(""));
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("0", environment.Output);
    }

    [Fact]
    public void Length_works_with_string_variable()
    {
        const string code = """
            main {
                var s: string = "abc";
                print(length(s));
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("3", environment.Output);
    }
}
