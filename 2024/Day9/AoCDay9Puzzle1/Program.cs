namespace AoCDay9Puzzle1;

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
                for (var i = 0; i < currentCharAsNumber; ++i)
                {
                    yield return new FileBlock(id);
                }
                ++id;
                isFile = false;
            }
            else
            {
                for (var i = 0; i < currentCharAsNumber; ++i)
                {
                    yield return new FreeSpaceBlock();
                }
                isFile = true;
            }
        }
    }

    private static async Task<ulong> SolveAsync(IAsyncEnumerable<Block> input, bool printDebug = false)
    {
        var idx = 0;
        var freeSpaceIndices = new Queue<int>(10000); // queue because we need to find the first in each iteration
        var fileSpaceIndices = new Stack<int>(10000); // stack because we need to find the last in each iteration
        var allBlocks = new List<Block>(20000);

        await foreach (var block in input)
        {
            allBlocks.Add(block);

            if (block is FreeSpaceBlock)
            {
                freeSpaceIndices.Enqueue(idx);
            }
            else if (block is FileBlock)
            {
                fileSpaceIndices.Push(idx);
            }
            ++idx;
        }

        if (printDebug)
        {
            PrintDebug(allBlocks);
        }

        while (freeSpaceIndices.Count > 0 && fileSpaceIndices.Count > 0 && freeSpaceIndices.Peek() < fileSpaceIndices.Peek())
        {
            var firstFreeBlockIdx = freeSpaceIndices.Dequeue();
            var lastFileBlockIdx = fileSpaceIndices.Pop();

            var firstFreeBlock = allBlocks[firstFreeBlockIdx];
            var lastFileBlock = allBlocks[lastFileBlockIdx];

            allBlocks[firstFreeBlockIdx] = lastFileBlock;
            allBlocks[lastFileBlockIdx] = firstFreeBlock;

            if (printDebug)
            {
                PrintDebug(allBlocks);
            }
        }

        // Compute checksum
        var checksum = 0UL;
        for (var i = 0; i < allBlocks.Count && allBlocks[i] is FileBlock fileBlock; ++i)
        {
            checked
            {
                checksum += (ulong)(i * fileBlock.Id);
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
                Console.Write('.');
            }
            else if (block is FileBlock fileBlock)
            {
                Console.Write(fileBlock.Id.ToString());
            }
        }
        Console.WriteLine();
    }
}

abstract record Block();

record FreeSpaceBlock() : Block;

record FileBlock(int Id) : Block;