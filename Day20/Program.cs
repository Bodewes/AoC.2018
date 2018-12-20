using System;
using System.Collections.Generic;
using System.Linq;

namespace Day20
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = new Solver();
            s.Part1(Input.Data, 1000);
        }
    }

    public class Solver{

        Dictionary<(int x, int y), int> grid = new Dictionary<(int x, int y), int>();

        public void Part1(string data, int minDoorCount){
            Walk(0,0, data, 1, 0);
            Print(minDoorCount);
        }

        public int Walk(int x, int y, string data, int i, int d){
            // walk till end or split.
            while(i < data.Length && data[i] != '$' && data[i] != '|' && data[i] != ')'){

                switch(data[i]){
                    case 'N':
                        y--;
                        d++;
                        AddOrUpdate(x,y,d);
                        break;
                    case 'E':
                        x++;
                        d++;
                        AddOrUpdate(x,y,d);
                        break;
                    case 'W':
                        x--;
                        d++;
                        AddOrUpdate(x,y,d);
                        break;
                    case 'S':
                        y++;
                        d++;
                        AddOrUpdate(x,y,d);
                        break;
                    case '(':
                        var k = i;
                        do{
                            // read away '(' or '|'
                            k = Walk(x,y,data,k+1,d);
                        }while ( data[k] != ')'); // zolang geen einde sub.
                        i = k;  
                        break;
                }
                i++;
            }
            return i;
        }


        public void Print(int m){
            var max = 0;
            var count = 0;
            var minx = grid.Keys.Min(k => k.x);
            var miny = grid.Keys.Min(k => k.y);
            var maxx = grid.Keys.Max(k => k.x);
            var maxy = grid.Keys.Max(k => k.y);
            for(int y = miny; y <= maxy; y++){
                for(int x = minx; x <= maxx; x++){
                    if (grid.ContainsKey( (x,y) )){
                        var d = grid[(x,y)];
                        Console.Write($"{d}\t");
                        if (d > max){
                            max = d;
                        }
                        if (d >= m){
                            count++;
                        }
                        

                    }else{
                        Console.Write("#\t");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine($"Max: {max}, paths with minimal {m} doors: {count}");
        }

        public void AddOrUpdate(int x, int y, int d){
            if (grid.ContainsKey( (x,y) )){
                if (grid[ (x,y)] > d ){
                    grid[ (x,y)] = d;
                }
            }else{
                grid.Add( (x,y), d);
            }
        }
    }


}
