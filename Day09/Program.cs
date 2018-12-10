using System;
using System.Collections.Generic;
using System.Linq;

namespace Day09
{
    class Program
    {
        static void Main(string[] args)
        {
            var solver = new Solver();
            solver.Part2(Input.TestData.Item1, Input.TestData.Item2);
            solver.Part2(Input.TestData1.Item1, Input.TestData1.Item2);
            solver.Part2(Input.TestData2.Item1, Input.TestData2.Item2);
            solver.Part2(Input.TestData3.Item1, Input.TestData3.Item2);
            solver.Part2(Input.TestData4.Item1, Input.TestData4.Item2);
            solver.Part2(Input.TestData5.Item1, Input.TestData5.Item2);
            
            solver.Part2(Input.Data.Item1, Input.Data.Item2);
            solver.Part2(Input.Data.Item1, Input.Data.Item2*100);
        }
    }

    public class Solver{

        public void Part1(int players, int max){
            int i = 0; // current index.
            var p = 0; // current player
            List<int> cirlce = new List<int>();
            cirlce.Add(0); // start

            var score = new long[players];

            for(int m = 1; m <= max; m++){
                p++; 
                if ( p> players) p =1;

                if (m % 23 == 0){ // score!
                    var toRemove = ((i + cirlce.Count) - 7)% cirlce.Count; 

                    score[p-1] += (m + cirlce[toRemove]);
                    cirlce.RemoveAt(toRemove);
                    i = toRemove;

                }else{
                    var nextto = (i+1)%cirlce.Count;
                    cirlce.Insert(nextto+1, m);
                    i = nextto+1;
                }

                if (m % 1000 == 0) Console.Write(".");
                //Console.WriteLine($"[{p}] "+string.Join(" ",cirlce)+ "   >>i:"+i+" >>"+m);

            }

            // for(int k = 0; k < players; k++){
            //     Console.WriteLine($"{k+1} : {score[k]}");
            // }
            Console.WriteLine(score.Max());
        }

        public void Part2(int players, int max){
            var p = 0; // current player
            var score = new long[players];

            Node current = new Node(0, null, null);
            current.CW = current;
            current.CCW = current;
            
            var start = current;

            for(int m = 1; m <= max; m++){
                p++; 
                if ( p> players) p =1;

                if (m % 23 == 0){ // score!
                    for(int i =0; i < 7; i++){
                        current = current.CCW;
                    }
                    score[p-1] += (m + current.Value);
                    var toRemove = current;
                    toRemove.CCW.CW = toRemove.CW;
                    toRemove.CW.CCW = toRemove.CCW;
                    current = toRemove.CW;

                }else{

                    var next = current.CW;

                    var n = new Node(m, next.CW, next);
                    next.CW.CCW = n;
                    next.CW = n;

                    current = n;
                }

            }
            Console.WriteLine(score.Max());

        }

        public class Node{
            public Node(int value, Node cw, Node ccw)
            {
                this.Value = value;
                this.CW = cw;
                this.CCW = ccw;
            }
            public int Value;
            public Node CW;
            public Node CCW;
        }
    }


    public class Input{
        public static (int,int) TestData = (9,25);

        public static (int,int) TestData1 = (10,1618);
        public static (int,int) TestData2 = (13,7999);
        public static (int,int) TestData3 = (17,1104);
        public static (int,int) TestData4 = (21,6111);
        public static (int,int) TestData5 = (30,5907);

        public static (int, int) Data = (459, 72103);
    }
}
