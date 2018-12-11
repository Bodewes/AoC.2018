using System;

namespace Day11
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = new Solver();
            //s.Part1(8);
            //Console.WriteLine(s.Power(3,5,8));
            //Console.WriteLine(s.Power(122,79,57));
            //Console.WriteLine(s.Power(217,196,39));
            //Console.WriteLine(s.Power(101,153,71));
            s.Part1(18);
            s.Part1(42);
            s.Part1(5468);

            s.Part2(18);
            s.Part2(42);
            s.Part2(5468);
        }
    }

    public class Solver{
        
        public void Part1(int gridId)
        {
            int[,] grid = BuildGrid(gridId);

            (int x, int y, int power) maxPos = FindMax(3, grid);
            Console.WriteLine($"{gridId} > {maxPos.x + 1},{maxPos.y + 1}");

        }

        public void Part2(int gridId){
            int[,] grid = BuildGrid(gridId);

            int currentMax = int.MinValue;
            (int x, int y) maxPos = (0,0);
            int maxsize = 0;
            for(int i = 1; i <= 30; i++){ // set i to 20 or 30 for far better performace. Or use sliding window for calculation instead of brute force.
                Console.Write(".");
                var maxPosPower = FindMax(i, grid);
                if (maxPosPower.power > currentMax){
                    currentMax = maxPosPower.power;
                    maxPos = (maxPosPower.x, maxPosPower.y);
                    maxsize = i;

                }
            }
            Console.WriteLine($"{gridId} > {maxPos.x + 1},{maxPos.y + 1},{maxsize}");


        }

        private int[,] BuildGrid(int gridId)
        {
            var grid = new int[300, 300];
            for (int y = 0; y < 300; y++)
            {
                for (int x = 0; x < 300; x++)
                {
                    grid[x, y] = Power(x + 1, y + 1, gridId);
                }
            }

            return grid;
        }

        private (int x, int y, int power) FindMax(int squareSize, int[,] grid)
        {
            var currentMax = int.MinValue;
            (int x, int y) maxPos = (-1, -1);
            for (int y = 0; y <= 300 - squareSize; y++)
            {
                for (int x = 0; x <= 300 - squareSize; x++)
                {

                    var sum = 0;
                    for (int y2 = 0; y2 < squareSize; y2++)
                    {
                        for (int x2 = 0; x2 < squareSize; x2++)
                        {
                            sum += grid[x + x2, y + y2];
                        }
                    }
                    if (sum > currentMax)
                    {
                        maxPos = (x, y);
                        currentMax = sum;
                    }
                }
            }

            return (maxPos.x, maxPos.y, currentMax);
        }

        public int Power(int x, int y, int gridId){
            return  ((((x+10)*y + gridId )*(x+10))/100)%10 - 5;
        }
    }
}
