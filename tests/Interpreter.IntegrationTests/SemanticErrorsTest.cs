using Interpreter;

using Runtime;

using Semantics.Exceptions;

using Tests.TestLibrary.TestDoubles;

using Xunit;

namespace Interpreter.IntegrationTests;

public class SemanticErrorsTest
{
    [Fact]
    public void Throws_on_undefined_variable()
    {
        const string code = """
            main {
                print(x);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<UnknownSymbolException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_undefined_variable_in_assignment()
    {
        const string code = """
            main {
                x = 10;
                print(x);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<UnknownSymbolException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_assign_to_const()
    {
        const string code = """
            main {
                const x: int = 10;
                x = 20;
                print(x);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<InvalidAssignmentException>(() => interpreter.Execute(code));
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
        Interpreter interpreter = new(environment);

        Assert.Throws<InvalidAssignmentException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_duplicate_variable_declaration()
    {
        const string code = """
            main {
                var x: int = 10;
                var x: int = 20;
                print(x);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<DuplicateSymbolException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_duplicate_const_declaration()
    {
        const string code = """
            main {
                const x: int = 10;
                const x: int = 20;
                print(x);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<DuplicateSymbolException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_float_assigned_to_int_variable()
    {
        const string code = """
            main {
                var x: int = 1.5;
                print(x);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_string_assigned_to_int_variable()
    {
        const string code = """
            main {
                var x: int = "hello";
                print(x);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_int_assigned_to_float_variable()
    {
        const string code = """
            main {
                var x: float = 1;
                print(x);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_string_assigned_to_float_variable()
    {
        const string code = """
            main {
                var x: float = "hello";
                print(x);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_int_assigned_to_string_variable()
    {
        const string code = """
            main {
                var x: string = 1;
                print(x);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_float_assigned_to_string_variable()
    {
        const string code = """
            main {
                var x: string = 1.5;
                print(x);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_float_reassigned_to_int_variable()
    {
        const string code = """
            main {
                var x: int = 0;
                x = 1.5;
                print(x);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_int_reassigned_to_float_variable()
    {
        const string code = """
            main {
                var x: float = 0.0;
                x = 1;
                print(x);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_int_reassigned_to_string_variable()
    {
        const string code = """
            main {
                var x: string = "";
                x = 1;
                print(x);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_int_plus_float()
    {
        const string code = """
            main {
                print(1 + 1.5);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_string_minus_string()
    {
        const string code = """
            main {
                print("a" - "b");
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_string_multiply_int()
    {
        const string code = """
            main {
                print("hello" * 2);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_float_plus_string()
    {
        const string code = """
            main {
                print(1.5 + "x");
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_unary_minus_on_string()
    {
        const string code = """
            main {
                print(-"hello");
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_length_of_int()
    {
        const string code = """
            main {
                print(length(42));
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_length_of_float()
    {
        const string code = """
            main {
                print(length(1.5));
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
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
    public void Substr_handles_unicode_characters_correctly()
    {
        const string code = """
        main {
            print(substr("Hello, 🚀", 7, 1));
        }
        """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("🚀", environment.Output);
    }
}