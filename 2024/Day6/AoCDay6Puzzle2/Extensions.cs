namespace AoCDay6Puzzle2;

internal static class Extensions
{
    public static Direction RotateRight(this Direction direction)
    {
        return (Direction)((((int)direction) + 1) % 4);
    }
}