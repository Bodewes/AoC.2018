using System;
using System.Linq;
using System.Collections.Generic;

namespace day21
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = new Solver();
            s.ParseInput(Input.Data);
            s.Part1();
        }
    }

      public class Solver{

        public Int64[] reg = new Int64[6];
        int pc = 0;

        int pc_reg = 0;

        List<(OpCode opCode, Int64 a, Int64 b, Int64 c)> prog = new List<(OpCode opCode, Int64 a, Int64 b, Int64 c)>();

        HashSet<long> x = new HashSet<long>();
        long prev = 0;

        public void Part1(){
            //Console.WriteLine($"PC Reg = {pc_reg}");
            var counter = 0;
            while (pc < prog.Count && pc >= 0){
                // step
                // set pc value to reg
                reg[pc_reg] = pc;
                //Console.Write($"{pc}\t {PrintReg()} \t {PrintOp(prog[pc])}");
                ApplyOpCode(prog[pc].opCode, prog[pc].a, prog[pc].b, prog[pc].c);
                //Console.WriteLine($"\t{PrintReg()} \t{counter++}");
                pc = (int)reg[pc_reg];
                pc++;
                counter++;
                //Console.WriteLine($"{counter}\t{PrintReg()}");
                //Console.ReadLine();
                if(pc == 28){
                    // First output for part 1
                    //Console.WriteLine($"\t{PrintReg()}\t {counter} \t {x.Count}");
                    if (x.Count % 1000 == 0)
                    {
                        Console.WriteLine(x.Count);
                    }
                    // Part 2
                    if(!x.Add(reg[3])){
                        Console.WriteLine($"Last added before looping: {x.Last()} {prev}");
                        return;
                    }else{
                        prev = reg[3];
                    }
                }
                    
            }
            PrintReg();
            Console.WriteLine();
        }

        public string PrintReg(){
            return $"[{reg[0]}, {reg[1]}, {reg[2]}, {reg[3]}, {reg[4]}, {reg[5]},]";
        }
        public string PrintOp((OpCode opCode, long a, long b, long c) x){
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




        public void ApplyOpCode(OpCode op, long a, long b, long c){
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

        public long Read(long i){
            return reg[i];
        }
        public void Write(long i, long v){
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
        public const string Data = @"#ip 4
seti 123 0 3
bani 3 456 3
eqri 3 72 3
addr 3 4 4
seti 0 0 4
seti 0 2 3
bori 3 65536 2
seti 1397714 1 3
bani 2 255 5
addr 3 5 3
bani 3 16777215 3
muli 3 65899 3
bani 3 16777215 3
gtir 256 2 5
addr 5 4 4
addi 4 1 4
seti 27 6 4
seti 0 6 5
addi 5 1 1
muli 1 256 1
gtrr 1 2 1
addr 1 4 4
addi 4 1 4
seti 25 2 4
addi 5 1 5
seti 17 0 4
setr 5 7 2
seti 7 4 4
eqrr 3 0 5
addr 5 4 4
seti 5 8 4";
    }
}
