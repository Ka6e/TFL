using Ast.Expressions;
using Ast.Statements;

using Semantics.Exceptions;

using ValueType = Runtime.ValueType;

namespace Semantics.Passes;

public sealed class ResolveTypesPass : AbstractPass
{
    public override void Visit(LiteralExpression e)
    {
        base.Visit(e);
        e.ResultType = e.Value.GetValueType();
    }

    public override void Visit(BinaryOperationExpression e)
    {
        base.Visit(e);

        ValueType? resultType = GetBinaryOperationResultType(e.Operation, e.Left.ResultType, e.Right.ResultType);
        if (resultType is null)
        {
            throw new TypeErrorException(
                $"Binary operation {e.Operation} is not allowed for types {e.Left.ResultType} and {e.Right.ResultType}"
            );
        }

        e.ResultType = resultType;
    }

    public override void Visit(UnaryOperationExpression e)
    {
        base.Visit(e);

        if (e.Operand.ResultType != ValueType.Int)
        {
            throw new TypeErrorException("Unary minus allowed only for int");
        }

        e.ResultType = ValueType.Int;
    }

    public override void Visit(VariableExpression e)
    {
        base.Visit(e);
        e.ResultType = ValueType.Int;
    }

    public override void Visit(VariableDeclarationStatement s)
    {
        base.Visit(s);
        s.ResultType = s.Value.ResultType;
        //if (s.Value != null)
        //{
        //    ValueType valueType = s.Value.ResultType;

        //    if (s.Type != valueType)
        //    {
        //        throw new TypeMismatchException($"You cannot initialize a variable of type {s.Type} with a value of type {valueType}");
        //    }
        //}

        //s.ResultType = s.Type;
    }

    public override void Visit(ConstDeclarationStatement s)
    {
        base.Visit(s);
        s.ResultType = s.Value.ResultType;

        //ValueType valueType = s.Value.ResultType;

        //if (s.Type != valueType)
        //{
        //    throw new TypeMismatchException($"You cannot initialize a variable of type {s.Type} with a value of type {valueType}");
        //}

        //s.ResultType = s.Type;
    }

    public override void Visit(AssignmentStatement s)
    {
        s.Expression.Accept(this);
        s.ResultType = ValueType.Void;
    }

    public override void Visit(PrintStatement s)
    {
        base.Visit(s);
        s.ResultType = ValueType.Void;
    }

    public override void Visit(ReadStatement s)
    {
        s.ResultType = ValueType.Void;
    }

    private static ValueType? GetBinaryOperationResultType(BinaryOperation operaion, ValueType left, ValueType right)
    {
        switch (operaion)
        {
            case BinaryOperation.Add:
            case BinaryOperation.Subtract:
            case BinaryOperation.Multiply:
            case BinaryOperation.Divide:
            case BinaryOperation.Module:
                if (left == ValueType.Int && right == ValueType.Int)
                {
                    return ValueType.Int;
                }

                return null;

            case BinaryOperation.Equal:
            case BinaryOperation.NotEqual:
                if (left == right && left != ValueType.Void)
                {
                    return ValueType.Int;
                }

                return null;
            default:
                throw new InvalidOperationException($"Unknown binary operation {operaion}");
        }
    }
}