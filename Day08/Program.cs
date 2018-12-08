using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day08
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
        public void Part1(string data){
            var code = data.Split(new char[]{'\r','\n',' '}, StringSplitOptions.RemoveEmptyEntries).Select(x=> x.Trim()).Select(Int32.Parse).ToList();

            i = 0;
            summeta = 0;
            var root = parse(code);
            Console.WriteLine(root);
            Console.WriteLine("All Meta: "+summeta);
            Console.WriteLine("Node value: "+ root.Value());
            
        }




        private int i;
        private int summeta;
        private Node parse(List<int> code){
            var n = new Node();
            var childs = code[i++]; // child count
            var meta = code[i++]; // meta count
            Console.WriteLine($"{i}:>{childs}-{meta}");
            for(int c = 0; c < childs; c++){
                n.Childs.Add(parse(code));
            }
            for(int m = 0; m< meta; m++){
                n.Meta.Add(code[i++]);
            }
            summeta += n.Meta.Sum();
            return n;
        }
    }

    class Node{
        public Node()
        {
            Childs = new List<Node>();
            Meta = new List<int>();
        }
        public List<Node> Childs{get;set;}
        public List<int> Meta{get;set;}

        public int Value(){
            if (Childs.Count == 0){
                return Meta.Sum();
            }else{
                var sum = 0;
                for(int m = 0; m< Meta.Count; m++){
                    var cindex = Meta[m]-1; // index into childs
                    if (cindex >= 0 && cindex < Childs.Count ){ 
                        sum += Childs[cindex].Value();
                    }

                }
                return sum;
            }
        }

        public override string ToString(){
            return $" {Childs.Count} {Meta.Count} ({string.Join(" ", Childs)} [{string.Join(" ", Meta)}])";
        }
    }
}
