using System;
using System.Linq;

namespace Day17
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.Clear();
            var s = new Solver();
            s.InitGrid(Input.TestData);
            s.Part1a();

            var s2 = new Solver();
            s2.InitGrid(Input.Data); // 36173 too high
            s2.Part1a();
            //s2.Print();

        }
    }




    public class Solver{

        (int x, int y) spring = (500,0);
        char[,] grid;

        int minx, miny, maxx, maxy;

        public void InitGrid(string input){
            var lines = input.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            var veins = lines.Select(Vein.FromLine);

            minx = veins.Min(v => v.start.x);
            miny = veins.Min(v => v.start.y);

            maxx = veins.Max(v => v.stop.x);
            maxy = veins.Max(v => v.stop.y);

            Console.WriteLine($"{minx},{miny} - {maxx},{maxy}");

            var grid_w = maxx-minx +4+1;
            var grid_h = maxy+1;
            grid = new char[grid_w, grid_h]; // x,y

            for(int y =0; y < grid_h; y++){
                for(int x = 0; x< grid_w; x++){
                    grid[x,y] = '.';
                }
            }
            grid[500-minx+2, 0] = '+';

            foreach(var v in veins){
                if (v.direction == 0){ // horizontal
                    for(int x = v.start.x-minx+2; x <= v.stop.x-minx+1; x++){
                        grid[x,v.start.y] = '#';
                    }
                }else{ //ver
                    for(int y = v.start.y; y <= v.stop.y; y++){
                        grid[v.start.x-minx+2, y] = '#';
                    }
                }
            }
        }
        
        public void Part1a(){
            //Print();
            Console.WriteLine();
            MoveWater(spring.x -minx+2, 0);
            //Print();
           
            var sum = (from item in grid.Cast<char>()
                        where item == '|' || item == '~'
                        select item).Count();
            sum -= miny -1;
            Console.WriteLine($"Water at {sum}");

            var retained = (from item in grid.Cast<char>()
                        where item == '~'
                        select item).Count();
            Console.WriteLine($"Water retained {retained}");
            
        }

        //return if water is flowing.
        public bool MoveWater(int x, int y){
            
            if (y > maxy) // dropped of the grid.
              return true;


            if (grid[x,y] == '#'){  // wall
                return false; 
            }else if (grid[x,y] == '~'){ // still water
                return false;
            }
            //Print(x,y);

            if (grid[x,y] == '|'){
                return true; // into already flowint water
            }
            
            //flow here
            if (grid[x,y]!= '+')
                grid[x,y] = '|';

            var down = MoveWater(x, y+1); // try down

            if(!down){ // cannot go down. split left and right
                MoveWater(x-1, y);
                MoveWater(x+1, y);

                // check row. 
                var xl = x;
                var xr = x;
                var left_closed = false;
                var right_closed = false;
                while(grid[xl, y] =='|'){xl--;};
                while(grid[xr, y] =='|'){xr++;};
                left_closed = (grid[xl,y] == '#' || grid[xl,y]=='~');
                right_closed = (grid[xr,y] == '#' || grid[xr,y]=='~');
                //Console.WriteLine($" L: {xl}, R: {xr}  {left_closed} {right_closed}");

                if (left_closed && right_closed){ 
                    for(int i = xl+1;  i< xr; i++){
                        grid[i,y] = '~';
                    }
                    return false; // row is filled, thus no flowing water.
                }
                return true; // left or right is open, thus flowing water.
            }

            return true; // down was ok, thus flowing

        }

        public void Print(int xx = -1, int yy = -1){
            //Console.SetCursorPosition(0,0);
            for(int y = 0; y< grid.GetLength(1);y++){
                for(int x = 0; x< grid.GetLength(0);x++){
                    if (x == xx && y == yy) Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(grid[x,y]);
                    if (x == xx && y == yy) Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine();
            }
        }
    }

    public class Vein{
        public (int x, int y) start;
        public (int x, int y) stop;

        public int direction = 0; // 0 = horizontal, 1 = vertical

        public static Vein FromLine(string input){
            var tokens = input.Split(", ", StringSplitOptions.RemoveEmptyEntries);
            
            var a = int.Parse(tokens[0].Substring(2));
            var b2 = tokens[1].Substring(2).Split('.', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
            
            var v = new Vein();
            if (tokens[0][0] == 'x'){ // vertical
                v.direction = 1;
                v.start = (a, b2.First());
                v.stop = (a, b2.Last());
            } else{
                v.direction = 0;
                v.start = (b2.First(), a);
                v.stop = (b2.Last(), a);
            }
            return v;
        }
    }
}
