using System.Linq.Expressions;

namespace CleanArchitecture.Application.Common.Utilities;
public static class ExpressionToSqlConverter
{
    public static (string, object) ConvertWithParams<T>(Expression<Func<T, bool>> expression)
    {
        var parameters = new Dictionary<string, object>();
        string sqlWhereClause = VisitExpression(expression.Body, parameters);
        return (sqlWhereClause, parameters);
    }

    private static string VisitExpression(Expression expression, Dictionary<string, object> parameters)
    {
        switch (expression)
        {
            case BinaryExpression binaryExpression:
                return $"{VisitExpression(binaryExpression.Left, parameters)} {GetSqlOperator(binaryExpression.NodeType)} {VisitExpression(binaryExpression.Right, parameters)}";

            case MemberExpression memberExpression:
                string paramName = memberExpression.Member.Name;
                if (!parameters.ContainsKey(paramName))
                {
                    parameters[paramName] = ((ConstantExpression)memberExpression.Expression).Value;
                }
                return paramName; // The member name will be used as a parameter placeholder

            case ConstantExpression constantExpression:
                return constantExpression.Value.ToString();

            default:
                throw new NotImplementedException($"Expression type {expression.GetType()} is not supported.");
        }
    }

    private static string GetSqlOperator(ExpressionType expressionType)
    {
        switch (expressionType)
        {
            case ExpressionType.Equal:
                return "=";
            case ExpressionType.NotEqual:
                return "<>";
            case ExpressionType.GreaterThan:
                return ">";
            case ExpressionType.GreaterThanOrEqual:
                return ">=";
            case ExpressionType.LessThan:
                return "<";
            case ExpressionType.LessThanOrEqual:
                return "<=";
            case ExpressionType.AndAlso:
            case ExpressionType.OrElse:
                return "AND"; // Implement OR if needed
            default:
                throw new NotImplementedException($"Operator {expressionType} is not supported.");
        }
    }
}
