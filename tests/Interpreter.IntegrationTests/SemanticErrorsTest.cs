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
}