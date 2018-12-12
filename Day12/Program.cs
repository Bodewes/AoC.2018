using System;
using System.Collections.Generic;
using System.Linq;

namespace Day12
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = new Solver();
            Console.WriteLine("Test");
            s.Part1(Input.TestStart, Input.TestSteps, 20);

            Console.WriteLine("Input");
            s.Part1(Input.Start, Input.Steps, 20);

            s.Part1(Input.Start, Input.Steps, 300);
            //s.Part1(Input.StartHW, Input.StepsHW, 1000);
            // bepaal herhaling aan de hand van de output (zodra die stabiel is)
            // Waargenomen groei: 8 per iteratie.
            // (waarde bij 1000)  + 8 * (50_000_000_000 - 1000)
        }
    }

    public class Solver {
        public void Part1(string start, string steps, int iter){
            var s = ParseSteps(steps); // configs with offspring.

            int startSize = start.Length;
            int offset = 10;
            int maxSize = startSize + (iter) + 2*offset; // max growth right = 1, left none.
            

            var plants = new bool[maxSize]; // -100 neg, 0 and 100 pos.
            
            
            Console.WriteLine($"Offset: {offset}");

            for(int i = 0 ; i < start.Length; i++){
                plants[offset + i] = start[i]=='#';
            }

            var prevSum = 0;
            var sum = 0;
            var minIndex = 0+offset;
            var maxIndex = startSize+offset;
            for(int i =0; i < iter; i++)
            {
                var newPlants = new bool[maxSize];

                for (int j = minIndex-2; j < maxIndex + 2; j++)
                {
                    int config = (plants[j - 2] ? 1 : 0) + (plants[j - 1] ? 2 : 0) + (plants[j] ? 4 : 0) + (plants[j + 1] ? 8 : 0) + (plants[j + 2] ? 16 : 0);
                    if (s[config])
                    {
                        newPlants[j] = true;
                        if (j < minIndex) minIndex = j;
                        if (j > maxIndex) maxIndex = j;
                    }else{
                        newPlants[j] = false;
                    }
                }
                plants = newPlants;
                prevSum = sum;
                sum = WritePlants(plants, i, offset, minIndex, maxIndex);
            }


            Console.WriteLine($"PART2: Inc per step {sum-prevSum}");
            Console.WriteLine($"50.000.000.000 sum: {(((long)sum-(long)prevSum)*(50_000_000_000L-(long)iter))+(long)sum}");

            WritePlants(plants, iter, offset, minIndex, maxIndex);
        }

        private static int WritePlants(bool[] plants, int i, int offset, int min = 0, int max = 0)
        {
            Console.Write($"{i}\t>> [{min},{max}] ");
            int sum = 0;
            for (int j = min-2; j < max+2; j++)
            {
                if (plants[j]) sum += j - offset;
                //Console.Write(plants[j] ? '#' : '.');

            }
            Console.WriteLine($" >> {sum}");

            return sum;
        }

        public bool[] ParseSteps(string steps){
            var lines = steps.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            var result = new bool[32];
            foreach(var line in lines){
                var tokens = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                int x = 0;
                x += tokens[0][0]=='#'?1:0;
                x += tokens[0][1]=='#'?2:0;
                x += tokens[0][2]=='#'?4:0;
                x += tokens[0][3]=='#'?8:0;
                x += tokens[0][4]=='#'?16:0;

                result[x] = tokens[2] == "#";
            }
            return result;
        }
    }
}
