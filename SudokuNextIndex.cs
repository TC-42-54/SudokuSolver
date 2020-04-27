using System;

namespace SudokuSolver
{
    class SudokuNextIndex
    {
        public int nextSquareIndex;
        public int nextValueIndex;

        public SudokuNextIndex(int squareIndex, int valueIndex, bool includeWriteLine) {
            nextSquareIndex = squareIndex;
            nextValueIndex = valueIndex;
            if ((nextValueIndex + 1) % SudokuMapBuilder.SQUARE_SIZE_PER_LINE == 0)
            {
                if ((nextValueIndex + 1) == SudokuMapBuilder.COMMON_SIZE)
                {
                    if ((nextSquareIndex + 1) != SudokuMapBuilder.COMMON_SIZE)
                    {
                        if ((nextSquareIndex + 1) % SudokuMapBuilder.SQUARE_SIZE_PER_LINE == 0)
                        {
                            nextSquareIndex++;
                            nextValueIndex = 0;
                            if (includeWriteLine) Console.WriteLine("\n");
                        }
                        else
                        {
                            nextSquareIndex++;
                            nextValueIndex = SudokuMap.GetFirstInLineFromIndex(nextValueIndex);
                        }
                    }
                    else if (includeWriteLine) Console.WriteLine("\n");
                }
                else
                {
                    if ((nextSquareIndex + 1) % SudokuMapBuilder.SQUARE_SIZE_PER_LINE == 0)
                    {
                        nextValueIndex++;
                        nextSquareIndex = SudokuMap.GetFirstInLineFromIndex(nextSquareIndex);
                        if (includeWriteLine) Console.WriteLine("\n");
                    }
                    else
                    {
                        nextSquareIndex++;
                        nextValueIndex = SudokuMap.GetFirstInLineFromIndex(nextValueIndex);
                    }
                }
            }
            else
            {
                nextValueIndex++;
            }
        }
    }
}
