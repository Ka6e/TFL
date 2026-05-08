using Tests.TestLibrary.TestDoubles;

using VirtualMachine.Builtins;
using VirtualMachine.Instructions;

namespace VirtualMachine.UnitTests;

public class CallReturnTest
{
    [Fact]
    public void Can_call_function_and_return()
    {
        FakeEnvironment env = new FakeEnvironment();

        // [0] Jump(3)  — пропустить тело функции
        // [1] Push 99  — тело функции
        // [2] Return
        // [3] Call(1)  — вызвать функцию
        // [4] CallBuiltin(Print)
        // [5] Push 0
        // [6] Halt
        List<Instruction> program =
        [
            new Instruction(InstructionCode.Jump, 3),
            new Instruction(InstructionCode.Push, 99),
            new Instruction(InstructionCode.Return),
            new Instruction(InstructionCode.Call, 1),
            new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
            new Instruction(InstructionCode.Push, 0),
            new Instruction(InstructionCode.Halt),
        ];

        VirtualMachine vm = new VirtualMachine(env, program);
        vm.RunProgram();

        Assert.Equal("99", env.Output);
    }

    [Fact]
    public void Can_call_two_functions_sequentially()
    {
        FakeEnvironment env = new FakeEnvironment();

        // [0]  Jump(5)   — пропустить тела функций
        // [1]  Push 10   — функция A
        // [2]  Return
        // [3]  Push 20   — функция B
        // [4]  Return
        // [5]  Call(1)   — main: вызвать A
        // [6]  Call(3)   — main: вызвать B
        // [7]  Add       — 10 + 20 = 30
        // [8]  CallBuiltin(Print)
        // [9]  Push 0
        // [10] Halt
        List<Instruction> program =
        [
            new Instruction(InstructionCode.Jump, 5),
            new Instruction(InstructionCode.Push, 10),
            new Instruction(InstructionCode.Return),
            new Instruction(InstructionCode.Push, 20),
            new Instruction(InstructionCode.Return),
            new Instruction(InstructionCode.Call, 1),
            new Instruction(InstructionCode.Call, 3),
            new Instruction(InstructionCode.Add),
            new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
            new Instruction(InstructionCode.Push, 0),
            new Instruction(InstructionCode.Halt),
        ];

        VirtualMachine vm = new VirtualMachine(env, program);
        vm.RunProgram();

        Assert.Equal("30", env.Output);
    }

    [Fact]
    public void Function_call_preserves_caller_variables()
    {
        FakeEnvironment env = new FakeEnvironment();

        // Переменная x=5 определена в main.
        // Функция определяет свою переменную y=99 и печатает её.
        // После возврата из функции переменная main x=5 должна быть доступна.
        //
        // [0]  Jump(6)
        // [1]  Push 99         — функция
        // [2]  DefineVar("y")
        // [3]  LoadVar("y")
        // [4]  CallBuiltin(Print)   → "99"
        // [5]  Return
        // [6]  Push 5         — main
        // [7]  DefineVar("x")
        // [8]  Call(1)
        // [9]  LoadVar("x")
        // [10] CallBuiltin(Print)   → "5"
        // [11] Push 0
        // [12] Halt
        List<Instruction> program =
        [
            new Instruction(InstructionCode.Jump, 6),
            new Instruction(InstructionCode.Push, 99),
            new Instruction(InstructionCode.DefineVar, "y"),
            new Instruction(InstructionCode.LoadVar, "y"),
            new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
            new Instruction(InstructionCode.Return),
            new Instruction(InstructionCode.Push, 5),
            new Instruction(InstructionCode.DefineVar, "x"),
            new Instruction(InstructionCode.Call, 1),
            new Instruction(InstructionCode.LoadVar, "x"),
            new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
            new Instruction(InstructionCode.Push, 0),
            new Instruction(InstructionCode.Halt),
        ];

        VirtualMachine vm = new VirtualMachine(env, program);
        vm.RunProgram();

        Assert.Equal("995", env.Output);
    }
}
