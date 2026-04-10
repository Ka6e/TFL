using Runtime;

using Tests.TestLibrary.TestDoubles;

using VirtualMachine.Builtins;
using VirtualMachine.Instructions;

namespace VirtualMachine.UnitTests;

public class CallBuiltinTest
{
    [Theory]
    [MemberData(nameof(GetPrintData))]
    public void Can_call_print(List<Instruction> program, string expectedOutput)
    {
        FakeEnvironment env = new FakeEnvironment();
        VirtualMachine vm = new VirtualMachine(env, program);

        int result = vm.RunProgram();

        Assert.Equal(0, result);
        Assert.Equal(expectedOutput, env.Output);
    }

    [Theory]
    [MemberData(nameof(GetReadData))]
    public void Can_call_readi(List<Instruction> program, int[] input, string expectedOutput)
    {
        FakeEnvironment env = new FakeEnvironment();
        env.AddInput(input);

        VirtualMachine vm = new VirtualMachine(env, program);

        int result = vm.RunProgram();

        Assert.Equal(0, result);
        Assert.Equal(expectedOutput, env.Output);
    }

    [Fact]
    public void ReadI_returns_value_to_exit_code()
    {
        FakeEnvironment env = new FakeEnvironment();
        env.AddInput(777);

        List<Instruction> program = new List<Instruction>
        {
            new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.ReadI),
            new Instruction(InstructionCode.Halt),
        };

        VirtualMachine vm = new VirtualMachine(env, program);

        int result = vm.RunProgram();

        Assert.Equal(777, result);
    }

    public static TheoryData<List<Instruction>, string> GetPrintData()
    {
        return new TheoryData<List<Instruction>, string>
        {
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.Push, 123),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                "123"
            },
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.Push, -456),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                "-456"
            },
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.Push, 10),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),

                    new Instruction(InstructionCode.Push, 20),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),

                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                "1020"
            },
        };
    }

    public static TheoryData<List<Instruction>, int[], string> GetReadData()
    {
        return new TheoryData<List<Instruction>, int[], string>
        {
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.ReadI),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                new[] { 42 },
                "42"
            },
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.ReadI),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),

                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.ReadI),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),

                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                new[] { 10, 20 },
                "1020"
            },
        };
    }
}