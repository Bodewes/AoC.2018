using System;
using System.Collections.Generic;
using System.Linq;

namespace Day06
{
    class Program
    {
        static void Main(string[] args)
        {
            var solver = new Solver();
            solver.Part1(Input.TestData);
            solver.Part1(Input.Data, 400);

            solver.Part2(Input.TestData);
            solver.Part2(Input.Data, 400, 10000);
        }
    }

    public class Solver{
        public void Part1(string data, int gridSize = 10){
            var lines = data.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).ToList();
            var points = lines.Select( x=> x.Split(",")).Select(a => (x:Int32.Parse(a[0]), y:Int32.Parse(a[1]))).ToList();

            var grid = new int[gridSize,gridSize];
            for(int i = 0; i<gridSize; i++){ // y
                for(int j = 0 ; j< gridSize; j++){ // x
                    var minDist = int.MaxValue;
                    for(int p = 0; p< points.Count;p++ ){
                        int dist = Math.Abs(j-points[p].x) + Math.Abs(i-points[p].y);
                        if (dist < minDist){
                            grid[j,i] = p;
                            minDist = dist;
                        }else if (dist == minDist){
                            grid[j,i] = -1; // equidistance!
                        }
                    }
                }
            }

            var count = new int[points.Count];
            for(int i = 0; i<gridSize; i++){
                for(int j = 0 ; j< gridSize; j++){
                    //Console.Write( grid[j,i] == -1?".":grid[j,i].ToString());
                    if (grid[j,i] == -1){
                        // noop.
                    }else{
                        count[grid[j,i]]++;
                    }
                }
                //Console.WriteLine();
            }

            // walk edge:
            var edge = new List<int>();
            for(int i = 0; i< gridSize; i++){
                edge.Add(grid[0,i]);
                edge.Add(grid[gridSize-1,i]);
                edge.Add(grid[i,0]);
                edge.Add(grid[i,gridSize-1]);
            }
            edge.Distinct();
           
            Console.WriteLine(string.Join(",",count));
            Console.WriteLine(string.Join(",",edge.Distinct()));

            foreach(var i in edge.Distinct()){
                if (i>=0)
                    count[i] = -1;
            }
            Console.WriteLine(string.Join(",",count));
            Console.WriteLine(count.Max());


        }

        public void Part2(string data, int gridSize = 10, int limit = 32){
            var lines = data.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).ToList();
            var points = lines.Select( x=> x.Split(",")).Select(a => (x:Int32.Parse(a[0]), y:Int32.Parse(a[1]))).ToList();


            var grid = new int[gridSize,gridSize];
            for(int i = 0; i<gridSize; i++){ // y
                for(int j = 0 ; j< gridSize; j++){ // x
                    for(int p = 0; p< points.Count;p++ ){
                        int dist = Math.Abs(j-points[p].x) + Math.Abs(i-points[p].y);
                        grid[j,i] += dist;
                    }
                }
            }

            var count = 0;
            for(int i = 0; i<gridSize; i++){
                for(int j = 0 ; j< gridSize; j++){
                    //Console.Write( grid[j,i] == -1?".":grid[j,i].ToString());
                    if (grid[j,i] < limit){
                        count++;
                    }
                }
                //Console.WriteLine();
            }

            Console.WriteLine(count);


        }
    }
}
