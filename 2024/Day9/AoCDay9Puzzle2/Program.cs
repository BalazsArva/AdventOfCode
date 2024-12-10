namespace AoCDay9Puzzle2;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var checksum = await SolveAsync(TransformInputAsync(ReadInputAsync()), false);

        Console.WriteLine(checksum);
    }

    private static async IAsyncEnumerable<char> ReadInputAsync()
    {
        using var input = new StreamReader("Inputs\\input.txt");
        var buff = new char[1024];

        while (!input.EndOfStream)
        {
            var readCount = await input.ReadAsync(buff, 0, 1024);
            for (var i = 0; i < readCount; ++i)
            {
                yield return buff[i];
            }
        }
    }

    private static async IAsyncEnumerable<Block> TransformInputAsync(IAsyncEnumerable<char> input)
    {
        // Starts with true and alternates on each position
        var isFile = true;
        var id = 0;
        await foreach (var ch in input)
        {
            var currentCharAsNumber = ch - '0';
            if (isFile)
            {
                yield return new FileBlock(id, currentCharAsNumber);

                ++id;
                isFile = false;
            }
            else
            {
                yield return new FreeSpaceBlock(currentCharAsNumber);

                isFile = true;
            }
        }
    }

    private static async Task<ulong> SolveAsync(IAsyncEnumerable<Block> input, bool printDebug = false)
    {
        var allBlocks = new List<Block>(20000);

        await foreach (var block in input)
        {
            allBlocks.Add(block);
        }

        if (printDebug)
        {
            PrintDebug(allBlocks);
        }

        for (var i = allBlocks.Count - 1; i >= 0; --i)
        {
            if (allBlocks[i] is not FileBlock fileBlock)
            {
                continue;
            }

            for (var j = 0; j < i; ++j)
            {
                if (allBlocks[j] is FreeSpaceBlock freeSpaceBlock && allBlocks[j].Length >= allBlocks[i].Length)
                {
                    allBlocks[j] = fileBlock;

                    if (freeSpaceBlock.Length > fileBlock.Length)
                    {
                        allBlocks[i] = new FreeSpaceBlock(fileBlock.Length);
                        allBlocks.Insert(j + 1, new FreeSpaceBlock(freeSpaceBlock.Length - fileBlock.Length));
                    }
                    else
                    {
                        allBlocks[i] = freeSpaceBlock;
                    }

                    if (printDebug)
                    {
                        PrintDebug(allBlocks);
                    }
                    break;
                }
            }
        }

        // Compute checksum
        var checksum = 0UL;
        var idx = 0;
        for (var i = 0; i < allBlocks.Count; ++i)
        {
            var block = allBlocks[i];
            if (block is FreeSpaceBlock)
            {
                idx += block.Length;
            }
            else if (block is FileBlock fileBlock)
            {
                for (var j = 0; j < block.Length; ++j)
                {
                    checked
                    {
                        checksum += (ulong)(fileBlock.Id * idx);
                    }
                    ++idx;
                }
            }
        }

        return checksum;
    }

    private static void PrintDebug(List<Block> currentState)
    {
        foreach (var block in currentState)
        {
            if (block is FreeSpaceBlock)
            {
                Console.Write(new string('.', block.Length));
            }
            else if (block is FileBlock fileBlock)
            {
                Console.Write(
                    string.Concat(Enumerable.Repeat(fileBlock.Id.ToString(), block.Length)));
            }
        }
        Console.WriteLine();
    }
}

abstract record Block(int Length);

record FreeSpaceBlock(int Length) : Block(Length);

record FileBlock(int Id, int Length) : Block(Length);