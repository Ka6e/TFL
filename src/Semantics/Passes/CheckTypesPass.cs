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

    private static void CheckAreSameTypes(string category, Expression expression, ValueType expectedType)
    {
        if (!ValueTypeUtil.AreCompatibleTypes(expression.ResultType, expectedType))
        {
            throw new TypeErrorException(category, expectedType, expression.ResultType);
        }
    }
}