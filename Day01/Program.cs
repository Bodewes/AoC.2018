using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Day01
{
    class Program
    {
        static void Main(string[] args)
        {
            var solver = new Solver();

            foreach(var d in Input.Data()){
                Console.WriteLine($"Part1: {solver.Part1(d)}");
            }
            
            foreach(var d in Input.Data()){
                Console.WriteLine($"Part2: {solver.Part2(d)}");
            }
        }
    }

    public class Solver{
        public int Part1(string line){
            return line.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).Sum();
        }

        public int Part2(string line){
            int current = 0;
            var seen = new List<int>();
            var index = 0;
            var freq = line.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToList();
            do
            {
                seen.Add(current);
                current += freq[index];
                index++;
                if (index == freq.Count) index = 0;
            }
            while (!seen.Contains(current));
            return current;
        }

    }

    public class Input{

        public static List<string> Data(){
            return File.ReadAllLines("day01.txt").ToList();
        }

    }
}
