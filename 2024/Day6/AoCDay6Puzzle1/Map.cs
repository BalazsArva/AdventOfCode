namespace AoCDay6Puzzle1;

internal class Map
{
    private readonly int _rows;
    private readonly int _cols;
    private readonly IReadOnlyCollection<(int Row, int Col)> _obstaclePositions;

    private readonly IList<(int Row, int Col)> _guardPath;
    private (int Row, int Col) _guardPosition;
    private Direction _guardDirection;

    public Map(
        int rows,
        int cols,
        IReadOnlyCollection<(int Row, int Col)> obstaclePositions,
        (int Row, int Col) guardPosition)
    {
        _rows = rows;
        _cols = cols;
        _obstaclePositions = obstaclePositions;
        _guardPosition = guardPosition;

        _guardPath = new List<(int Row, int Col)>
        {
            guardPosition,
        };
        _guardDirection = Direction.Up;
    }

    public IReadOnlyList<(int Row, int Col)> GuardPath => _guardPath.AsReadOnly();

    public bool MoveGuard()
    {
        for (var i = 0; i < 4; ++i)
        {
            var nextPosition = ComputePosition(_guardPosition, _guardDirection);
            if (IsOffMap(nextPosition))
            {
                return false;
            }

            if (_obstaclePositions.Contains(nextPosition))
            {
                _guardDirection = _guardDirection.RotateRight();
            }
            else
            {
                _guardPosition = nextPosition;
                _guardPath.Add(nextPosition);
                return true;
            }
        }

        throw new Exception("Tried all directions, cannot move.");
    }

    public void Draw(bool drawPath = false)
    {
        for (var row = 0; row < _rows; ++row)
        {
            for (var col = 0; col < _cols; ++col)
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
            position.Row >= _rows ||
            position.Col < 0 ||
            position.Col >= _cols;
    }

    private static (int Row, int Col) ComputePosition((int Row, int Col) from, Direction direction) => direction switch
    {
        Direction.Up => (from.Row - 1, from.Col),
        Direction.Right => (from.Row, from.Col + 1),
        Direction.Down => (from.Row + 1, from.Col),
        _ => (from.Row, from.Col - 1),
    };
}