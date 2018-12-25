using System;
using System.Collections.Generic;
using System.Linq;

namespace Day23
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = new Solver();
            //s.Part1(Input.TestData);
            //s.Part1(Input.Data);

            //s.Part2(Input.TestData2);
            s.Part2Redux(Input.TestData2);
            s.Part2Redux(Input.Data);
        }
    }

    public class Solver{

        public void Part1(string data){
            Console.WriteLine("Part1");
            var lines = data.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            
            var bots = lines.Select(NanoBot.fromLine);
            Console.WriteLine($"Got {bots.Count()} bots.");
            var bot_max_r = bots.OrderByDescending(b => b.r).First();
            Console.WriteLine($"Bot with max R: {bot_max_r}");
            var in_range = bots.Count( b => bot_max_r.dist(b) <= bot_max_r.r );
            Console.WriteLine($"\tIn range: {in_range}");

        }

        // based on: https://raw.githack.com/ypsu/experiments/master/aoc2018day23/vis.html
        public void Part2Redux(string data){
            Console.WriteLine("Part2 - Redux");
            var lines = data.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            var bots = lines.Select(NanoBot.fromLine).ToList();
            Console.WriteLine($"Got {bots.Count()} bots.");

            Console.WriteLine($"x range: {bots.Min(b => b.x)} {bots.Max(b => b.x)}");
            Console.WriteLine($"y range: {bots.Min(b => b.y)} {bots.Max(b => b.y)}");
            Console.WriteLine($"z range: {bots.Min(b => b.z)} {bots.Max(b => b.z)}");

            var minx = bots.Min(b => b.x);
            var miny = bots.Min(b => b.y);
            var minz = bots.Min(b => b.z);
            var maxx = bots.Max(b => b.x);
            var maxy = bots.Max(b => b.y);
            var maxz = bots.Max(b => b.z);

            // Initial search space, lower left corner
            Console.WriteLine($"{minx},{miny},{minz}");
            // find start size. multiple of 2.
            var max = new List<int>{ maxx-minx, maxy-miny, maxz-minz }.Max();
            var startsize = 1;
            while(startsize < max){
                startsize *= 2;
            }
            Console.WriteLine($"Size: {startsize}");

            var spaces = new List<Space>();

            // create fist search box (containing all bots)
            var firstSpace = new Space{ x = minx, y = miny, z = minz, size = startsize, bot_count = bots.Count};
            // Add first space 
            spaces.Add(firstSpace);

            while(true){
                // sort, space with most bots first, then by distance, then by size;
                spaces = spaces.OrderByDescending(x =>x.bot_count).ThenBy(x => x.dist).ThenBy(x => x.size).ToList();
                var s = spaces.First(); 
                if (s.size == 1){  // s is alwaus the space with most bots (spaces was sorted that way!) if the space has size one, we're done!
                    Console.WriteLine($"DONE: {s}  [#subspaces: {spaces.Count}]");
                    return;
                }
                // not done. Split space in 8 subspaces.
                var hs = s.size/2;
                var s1 = new Space{ x= s.x      , y = s.y       , z = s.z       , size = hs}; // todo count bots!
                var s2 = new Space{ x= s.x + hs , y = s.y       , z = s.z       , size = hs};
                var s3 = new Space{ x= s.x      , y = s.y + hs  , z = s.z       , size = hs};
                var s4 = new Space{ x= s.x      , y = s.y       , z = s.z + hs  , size = hs};
                var s5 = new Space{ x= s.x + hs , y = s.y + hs  , z = s.z       , size = hs};
                var s6 = new Space{ x= s.x      , y = s.y + hs  , z = s.z + hs  , size = hs};
                var s7 = new Space{ x= s.x + hs , y = s.y       , z = s.z + hs  , size = hs};
                var s8 = new Space{ x= s.x + hs , y = s.y + hs  , z = s.z + hs  , size = hs};

                s1.bot_count = bots.Count(b => b.inRange( s1 ));
                s2.bot_count = bots.Count(b => b.inRange( s2 ));
                s3.bot_count = bots.Count(b => b.inRange( s3 ));
                s4.bot_count = bots.Count(b => b.inRange( s4 ));
                s5.bot_count = bots.Count(b => b.inRange( s5 ));
                s6.bot_count = bots.Count(b => b.inRange( s6 ));
                s7.bot_count = bots.Count(b => b.inRange( s7 ));
                s8.bot_count = bots.Count(b => b.inRange( s8 ));

                // remove current from list (the first in the sorted list)
                spaces.RemoveAt(0);
                // Add new spaces (if they have bots, no need for adding empty spaces)
                if (s1.bot_count > 0) spaces.Add(s1);
                if (s2.bot_count > 0) spaces.Add(s2);
                if (s3.bot_count > 0) spaces.Add(s3);
                if (s4.bot_count > 0) spaces.Add(s4);
                if (s5.bot_count > 0) spaces.Add(s5);
                if (s6.bot_count > 0) spaces.Add(s6);
                if (s7.bot_count > 0) spaces.Add(s7);
                if (s8.bot_count > 0) spaces.Add(s8);

                // Next round!
            }

        }
    }

    public class Space{
        public int bot_count;
        public int x, y,z; // lowerleft corner;
        public int size;
        public int dist => Math.Abs(x)+Math.Abs(y)+Math.Abs(z);

        public override string ToString(){
            return $"{x},{y},{z} with size {size} at distance {dist} with {bot_count}";
        }
    }

    public class NanoBot{
        public int x {get;set;}
        public int y {get;set;}
        public int z {get;set;}
        public int r {get;set;}

        public int dist (NanoBot other){
            return (Math.Abs(x-other.x) + Math.Abs(y-other.y)+ Math.Abs(z-other.z));
        }

        public int dist ( int x, int y, int z){
            return (Math.Abs(x-this.x) + Math.Abs(y-this.y)+ Math.Abs(z-this.z));
        }

        public bool inRange(int x, int y, int z){
            return dist(x,y,z) <= this.r;
        }

        // is a box with given min/max x,y,z in range of this bot
        // does this bot range (partly) overlaps with the box
        public bool inRange(int minx, int maxx, int miny, int maxy, int minz, int maxz){
            // take distances
            var d = 0;
            if (x > maxx) d += Math.Abs(x - maxx);
            if (x < minx) d += Math.Abs(minx - x);
            if (y > maxy) d += Math.Abs(y - maxy);
            if (y < miny) d += Math.Abs(miny - y);
            if (z > maxz) d += Math.Abs(z - maxz);
            if (z < minz) d += Math.Abs(minz - z);
            return d <= this.r;
        }

        public bool inRange(Space s){
            return this.inRange (s.x, s.x+s.size-1,  s.y, s.y+s.size-1, s.z, s.z+s.size-1);
        }


        public override string ToString(){
            return $"[{x},{y},{z}] {r}";
        }
        public static NanoBot fromLine(string line){
            var tokens = line.Split(new char[]{',','>','<','='}, StringSplitOptions.RemoveEmptyEntries);

            return new NanoBot(){
                x = int.Parse(tokens[1]),
                y = int.Parse(tokens[2]),
                z = int.Parse(tokens[3]),
                r = int.Parse(tokens[5])
            };
        }
    }
}
