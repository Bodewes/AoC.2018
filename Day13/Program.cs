using System;
using System.Collections.Generic;
using System.Linq;

namespace Day13
{
    class Program
    {
        static void Main(string[] args)
        {
            Solver s= new Solver();
            s.SolvePart1(Input.TestData);
            s.SolvePart1(Input.TestData2);
            s.SolvePart1(Input.Data);
        }
    }

    public class Solver{
        public void SolvePart1(string input){
            var lines = input.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            var grid = new char[lines.Length, lines[0].Length];

            List<Cart> carts = new List<Cart>();
            // Find all carts + fill grid.
            for(int y=0; y< lines.Length; y++ ){
                for(int x = 0; x<lines[0].Length; x++ ){
                    //Console.WriteLine($"{x}-{y} = {lines[y][x]}");
                    grid[y,x] = lines[y][x];
                    switch(lines[y][x]){
                        case '>': 
                            carts.Add( new Cart(x,y,Direction.E)); 
                            grid[y,x] = '-';
                            break;
                        case '<':
                            carts.Add( new Cart(x,y,Direction.W)); 
                            grid[y,x] = '-';
                            break;
                        case '^':
                            carts.Add( new Cart(x,y,Direction.N)); 
                            grid[y,x] = '|';
                            break;
                        case 'v':
                            carts.Add( new Cart(x,y,Direction.S)); 
                            grid[y,x] = '|';
                            break;
                    }
                }
            }
            // Got all carts, filled grid.
            carts.ForEach(c => Console.WriteLine($"[{c.x},{c.y}]"));
            int cartCount = carts.Count;
            int i = 0;
            do{
                carts.Sort();
                foreach(Cart cart in carts){ // step!
                    if (cart.Dir == Direction.E){
                        cart.x++;
                        if (grid[cart.y,cart.x] == '\\')
                            cart.Dir = Direction.S;
                        if (grid[cart.y, cart.x] == '/')
                            cart.Dir = Direction.N;
                        if (grid[cart.y,cart.x] == '+'){
                            if (cart.Next == Intersection.Left){
                                cart.Dir  = Direction.N;
                                cart.Next = Intersection.Straight;
                            }else if (cart.Next == Intersection.Right){
                                cart.Dir = Direction.S;
                                cart.Next = Intersection.Left;
                            }else if (cart.Next == Intersection.Straight){
                                cart.Next = Intersection.Right;
                            }
                        }
                    }else if (cart.Dir == Direction.W){
                        cart.x--;
                        if (grid[cart.y,cart.x] == '\\')
                            cart.Dir = Direction.N;
                        if (grid[cart.y, cart.x] == '/')
                            cart.Dir = Direction.S;
                        if (grid[cart.y,cart.x] == '+'){
                            if (cart.Next == Intersection.Left){
                                cart.Dir  = Direction.S;
                                cart.Next = Intersection.Straight;
                            }else if (cart.Next == Intersection.Right){
                                cart.Dir = Direction.N;
                                cart.Next = Intersection.Left;
                            }else if (cart.Next == Intersection.Straight){
                                cart.Next = Intersection.Right;
                            }
                        }
                    }else if (cart.Dir == Direction.N){
                        cart.y--;
                        if (grid[cart.y,cart.x] == '\\')
                            cart.Dir = Direction.W;
                        if (grid[cart.y, cart.x] == '/')
                            cart.Dir = Direction.E;
                        if (grid[cart.y,cart.x] == '+'){
                            if (cart.Next == Intersection.Left){
                                cart.Dir  = Direction.W;
                                cart.Next = Intersection.Straight;
                            }else if (cart.Next == Intersection.Right){
                                cart.Dir = Direction.E;
                                cart.Next = Intersection.Left;
                            }else if (cart.Next == Intersection.Straight){
                                cart.Next = Intersection.Right;
                            }
                        }
                    }else if(cart.Dir == Direction.S){
                        cart.y++;
                        if (grid[cart.y,cart.x] == '\\')
                            cart.Dir = Direction.E;
                        if (grid[cart.y, cart.x] == '/')
                            cart.Dir = Direction.W;
                        if (grid[cart.y,cart.x] == '+'){
                            if (cart.Next == Intersection.Left){
                                cart.Dir  = Direction.E;
                                cart.Next = Intersection.Straight;
                            }else if (cart.Next == Intersection.Right){
                                cart.Dir = Direction.W;
                                cart.Next = Intersection.Left;
                            }else if (cart.Next == Intersection.Straight){
                                cart.Next = Intersection.Right;
                            }
                        }
                    }
                    // Check if collided with other carts.
                    var boom = carts.Where(c => c.x == cart.x && c.y == cart.y).ToList();
                    if (boom.Count == 2){
                        Console.WriteLine($"Collision @ {cart.x},{cart.y}");
                        //carts.RemoveAll(c => c.x == cart.x && c.y == cart.y);
                        boom.ForEach(x => x.Crashed= true);
                        cartCount -= 2;
                        Console.WriteLine($"Carts left: {cartCount}");
                    }
                }
                
                // remove crashed carts
                carts.RemoveAll(c => c.Crashed);

                i++;
                //carts.ForEach(c => Console.WriteLine($"{i} >> [{c.x},{c.y}]"));

            }while(cartCount > 1);
            
            // Part 2
            if (carts.Count == 1){
                Console.WriteLine($"Last cart @ {carts.First().x},{carts.First().y}");
            }


        }
    }
    public class Cart : IComparable{

        public Cart(int x, int y, Direction d)
        {
            this.x = x;
            this.y = y;
            Dir = d;
            Next = Intersection.Left;
        }

        public bool Crashed{get;set;}
        public int x {get;set;}
        public int y {get;set;}
        public Direction Dir {get;set;}

        public Intersection Next {get;set;}
        public int CompareTo(object obj)
        {
            var other = (Cart)obj;
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
    }

    public enum Direction{
        N,
        E,
        S,
        W
    }

    public enum Intersection{
        Left,
        Straight,
        Right,
    }
}

