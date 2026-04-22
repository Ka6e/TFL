using Ast.Expressions;
using Ast.Statements;

using Semantics.Exceptions;

using ValueType = Runtime.ValueType;

namespace Semantics.Passes;

/// <summary>
/// Проход по AST для вычисления типов данных.
/// </summary>
/// <exception cref="TypeErrorException">Бросается при несоответствии типов данных в процессе вычисления типов.</exception>
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

        e.ResultType = resultType ?? ValueType.Void;
    }

    public override void Visit(UnaryOperationExpression e)
    {
        base.Visit(e);
        e.ResultType = e.Operand.ResultType;
    }

    public override void Visit(LengthExpression e)
    {
        base.Visit(e);

        if (e.Operand.ResultType != ValueType.String)
        {
            throw new TypeErrorException(
                $"length expects string, got {e.Operand.ResultType}"
            );
        }

        e.ResultType = ValueType.Int;
    }

    public override void Visit(VariableExpression e)
    {
        base.Visit(e);
        e.ResultType = e.Variable.ResultType;
    }

    public override void Visit(VariableDeclarationStatement s)
    {
        base.Visit(s);

        if (s.Value != null)
        {
            s.ResultType = s.Value.ResultType;
        }
        else
        {
            s.ResultType = s.Type;
        }
    }

    public override void Visit(ConstDeclarationStatement s)
    {
        base.Visit(s);
        s.ResultType = s.Value.ResultType;
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
        Runtime.ValueType? variableType = null;

        if (s.Variable is VariableDeclarationStatement varDecl)
        {
            variableType = varDecl.Type;
        }
        else if (s.Variable is ConstDeclarationStatement constDecl)
        {
            variableType = constDecl.Type;
        }

        s.ResultType = variableType ?? Runtime.ValueType.Int;
    }

    private static ValueType? GetBinaryOperationResultType(BinaryOperation operaion, ValueType left, ValueType right)
    {
        switch (operaion)
        {
            case BinaryOperation.Add:
                if (left == ValueType.Int && right == ValueType.Int)
                {
                    return ValueType.Int;
                }

                if (left == ValueType.Float && right == ValueType.Float)
                {
                    return ValueType.Float;
                }

                if (left == ValueType.String && right == ValueType.String)
                {
                    return ValueType.String;
                }

                return null;
            case BinaryOperation.Subtract:
            case BinaryOperation.Multiply:
            case BinaryOperation.Divide:
            case BinaryOperation.Module:
                if (left == ValueType.Int && right == ValueType.Int)
                {
                    return ValueType.Int;
                }

                if (left == ValueType.Float && right == ValueType.Float)
                {
                    return ValueType.Float;
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