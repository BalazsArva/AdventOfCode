namespace AoCDay7Puzzle1;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var input = await ReadInputAsync();
        var total = 0UL;
        foreach (var equation in input)
        {
            var operatorCombinations = OperatorHelper.CreateAllPossibleOperatorCombinationsNoRecursion(equation.Operands.Count - 1);
            foreach (var operatorCombination in operatorCombinations)
            {
                var isValid = EquationCalculator.IsValidEquation(equation, operatorCombination);
                if (isValid)
                {
                    total += equation.Result;
                    break;
                }
            }
        }

        Console.WriteLine(total);
    }

    private static async Task<IReadOnlyList<Equation>> ReadInputAsync()
    {
        using var streamReader = new StreamReader("Inputs\\input.txt");
        var result = new List<Equation>();

        while (!streamReader.EndOfStream)
        {
            var line = await streamReader.ReadLineAsync();

            if (!string.IsNullOrWhiteSpace(line))
            {
                result.Add(Equation.Parse(line));
            }
        }

        return result;
    }
}