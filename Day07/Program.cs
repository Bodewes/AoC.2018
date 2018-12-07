using System;
using System.Collections.Generic;
using System.Linq;

namespace Day07
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = new Solver();
            //s.Part1(Input.TestData);
            //s.Part1(Input.Data);

            s.Part2(Input.TestData, 2, 0);
            s.Part2(Input.Data, 5, 60);
        }
    }

    public class Solver{

        public void Part1(string data){
            var lines = data.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).Select(x => (x[5], x[36])).ToList();
            
            var order = new List<char>();
            //Console.WriteLine("Lines:");
            //lines.ForEach(x => Console.WriteLine(x));

            do{
                Console.WriteLine("Lines:");
                lines.ForEach(x => Console.WriteLine(x));

                var candidates = lines.Where( x=> !lines.Any( y => y.Item2 == x.Item1)).Select(x=>x.Item1).OrderBy(x=>x).Distinct();
                Console.WriteLine("Candidates: "+string.Join("",candidates));

                var start = candidates.First();
                order.Add(start);
                Console.WriteLine($" Picked {start}");
                Console.WriteLine("Order: " + string.Join("",order.ToArray()));
                lines.RemoveAll( x=> x.Item1 == start); // alles eruit dat begint met huidige keuze

            }while(lines.Count > 0);
            Console.WriteLine( string.Join("",order.ToArray()));
            // laatste letter met de hand
        }

        public void Part2(string data, int workerCount = 5, int extraTime = 60){
            var lines = data.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).Select(x => (x[5], x[36])).ToList();

            var done = new List<char>();

            var worker = new int[workerCount]; // timer
            var workerItem = new char[workerCount];
            for (int i = 0; i< worker.Length; i++){
                workerItem[i] = '.';
            }


            for(int time = 0; time < 1000; time++){
                for (int i = 0; i< worker.Length; i++){
                    if (worker[i] == 0){ // done

                        if (workerItem[i] != '.'){
                            done.Add(workerItem[i]); // add to done.
                            //lines.RemoveAll( x=> x.Item1 == workerItem[i]); // alles eruit dat begint met klaar 
                            workerItem[i] = '.';
                            
                        }
                        var next = Next(lines, done, workerItem.ToList());
                        workerItem[i] = next;
                        if (next == '.'){ // set timer?
                            worker[i] = 0;
                        }else{
                            worker[i] = next - 'A' + extraTime;  
                        }
                    }else{
                        worker[i]--;
                    }
                }
                //Console.WriteLine($"{time} >\t{workerItem[0]}({worker[0]})\t{workerItem[1]}({worker[1]})\t{string.Join("",done)}");
                Console.Write($"{time}");
                for(int i = 0; i< workerCount; i++){
                    Console.Write($"\t{workerItem[i]}({worker[i]})");
                }
                Console.WriteLine($"\t{string.Join("",done)}");

                if(workerItem.All( x=> x=='.')){
                    Console.WriteLine($"Done @ {time}");
                    break;
                } 
            }
        }

        private char Next(List<(char,char)> lines, List<char> done, List<char> workerItem){
            // find a root
            var roots = lines.Where( x=> !lines.Any( y => y.Item2 == x.Item1) && !done.Contains(x.Item1)).Select(x => x.Item1).Distinct().ToList();

            //Console.WriteLine($"Done  : {string.Join("",done)}");
            //Console.WriteLine($"Roots  : {string.Join("",roots)}");

            // available via done-items.
            var open =  lines.Where(x => done.Contains(x.Item1) && !done.Contains(x.Item2)).Select(x => x.Item2); // voorwaarde voldaan en nog niet gemaakt.
            var closed = lines.Where( x => !done.Contains(x.Item1)).Select(x => x.Item2); // voorwaade niet voldaan
            var available = open.Where( x=> !closed.Contains(x));

            //Console.WriteLine($"Open  : {string.Join("",open)}");
            //Console.WriteLine($"Closed  : {string.Join("",closed)}");

            //Console.WriteLine($"Avail  : {string.Join("",available)}");

            var choices = roots.Concat(available).Where(x => !workerItem.Contains(x)).OrderBy(x => x).Distinct().ToList();
            //Console.WriteLine($"Choices  : {string.Join("",choices)}");


            if (choices.Count > 0)
                return choices.First();
            return '.';
        }
    }
}
