using System;
using System.Collections.Generic;
using System.Linq;

namespace Day16
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = new Solver();
            s.Part1(Input.TestData);
            s.Part1(Input.Data);

            s.Part2(Input.Data);

            s.Part3(Input.Program);

        }
    }

    public class Solver{
        int[] reg = new int[4];
        public void Part1(string input){
            var sets = input.Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries);

            var tests = sets.Select(TestSet.CreateFromData).ToList();

            var sum = tests.Where(s => FindOpcodeMatch(s.before, s.after, s.instructions) >= 3 ).Count();
            Console.WriteLine($"Match three or more: {sum}");

        }

        public void Part2(string input){
            var sets = input.Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries);

            var tests = sets.Select(TestSet.CreateFromData).ToList();

            // group by opcode.
            // find opcode that matches all in group.

            // run multiple times to and find single matching. Once found, remove as option to match (manual)
            var groups = tests.GroupBy(t => t.instructions[0]).OrderBy(g => g.Key);
            foreach(var g in groups){
                Console.WriteLine($"Working on {g.Key}");

                for(int i = 0; i<16; i++){ // loop over all opcodes
                    // skip known
                    
                    if ((OpCode)i == OpCode.bori) continue; // 0
                    if ((OpCode)i == OpCode.borr) continue; // 1
                    if ((OpCode)i == OpCode.addi) continue; // 2
                    if ((OpCode)i == OpCode.muli) continue; // 3
                    if ((OpCode)i == OpCode.addr) continue; // 4
                    if ((OpCode)i == OpCode.bani) continue; // 5
                    if ((OpCode)i == OpCode.gtri) continue; // 6
                    if ((OpCode)i == OpCode.setr) continue; // 7
                    if ((OpCode)i == OpCode.gtrr) continue; // 8
                    if ((OpCode)i == OpCode.seti) continue; // 9
                    if ((OpCode)i == OpCode.eqir) continue; // 10
                    if ((OpCode)i == OpCode.eqrr) continue; // 11
                    if ((OpCode)i == OpCode.mulr) continue; // 12
                    if ((OpCode)i == OpCode.eqri) continue; // 13
                    if ((OpCode)i == OpCode.gtir) continue; // 14
                    if ((OpCode)i == OpCode.banr) continue; // 15
                    

                    var valid = true;
                    foreach(var t in g){
                        var match = TestOpcode((OpCode)i, t.before, t.after, t.instructions);
                        valid &= match;
                    }
                    if (valid){
                        Console.WriteLine($"{g.Key} matches with {(OpCode)i}");
                    }
                }
            }
        }


        public void Part3(string prog){
            var lines = prog.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            var p = new List<int[]>();
            foreach(var line in lines){
                p.Add(line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray());
            }

            int pc = 0;
            foreach(var i in p){
                this.ApplyOpCode((OpCode)i[0],i[1], i[2], i[3]);
                pc++;
            }
            Console.WriteLine($"{pc} => {string.Join(", ",reg)}");
        }

        public void Reset(){
            reg[0] = reg[1] = reg[2] = reg[3] = 0;
        }
        // in , out, action
        public int FindOpcodeMatch(int[] input, int[] output, int[] instruction) {
            //Console.WriteLine($"[{string.Join(", ",input)}] => {string.Join(", ",instruction)} => [{string.Join(", ",output)}]");
            var count = 0;
            for(int i = 0 ; i< 16; i++){
                if (TestOpcode((OpCode)i, input, output,  instruction)){
                    count++;
                }
                // var r = ApplyOpCode(input, (OpCode)i, instruction[1], instruction[2], instruction[3]);
                // //Console.WriteLine($"{(OpCode)i} => [{string.Join(", ",r)}]");
                // if (r[0] == output[0] && r[1] == output[1] && r[2] == output[2] && r[3] == output[3]){
                //     //Console.WriteLine($" Match {(OpCode)i}");
                //     count++;
                // }
            }
            return count;
        }

        public bool TestOpcode(OpCode i, int[] input, int[] output, int[] instruction){
            var r = ApplyOpCode(input, (OpCode)i, instruction[1], instruction[2], instruction[3]);
            //Console.WriteLine($"{(OpCode)i} => [{string.Join(", ",r)}]");
            return (r[0] == output[0] && r[1] == output[1] && r[2] == output[2] && r[3] == output[3]);
        }

        public int[] ApplyOpCode(int[] reg, OpCode op, int a, int b, int c){
            int[] r = new int[]{reg[0], reg[1], reg[2], reg[3]};
            switch(op){
                case OpCode.addr: r[c] = reg[a]+reg[b]; break;
                case OpCode.addi: r[c] = reg[a]+b; break;
                case OpCode.mulr: r[c] = reg[a]*reg[b]; break;
                case OpCode.muli: r[c] = reg[a]*b; break;
                case OpCode.banr: r[c] = reg[a]&reg[b]; break;
                case OpCode.bani: r[c] = reg[a]&b; break;
                case OpCode.borr: r[c] = reg[a]|reg[b]; break;
                case OpCode.bori: r[c] = reg[a]|b; break;
                case OpCode.setr: r[c] = reg[a]; break;
                case OpCode.seti: r[c] = a; break;
                case OpCode.gtir: r[c] = a > reg[b]?1:0; break;
                case OpCode.gtri: r[c] = reg[a]>b?1:0; break;
                case OpCode.gtrr: r[c] = reg[a]>reg[b]?1:0; break;
                case OpCode.eqir: r[c] = a == reg[b]?1:0; break;
                case OpCode.eqri: r[c] = reg[a]==b?1:0; break;
                case OpCode.eqrr: r[c] = reg[a]==reg[b]?1:0; break;
            }
            return r;
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
        public int Read(int i){
            return reg[i];
        }
        public void Write(int i, int v){
            reg[i] = v;
        }

    }

    public class TestSet{
        public int[] before;
        public int[] after;
        public int[] instructions;

        public static TestSet CreateFromData(string s){
            var lines = s.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            var i = lines[0].Substring(8).Split(new char[]{',','[',']'}, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            var o = lines[2].Substring(8).Split(new char[]{',','[',']'}, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            var instructions = lines[1].Split(" ").Select(int.Parse).ToArray();
            return new TestSet(){
                before = i,
                after = o,
                instructions = instructions
            };
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
}

