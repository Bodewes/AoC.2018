using System;
using System.Linq;

namespace Day05
{
    class Program
    {
        static void Main(string[] args)
        {
            var solver = new Solver();
            // solver.Part1(Input.TestData);

            // solver.Part1(Input.TestData2);
            // solver.Part1(Input.TestData3);
            // solver.Part1(Input.TestData4);
            // solver.Part1(Input.TestData5);

            solver.Part1(Input.Data);

            solver.Part2(Input.Data);
        }
    }

    public class Solver{
        public int Part1(string data){
            Console.WriteLine($"polymer length {data.Length}");
            var size = data.Length;
            var i = 0;
            while(i < size-1){
                // System.Console.WriteLine(data);
                // System.Console.SetCursorPosition(i, System.Console.CursorTop);
                // System.Console.WriteLine($"^ {i} {data[i]}");
                if (data[i] == data[i+1]-32 || data[i]-32 == data[i+1]){
                    data = data.Substring(0,i)+data.Substring(i+2);
                    i--;
                    size = size - 2;
                    if (i == -1) i = 0;
                }else{
                    i++;
                }
            }
            Console.WriteLine($"polymer length {data.Length} (after reduce)");
            return data.Length;
        }

        public void Part2(string data){
            int min = int.MaxValue;
            char minLetter = '0';
            for(int i =  'a'; i<='z'; i++){
                var filtered = new String(data.Where(c => c!=i && c!=i-32).ToArray());
                // Console.WriteLine($"Zonder {(char)i} en {(char)(i-32)} : {filtered} ");
                var size = Part1(filtered);
                if (size < min){
                    min = size;
                    minLetter = (char)i;
                }
            }
            Console.WriteLine($"{minLetter}: {min}");
        }
    }
}
