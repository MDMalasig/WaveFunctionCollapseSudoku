using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveFunctionCollapseSudoku
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[][,] sBoard = new int[2][,];
            List<int>[,] indivPoss = new List<int>[9, 9];
            // sboard[0] contains the number of possible values for each cell
            // sboard[1] contains the actual number stored
            List<int>[] rowPossiblities = new List<int>[9];
            List<int>[] colPossiblities = new List<int>[9];
            List<int>[,] cellPossibilities = new List<int>[3, 3];
            List<int> possibilities = new List<int>();
            bool cellSelect = false;
            int[] selectedCell = { 0, 0 };
            Random rnd = new Random();
            bool endState = false;

            // initialize boards
            sBoard[0] = new int[9, 9];
            sBoard[1] = new int[9, 9];
            for (int a = 0; a < sBoard.Length; a++) 
            {
                for(int b = 0; b < sBoard[a].GetLength(0); b++)
                {
                    for(int c = 0; c < sBoard[a].GetLength(1); c++)
                    {
                        sBoard[a][b, c] = 0;
                    }
                }
            }

            // initialize row possiblities
            for(int a = 0; a < rowPossiblities.Length; a++)
            {
                rowPossiblities[a] = new List<int>();
                for(int b = 1; b <= 9; b++)
                {
                    rowPossiblities[a].Add(b);
                }
            }
            // initialize col possiblities
            for (int a = 0; a < colPossiblities.Length; a++)
            {
                colPossiblities[a] = new List<int>();
                for (int b = 1; b <= 9; b++)
                {
                    colPossiblities[a].Add(b);
                }
            }
            // initialize cell possiblities
            for(int a = 0; a < cellPossibilities.GetLength(0); a++)
            {
                for(int b = 0; b < cellPossibilities.GetLength(1); b++)
                {
                    cellPossibilities[a,b] = new List<int>();
                    for(int c = 1; c <= 9; c++)
                        cellPossibilities[a,b].Add(c);
                }
            }

            while (true)
            {
                // calculate possibility count
                for (int a = 0; a < sBoard[0].GetLength(0); a++)
                {
                    for (int b = 0; b < sBoard[0].GetLength(1); b++)
                    {
                        possibilities.Clear();
                        // generate default possiblities
                        for (int c = 1; c <= 9; c++)
                            possibilities.Add(c);

                        // remove not possible numbers from row possibilies
                        for (int c = 0; c < possibilities.Count; c++)
                        {
                            if (rowPossiblities[a].Contains(possibilities[c]))
                            {

                            }
                            else
                            {
                                possibilities.RemoveAt(c);
                            }
                        }

                        // remove not possible numbers from col possibilies
                        for (int c = 0; c < possibilities.Count; c++)
                        {
                            if (colPossiblities[b].Contains(possibilities[c]))
                            {

                            }
                            else
                            {
                                possibilities.RemoveAt(c);
                            }
                        }

                        // remove not possible numbers from cell possibilies
                        for (int c = 0; c < possibilities.Count; c++)
                        {
                            if (cellPossibilities[(int)a / 3, (int)b / 3].Contains(possibilities[c]))
                            {

                            }
                            else
                            {
                                possibilities.RemoveAt(c);
                            }
                        }

                        if (sBoard[1][a, b] == 0)
                        {
                            sBoard[0][a, b] = possibilities.Count;
                            indivPoss[a, b] = new List<int>(possibilities);
                        }
                        else
                        {
                            sBoard[0][a, b] = 0;
                            indivPoss[a, b] = new List<int>();
                        }
                    }
                }

                // search for max probability
                cellSelect = false;
                for (int max = 9; max > 0; max--)
                {
                    for (int a = 0; a < sBoard[0].GetLength(0); a++)
                    {
                        for (int b = 0; b < sBoard[0].GetLength(1); b++)
                        {
                            if (sBoard[0][a, b] == max)
                            {
                                cellSelect = true;
                                selectedCell = new int[] { a, b };
                                break;
                            }
                        }
                        if (cellSelect)
                            break;
                    }
                    if (cellSelect)
                        break;
                }

                // populate selected
                sBoard[1][selectedCell[0], selectedCell[1]] = indivPoss[selectedCell[0], selectedCell[1]][rnd.Next(0, indivPoss[selectedCell[0], selectedCell[1]].Count)];
                sBoard[0][selectedCell[0], selectedCell[1]] = 0;
                // remove from possibilities
                rowPossiblities[selectedCell[0]].Remove(sBoard[1][selectedCell[0], selectedCell[1]]);
                colPossiblities[selectedCell[1]].Remove(sBoard[1][selectedCell[0], selectedCell[1]]);
                cellPossibilities[(int)(selectedCell[0] / 3), (int)(selectedCell[1] / 3)].Remove(sBoard[1][selectedCell[0], selectedCell[1]]);

                Console.Clear();
                Console.WriteLine("Possibilities");
                for (int a = 0; a < sBoard[0].GetLength(0); a++)
                {
                    for (int b = 0; b < sBoard[0].GetLength(1); b++)
                    {
                        if (sBoard[0][a, b] == 0)
                            Console.ForegroundColor = ConsoleColor.Red;
                        else
                            Console.ResetColor();
                        Console.Write(sBoard[0][a, b] + "\t");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("Values");
                for (int a = 0; a < sBoard[1].GetLength(0); a++)
                {
                    for (int b = 0; b < sBoard[1].GetLength(1); b++)
                    {
                        if (sBoard[0][a, b] == 0)
                            if (a == selectedCell[0] && b == selectedCell[1])
                                Console.ForegroundColor = ConsoleColor.Green;
                            else
                                Console.ForegroundColor = ConsoleColor.Red;
                        else
                            Console.ResetColor();
                        Console.Write(sBoard[1][a, b] + "\t");
                    }
                    Console.WriteLine();
                }
                Console.ReadKey();

                // check end state
                endState = true;
                for(int a = 0; a < sBoard[0].GetLength(0); a++)
                {
                    for(int b = 0; b < sBoard[0].GetLength(1); b++)
                    {
                        if (sBoard[0][a, b] > 0)
                        {
                            endState = false;
                            break;
                        }
                    }
                    if (!endState)
                        break;
                }

                if (endState)
                    break;

            }

            Console.WriteLine("Done generating sudoku board");
            Console.ReadKey();
        }
    }
}
