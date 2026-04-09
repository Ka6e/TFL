using Ast.Program;

using Parsing;

using Runtime;

using Semantics;

using VirtualMachine;
using VirtualMachine.Instructions;

namespace Interpreter;

public class SwistonInterpreter
{
    private readonly IEnvironment _environment;
    private int _exitCode;

    public SwistonInterpreter(IEnvironment environment)
    {
        _environment = environment;
    }

    public int ExitCode => _exitCode;

    public Value Execute(string code)
    {
        Parser parser = new(code);
        ProgramNode program = parser.ParseProgram();

        SemanticsChecker checker = new();
        checker.Check(program);

        VirtualMachineCodegen.VirtualMachineCodegen codegen = new();
        List<Instruction> instructions = codegen.Generate(program);

        VirtualMachine.VirtualMachine vm = new(_environment, instructions);
        Value result = vm.RunProgram();
        _exitCode = vm.ExitCode;

        return result;
    }
}