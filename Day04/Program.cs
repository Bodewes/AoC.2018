using System;
using System.Collections.Generic;
using System.Linq;

namespace Day04
{
    class Program
    {
        static void Main(string[] args)
        {
            var solver = new Solver();

            var parsed = solver.parseInput(Input.Data);
            parsed.ForEach(Console.WriteLine);

            solver.SolvePart1(parsed);
        }
    }

    public class Solver{

        public void SolvePart1(List<Shift> shifts){
            var groups = shifts.GroupBy(x=> x.Id);

            var answer = 0;
            var answer2 = 0;
            var maxSleep = 0;
            var maxSleepMin = 0;
            var maxSleepMinTime = 0;

            foreach(var g in groups){
                int[] minutes = new int[60];
                Console.WriteLine($"{g.Key}: {string.Join(",", minutes)}");
                foreach(var shift in g){
                    //minutes = minutes.Zip(shift.Sleep, (x,s) => s?x++:x ).ToArray();
                    for (int i = 0; i< minutes.Length; i++){
                        if(shift.Sleep[i]) minutes[i]++;
                    }
                }

                Console.WriteLine($"{g.Key}: {string.Join(",", minutes)}");
                var sum = minutes.Sum();
                var max = minutes.Max();
                var m = minutes.ToList().IndexOf(minutes.Max());
                
                Console.WriteLine($"{g.Key} sleeps: {sum} max: {m} ==> {g.Key*m} ");
                if (sum > maxSleep){ // most sleep time
                    maxSleep = sum;
                    Console.WriteLine($"More sleepy: {g.Key} sleeps {sum} , max @ {m} ==> {g.Key*m}");
                    answer = g.Key*m;
                }

                if (max > maxSleepMinTime){ // most asleep at
                    maxSleepMin = m;
                    maxSleepMinTime = max;
                    Console.WriteLine($"More sleepy: {g.Key} sleeps {sum} , max @ {m} ==> {g.Key*m}");
                    answer2 = g.Key*m;
                }
            }
            Console.WriteLine($"antwoord: {answer}");
            Console.WriteLine($"antwoord2: {answer2}");
        }


        public List<Shift> parseInput(string input){
            var lines = input.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);

            List<Shift> shifts = new List<Shift>();

            Shift shift = null;
            int startSleep = 0;
            foreach(var line in lines.OrderBy(s => s)){
                var tokens = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                if (tokens[2] == "Guard"){
                    // found new shift, add previous
                    if (shift != null) shifts.Add(shift);

                    shift = new Shift();
                    shift.Id = Int32.Parse(tokens[3].Substring(1));
                }
                if (tokens[2] == "falls"){
                    startSleep = Int32.Parse(tokens[1].Substring(3,2));
                }
                if (tokens[2] == "wakes"){
                    var endSleep = Int32.Parse(tokens[1].Substring(3,2));
                    for(int i = startSleep; i< endSleep; i++){
                        shift.Sleep[i] = true;
                    }
                }

            }
            // Add last one
            if (shift != null) shifts.Add(shift);

            return shifts;
        }
    }

    public class Shift{
        
        public int Id{get;set;}
        public bool[] Sleep{get;set;}
        
        public Shift()
        {
            Sleep = new bool[60];
        }

        public override string ToString(){
            return $"{Id}\t{String.Join("",Sleep.Select(x=>x?"#":"."))}";
        }
    }
}
