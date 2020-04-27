using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class SudokuMap
    {
        public List<List<int>> Map = new List<List<int>>(Sudoku.COMMON_SIZE);
        public bool Solved = false;
        public SudokuMap()
        {
            for (int i = 0; i < Sudoku.COMMON_SIZE; i++)
            {
                Map.Add(new List<int>(Sudoku.COMMON_SIZE));
            }
        }

        public SudokuMap(List<List<int>> map)
        {
            Map = map;
        }

        public void FillLineValues(String line, int lineIndex)
        {
            if (String.IsNullOrEmpty(line) || String.IsNullOrWhiteSpace(line))
                throw new ArgumentException($"The argument {nameof(line)} value is not valid.");
            if (lineIndex >= Sudoku.COMMON_SIZE || lineIndex < 0)
                throw new ArgumentOutOfRangeException($"The argument {nameof(lineIndex)} is out of range.");
            String[] splittedValues = line.Split(" ");

            for (int valueIndex = 0; valueIndex < Sudoku.COMMON_SIZE; valueIndex++)
            {
                Map[lineIndex].Add(int.Parse(splittedValues[valueIndex]));
            }
        }

        public bool IsValueInSquare(int lineIndex, int valueIndex, int value)
        {
            bool IsValueAlreadyInSquare = false;
            int lineIndexStart = (lineIndex / Sudoku.SQUARE_SIZE) * Sudoku.SQUARE_SIZE;
            int valueIndexStart = (valueIndex / Sudoku.SQUARE_SIZE) * Sudoku.SQUARE_SIZE;
            for (int checkedLineIndex = lineIndexStart; checkedLineIndex < (lineIndexStart + Sudoku.SQUARE_SIZE); checkedLineIndex++)
            {
                for (int checkedValueIndex = valueIndexStart; checkedValueIndex < (valueIndexStart + Sudoku.SQUARE_SIZE); checkedValueIndex++)
                {
                    if (((checkedLineIndex != lineIndex) || (checkedValueIndex != valueIndex)) &&
                        (Map[checkedLineIndex][checkedValueIndex] == value))
                    {
                        IsValueAlreadyInSquare = true;
                        break;
                    }
                }
            }
            return IsValueAlreadyInSquare;
        }

        public bool IsValueInLine(int lineIndex, int valueIndex, int value)
        {
            var IsValueAlreadyInLine = false;
            for (int checkedValueIndex = 0; checkedValueIndex < Sudoku.COMMON_SIZE; checkedValueIndex++)
            {
                if (checkedValueIndex != valueIndex && Map[lineIndex][checkedValueIndex] == value)
                {
                    IsValueAlreadyInLine = true;
                    break;
                }
            }
            return IsValueAlreadyInLine;
        }

        public bool IsValueInColumn(int lineIndex, int valueIndex, int value)
        {
            var IsValueAlreadyInColumn = false;
            for (int checkedLineIndex = 0; checkedLineIndex < Sudoku.COMMON_SIZE; checkedLineIndex++)
            {
                if (checkedLineIndex != lineIndex && Map[checkedLineIndex][valueIndex] == value)
                {
                    IsValueAlreadyInColumn = true;
                    break;
                }
            }
            return IsValueAlreadyInColumn;
        }

        public bool IsValueCorrect(int lineIndex, int valueIndex, int value)
        {
            return !IsValueInLine(lineIndex, valueIndex, value) &&
                !IsValueInColumn(lineIndex, valueIndex, value) &&
                !IsValueInSquare(lineIndex, valueIndex, value);
        }

        public bool Resolve(int lineIndex, int valueIndex)
        {
            int nextLineIndex = ((valueIndex + 1) == Sudoku.COMMON_SIZE) ? (lineIndex + 1) : lineIndex;
            int nextValueIndex = ((valueIndex + 1) == Sudoku.COMMON_SIZE) ? 0 : (valueIndex + 1);
            if (Map[lineIndex][valueIndex] == 0)
            {
                for (var newValue = 1; newValue <= Sudoku.COMMON_SIZE; newValue++)
                {
                    if (IsValueCorrect(lineIndex, valueIndex, newValue))
                    {
                        Map[lineIndex][valueIndex] = newValue;
                        if (Map.Count == (lineIndex + 1) && Map[lineIndex].Count == (valueIndex + 1))
                        {
                            return true;
                        }
                        else if (Resolve(nextLineIndex, nextValueIndex))
                        {
                            return true;
                        }
                    }
                }
                Map[lineIndex][valueIndex] = 0;
                return false;
            }
            if (Map.Count == (lineIndex + 1) && Map[lineIndex].Count == (valueIndex + 1))
                return true;
            return Resolve(nextLineIndex, nextValueIndex);
        }

        private bool ResolveReverse(int lineIndex, int valueIndex)
        {
            int nextLineIndex = (valueIndex == 0) ? (lineIndex - 1) : lineIndex;
            int nextValueIndex = (valueIndex == 0) ? Sudoku.COMMON_SIZE - 1 : (valueIndex - 1);
            if (Map[lineIndex][valueIndex] == 0)
            {
                for (var newValue = 1; newValue <= Sudoku.COMMON_SIZE; newValue++)
                {
                    if (IsValueCorrect(lineIndex, valueIndex, newValue))
                    {
                        Map[lineIndex][valueIndex] = newValue;
                        if (lineIndex == 0 && valueIndex == 0)
                        {
                            return true;
                        }
                        else if (ResolveReverse(nextLineIndex, nextValueIndex))
                        {
                            return true;
                        }
                    }
                }
                Map[lineIndex][valueIndex] = 0;
                return false;
            }
            if (lineIndex == 0 && valueIndex == 0)
                return true;
            return ResolveReverse(nextLineIndex, nextValueIndex);
        }

        public bool ResolveMap()
        {
            bool executionFinished = false;
            Solved = false;
            SudokuMap[] subMaps = new SudokuMap[2] { new SudokuMap(Map),  new SudokuMap(Map)};
            Task<bool> resolveTask = Task.Run(() => {
                return subMaps[0].Resolve(0, 0) ? true : false;
            });
            Task<bool> resolveReverseTask = Task.Run(() => {
                return subMaps[1].ResolveReverse(8, 8) ? true : false;
            });
            Task<bool> timeoutTask = Task.Run(async () =>
            {
                await Task.Delay(10000);
                return false;
            });

            resolveTask.ContinueWith(task =>
            {
                if (!executionFinished)
                {
                    task.Wait();
                    if (task.Result)
                    {
                        Solved = true;
                        Map = subMaps[0 ].Map;
                        Console.WriteLine("\nThe Grid was solved within the time ! (limit 10 seconds)\n\n");
                        executionFinished = true;
                    }
                }
            });
            resolveReverseTask.ContinueWith(task =>
            {
                if (!executionFinished)
                {
                    task.Wait();
                    if (task.Result)
                    {
                        Solved = true;
                        Map = subMaps[1].Map;
                        Console.WriteLine("\nThe Grid was solved within the time ! (limit 10 seconds)\n\n");
                        executionFinished = true;
                    }
                }
            });
            timeoutTask.ContinueWith(task =>
            {
                if (!executionFinished) {
                    task.Wait();
                    executionFinished = true;
                    Console.WriteLine("\nThe Grid wasn't solved within the time ! (10 seconds)\n\n");
                }
            });
            while (!executionFinished) { }

            return executionFinished && Solved;
        }

        public void PrintGrid()
        {
            Map.ForEach(line =>
            {
                line.ForEach(value => Console.Write($" {value} "));
                Console.WriteLine("\n");
            });
        }
    }
}
