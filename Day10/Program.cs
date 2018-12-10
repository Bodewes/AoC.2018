using System;
using System.Linq;

namespace Day10
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = new Solver();
            s.Part1(Input.TestData);
            s.Part1(Input.Data);
        }
    }

    public class Solver{

        public void Part1(string lines){
            var points = lines.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).Select(Point.ParseLine).ToList();
            int timer = 0;
            long maxx, maxy, minx, miny;
            long psize  = long.MaxValue;
            long size = long.MaxValue;
            do{
                psize = size;

                // step:
                foreach(var p in points){
                    p.Pos = (p.Pos.x + p.Vel.x, p.Pos.y + p.Vel.y);
                }
                timer++;
                maxx = points.Max( x=> x.Pos.x);
                maxy = points.Max( x=> x.Pos.y);
                minx = points.Min( x=> x.Pos.x);
                miny = points.Min( x=> x.Pos.y);

                size = (maxx-minx)*(maxy-miny);
                Console.WriteLine($"{minx},{miny} - {maxx},{maxy} >> {size}");
            }while(size < psize); // one step to far.

            // step BACK:
            foreach(var p in points){
                p.Pos = (p.Pos.x - p.Vel.x, p.Pos.y - p.Vel.y);
            }


            Console.Clear();
            //Console.WriteLine($"{minx}-{miny} - {maxx}-{maxy} >> {size} @ {timer}");

            foreach(var p in points){
                Console.SetCursorPosition( (int)(p.Pos.x - minx),(int)( p.Pos.y-miny));
                Console.Write("#");
            }
            Console.SetCursorPosition(0, (int)(maxy+1-miny));
            Console.WriteLine($"{minx}{miny} - {maxx}{maxy} @ {timer-1}");


        }
    }

    public class Point{
        public (int x, int y) Pos{get;set;}
        public (int x, int y) Vel {get;set;}

        public static Point ParseLine(string line){
            var tokens = line.Split(new char[]{'<','>',','}, StringSplitOptions.RemoveEmptyEntries);
            var p = new Point(){
                Pos = (int.Parse(tokens[1]), int.Parse(tokens[2])),
                Vel = (int.Parse(tokens[4]), int.Parse(tokens[5]))
            };
            return p;
        }
    }
}
