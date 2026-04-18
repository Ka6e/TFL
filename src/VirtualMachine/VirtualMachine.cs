using System.Collections;

using Runtime;

using VirtualMachine.Builtins;
using VirtualMachine.Instructions;

namespace VirtualMachine;

public class VirtualMachine
{
    private readonly BuiltinFunctions _builtinFunctions;
    private readonly IReadOnlyList<Instruction> _instructions;

    private int _instructionPointer;
    private int _exitCode;

    private readonly Stack<Value> _evaluationStack;
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

    // правый операнд можно не проверять на тип в if  так как у нас типы операндов всегда совпадают по спеке
    public int RunProgram()
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
                        _evaluationStack.Push(_variables.GetVariable(name));
                        break;
                    }

                case InstructionCode.Add:
                    {
                        Value right = _evaluationStack.Pop();
                        Value left = _evaluationStack.Pop();

                        if (left.IsString())
                        {
                            _evaluationStack.Push(
                                new Value(left.AsString() + right.AsString())
                            );
                        }
                        else if (left.IsFloat())
                        {
                            _evaluationStack.Push(
                                new Value(left.AsFloat() + right.AsFloat())
                            );
                        }
                        else
                        {
                            _evaluationStack.Push(
                                new Value(left.AsInt() + right.AsInt())
                            );
                        }

                        break;
                    }

                case InstructionCode.Subtract:
                    {
                        Value right = _evaluationStack.Pop();
                        Value left = _evaluationStack.Pop();

                        if (left.IsFloat())
                        {
                            _evaluationStack.Push(
                                new Value(left.AsFloat() - right.AsFloat())
                            );
                        }
                        else
                        {
                            _evaluationStack.Push(
                                new Value(left.AsInt() - right.AsInt())
                            );
                        }

                        break;
                    }

                case InstructionCode.Multiply:
                    {
                        Value right = _evaluationStack.Pop();
                        Value left = _evaluationStack.Pop();

                        if (left.IsFloat())
                        {
                            _evaluationStack.Push(
                                new Value(left.AsFloat() * right.AsFloat())
                            );
                        }
                        else
                        {
                            _evaluationStack.Push(
                                new Value(left.AsInt() * right.AsInt())
                            );
                        }

                        break;
                    }

                case InstructionCode.Divide:
                    {
                        Value right = _evaluationStack.Pop();
                        Value left = _evaluationStack.Pop();

                        if (left.IsFloat())
                        {
                            _evaluationStack.Push(
                                new Value(left.AsFloat() / right.AsFloat())
                            );
                        }
                        else
                        {
                            _evaluationStack.Push(
                                new Value(left.AsInt() / right.AsInt())
                            );
                        }

                        break;
                    }

                case InstructionCode.Modulo:
                    {
                        Value right = _evaluationStack.Pop();
                        Value left = _evaluationStack.Pop();

                        if (left.IsFloat())
                        {
                            _evaluationStack.Push(
                                new Value(left.AsFloat() % right.AsFloat())
                            );
                        }
                        else
                        {
                            _evaluationStack.Push(
                                new Value(left.AsInt() % right.AsInt())
                            );
                        }

                        break;
                    }

                case InstructionCode.Negate:
                    {
                        Value value = _evaluationStack.Pop();

                        if (value.IsFloat())
                        {
                            _evaluationStack.Push(new Value(-value.AsFloat()));
                        }
                        else
                        {
                            _evaluationStack.Push(new Value(-value.AsInt()));
                        }

                        break;
                    }

                case InstructionCode.Equal:
                    {
                        Value right = _evaluationStack.Pop();
                        Value left = _evaluationStack.Pop();

                        if (left.IsString())
                        {
                            if (left.AsString() == right.AsString())
                            {
                                _evaluationStack.Push(new Value(1));
                            }
                            else
                            {
                                _evaluationStack.Push(new Value(0));
                            }
                        }
                        else if (left.IsFloat())
                        {
                            if (Math.Abs(left.AsFloat() - right.AsFloat()) < double.Epsilon)
                            {
                                _evaluationStack.Push(new Value(1));
                            }
                            else
                            {
                                _evaluationStack.Push(new Value(0));
                            }
                        }
                        else
                        {
                            if (left.AsInt() == right.AsInt())
                            {
                                _evaluationStack.Push(new Value(1));
                            }
                            else
                            {
                                _evaluationStack.Push(new Value(0));
                            }
                        }

                        break;
                    }

                case InstructionCode.NotEqual:
                    {
                        Value right = _evaluationStack.Pop();
                        Value left = _evaluationStack.Pop();

                        if (left.IsString())
                        {
                            if (left.AsString() != right.AsString())
                            {
                                _evaluationStack.Push(new Value(1));
                            }
                            else
                            {
                                _evaluationStack.Push(new Value(0));
                            }
                        }
                        else if (left.IsFloat())
                        {
                            if (Math.Abs(left.AsFloat() - right.AsFloat()) >= double.Epsilon)
                            {
                                _evaluationStack.Push(new Value(1));
                            }
                            else
                            {
                                _evaluationStack.Push(new Value(0));
                            }
                        }
                        else
                        {
                            if (left.AsInt() != right.AsInt())
                            {
                                _evaluationStack.Push(new Value(1));
                            }
                            else
                            {
                                _evaluationStack.Push(new Value(0));
                            }
                        }

                        break;
                    }

                case InstructionCode.CallBuiltin:
                    {
                        CallBuiltin((BuiltinFunctionCode)instruction.Operand.AsInt());
                        break;
                    }

                case InstructionCode.Halt:
                    {
                        _exitCode = _evaluationStack.Pop().AsInt();
                        return _exitCode;
                    }

                default:
                    throw new NotImplementedException(instruction.Code.ToString());
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

            case BuiltinFunctionCode.ReadF:
                _evaluationStack.Push(_builtinFunctions.ReadF());
                break;

            case BuiltinFunctionCode.ReadS:
                _evaluationStack.Push(_builtinFunctions.ReadS());
                break;

            case BuiltinFunctionCode.Length:
                Value arg = _evaluationStack.Pop();
                _evaluationStack.Push(_builtinFunctions.Length(arg));
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