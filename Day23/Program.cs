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
            // find size
            var max = new List<int>{ maxx-minx, maxy-miny, maxz-minz }.Max();
            var startsize = 1;
            while(startsize < max){
                startsize *= 2;
            }
            Console.WriteLine($"Size: {startsize}");

            var spaces = new List<Space>();

            // create fist search box
            var sb = new Space{ x = minx, y = miny, z = minz, size = startsize, bot_count = bots.Count};

            // Add first space 
            spaces.Add(sb);
            while(true){
                // sort, space with most bots first, then by distance, then by size;
                spaces = spaces.OrderByDescending(x =>x.bot_count).ThenBy(x => x.dist).ThenBy(x => x.size).ToList();
                var s = spaces.First(); 
                if (s.size == 1){
                    Console.WriteLine($"DONE: {s}");
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

                // remove current from list
                spaces.RemoveAt(0);
                // Add new spaces (if they have bots)
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




        public void Part2(string data){
            Console.WriteLine("Part2");
            var lines = data.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            var bots = lines.Select(NanoBot.fromLine).ToList();
            Console.WriteLine($"Got {bots.Count()} bots.");

            Console.WriteLine($"x range: {bots.Min(b => b.x)} {bots.Max(b => b.x)}");
            Console.WriteLine($"y range: {bots.Min(b => b.y)} {bots.Max(b => b.y)}");
            Console.WriteLine($"z range: {bots.Min(b => b.z)} {bots.Max(b => b.z)}");

            

            bool[][] overlaps = new bool[bots.Count()][];
            for(int i = 0; i < bots.Count(); i++){
                overlaps[i] = new bool[bots.Count()];
            }



            // vind de meeste overlappend bollen
            // overlap als b1.r+b2.r >= dist(b1.b2)
            for(int i = 0; i < bots.Count(); i++){
                for(int j = i; j< bots.Count(); j++){
                    if (bots[i].r + bots[j].r >= bots[i].dist(bots[j])){
                        overlaps[i][j] = true; // i <= j
                        overlaps[j][i] = true; // i <= j
                        
                    }
                }
            }
/*
            for(int i = 0; i < bots.Count(); i++){
                Console.Write($"Bot {i}: \t");
                for(int j = 0; j< bots.Count(); j++){
                    Console.Write($"{overlaps[i][j]}\t");
                }
                Console.WriteLine();
            }
*/
            var counts = overlaps.Select((r,i)=> new{index = i, count =  r.Count( x=> x)});

            var k = 0;
            var k_max= 0;
            var k_max_index = 0;
            foreach(var c in counts.OrderByDescending(x => x.count)){
                k++;
                //Console.WriteLine($"{c.index} - {c.count} => {c.count*k}");
                if (c.count*k > k_max){
                    k_max = c.count*k;
                    k_max_index = k;
                }
            }
            
            // bots sorted by amount of overlapping
            // take first n where first n have max total overlaps.
            Console.WriteLine($"Max overlap with first {k_max_index} bots.");
            
            // bounding box check? for x, y and z
            var sorted =counts.OrderByDescending(x => x.count).ToList();

            int minx,miny,minz,maxx, maxy,maxz;
            minx=miny=minz = int.MinValue;
            maxx=maxy=maxz = int.MaxValue;
                        
            for(int i = 0; i < k_max_index; i++){
                var b = bots[sorted[i].index];
                minx = Math.Max( minx, b.x-b.r);
                maxx = Math.Min( maxx, b.x+b.r);

                miny = Math.Max( miny, b.y-b.r);
                maxy = Math.Min( maxy, b.y+b.r);

                minz = Math.Max( minz, b.z-b.r);
                maxz = Math.Min( maxz, b.z+b.r);
            }
            Console.WriteLine($"{minx},{miny},{minz}  - {maxx},{maxy},{maxz}");

            //var d = Math.Abs(minx)+Math.Abs(miny)+Math.Abs(minz);
            var d = ((maxx-minx)/2+minx)  + ((maxy-miny)/2+miny) + ((maxz-minz)/2+minz);
            Console.WriteLine($"Distance {d}");

            // 105520157 is te laag.
            // 119894328 is te laag.

            // 120025394 niet goed

            // 124025394 is te hoog
            

            /*
            var step = 1_000_000;
            var max_in_range = 0;
            for(int x = minx; x<maxx; x += step){
                for(int y = miny; y<maxy; y += step){
                    for(int z = minz; z<maxz; z += step){
                        var b_in_range = bots.Count(b => b.inRange(x, y, z));
                        //Console.WriteLine($"{b_in_range}");            
                        if (b_in_range > max_in_range){
                            max_in_range = b_in_range;
                        }

                    }   
                    //Console.WriteLine($"y: {y}")   ;
                }
                //Console.WriteLine($"x: {x}")   ;
            }
            Console.WriteLine($"Max: {max_in_range}");
            */

            // alternatief
            // doe een stap vanaf mean in elke richting (+x -x, +y -y +z -z) met grote 1_000_000, net zolang als in_range stijgt.
            // daarna met stap 100_000 etc etc
            var mean_x = 50995978; //(minx+maxx)/2;//bots.Sum(b => b.x/1000);
            var mean_y = 21678597; //(miny+maxy)/2;//bots.Sum(b => b.y/1000);
            var mean_z = 48819396; //(minz+maxz)/2;//bots.Sum(b => b.z/1000);
            var mean_in_range = bots.Count(b => b.inRange(mean_x, mean_y, mean_z));
            Console.WriteLine($"Mean: {mean_x},{mean_y},{mean_z}  => {mean_in_range}");

            // s = 1_000_000;
            // Console.WriteLine($" ==> {s}");
            // Console.WriteLine( bots.Count(b => b.inRange(mean_x+s, mean_y, mean_z)));
            // Console.WriteLine( bots.Count(b => b.inRange(mean_x-s, mean_y, mean_z)));
            // Console.WriteLine( bots.Count(b => b.inRange(mean_x, mean_y+s, mean_z)));
            // Console.WriteLine( bots.Count(b => b.inRange(mean_x, mean_y-s, mean_z)));
            // Console.WriteLine( bots.Count(b => b.inRange(mean_x, mean_y, mean_z+s)));
            // Console.WriteLine( bots.Count(b => b.inRange(mean_x, mean_y, mean_z-s)));

            // mean_x -= s;
            // mean_y += s;
            // mean_z -= s;


            // s = 1_000_000;
            // Console.WriteLine($" ==> {s}");
            // Console.WriteLine( bots.Count(b => b.inRange(mean_x+s, mean_y, mean_z)));
            // Console.WriteLine( bots.Count(b => b.inRange(mean_x-s, mean_y, mean_z)));
            // Console.WriteLine( bots.Count(b => b.inRange(mean_x, mean_y+s, mean_z)));
            // Console.WriteLine( bots.Count(b => b.inRange(mean_x, mean_y-s, mean_z)));
            // Console.WriteLine( bots.Count(b => b.inRange(mean_x, mean_y, mean_z+s)));
            // Console.WriteLine( bots.Count(b => b.inRange(mean_x, mean_y, mean_z-s)));


            int maxIndex = 0;
            int maxCount = 0;

            // vind de bot met die bij meeste in range is
            for(int i = 0; i < bots.Count(); i++){
                var rcount = 0;
                for(int j = 0; j< bots.Count(); j++){
                    if (bots[j].inRange(bots[i].x, bots[i].y, bots[i].z)){
                        rcount++;
                    }

                }
                if (rcount >  maxCount){
                    maxIndex = i;
                    maxCount = rcount;
                }

            }

            // Bot with most in range:
            var max_bot = bots[maxIndex];
            Console.WriteLine($"{max_bot} has in range: {maxCount}");

            //mean_x = max_bot.x;
            //mean_y = max_bot.y;
            //mean_z = max_bot.z;

            var best = bots.Count(b => b.inRange(mean_x, mean_y, mean_z)); // aantal in range op huidige locatie.
            Console.WriteLine($"{mean_x},{mean_y},{mean_z} has in range {best}");
            int best_x = mean_x;
            int best_y = mean_y;
            int best_z = mean_z;
            int min_dist_at_best = int.MaxValue;

            var step = 1_000_000;
            var offset = 40;

            for(int s = step; s >= 1; s = s/10){
                Console.WriteLine($"Step size: {s}  current best {best}");
                for(int x = mean_x-(offset*s); x<mean_x+(offset*s); x += s){
                    //Console.WriteLine($"{mean_x} -> {x}");
                    for(int y = mean_y-(offset*s); y<mean_y+(offset*s); y += s){
                        for(int z = mean_z-(offset*s); z<mean_z+(offset*s); z += s){
                            var cnt_range = bots.Count(b => b.inRange(x, y, z));
                            
                            if (cnt_range > best){
                                Console.WriteLine($"{x},{y},{z}  {cnt_range}  d={Math.Abs(x)+Math.Abs(y)+Math.Abs(z)}");
                                best = cnt_range;
                                best_x = x;
                                best_y = y;
                                best_z = z;
                                min_dist_at_best = Math.Abs(best_x)+Math.Abs(best_y)+Math.Abs(best_z);
                            }else if (cnt_range == best){ // same in range, but closer
                                if (Math.Abs(x)+Math.Abs(y)+Math.Abs(z) < min_dist_at_best){
                                    Console.WriteLine($"{x},{y},{z}  {cnt_range}  d={Math.Abs(x)+Math.Abs(y)+Math.Abs(z)} (CLOSER)");
                                    best_x = x;
                                    best_y = y;
                                    best_z = z;
                                    min_dist_at_best = Math.Abs(best_x)+Math.Abs(best_y)+Math.Abs(best_z);
                                }
                            }
                        }
                    }
                }
                mean_x = best_x; mean_y = best_y; mean_z = best_z; // Move to new best location.
                Console.WriteLine($"Best: {best}  @ {best_x},{best_y},{best_z}  dist = {Math.Abs(best_x)+Math.Abs(best_y)+Math.Abs(best_z)}");
            }

            


/*/
            for(int s = 1_000_000; s> 0; s = s/10){
                for(int i = 0; i < 100; i++){
                    if (bots.Count(b => b.inRange(mean_x+s, mean_y, mean_z)) > best){
                        mean_x += s; moved = true;
                    }else if (bots.Count(b => b.inRange(mean_x-s, mean_y, mean_z)) > best){
                        mean_x -= s; moved = true;
                    }else if (bots.Count(b => b.inRange(mean_x, mean_y+s, mean_z)) > best){
                        mean_y += s; moved = true;
                    }else if (bots.Count(b => b.inRange(mean_x, mean_y-s, mean_z)) > best){
                        mean_y -= s; moved = true;
                    }else if (bots.Count(b => b.inRange(mean_x, mean_y, mean_z+s)) > best){
                        mean_z += s; moved = true;
                    }else if (bots.Count(b => b.inRange(mean_x, mean_y, mean_z-s)) > best){
                        mean_z -= s; moved = true;
                    }
                    if (moved){
                        best = bots.Count(b => b.inRange(mean_x, mean_y, mean_z));
                        moved = false;
                    }
                    
                    Console.WriteLine($"{s} moved: {moved} now in range {best} - {mean_x},{mean_y},{mean_z}");
                }
            }
            */

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
