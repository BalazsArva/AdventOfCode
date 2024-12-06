namespace AoCDay6Puzzle2;

internal class Map
{
    public enum MovementResult
    {
        MovedSuccessfully,
        MovedOffMap,
        EnteredLoop,
    }

    public enum Field
    {
        Empty,
        Guard,
        Obstacle,
    }

    private readonly IReadOnlyCollection<(int Row, int Col)> _obstaclePositions;

    private readonly IList<(int Row, int Col)> _guardPath;
    private readonly ISet<(int Row, int Col)> _guardPathLookup;
    private (int Row, int Col) _guardPosition;
    private Direction _guardDirection;

    public Map(
        int rows,
        int cols,
        IReadOnlyCollection<(int Row, int Col)> obstaclePositions,
        (int Row, int Col) guardPosition)
    {
        Rows = rows;
        Cols = cols;
        _obstaclePositions = obstaclePositions;
        _guardPosition = guardPosition;

        _guardPath = new List<(int Row, int Col)>
        {
            guardPosition,
        };
        _guardPathLookup = _guardPath.ToHashSet();
        _guardDirection = Direction.Up;
    }

    public int Rows { get; }

    public int Cols { get; }

    public IReadOnlyList<(int Row, int Col)> GuardPath => _guardPath.AsReadOnly();

    public MovementResult MoveGuard()
    {
        for (var i = 0; i < 4; ++i)
        {
            var nextPosition = ComputePosition(_guardPosition, _guardDirection);

            if (IsOffMap(nextPosition))
            {
                return MovementResult.MovedOffMap;
            }

            if (EnteredIntoALoop(_guardPosition, nextPosition))
            {
                return MovementResult.EnteredLoop;
            }

            if (_obstaclePositions.Contains(nextPosition))
            {
                _guardDirection = _guardDirection.RotateRight();
            }
            else
            {
                _guardPosition = nextPosition;
                _guardPath.Add(nextPosition);
                _guardPathLookup.Add(nextPosition);
                return MovementResult.MovedSuccessfully;
            }
        }

        throw new Exception("Tried all directions, cannot move.");
    }

    public void Draw(bool drawPath = false)
    {
        for (var row = 0; row < Rows; ++row)
        {
            for (var col = 0; col < Cols; ++col)
            {
                if ((row, col) == _guardPosition)
                {
                    Console.Write('^');
                }
                else if (_obstaclePositions.Contains((row, col)))
                {
                    Console.Write('#');
                }
                else
                {
                    if (!drawPath || !_guardPath.Contains((row, col)))
                    {
                        Console.Write('.');
                    }
                    else
                    {
                        Console.Write('X');
                    }
                }
            }
            Console.WriteLine();
        }
    }

    private bool IsOffMap((int Row, int Col) position)
    {
        return
            position.Row < 0 ||
            position.Row >= Rows ||
            position.Col < 0 ||
            position.Col >= Cols;
    }

    private bool EnteredIntoALoop(
        (int Row, int Col) previousPosition,
        (int Row, int Col) newPosition)
    {
        if (!_guardPathLookup.Contains(previousPosition) ||
            !_guardPathLookup.Contains(newPosition))
        {
            return false;
        }

        for (var i = 0; i < _guardPath.Count - 1; ++i)
        {
            if (_guardPath[i] == previousPosition && _guardPath[i + 1] == newPosition)
            {
                return true;
            }
        }
        return false;
    }

    private static (int Row, int Col) ComputePosition((int Row, int Col) from, Direction direction) => direction switch
    {
        Direction.Up => (from.Row - 1, from.Col),
        Direction.Right => (from.Row, from.Col + 1),
        Direction.Down => (from.Row + 1, from.Col),
        _ => (from.Row, from.Col - 1),
    };
}