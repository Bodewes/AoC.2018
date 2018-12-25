using System;
using System.Collections.Generic;
using System.Linq;

namespace Day25
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = new Solver();
            s.Part1(Input.TestData);
            s.Part1(Input.TestData2);
            s.Part1(Input.TestData3);
            s.Part1(Input.TestData4);
            s.Part1(Input.Data);
        }
    }

    public class Solver{
        public void Part1(string data){
            var lines = data.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            Console.WriteLine($"Aantal punten: {lines.Count()}");
            var points = lines.Select(Point.fromLine).ToList();


            List<List<Point>> groups= new List<List<Point>>(); // een lijst van groepen (een lijst van punten)
            foreach(var p in points){
                // get list of groups this point is near
                var nearGroups = new List<int>(); // indexen van alle groepen dichtbij deze
                for(int i = 0; i < groups.Count(); i++){ // loop door alle groepn
                    if (groups[i].Any( ping => ping.Near(p))){ // is er een punt in de groep dichtbij deze?
                        nearGroups.Add(i);
                    }
                }

                // merge all groups, maak een nieuwe group,
                var new_group = new List<Point>();
                foreach(var index in nearGroups){
                    new_group.AddRange(groups[index]);
                }
                new_group.Add(p);  // merge point with new group
                foreach(var index in nearGroups.OrderByDescending(x => x)){ //, verwijder oude groepen;
                    groups.RemoveAt(index);
                }
                groups.Add(new_group);

                //Console.WriteLine($"Groepen: {groups.Count()}");
            }
            Console.WriteLine($"Groepen: {groups.Count()}");

        }
    }

    public class Point{
        public int x;
        public int y;
        public int z;
        public int t;

        public bool Near(Point other){
            return (Math.Abs(x-other.x)+ Math.Abs(y-other.y)+Math.Abs(z-other.z)+Math.Abs(t-other.t)) <= 3;
        }

        public static Point fromLine(string line){
            var tokens = line.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            return new Point(){ x = tokens[0], y = tokens[1], z = tokens[2], t = tokens[3]};
        }
    }
}
