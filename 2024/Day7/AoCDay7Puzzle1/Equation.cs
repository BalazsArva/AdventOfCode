namespace AoCDay7Puzzle1;

record Equation(ulong Result, IReadOnlyList<ulong> Operands)
{
    public static Equation Parse(string input)
    {
        var splitResultOnColon = input.Split(':');
        var result = ulong.Parse(splitResultOnColon[0]);

        var operands = splitResultOnColon[1]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(ulong.Parse)
            .ToList();

        return new(result, operands);
    }
}