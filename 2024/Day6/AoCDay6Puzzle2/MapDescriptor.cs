namespace AoCDay6Puzzle2;

record MapDescriptor(int Rows, int Cols, IReadOnlyList<(int Row, int Col)> ObstaclePositions, (int Row, int Col) GuardPosition);