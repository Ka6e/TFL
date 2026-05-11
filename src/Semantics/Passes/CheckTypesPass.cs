using Ast.Expressions;
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
    private int _loopDepth = 0;
    private bool _insideFunction = false;
    private ValueType? _currentFunctionReturnType = null;

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

        if (e.Operation == UnaryOperation.Minus)
        {
            if (e.Operand.ResultType != ValueType.Int &&
                e.Operand.ResultType != ValueType.Float)
            {
                throw new TypeErrorException("Unary minus allowed only for int or float");
            }
        }
        else if (e.Operation == UnaryOperation.LogicalNot)
        {
            if (e.Operand.ResultType != ValueType.Bool)
            {
                throw new TypeErrorException("Logical not allowed only for bool");
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
        s.Expression.Accept(this);
        CheckAreSameTypes("while loop condition", s.Expression, ValueType.Bool);

        _loopDepth++;
        s.Block.Accept(this);
        _loopDepth--;
    }

    public override void Visit(BreakStatement s)
    {
        if (_loopDepth == 0)
            throw new BreakContinueOutsideLoopException("break");
    }

    public override void Visit(ContinueStatement s)
    {
        if (_loopDepth == 0)
            throw new BreakContinueOutsideLoopException("continue");
    }

    public override void Visit(ReturnStatement s)
    {
        if (!_insideFunction)
            throw new TypeErrorException("'return' is not allowed outside a function");

        base.Visit(s);

        if (_currentFunctionReturnType == ValueType.Void)
        {
            if (s.Expression != null)
                throw new TypeErrorException("void function cannot return a value");
        }
        else if (_currentFunctionReturnType != null)
        {
            if (s.Expression == null)
                throw new TypeErrorException($"function must return a value of type {_currentFunctionReturnType}");

            CheckAreSameTypes("return value", s.Expression, _currentFunctionReturnType);
        }
    }

    public override void Visit(FunctionDeclarationStatement s)
    {
        bool savedInsideFunction = _insideFunction;
        ValueType? savedReturnType = _currentFunctionReturnType;

        _insideFunction = true;
        _currentFunctionReturnType = s.ReturnType;

        base.Visit(s);

        if (s.ReturnType != ValueType.Void && !BlockAlwaysReturns(s.Body))
        {
            throw new TypeErrorException(
                $"Function '{s.Name}' does not return a value on all execution paths");
        }

        _insideFunction = savedInsideFunction;
        _currentFunctionReturnType = savedReturnType;
    }

    public override void Visit(FunctionCallExpression e)
    {
        base.Visit(e);

        FunctionDeclarationStatement func = (FunctionDeclarationStatement)e.Function;

        if (e.Arguments.Count != func.Parameters.Count)
        {
            throw new TypeErrorException(
                $"Function '{e.Name}' expects {func.Parameters.Count} argument(s), got {e.Arguments.Count}");
        }

        for (int i = 0; i < e.Arguments.Count; i++)
        {
            CheckAreSameTypes($"argument {i + 1} of '{e.Name}'", e.Arguments[i], func.Parameters[i].ResultType);
        }

        if (func.ReturnType == ValueType.Void)
        {
            throw new TypeErrorException(
                $"void function '{e.Name}' cannot be used as an expression");
        }
    }

    public override void Visit(FunctionCallStatement s)
    {
        // Visit the call expression but skip the void-in-expression check.
        FunctionDeclarationStatement callFunc = (FunctionDeclarationStatement)s.Call.Function;

        foreach (Expression arg in s.Call.Arguments)
        {
            arg.Accept(this);
        }

        if (s.Call.Arguments.Count != callFunc.Parameters.Count)
        {
            throw new TypeErrorException(
                $"Function '{s.Call.Name}' expects {callFunc.Parameters.Count} argument(s), got {s.Call.Arguments.Count}");
        }

        for (int i = 0; i < s.Call.Arguments.Count; i++)
        {
            CheckAreSameTypes($"argument {i + 1} of '{s.Call.Name}'", s.Call.Arguments[i], callFunc.Parameters[i].ResultType);
        }
    }

    private static bool StatementAlwaysReturns(Statement s)
    {
        if (s is ReturnStatement)
            return true;

        if (s is IfElseStatement ifElse && ifElse.ElseStatement != null)
        {
            return BlockAlwaysReturns(ifElse.Block)
                && StatementAlwaysReturns(ifElse.ElseStatement);
        }

        if (s is BlockStatement block)
            return BlockAlwaysReturns(block);

        return false;
    }

    private static bool BlockAlwaysReturns(BlockStatement block)
    {
        foreach (Statement s in block.Statements)
        {
            if (StatementAlwaysReturns(s))
                return true;
        }

        return false;
    }

    private static void CheckAreSameTypes(string category, Expression expression, ValueType expectedType)
    {
        if (!ValueTypeUtil.AreCompatibleTypes(expression.ResultType, expectedType))
        {
            throw new TypeErrorException(category, expectedType, expression.ResultType);
        }
    }
}
