using Runtime;
using VirtualMachine.Builtins;
using VirtualMachine.Instructions;

namespace VirtualMachine;

public class VirtualMachine
{
    private readonly BuiltinFunctions _builtinFunctions;
    private readonly IReadOnlyList<Instruction> _instructions;

    /// <summary>
    /// Указатель на текущую инструкцию.
    /// </summary>
    private int _instructionPointer;

    /// <summary>
    /// Код завершения программы.
    /// </summary>
    private int _exitCode;

    /// <summary>
    /// Стек для вычисления выражений и передачи аргументов функций.
    /// </summary>
    private readonly Stack<Value> _evaluationStack;

    /// <summary>
    /// Таблица переменных.
    /// </summary>
    private readonly VariablesTable _variables;

    public VirtualMachine(IEnvironment environment, IReadOnlyList<Instruction> instructions)
    {
        ValidateInstructions(instructions);

        _builtinFunctions = new BuiltinFunctions(environment);
        _instructions = instructions;
        _instructionPointer = 0;
        _exitCode = 0;
        _evaluationStack = new Stack<Value>();
        _variables = new VariablesTable();
    }

    public int ExitCode => _exitCode;

    public void RunProgram()
    {
        while (true)
        {
            Instruction instruction = _instructions[_instructionPointer++];

            switch (instruction.Code)
            {
                case InstructionCode.Push:
                    _evaluationStack.Push(instruction.Operand);
                    break;

                case InstructionCode.Pop:
                    _evaluationStack.Pop();
                    break;

                case InstructionCode.DefineVar:
                    {
                        Value value = _evaluationStack.Pop();
                        string name = instruction.Operand.ToString();
                        _variables.DefineVariable(name, value);
                        break;
                    }

                case InstructionCode.StoreVar:
                    {
                        Value value = _evaluationStack.Pop();
                        string name = instruction.Operand.ToString();
                        _variables.AssignVariable(name, value);
                        break;
                    }

                case InstructionCode.LoadVar:
                    {
                        string name = instruction.Operand.ToString();
                        Value value = _variables.GetVariable(name);
                        _evaluationStack.Push(value);
                        break;
                    }

                case InstructionCode.Add:
                    {
                        Value r = _evaluationStack.Pop();
                        Value l = _evaluationStack.Pop();
                        _evaluationStack.Push(new Value(l.AsInt() + r.AsInt()));
                        break;
                    }

                case InstructionCode.Subtract:
                    {
                        Value r = _evaluationStack.Pop();
                        Value l = _evaluationStack.Pop();
                        _evaluationStack.Push(new Value(l.AsInt() - r.AsInt()));
                        break;
                    }

                case InstructionCode.Multiply:
                    {
                        Value r = _evaluationStack.Pop();
                        Value l = _evaluationStack.Pop();
                        _evaluationStack.Push(new Value(l.AsInt() * r.AsInt()));
                        break;
                    }

                case InstructionCode.Divide:
                    {
                        Value r = _evaluationStack.Pop();
                        Value l = _evaluationStack.Pop();
                        _evaluationStack.Push(new Value(l.AsInt() / r.AsInt()));
                        break;
                    }

                case InstructionCode.Modulo:
                    {
                        Value r = _evaluationStack.Pop();
                        Value l = _evaluationStack.Pop();
                        _evaluationStack.Push(new Value(l.AsInt() % r.AsInt()));
                        break;
                    }

                case InstructionCode.CallBuiltin:
                    CallBuiltin((BuiltinFunctionCode)instruction.Operand.AsInt());
                    break;

                case InstructionCode.Halt:
                    _exitCode = _evaluationStack.Pop().AsInt();
                    return;

                default:
                    throw new NotImplementedException($"Unknown instruction {instruction.Code}");
            }
        }
    }

    private void CallBuiltin(BuiltinFunctionCode code)
    {
        switch (code)
        {
            case BuiltinFunctionCode.Print:
                _builtinFunctions.Print(_evaluationStack.Pop());
                break;

            case BuiltinFunctionCode.ReadI:
                _evaluationStack.Push(_builtinFunctions.ReadI());
                break;

            default:
                throw new InvalidOperationException($"Unknown builtin {code}");
        }
    }

    private static void ValidateInstructions(IReadOnlyList<Instruction> instructions)
    {
        if (instructions.Count == 0)
        {
            throw new InvalidOperationException("Program is empty");
        }

        if (instructions[^1].Code != InstructionCode.Halt)
        {
            throw new InvalidOperationException("Program must end with Halt");
        }
    }
}