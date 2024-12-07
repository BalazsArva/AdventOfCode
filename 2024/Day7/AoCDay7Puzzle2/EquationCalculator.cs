using System.Globalization;

namespace AoCDay7Puzzle2;

internal static class EquationCalculator
{
    public static bool IsValidEquation(Equation equation, string operators)
    {
        if (equation.Operands.Count == 0)
        {
            return false;
        }

        if (equation.Operands.Count == 1)
        {
            return equation.Operands[0] == equation.Result;
        }

        if (equation.Operands.Count != operators.Length + 1)
        {
            return false;
        }

        var tempResult = equation.Operands[0];
        for (var i = 1; i < equation.Operands.Count; i++)
        {
            var op = operators[i - 1];
            if (op == '+')
            {
                tempResult += equation.Operands[i];
            }
            else if (op == '*')
            {
                tempResult *= equation.Operands[i];
            }
            else
            {
                tempResult = ulong.Parse(
                    tempResult.ToString(CultureInfo.InvariantCulture) +
                    equation.Operands[i].ToString(CultureInfo.InvariantCulture));
            }
        }

        return tempResult == equation.Result;
    }
}