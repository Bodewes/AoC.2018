using System;
using System.Collections.Generic;
using System.Linq;

namespace Day19
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = new Solver();
            s.ParseInput(Input.Data);
            s.reg[0] = 1; // for part 2
            s.Part1();

            // Part2 after manual analysis and running for a while
            // reg[0] sums all dividers of 10551315
            // var sum = 0;
            // for(int i = 1; i <= 10551315; i++){
            // 	if (10551315% i == 0)
            // 		sum += i;
            // }
            // Answer is sum =17427456
            
        }
    }

    public class Solver{

        public int[] reg = new int[6];
        int pc = 0;

        int pc_reg = 0;

        List<(OpCode opCode, int a, int b, int c)> prog = new List<(OpCode opCode, int a, int b, int c)>();


        public void Part1(){
            //Console.WriteLine($"PC Reg = {pc_reg}");
            var counter = 0;
            var prev0 = 0;
            while (pc < prog.Count && pc >= 0){
                // step
                // set pc value to reg
                reg[pc_reg] = pc;
                //Console.Write($"{pc}\t {PrintReg()} \r {PrintOp(prog[pc])}");
                ApplyOpCode(prog[pc].opCode, prog[pc].a, prog[pc].b, prog[pc].c);
                //Console.WriteLine($"{PrintReg()}  {counter++}");
                pc = reg[pc_reg];
                pc++;
                counter++;
                if (prev0 != reg[0]){
                    Console.WriteLine($"{counter}\t{PrintReg()}");
                    prev0 = reg[0];
                } 
            }
            PrintReg();
            Console.WriteLine();
        }

        public string PrintReg(){
            return $"[{reg[0]}, {reg[1]}, {reg[2]}, {reg[3]}, {reg[4]}, {reg[5]},]";
        }
        public string PrintOp((OpCode opCode, int a, int b, int c) x){
            return $"{x.opCode} {x.a} {x.b} {x.c}";
        }

        public void ParseInput(string input){
            var lines = input.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            pc_reg = int.Parse(lines.First().Split(" ")[1]);
            foreach(var line in lines.Skip(1)){
                var tokens = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                prog.Add( ( (OpCode)Enum.Parse(typeof(OpCode), tokens[0]), int.Parse(tokens[1]), int.Parse(tokens[2]), int.Parse(tokens[3])));
            }
        }




        public void ApplyOpCode(OpCode op, int a, int b, int c){
            switch(op){
                case OpCode.addr: Write(c, Read(a)+Read(b)); break;
                case OpCode.addi: Write(c, Read(a)+b); break;

                case OpCode.mulr: Write(c, Read(a)*Read(b)); break;
                case OpCode.muli: Write(c, Read(a)*b); break;

                case OpCode.banr: Write(c, Read(a)&Read(b)); break;
                case OpCode.bani: Write(c, Read(a)&b); break;

                case OpCode.borr: Write(c, Read(a)|Read(b)); break;
                case OpCode.bori: Write(c, Read(a)|b); break;

                case OpCode.setr: Write(c, Read(a)); break;
                case OpCode.seti: Write(c, a); break;

                case OpCode.gtir: Write(c, a > Read(b)?1:0); break;
                case OpCode.gtri: Write(c, Read(a)>b?1:0); break;
                case OpCode.gtrr: Write(c, Read(a)>Read(b)?1:0); break;

                case OpCode.eqir: Write(c, a == Read(b)?1:0); break;
                case OpCode.eqri: Write(c, Read(a)==b?1:0); break;
                case OpCode.eqrr: Write(c, Read(a)==Read(b)?1:0); break;

            }
        }

        public void Reset(){
            reg[0] = reg[1] = reg[2] = reg[3] = reg[4] = reg[5] = 0;
        }

        public int Read(int i){
            return reg[i];
        }
        public void Write(int i, int v){
            reg[i] = v;
        }

    }


    public enum OpCode{
        addr = 4,
        addi = 2,
        mulr = 12,
        muli = 3,
        banr = 15,
        bani = 5,
        borr = 1,
        bori = 0,
        setr = 7, ///
        seti = 9,
        gtir = 14,
        gtri = 6, ///
        gtrr = 8,
        eqir = 10,
        eqri = 13,
        eqrr = 11,
    }


    public static class Input{
        public const string TestData = @"#ip 0
seti 5 0 1
seti 6 0 2
addi 0 1 0
addr 1 2 3
setr 1 0 0
seti 8 0 4
seti 9 0 5";

        public const string Data = @"#ip 2
addi 2 16 2
seti 1 1 3
seti 1 7 5
mulr 3 5 4
eqrr 4 1 4
addr 4 2 2
addi 2 1 2
addr 3 0 0
addi 5 1 5
gtrr 5 1 4
addr 2 4 2
seti 2 3 2
addi 3 1 3
gtrr 3 1 4
addr 4 2 2
seti 1 9 2
mulr 2 2 2
addi 1 2 1
mulr 1 1 1
mulr 2 1 1
muli 1 11 1
addi 4 3 4
mulr 4 2 4
addi 4 13 4
addr 1 4 1
addr 2 0 2
seti 0 1 2
setr 2 0 4
mulr 4 2 4
addr 2 4 4
mulr 2 4 4
muli 4 14 4
mulr 4 2 4
addr 1 4 1
seti 0 4 0
seti 0 5 2";
    }
}
