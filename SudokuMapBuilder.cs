using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SudokuSolver
{
    public class SudokuMapBuilder
    {
        public static int COMMON_SIZE = 9;
        public static int SQUARE_SIZE_PER_LINE = 3;
        public static int SUM_VALUES_LIMIT = 45;
        public static List<int> VALID_VALUES= new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        SudokuMapBuilder() {
        }
        public static SudokuMap GenerateMap(String filePath) {
            StreamReader mapFileStream;
            SudokuMap GeneratedMap = new SudokuMap();
            String readedLine;
            int lineReaded = 0;
            int startIndex = 0;
            List<List<int>> ListMap = new List<List<int>>(SudokuMapBuilder.COMMON_SIZE);

            if (String.IsNullOrEmpty(filePath)) throw new ArgumentNullException($"Argument {nameof(filePath)} not provided.");
            if (!File.Exists(filePath)) throw new FileNotFoundException($"File {filePath} not found.");
            
            mapFileStream = new StreamReader(filePath);

            while (!mapFileStream.EndOfStream) {
                readedLine = mapFileStream.ReadLine();
                if (!String.IsNullOrEmpty(readedLine) && !String.IsNullOrWhiteSpace(readedLine)) {
                    GeneratedMap.FillLineValues(readedLine, startIndex);
                    if ((++lineReaded) % SudokuMapBuilder.SQUARE_SIZE_PER_LINE == 0)
                        startIndex += SudokuMapBuilder.SQUARE_SIZE_PER_LINE;
                }
            }
            Console.WriteLine("The map was generated !");
            return GeneratedMap;
        }
    }
}
