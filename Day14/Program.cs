using System;
using System.Collections.Generic;

namespace Day14
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = new Solver();
            s.Part1(9);
            s.Part1(5);
            s.Part1(18);
            s.Part1(2018);
            s.Part1(505961); // puzzle

            s.Part2(51589);
            s.Part2(01245);
            s.Part2(92510);
            s.Part2(59414);
            s.Part2(505961, 6); // puzzle


        }
    }

    public class Solver{
        public void Part1(int iter){
            var r = new List<int>();
            r.Add(3);
            r.Add(7);
            int elf1 = 0;
            int elf2 = 1;

            while(r.Count< iter+10){
                var n = r[elf1] + r[elf2];
                if (n > 9){
                    r.Add(n/10);
                }
                r.Add(n%10);
                elf1 = (elf1+ r[elf1]+1)%r.Count;
                elf2 = (elf2+ r[elf2]+1)%r.Count;

                //Console.WriteLine(string.Join(", ", r));
            }
            var result = "";
            for(int i = iter; i< iter+10; i++){
                result += r[i];
            }
            Console.WriteLine($"{iter} => {result}");

        }

        public void Part2(long iter, int digits = 5){
            var r = new List<int>();
            r.Add(3);
            r.Add(7);
            int elf1 = 0;
            int elf2 = 1;

            
            int i = 0;
            while(true){
                var n = r[elf1] + r[elf2];
                if (n > 9){
                    r.Add(n/10);
                    if(Check(iter, digits, r))
                        break;;
                    i++;
                }
                i++;
                r.Add(n%10);
                if(Check(iter, digits, r))
                        break;;
                elf1 = (elf1+ r[elf1]+1)%r.Count;
                elf2 = (elf2+ r[elf2]+1)%r.Count;

                //Console.WriteLine(string.Join(", ", r));
            }
        }

        private bool Check(long iter, int digits, List<int> r)
        {
            var size = r.Count;
            if(size < digits)
                return false;
            long mult = 1;
            long score = 0;
            for (int d = 1; d <= digits; d++)
            {
                score += r[size - d] * mult;
                mult *= 10;
            }
            if (score == iter)
            {
                Console.WriteLine($"after {r.Count-digits}");
                return true;
            }
            return false;
        }
    }
}
