namespace AoCDay6Puzzle2;

internal class Map
{
    public enum MovementResult
    {
        MovedSuccessfully,
        MovedOffMap,
        EnteredLoop,
    }

    private readonly IReadOnlySet<(int Row, int Col)> _obstaclePositions;
    private readonly HashSet<(int Row, int Col, Direction Direction)> _guardPathLookup;
    private (int Row, int Col, Direction Direction) _guardPosition;

    public Map(
        int rows,
        int cols,
        IReadOnlyCollection<(int Row, int Col)> obstaclePositions,
        (int Row, int Col) guardPosition)
    {
        Rows = rows;
        Cols = cols;
        _obstaclePositions = obstaclePositions.ToHashSet();
        _guardPosition = (guardPosition.Row, guardPosition.Col, Direction.Up);

        _guardPathLookup = new HashSet<(int Row, int Col, Direction Direction)>
        {
            (guardPosition.Row, guardPosition.Col, Direction.Up),
        };
    }

    public int Rows { get; }

    public int Cols { get; }

    public IReadOnlySet<(int Row, int Col, Direction Direction)> GuardPath => _guardPathLookup;

    public MovementResult MoveGuard()
    {
        for (var i = 0; i < 4; ++i)
        {
            var nextPosition = ComputeNextPosition(_guardPosition);

            if (IsOffMap(nextPosition))
            {
                return MovementResult.MovedOffMap;
            }

            if (EnteredIntoALoop(nextPosition))
            {
                return MovementResult.EnteredLoop;
            }

            if (_obstaclePositions.Contains((nextPosition.Row, nextPosition.Col)))
            {
                // This is not a move, just a change of direction. The next iteration will check whether we can move in this direction,
                // so don't add it to the path yet, only on successful move.
                _guardPosition = (_guardPosition.Row, _guardPosition.Col, _guardPosition.Direction.RotateRight());
            }
            else
            {
                _guardPosition = nextPosition;
                _guardPathLookup.Add(nextPosition);
                return MovementResult.MovedSuccessfully;
            }
        }

        throw new Exception("Tried all directions, cannot move.");
    }

    private bool IsOffMap((int Row, int Col, Direction Direction) position)
    {
        return
            position.Row < 0 ||
            position.Row >= Rows ||
            position.Col < 0 ||
            position.Col >= Cols;
    }

    private bool EnteredIntoALoop((int Row, int Col, Direction Direction) newPosition)
    {
        return _guardPathLookup.Contains(newPosition);
    }

    private static (int Row, int Col, Direction Direction) ComputeNextPosition((int Row, int Col, Direction Direction) from)
    {
        return from.Direction switch
        {
            Direction.Up => (from.Row - 1, from.Col, from.Direction),
            Direction.Right => (from.Row, from.Col + 1, from.Direction),
            Direction.Down => (from.Row + 1, from.Col, from.Direction),
            _ => (from.Row, from.Col - 1, from.Direction),
        };
    }
}