using CalculatorAPI.Helpers;

namespace CalculatorAPI.Services;

public class MathOperationsService
{
    public double CalculateExpression(string expr)
    {
        var filteredExpression = FilterExpression(expr);
        var expressionParser = new ExpressionParser(filteredExpression);
        return expressionParser.ParseExpression();
    }

    public bool ValidateExpression(string expression, out string message)
    {
        expression = FilterExpression(expression);
        
        var operatorStack = new Stack<char>();
        var lastWasOperator = true;
        var lastWasOpenParenthesis = false;

        if (expression.Contains("/0"))
        {
            message = "Division by zero is deprecated";
            return false;
        }

        foreach (var c in expression)
        {
            if (char.IsWhiteSpace(c)) continue;

            if (c == '(')
            {
                operatorStack.Push(c);
                lastWasOperator = true;
                lastWasOpenParenthesis = true;
            }
            else if (c == ')')
            {
                if (operatorStack.Count == 0 || operatorStack.Peek() != '(')
                {
                    message = "Error: Unmatched closing parenthesis.";
                    return false;
                }

                operatorStack.Pop();
                lastWasOperator = false;
                lastWasOpenParenthesis = false;
            }
            else if (IsOperator(c))
            {
                if (lastWasOperator && !lastWasOpenParenthesis)
                {
                    message = "Error: Two consecutive operators detected.";
                    return false;
                }

                lastWasOperator = true;
                lastWasOpenParenthesis = false;
            }
            else if (char.IsDigit(c) || c == '.')
            {
                lastWasOperator = false;
                lastWasOpenParenthesis = false;
            }
            else
            {
                message = $"Error: Invalid character '{c}' detected.";
                return false;
            }
        }

        if (operatorStack.Count != 0)
        {
            message = "Error: Unmatched opening parenthesis.";
            return false;
        }

        if (lastWasOperator)
        {
            message = "Error: Expression cannot end with an operator.";
            return false;
        }

        message = "Valid expression.";
        return true;
    }

    private static bool IsOperator(char c)
        => "+ - / *".Contains(c);

    private static string FilterExpression(string expression)
        => expression
            .Replace(" ", "")
            .Replace(",", ".")
            .ToLower();
}