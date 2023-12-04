using System.Globalization;

namespace CalculatorAPI.Helpers;

public class ExpressionParser
{
    private readonly string _expression;
    private int _index;

    public ExpressionParser(string expression)
    {
        _expression = expression;
    }
    
    public double ParseExpression()
    {
        var result = ParseTerm();

        while (_index < _expression.Length)
        {
            if (_expression[_index] == '+')
            {
                _index++;
                result += ParseTerm();
            }
            else if (_expression[_index] == '-')
            {
                _index++;
                result -= ParseTerm();
            }
            else if (_expression[_index] == '/')
            {
                _index++;
                result /= ParseTerm();
            }
            else if (_expression[_index] == '*')
            {
                _index++;
                result *= ParseTerm();
            }
            else
            {
                break;
            }
        }

        return result;
    }

    private double ParseTerm()
    {
        var result = ParseFactor();

        while (_index < _expression.Length)
        {
            if (_expression[_index] == '+')
            {
                _index++;
                result += ParseFactor();
            }
            else if (_expression[_index] == '-')
            {
                _index++;
                result -= ParseFactor();
            }
            else if (_expression[_index] == '/')
            {
                _index++;
                result /= ParseFactor();
            }
            else if (_expression[_index] == '*')
            {
                _index++;
                result *= ParseFactor();
            }
            else
            {
                break;
            }
        }

        return result;
    }

    private double ParseFactor()
    {
        if (_expression[_index] == '(')
        {
            _index++;
            var result = ParseExpression();
            _index++;
            return result;
        }
        
        return ParseNumber();
    }

    private double ParseNumber()
    {
        var startIndex = _index;

        while (_index < _expression.Length && (char.IsDigit(_expression[_index]) || _expression[_index] == '.'))
        {
            _index++;
        }

        return double.Parse(_expression.AsSpan(startIndex, _index - startIndex), CultureInfo.InvariantCulture);
    }
}