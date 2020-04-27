using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class SudokuMap
    {
        public List<List<int>> Map = new List<List<int>>(SudokuMapBuilder.COMMON_SIZE);
        public bool Solved = false;
        public SudokuMap()
        {
            for (int i = 0; i < SudokuMapBuilder.COMMON_SIZE; i++) {
                Map.Add(new List<int>());
            }
        }

        public SudokuMap(List<List<int>> map) {
            Map = map;
        }

        public void FillLineValues(String readedLine, int startIndex)
        {
            String[] splittedStringValues;

            if (String.IsNullOrEmpty(readedLine) || String.IsNullOrWhiteSpace(readedLine))
                throw new ArgumentNullException($"The {nameof(readedLine)} parameter is empty or full of white spaces.");
            splittedStringValues = readedLine.Split(" ");
            if (splittedStringValues.Length != SudokuMapBuilder.COMMON_SIZE)
                throw new IndexOutOfRangeException("The expected number of values is not respected in the readed line.");
            for (int i = 0; i < splittedStringValues.Length; i++)
            {
                this.AddValueToSquare(startIndex, int.Parse(splittedStringValues[i]));
                if ((i + 1) < SudokuMapBuilder.COMMON_SIZE && (i + 1) % SudokuMapBuilder.SQUARE_SIZE_PER_LINE == 0) startIndex++;
            }
        }

        private void AddValueToSquare(int squareIndex, int value)
        {
            if (Map.Count <= squareIndex)
                throw new ArgumentOutOfRangeException($"The provided {nameof(squareIndex)} is out of range compared to the Map size.");
            if (Map[squareIndex].Count == SudokuMapBuilder.COMMON_SIZE)
                throw new ArgumentOutOfRangeException($"The Square at index {squareIndex} is already filled by values.");
            Map[squareIndex].Add(value);
        }

        private bool IsValueCorrect(int squareIndex, int valueIndex, int value)
        {
            var lineSum = GetLineSum(squareIndex, valueIndex) + value;
            var columnSum = GetColumnSum(squareIndex, valueIndex) + value;
            var squareSum = GetSquareSum(squareIndex) + value;
            return lineSum <= SudokuMapBuilder.SUM_VALUES_LIMIT &&
                columnSum <= SudokuMapBuilder.SUM_VALUES_LIMIT &&
                squareSum <= SudokuMapBuilder.SUM_VALUES_LIMIT &&
                !IsAlreadyInLine(squareIndex, valueIndex, value) &&
                !IsAlreadyInColumn(squareIndex, valueIndex, value) &&
                !SquareHasValue(squareIndex, value);
        }

        private List<int> UsableValues(int squareIndex)
        {
            List<int> usableValues = new List<int>();

            SudokuMapBuilder.VALID_VALUES.ForEach(validValue =>
            {
                if (!Map[squareIndex].Exists(existingValue => existingValue == validValue)) usableValues.Add(validValue);
            });

            return usableValues;
        }

        private bool SquareHasValue(int squareIndex, int value) => Map[squareIndex].Exists(existingValue => existingValue == value);

        private int GetSquareSum(int squareIndex)
        {
            int sum = 0;
            Map[squareIndex].ForEach(value => sum += value);
            return sum;
        }

        private bool IsAlreadyInLine(int squareIndex, int valueIndex, int value)
        {
            bool isInLine = false;
            int firstSquareIndex = GetFirstInLineFromIndex(squareIndex);
            int firstValueIndex = GetFirstInLineFromIndex(valueIndex);
            int squareIndexLimit = firstSquareIndex + SudokuMapBuilder.SQUARE_SIZE_PER_LINE;
            int valueIndexLimit = firstValueIndex + SudokuMapBuilder.SQUARE_SIZE_PER_LINE;

            for (int checkedSquareIndex = firstSquareIndex; checkedSquareIndex < squareIndexLimit; checkedSquareIndex++)
            {
                for (int checkedValueIndex = firstValueIndex; checkedValueIndex < valueIndexLimit; checkedValueIndex++) {
                    if (checkedSquareIndex != squareIndex || checkedValueIndex != valueIndex) {
                        if (Map[checkedSquareIndex][checkedValueIndex] == value) isInLine = true;
                    }
                }
            }

            return isInLine;
        }

        private bool IsAlreadyInColumn(int squareIndex, int valueIndex, int value)
        {
            bool isInColumn = false;
            int firstSquareIndex = GetFirstColumnFromIndex(squareIndex);
            int firstValueIndex = GetFirstColumnFromIndex(valueIndex);
            int squareIndexLimit = firstSquareIndex + (SudokuMapBuilder.SQUARE_SIZE_PER_LINE * 2);
            int valueIndexLimit = firstValueIndex + (SudokuMapBuilder.SQUARE_SIZE_PER_LINE * 2);

            for (int checkedSquareIndex = firstSquareIndex; checkedSquareIndex <= squareIndexLimit; checkedSquareIndex += SudokuMapBuilder.SQUARE_SIZE_PER_LINE)
            {
                for (int checkedValueIndex = firstValueIndex; checkedValueIndex <= valueIndexLimit; checkedValueIndex += SudokuMapBuilder.SQUARE_SIZE_PER_LINE)
                {
                    if (checkedSquareIndex != squareIndex || checkedValueIndex != valueIndex)
                    {
                        if (Map[checkedSquareIndex][checkedValueIndex] == value) isInColumn = true;
                    }
                }
            }

            return isInColumn;
        }

        private bool IsSquareValid(int squareIndex) => GetSquareSum(squareIndex) <= SudokuMapBuilder.SUM_VALUES_LIMIT;

        private int GetLineSum(int squareIndex, int valueIndex)
        {
            int firstSquareIndex = GetFirstInLineFromIndex(squareIndex);
            int firstValueIndex = GetFirstInLineFromIndex(valueIndex);
            int squareIndexLimit = firstSquareIndex + SudokuMapBuilder.SQUARE_SIZE_PER_LINE;
            int valueIndexLimit = firstValueIndex + SudokuMapBuilder.SQUARE_SIZE_PER_LINE;
            int sum = 0;

            for (var squareLineIndex = firstSquareIndex; squareLineIndex < squareIndexLimit; squareLineIndex++)
            {
                for (int valueLineIndex = firstValueIndex; valueLineIndex < valueIndexLimit; valueLineIndex++)
                {
                    sum += Map[squareLineIndex][valueLineIndex];
                }
            }

            return sum;
        }

        private int GetColumnSum(int squareIndex, int valueIndex)
        {
            int firstSquareIndex = GetFirstColumnFromIndex(squareIndex);
            int firstValueIndex = GetFirstColumnFromIndex(valueIndex);
            int squareIndexLimit = firstSquareIndex + SudokuMapBuilder.SQUARE_SIZE_PER_LINE;
            int valueIndexLimit = firstValueIndex + SudokuMapBuilder.SQUARE_SIZE_PER_LINE;
            int sum = 0;

            for (var squareLineIndex = firstSquareIndex; squareLineIndex < squareIndexLimit; squareLineIndex += SudokuMapBuilder.SQUARE_SIZE_PER_LINE)
            {
                for (int valueLineIndex = firstValueIndex; valueLineIndex < valueIndexLimit; valueLineIndex += SudokuMapBuilder.SQUARE_SIZE_PER_LINE)
                {
                    sum += Map[squareLineIndex][valueLineIndex];
                }
            }

            return sum;
        }
        private bool IsLineValid(int squareIndex, int valueIndex) => GetLineSum(squareIndex, valueIndex) <= SudokuMapBuilder.SUM_VALUES_LIMIT;

        private bool IsColumnValid(int squareIndex, int valueIndex) => GetColumnSum(squareIndex, valueIndex) <= SudokuMapBuilder.SUM_VALUES_LIMIT;

        static public int GetFirstColumnFromIndex(int index)
        {
            if (index < SudokuMapBuilder.SQUARE_SIZE_PER_LINE)
                return index;
            
            return index - ((index / SudokuMapBuilder.SQUARE_SIZE_PER_LINE) * SudokuMapBuilder.SQUARE_SIZE_PER_LINE);
        }

        static public int GetFirstInLineFromIndex(int index)
        {
            if (index % SudokuMapBuilder.SQUARE_SIZE_PER_LINE == 0)
                return index;
            if ((index + 1) % SudokuMapBuilder.SQUARE_SIZE_PER_LINE == 0)
                return (index + 1) - SudokuMapBuilder.SQUARE_SIZE_PER_LINE;
            return index - 1;
        }

        private SudokuNextIndex GetNextIndexes(int squareIndex, int valueIndex, bool write) {
            return new SudokuNextIndex(squareIndex, valueIndex, write);
        }

        public async Task<bool> ResolveMap() {
            int tasks = 1;
            for (var idx = 1; idx <= SudokuMapBuilder.COMMON_SIZE; idx++) {
                Task.Run(async () =>
                {
                    var index = tasks;
                    tasks++;
                    SudokuMap subMap = new SudokuMap(Map);
                    if (subMap.Resolve(0, 0, index))
                    {
                        Map = subMap.Map;
                        Solved = true;
                        return;
                    }
                    else
                    {
                        return;
                    }
                });
                await Task.Delay(30);
            }
            while (!Solved)
            {
                await Task.Delay(2);
            }
            PrintGrid();
            Console.WriteLine("The map was solved !");
            return Solved;
        }

        public bool Resolve(int squareIndex, int valueIndex, int startValue)
        {
            SudokuNextIndex nextIndexes = GetNextIndexes(squareIndex, valueIndex, false);

            if (Map[squareIndex][valueIndex] == 0)
            {
                for (var usableValue = startValue; usableValue <= SudokuMapBuilder.COMMON_SIZE; usableValue++)
                {
                    if (IsValueCorrect(squareIndex, valueIndex, usableValue))
                    {
                        Map[squareIndex][valueIndex] = usableValue;
                        if (Map.Count == (squareIndex + 1) && Map[squareIndex].Count == (valueIndex + 1)) return true;
                        if (Resolve(nextIndexes.nextSquareIndex, nextIndexes.nextValueIndex, 1)) return true;
                    }
                }
                Map[squareIndex][valueIndex] = 0;
                return false;
            }
            if (Map.Count == (squareIndex + 1) && Map[squareIndex].Count == (valueIndex + 1)) return true;
            return Resolve(nextIndexes.nextSquareIndex, nextIndexes.nextValueIndex, 1);
        }

        public void PrintGrid()
        {
            SudokuNextIndex nextIndexes;

            for (var usedSquareIndex = 0; usedSquareIndex < SudokuMapBuilder.COMMON_SIZE;)
            {
                for (var valueIndex = 0; valueIndex < SudokuMapBuilder.COMMON_SIZE;) {
                    Console.Write($" [{Map[usedSquareIndex][valueIndex]}] ");
                    if ((valueIndex + 1) == SudokuMapBuilder.COMMON_SIZE && (usedSquareIndex + 1) == SudokuMapBuilder.COMMON_SIZE) return;
                    nextIndexes = GetNextIndexes(usedSquareIndex, valueIndex, true);
                    valueIndex = nextIndexes.nextValueIndex;
                    usedSquareIndex = nextIndexes.nextSquareIndex;
                }
            }
            Console.WriteLine("\n");
        }
    }
}
