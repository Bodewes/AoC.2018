using System;

namespace Day18
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = new Solver();
            s.Init(Input.Data);
            s.Part1(1000);
            // periode is 28 
            // => 35714250*28 + 1000 = 1_000_000_000
            // step 1000 is gelijk aan 1_000_000_000 (maar dan 35714250 periodes later)

        }
    }

    public class Solver{

        char[,] grid;
        int size;


        public void Part1(int steps){
            Print(-1);
            for(int i = 0; i < steps; i++){
                Step();
                //if (i % 1000 == 999)
                    Print(i);
            }

        }


        public void Step(){
            var grid2 = new char[size,size];
            for(int x= 0; x< size; x++){
                for(int y = 0; y <size; y++){
                    grid2[x,y] = GetNewState(x,y);
                }
            }
            grid = grid2;
        }


        public char GetNewState(int x, int y){
            var c = Count(x,y);
            if(grid[x,y] == '.' && c.t >= 3) return '|';
            if(grid[x,y] == '|' && c.y >= 3) return '#';
            if(grid[x,y] == '#'){
                if (c.y >= 1 && c.t >= 1) return '#';
                else return '.';
            }
            return grid[x,y];
        }
/* 
. open
| tree
# lumberyard
An open acre will become filled with trees if three or more adjacent acres contained trees. Otherwise, nothing happens.
An acre filled with trees will become a lumberyard if three or more adjacent acres were lumberyards. Otherwise, nothing happens.
An acre containing a lumberyard will remain a lumberyard if it was adjacent to at least one other lumberyard and at least one acre containing trees. Otherwise, it becomes open.
*/
        public (int o, int t, int y) Count(int x, int y)
        {
            return (Count(x,y,'.'), Count(x,y,'|'), Count(x,y,'#'));
        }

        public int Count(int x, int y, char item){
            var c = 0;
            if (x > 0 && y > 0 && grid[x-1, y-1] == item) c++;
            if (x > 0 && grid[x-1, y] == item) c++;
            if (x > 0 && y < size-1 && grid[x-1, y+1] == item) c++;
            if (y > 0 && grid[x, y-1] == item) c++;
            //if (grid[x, y] == '.') open++;
            if ( y < size-1 && grid[x, y+1] == item) c++;
            if (x < size-1 &&  y > 0 && grid[x+1, y-1] == item) c++;
            if (x < size-1 && grid[x+1, y] == item) c++;
            if (x < size-1 &&  y < size-1 && grid[x+1, y+1] == item) c++;
            return c;
        }

        public void Init(string data){
            var lines = data.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            size = lines.Length;
            grid  = new char[size,size];
            for(int y =0; y < size; y++){
                for(int x = 0; x< size; x++){
                    grid[x,y] = lines[y][x];
                }
            }
        }
        public void Print(int iter = 0){
            int wood = 0;
            int yard = 0;
            for(int y =0; y < size; y++){
                for(int x = 0; x< size; x++){
                    //Console.Write(grid[x,y]);
                    if (grid[x,y] == '|') wood++;
                    if (grid[x,y] == '#') yard++;
                }
                //Console.WriteLine();
            }
            Console.WriteLine($"{iter+1}\tvalue = {wood*yard}");
        }

    }
}
