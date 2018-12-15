using System;
using System.Collections.Generic;
using System.Linq;

// For data:
// too high
// 230332
// 251591
// 250668

// too low
// 227744

// Corrext: 229798. Took way to long.

namespace Day15
{
    class Program
    {
        static void Main(string[] args)
        {

            var data = new List<String>(){ 
                //Input.TestData, Input.TestData1,Input.TestData2,Input.TestData3,Input.TestData4,Input.TestData5, 
                //Input.TestData6,
                Input.Data

                };
            // Part 1
            // foreach(var input in data){
            //     var g = new Game();
            //     g.parseInput(input);
            //     g.Part1();
            //     Console.ReadKey();
            // }

            // Part 2
            bool elfDeath = true;
            int elfPower = 4;
            while(elfDeath){
                try{
                    elfPower++;
                    var g = new Game();
                    g.parseInput(data[0], elfPower);
                    g.Part2();
                    Console.WriteLine($"elfPower = {elfPower}");
                    elfDeath = false;
                    //Console.ReadKey();

                }catch(Exception e){
                    elfDeath = true;
                    Console.WriteLine($"ELF DIED, elfPower = {elfPower}");
                }
            }

        }
    }

    public class Game{

        public List<Actor> actors = new List<Actor>();
        public char[,] grid;
        public int round = 0;
        public int W, H;

        public void Part1(){
            Print();
            while(!Round()){
                //Console.ReadKey();
            }
        }

        public void Part2(){
            //Print();
            while(!Round(true)){
                //Console.ReadKey();
            }
        }

        public bool Round(bool throwOnElfDeath = false){
            // Foreach actor
            // if (in range) attack
            // else move and attack
            actors.Sort();
            foreach(var a in actors){
                //Console.WriteLine($"Actions for {a} ");
                if (a.Dead) continue; // Dead don't slay (day 16: zombie elfs?)

                var elfCount = actors.Where(e => e.IsElf && !e.Dead).Count();
                var goblinCount = actors.Where(g => !g.IsElf && !g.Dead).Count();
                if (elfCount == 0 || goblinCount == 0){
                    Console.WriteLine("ALL DONE ");
                    Print();
                    return true;
                }

                // in range?
                var enemy = SelectEnemy(a);
                if (enemy == null){
                    ///Console.WriteLine("\tNothing in range.");
                    FindTargetAndMove(a);
                    enemy = SelectEnemy(a);;
                }
                if (enemy != null){
                    //Console.WriteLine($"\tAttacking! {attack}");
                    enemy.HP -= a.Power;
                    if (enemy.Dead){
                        if (enemy.IsElf && throwOnElfDeath)
                            throw new Exception("Elf died!");
                        grid[enemy.y,enemy.x] = '.';
                    }
                }
            }
            //Print();
            round++;
            return false;
        }   

        public Actor SelectEnemy(Actor a){
            return actors.Where(g => g.IsElf != a.IsElf && g.NextTo(a) && !g.Dead).OrderBy(g => g.HP).ThenBy(g => g).FirstOrDefault();
        }

        public void FindTargetAndMove(Actor a){
            //Console.WriteLine($"Finding targer for {a}");
            // flood fill to find possible targets and distances
            List<(int x, int y, int h)> closed = new List<(int,int, int)>();
            Queue<(int x, int y, int h)> open = new Queue<(int,int, int)>();
            List<(int x, int y, int d)> enemyLocations = new List<(int x, int y, int d)>();
            List<(int x, int y, int d)> inRange = new List<(int x, int y, int d)>(); // locations with target in range.
            open.Enqueue((a.x, a.y, 0));

            while(open.Count> 0 ){
                // check all neighbours
                var p = open.Dequeue();

                // is in Range?
                if (actors.Where(e => e.IsElf != a.IsElf && !e.Dead).Where(e => e.NextTo(p.x, p.y)).Any())
                    inRange.Add((p.x, p.y, p.h));  // this location is in range of an enemy

                process(p.x, p.y-1, p.h);
                process(p.x-1, p.y, p.h);
                process(p.x+1, p.y, p.h);
                process(p.x, p.y+1, p.h);
                closed.Add(p);
            }
            
            if (enemyLocations.Count > 0){
                var minDistRange = inRange.Min(e => e.d);
                var nearRange = inRange.Where(e => e.d == minDistRange).OrderBy(r => r.y).ThenBy(r => r.x).ToList();

                //System.Console.WriteLine($"Near inRange locations: {string.Join(", ", nearRange)}");

                var firstSteps = nearRange.Take(1).Select(t => FindPath(a, (t.x, t.y))).ToList(); // get the first step op path needed to reach each enemy
                firstSteps = firstSteps.OrderBy(s => s.y).ThenBy(s => s.x).ToList();  

                //System.Console.WriteLine($"First Steps (sorted) {string.Join(", ", firstSteps)}");

                MoveTo(a, firstSteps.First().x, firstSteps.First().y);

            }else{
                //Console.WriteLine($"\t{a} @ found NO reachable enemy");
            }

            void process (int x, int y, int h){
                //Console.Write($"\t\tProcessing {x},{y} >>");
                if (closed.Any( c => c.x == x && c.y == y )){ // already visited.
                    //Console.WriteLine("Already seen");
                     if (closed.Any( c => c.x == x && c.y == y && c.h > h))
                        throw new Exception($"Shorterpath found to {x},{y}");
                    return;
                }

                if (grid[y,x] == '#'){
                    //Console.WriteLine("wall");
                }else if (grid[y,x] == 'E' && !a.IsElf){ // enemy!
                    //Console.WriteLine("ENEMY");
                    enemyLocations.Add((x,y, h));
                }else if (grid[y,x] == 'G' && a.IsElf){ // enemy!
                    //Console.WriteLine("ENEMY");
                    enemyLocations.Add((x,y, h));
                }else if (grid[y,x] == '.'){
                    //Console.WriteLine("open");
                    if (!open.Any( o => o.x == x && o.y == y)) // alleen toevoegen als nog niet in 'open'
                        open.Enqueue((x,y,h+1));
                }else{
                    //Console.WriteLine("Same team");
                }
            }
        }

        // find path from a to target and return first step
        public (int x, int y) FindPath(Actor a, (int x, int y) target){
            //Console.WriteLine($"\t Path from {a} to {target}");
            // flood fill for all paths.
            List<(int x, int y, int h)> closed = new List<(int,int, int)>();
            Queue<(int x, int y, int h)> open = new Queue<(int,int, int)>();

            open.Enqueue((target.x, target.y, 0));

            while(open.Count> 0){
                // check all neighbours
                var p = open.Dequeue();
                //Console.WriteLine($"> {p.x},{p.y}  open: {open.Count} closed: {closed.Count}");
                process(p.x, p.y-1, p.h);
                process(p.x-1, p.y, p.h);
                process(p.x+1, p.y, p.h);
                process(p.x, p.y+1, p.h);
                closed.Add(p);
            }

            // closed has all paths lengths;
            var nextPos = closed.Where(c => a.NextTo(c.x, c.y)).OrderBy(c => c.h).ThenBy(c => c.y).ThenBy(c => c.x);
            //Console.WriteLine($"Next steps: {string.Join(",",nextPos)}");

            return((nextPos.First().x, nextPos.First().y));


            void process (int x, int y, int h){
                //Console.Write($"\t\tProcessing {x},{y} >>"); 
                if (closed.Any( c => c.x == x && c.y == y )){ // already visited.
                    //Console.WriteLine("Already seen");
                    if (closed.Any( c => c.x == x && c.y == y && c.h > h))
                        throw new Exception($"Shorterpath found to {x},{y}");
                    return;
                }
                // if (a.x == x && a.y == y){
                //     //Console.WriteLine("It is ME!!");
                //     return true;
                // }

                if (grid[y,x] == '.'){
                    //Console.WriteLine(" open");
                    if (!open.Any( o => o.x == x && o.y == y)) // alleen toevoegen als nog niet in 'open'
                        open.Enqueue((x,y,h+1));
                }else{
                    //Console.WriteLine(" wall/actor");
                }
            }
        }
        
        public void MoveTo (Actor a, int x, int y){
            //Console.WriteLine($"\tMoving {a} to {x},{y}");
            grid[a.y, a.x] = '.';
            a.x = x;
            a.y = y;
            grid[a.y, a.x] = a.IsElf?'E':'G';
        }

        public void Print(){
            var hpSum = actors.Where(a => !a.Dead).Sum(a => a.HP);
            //Console.SetCursorPosition(0,0);
            Console.WriteLine($"After round {round} -> {hpSum} => {round*hpSum}");
            for(int y= 0; y < H ; y++){
                for(int x= 0; x< W; x++){
                    Console.Write(grid[y,x]);
                }
                Console.Write("\t");
                Console.Write(string.Join(" ,", actors.Where(a => a.y == y && !a.Dead).OrderBy(a => a.x).Select(a => a.ToString())));
                Console.WriteLine();
            }
        }

        

        public void parseInput(string data, int elfPower = 3){
            var lines = data.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            H = lines.Length;
            W = lines[0].Length;
            grid = new char[H, W];
            for(int y = 0; y < H; y++){ // y downard
                for(int x = 0; x < W; x++){
                    grid[y,x] = lines[y][x];
                    if (lines[y][x] == 'E'){
                        actors.Add(new Elf(elfPower){ x = x, y = y});
                    }else if (lines[y][x] == 'G'){
                        actors.Add(new Goblin(){x = x, y=y});
                    }
                }
            }
        }
    }
    


    public abstract class Actor : IComparable{

        public int x;
        public int y;
        public int HP = 200;
        public int Power = 3;
        public bool Dead => HP <= 0;

        public abstract bool IsElf {get; }

        public int CompareTo(object obj)
        {
            Actor other  = (Actor)obj;
            if (this.y < other.y)
                return -1;
            if (this.y > other.y)    
                return 1;
            if (this.x < other.x)
                return -1;
            if (this.x > other.x)
                return 1;
            return 0;
        }
        // returs true if next to given location
        public bool NextTo(Actor a){
            return NextTo(a.x, a.y);
        }
        public bool NextTo(int x, int y){
            return  (this.x == x && (this.y == y-1 || this.y == y+1))
                    ||
                    (this.y == y && (this.x == x-1 || this.x == x+1));
        }
    }
    public class Elf: Actor{
        public Elf(int power)
        {
            this.Power = power;
        }
        public override bool IsElf => true;
        public override string ToString(){
            return $"E({HP}[{x},{y}])";
        }
    }
    public class Goblin: Actor{
        public override bool IsElf => false;
        public override string ToString(){
            return $"G({HP}[{x},{y}])";
        }

    }
}
