using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SudokuSolver
{
    public class SudokuMapBuilder
    {
        SudokuMapBuilder() {
        }
        public static SudokuMap GenerateMap(String filePath) {
            StreamReader mapFileStream;
            SudokuMap GeneratedMap = new SudokuMap();
            String readedLine;
            int lineReaded = 0;
            List<List<int>> ListMap = new List<List<int>>(Sudoku.COMMON_SIZE);

            if (String.IsNullOrEmpty(filePath)) throw new ArgumentNullException($"Argument {nameof(filePath)} not provided.");
            if (!File.Exists(filePath)) throw new FileNotFoundException($"File {filePath} not found.");
            
            mapFileStream = new StreamReader(filePath);

            while (!mapFileStream.EndOfStream) {
                readedLine = mapFileStream.ReadLine();
                GeneratedMap.FillLineValues(readedLine, lineReaded++);
            }
            Console.WriteLine("The map was generated !\n");
            return GeneratedMap;
        }
    }
}
