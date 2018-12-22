using System;
using System.Collections.Generic;
using System.Linq;

namespace Day22
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = new Solver();
            // Part1
            //s.Part1(510, 10, 10); // test
            //s.Part1(10689, 11, 722);
            //s.Print();

            // Part2
            //s.Part1(510, 10, 10, 10); // test
            s.Part1(10689, 11, 722, 300); // add 300 extra range 
            s.Part2();


        }
    }

    public class Solver{
        public long[,] geo;
        public long[,] ero;
        public int[,] type;

        int xt, yt;

        public void Part1(long depth, int x_t, int y_t, int extra = 1){
            xt = x_t;
            yt = y_t;
            geo = new long[xt+extra,yt+extra];
            ero = new long[xt+extra,yt+extra];
            type = new int[xt+extra,yt+extra];

            for(int y = 0; y< yt+extra; y++){
                for(int x= 0; x <xt+extra;x++){
                    if ((x==0 && y==0) || x==xt && y==yt){
                        geo[x,y] = 0;
                    }else if (y==0){
                        geo[x,y] = x*16807;
                    }else if (x==0){
                        geo[x,y] = y*48271;
                    }else{
                        geo[x,y] = ero[x-1,y]*ero[x,y-1];
                    }
                    ero[x,y] = (geo[x,y]+depth)%20183;
                    type[x,y] = (int) (ero[x,y] %3); // - 0 rocky, 1 wet, 3 narrow
                }
            }
        }

        Dictionary< (int x, int y, int tool),(int g, int h)> open = new Dictionary<(int x, int y, int tool), (int g, int h)>();
        Dictionary< (int x, int y, int tool),(int g, int h)> closed = new Dictionary<(int x, int y, int tool), (int g, int h)>();


        public void Part2(){
            // tool: 0=neither, 1=torch, 2=climbing gear

            // add start to open
            open.Add( (0,0,1), (0,dist(0,0)));

            // current (from open list)
            int x = 0;
            int y = 0;
            int t = 1;
            int d = 0;

            while(open.Count > 0){
                //Console.WriteLine($"Open: {open.Count}");
                //Console.WriteLine($"Closed: {closed.Count}");

                var current = open.OrderBy( o=> o.Value.g + o.Value.h ).First();
                x = current.Key.x;
                y = current.Key.y;
                t = current.Key.tool;
                d = current.Value.g;

                //Console.WriteLine($"Handling: {x},{y} tool:{t} @ distance: {d}");

                if (x == xt && y == yt && t == 1){
                    Console.WriteLine($"Found path, length: {d}");
                    return;
                }

                // remove from open and store in closed
                open.Remove( current.Key);
                closed.Add( current.Key, current.Value);

                // try all neighbours with current tool
                foreach( var n in Neighbours(x,y)){

                    // try with current tool
                    walkTo(n.x, n.y, d+1, t);
                }
                // switch tool
                if (t !=0)
                    walkTo(x,y,d+7,0);
                if (t !=1)  
                    walkTo(x,y,d+7,1);
                if (t !=2)
                    walkTo(x,y,d+7,2);
               
                //Console.ReadLine();
            }

        }

        void walkTo(int nx, int ny, int d, int tool){
            //Console.Write($"\tTrying {nx},{ny} with {tool} after {d}\t");
            if (type[nx,ny] == 0 && tool == 0){ /* Console.WriteLine("invalid");*/ return;} // dont allow rocky with neither
            if (type[nx,ny] == 1 && tool == 1){ /* Console.WriteLine("invalid");*/ return;} // dont allow wet with torch
            if (type[nx,ny] == 2 && tool == 2){ /* Console.WriteLine("invalid");*/ return;} // dont allow narrow with climbing gear

            // with current tool.
            if (closed.ContainsKey( (nx,ny,tool))) { /* Console.WriteLine("in closed");*/ return;}  // already here with current tool.

            if (open.ContainsKey ( (nx, ny, tool))){ // kennen we al, zijn we nu sneller?
                //Console.WriteLine("update open");
                var (g,h) = open[ (nx, ny, tool)];

                if (d < g ){ // faster! Update,  only compare g (h is the same from here)
                    open[(nx, ny, tool)] = (d, h);
                }

            }else{ // nieuw; voeg toe.
                //Console.WriteLine("new for open");
                open.Add( (nx, ny, tool), (d, dist(nx, ny)));
            }
        }


        public IEnumerable<(int x, int y)> Neighbours(int x, int y){
            if (x>0){
                yield return (x-1,y);
            }
            if (y>0){
                yield return (x,y-1);
            }
            yield return (x+1,y);
            yield return (x,y+1);
        }


        public int dist(int x, int y){
            return Math.Abs(xt-x)+Math.Abs(yt-y);
        }

        public void Print(int extra = 1){
            int risk = 0;
            for(int y = 0; y< yt+extra; y++){
                for(int x= 0; x <xt+extra;x++){
                    risk += type[x,y];
                    if (x==0 && y==0)
                        Console.Write("M");
                    else if (x==xt && y==yt)
                        Console.Write("X");
                    else
                        Console.Write( type[x,y]==0?'.': type[x,y]==1? '=' : '|' );
                    //Console.Write(type[x,y]);
                }
                Console.WriteLine();
            }

            Console.WriteLine($"Total risk: {risk}");
        }

        public void Print(long[,] data){
            for(int y = 0; y< xt+1; y++){
                for(int x= 0; x <xt+1;x++){
                    // if (x==0 && y==0)
                    //     Console.Write("M\t");
                    // else if (x==xt && y==yt)
                    //     Console.Write("X\t");
                    // else
                        //Console.Write( type[x,y]==0?'.': type[x,y]=='1'? '=' : '|' );
                        Console.Write($"{data[x,y]}\t");
                }
                Console.WriteLine();
            }
        }
    }
}
