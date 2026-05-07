using Ast.Expressions;
using Ast.Program;
using Ast.Statements;

using Semantics.Exceptions;
using Semantics.Helpers;

using ValueType = Runtime.ValueType;

namespace Semantics.Passes;

/// <summary>
/// Проход по AST для проверки корректности программы с точки зрения совместимости типов данных.
/// </summary>
/// <exception cref="TypeErrorException">Бросается при несоответствии типов данных в процессе проверки.</exception>
public class CheckTypesPass : AbstractPass
{
    public override void Visit(BinaryOperationExpression e)
    {
        base.Visit(e);

        if (e.ResultType is null)
        {
            throw new TypeErrorException(
                $"Binary operation {e.Operation} is not allowed for types {e.Left.ResultType} and {e.Right.ResultType}"
            );
        }
    }

    public override void Visit(UnaryOperationExpression e)
    {
        base.Visit(e);

        if (e.Operation == UnaryOperation.LogicalNot)
        {
            if (e.Operand.ResultType != ValueType.Bool)
            {
                throw new TypeErrorException("Logical not allowed only for bool");
            }
        }
        else
        {
            if (e.Operand.ResultType != ValueType.Int &&
                e.Operand.ResultType != ValueType.Float)
            {
                throw new TypeErrorException("Unary minus allowed only for int or float");
            }
        }
    }

    public override void Visit(VariableDeclarationStatement s)
    {
        base.Visit(s);

        if (s.Value != null)
        {
            CheckAreSameTypes("variable initialization", s.Value, s.Type);
        }
    }

    public override void Visit(ConstDeclarationStatement s)
    {
        base.Visit(s);
        CheckAreSameTypes("const initialization", s.Value, s.Type);
    }

    public override void Visit(AssignmentStatement s)
    {
        base.Visit(s);
        CheckAreSameTypes("assignment", s.Expression, s.Variable.ResultType);
    }

    public override void Visit(IfElseStatement s)
    {
        base.Visit(s);

        CheckAreSameTypes("if-else condition", s.Condition, ValueType.Bool);
    }

    public override void Visit(WhileStatement s)
    {
        base.Visit(s);

        CheckAreSameTypes("while loop condition", s.Expression, ValueType.Bool);
    }

    private static void CheckAreSameTypes(string category, Expression expression, ValueType expectedType)
    {
        if (!ValueTypeUtil.AreCompatibleTypes(expression.ResultType, expectedType))
        {
            throw new TypeErrorException(category, expectedType, expression.ResultType);
        }
    }
}