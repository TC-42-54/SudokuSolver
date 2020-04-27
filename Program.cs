using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            SudokuMap map = SudokuMapBuilder.GenerateMap(args[0]);
            if (map.ResolveMap())
            {
                map.PrintGrid();
            }
        }
    }
}
