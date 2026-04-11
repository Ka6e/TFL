using Ast;
using Ast.Expressions;
using Ast.Program;
using Ast.Statements;

using Runtime;

using VirtualMachine.Builtins;
using VirtualMachine.Instructions;

namespace VirtualMachineCodegen;

public class VirtualMachineCodegen : IAstVisitor
{
    private readonly InstructionsBuilder _builder = new();

    public List<Instruction> Generate(ProgramNode program)
    {
        program.Accept(this);

        _builder.Append(new Instruction(InstructionCode.Push, 0));
        _builder.Append(new Instruction(InstructionCode.Halt));

        return _builder.Finish();
    }

    public void Visit(ProgramNode p)
    {
        p.Block.Accept(this);
    }

    public void Visit(BlockStatement s)
    {
        foreach (Statement stmt in s.Statements)
        {
            stmt.Accept(this);
        }
    }

    public void Visit(VariableExpression e)
    {
        _builder.Append(new Instruction(
            InstructionCode.LoadVar,
            e.Name));
    }

    public void Visit(VariableDeclarationStatement s)
    {
        if (s.Value != null)
        {
            s.Value.Accept(this);
        }
        else
        {
            _builder.Append(new Instruction(InstructionCode.Push, 0));
        }

        _builder.Append(new Instruction(InstructionCode.DefineVar, s.Name));
    }

    public void Visit(ConstDeclarationStatement s)
    {
        s.Value.Accept(this);

        _builder.Append(new Instruction(InstructionCode.DefineVar, s.Name));
    }

    public void Visit(AssignmentStatement s)
    {
        s.Expression.Accept(this);

        _builder.Append(new Instruction(
            InstructionCode.StoreVar,
            s.Name));
    }

    public void Visit(PrintStatement s)
    {
        s.Expression.Accept(this);

        _builder.Append(new Instruction(
            InstructionCode.CallBuiltin,
            (int)BuiltinFunctionCode.Print));
    }

    public void Visit(ReadStatement s)
    {
        _builder.Append(new Instruction(
            InstructionCode.CallBuiltin,
            (int)BuiltinFunctionCode.ReadI));

        _builder.Append(new Instruction(
            InstructionCode.StoreVar,
            s.Name));
    }

    public void Visit(LiteralExpression e)
    {
        _builder.Append(new Instruction(
            InstructionCode.Push,
            e.Value));
    }

    public void Visit(BinaryOperationExpression e)
    {
        e.Left.Accept(this);
        e.Right.Accept(this);

        switch (e.Operation)
        {
            case BinaryOperation.Add:
                _builder.Append(new Instruction(InstructionCode.Add));
                break;

            case BinaryOperation.Subtract:
                _builder.Append(new Instruction(InstructionCode.Subtract));
                break;

            case BinaryOperation.Multiply:
                _builder.Append(new Instruction(InstructionCode.Multiply));
                break;

            case BinaryOperation.Divide:
                _builder.Append(new Instruction(InstructionCode.Divide));
                break;

            case BinaryOperation.Module:
                _builder.Append(new Instruction(InstructionCode.Modulo));
                break;
            case BinaryOperation.Equal:
                _builder.Append(new Instruction(InstructionCode.Equal));
                break;
            case BinaryOperation.NotEqual:
                _builder.Append(new Instruction(InstructionCode.NotEqual));
                break;
            default:
                throw new NotImplementedException($"Unsupported operation {e.Operation}");
        }
    }

    public void Visit(UnaryOperationExpression e)
    {
        e.Operand.Accept(this);

        switch (e.Operation)
        {
            case UnaryOperation.Minus:
                _builder.Append(new Instruction(InstructionCode.Negate));
                break;

            default:
                throw new NotImplementedException("Unsupported unary operation");
        }
    }
}