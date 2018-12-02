using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Day02
{
    class Program
    {
        static void Main(string[] args)
        {
            var solver = new Solver();
            var answer = solver.Part1(Input.Test01);
            Console.WriteLine($"Part1 Test: {answer}");

            Console.WriteLine($"Part1 Test: {solver.Part1(Input.Data)}");

            Console.WriteLine($"Part2 Test: {solver.Part2(Input.Test02)}");

            Console.WriteLine($"Part2 Test: {solver.Part2(Input.Data)}");
        }
    }

    public class Solver{
        public int Part1(string input){
            var lines = input.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            int has2 = 0;
            int has3 = 0;
            foreach(var line in lines){
                var hist = getHistogram(line);
                if (hist.Any(h => h.Value == 2)) has2++;
                if (hist.Any(h => h.Value == 3)) has3++;
                Console.WriteLine($"line: {line} {has2} {has3}");
            }
            return has2*has3;
        }

        private Dictionary<char, int> getHistogram(string line){
            var hist = new Dictionary<char, int>();
            foreach(char c in line){
                if (hist.ContainsKey(c))
                    hist[c]++;
                else
                    hist.Add(c,1);
            }
            return hist;
        }

        public string Part2(string input){
             var lines = input.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
             for(int i =0; i < lines[0].Length; i++){
                var result = lines.Select(x => new StringBuilder(x).Remove(i,1).ToString()).GroupBy(x => x).Where(g => g.Count() > 1).Select(g =>g.Key).SingleOrDefault();
                if (result != null)
                    Console.WriteLine($"{i} => {result} ");
             }
             return "Done";
        }

    }




    public class Input{
        public static string Test01 = @"abcdef
bababc
abbcde
abcccd
aabcdd
abcdee
ababab";


        public static string Test02=@"abcde
fghij
klmno
pqrst
fguij
axcye
wvxyz";

        public static string Data = @"lnfgdsywjyleogambzuchirkpx
nnfqdskfjyteogambzuchirkpx
lnfqdvvwjyteofambzuchirkpf
lnfqdsvwjyteogvmbzuthirkpn
ltfqdsvwjyoeogambxuchirkpx
lnfqcsvwjytzogacbzuchirkpx
lpfpdsvwjyteogambyuchirkpx
pnfqdsvwjyteogqmbzuchinkpx
lnfqdsvwjytyopambzpchirkpx
lnfqisswjyteogadbzuchirkpx
lnfqdsuwjcteogambzuchirepx
lnfqdovwjnteigambzuchirkpx
lnfbdsvwjxteogambzuchirkax
lnfqdsawjyteogamxzuchiwkpx
lncqdsvwjoteogambzuchirfpx
lnfadsrwjyteogambzuchirktx
lnfqdsvhjyteorazbzuchirkpx
lwfqdsvwjytdogambzuchirkhx
lnfqdhvwjyteogambzuhairkpx
lnfqdsvwjytlogambzgchyrkpx
lnfqdsvwjyteogamnzjwhirkpx
lnfodsvwjyteogahuzuchirkpx
lnfqdsvtjyteogamvzwchirkpx
lnfqdsvwjzueogambzuxhirkpx
lnfqxsvljytxogambzuchirkpx
lnfqdsvwjyteogambpyvhirkpx
lqzqdsvwjnteogambzuchirkpx
lnfqdsvwjyteogumbzichirapx
lnfqdbvwjytedgaubzuchirkpx
lnfqdsvwpyteogabbhuchirkpx
nnfqdsvwryteogambzuchiwkpx
lrfqdsvwjeteogambzuchhrkpx
lnfqdsvwxyteogamtzucyirkpx
lnfsdsvwjyteogambzulhirknx
lnfqdsvwjyreoyambzmchirkpx
ltfqdsvwjytdogkmbzuchirkpx
lnfqwbvcjyteogambzuchirkpx
lnfqdsvwjyteogamrzfchirmpx
lnfqdsvwjqteogambzucwirkpy
lnfqdslwjyfeogambzuchikkpx
lnfqdsvwjybeogambzuchikjpx
lofqysvwjyteogasbzuchirkpx
lnfqusvwjyteogambzucverkpx
lnfqdsvwjyteogaibzuchfrkzx
lnfqdsvwjyleogabbzuchirkcx
lnfqdsvqjyteogambzuchdqkpx
lnfqdsvwjwtewgambzuciirkpx
lnfqisvwjatwogambzuchirkpx
lnfqdgvwjyteogambzuchizkix
lnfqdsxwjyteogambyuehirkpx
lpffdsvwjyteogamrzuchirkpx
lnirdsvwjyteogambzuchirkbx
lnfqdsxdjyteogazbzuchirkpx
lnfgdgvwyyteogambzuchirkpx
lnfqxsvwjyteogambzmcwirkpx
lnxqjsvwjyteogambzuchirkqx
lnrqdsvwjpteogkmbzuchirkpx
lnfqdstwjyteoeahbzuchirkpx
lnfqdsvwtuteogambzuchixkpx
lwfqvsvwjyteogambzughirkpx
lnkqdsvwjyfeogambzuuhirkpx
lvvqdsvwjyteogambzuchirkpn
jndqdsvwjyteogzmbzuchirkpx
enfqdszwjyteogambcuchirkpx
lnfqdsvwiyteogakbauchirkpx
lnfqdsvwjyteogtmbzxcdirkpx
fnfqdswwjyteogawbzuchirkpx
lnfqdsvwjydejqambzuchirkpx
lnqqdsvwjyteogambzucbdrkpx
lnfqdsvwjyteogadbzuchirxcx
lnfqdslwjyyeogambzulhirkpx
lnfqdsvwjytecgambzucgirkpb
lbmqdsvwjyteogamkzuchirkpx
lbfqdsvrjyteogambzuchirapx
lnfqdsmwjyteogambzucfarkpx
lnfqasvwoyteofambzuchirkpx
bnfudsvwjyteogambzucharkpx
lnfrdsvwjytxogambzuchirkpg
lbfqdsvwjyteagambzucdirkpx
lxfqdsvwjytuogambzucjirkpx
lnfqdsvwjcteogamyzuchiikpx
lnfodsvwjyteognmbzuchirapx
ltfqdsvwjytedgaxbzuchirkpx
lnfqdshwjyteogambzucsilkpx
lnfqdsvwpyteohambzuchitkpx
wnzqdsvwjyteogambiuchirkpx
lnfqdsvwayteogambzhchirkpw
ltfqcsvwjrteogambzuchirkpx
lnfqdsvwaytekgamvzuchirkpx
lnfqdsvwjyteogambzokpirkpx
lnfqysbwjyeeogambzuchirkpx
lnsqdsvwjyteogambzuchikkpd
lrfqdsvwjyteogahbzochirkpx
lnfqdsvwjyreoggmbzuchjrkpx
lxfqdsvwjyteogkmbzuchirkpp
enhqdbvwjyteogambzuchirkpx
jnfqdsvwjyteogamczuuhirkpx
lnfqdsvwuyteogadbzuchirkpw
lnfqdsvjjytergambznchirkpx
lnfqdsvwjyteoglmbzuceijkpx
lwfqdsvwjyteogamieuchirkpx
lnfqdsvwjfaeogambzqchirkpx
lfbqdjvwjyteogambzuchirkpx
lnfqdsvwjxteoaambzuchirkpp
lnfqdsvwjyheogjmbzgchirkpx
lnfqdskwjyteonambzuchiikpx
lnfqdwvwjyteogambxuchirkph
pnfqdsvwdyteogambzuchihkpx
lnoqdsvwjyteogaybznchirkpx
lnfqdsvwfyxefgambzuchirkpx
lnfqdsvwjyteotamxzmchirkpx
lnfqdsvwjyteigwmbzuchivkpx
lnfqdsvwjytekgambcuchirkwx
lnfqdsvwjuteogamrzulhirkpx
lnfqdsvwjyteogambzucczrgpx
wnfqzsvwjyteogambduchirkpx
lnfqdsowjyteogambuuthirkpx
lnfqdsvrjyteogcmbzuclirkpx
knfqdsvwgyteogambzuchorkpx
lnaqdsvwjytuogdmbzuchirkpx
lnfrdsvwjyteogambluchigkpx
lnfqqzvwjyteogambzkchirkpx
lnfqdsvwjyteogamuzuchgrkux
lnfqdsvnjyteogxmbznchirkpx
lnfqdsvwjyteolajbzuchdrkpx
lnfqdsvwjypeoiagbzuchirkpx
lnrqdsvwjyteozamuzuchirkpx
lnfqdsvwjytkogaubzucqirkpx
lnkbdsvwjyteogacbzuchirkpx
unfqdsvwjybeogambwuchirkpx
lnfqfsvzjyteogambzuchiikpx
lnfqdsvgjyreogahbzuchirkpx
lnfqdsewjyteogavbeuchirkpx
lnfqdsvwjdteogambbuchidkpx
lnfqdsvwjythogambzcchirkvx
lnfqdscwjyteorambzuchirgpx
cnfqdzvwjyteogambzushirkpx
lnfgdsgwjytedgambzuchirkpx
lnfqdsvwbyteogimbzuchdrkpx
lnfqdsswjyteogambzuohbrkpx
lnfqdsvwjytqogabbzachirkpx
lnfqdsvwjyteogmmbzucqiukpx
lnfxdsrwjyteogambzuchnrkpx
lnfqnqvwjyteogambzuchiwkpx
lffqisvwjyteogambzulhirkpx
lnfqdsxwjydeogambzucfirkpx
lnfqdsvwjyteogambzucjirkrp
lnfqdsnqjyteogambduchirkpx
fnfqdmvwjyteogamlzuchirkpx
lnfqvsvwjyteooamdzuchirkpx
lnfqdsvcyyteogambzuchickpx
onfqdsvwjyqeogambzuchirqpx
znfqdcvwjyteoaambzuchirkpx
lnfqdsvwjzteogambzuchidklx
lnfqjsvwjyteogjmbzuchirkpv
lnfqdsvwjytgorambzuchirppx
lzfqdsvwpfteogambzuchirkpx
lnfidsfwjyteogapbzuchirkpx
lnfodsvwbyteobambzuchirkpx
lnlqdsvwjytefgambzuchfrkpx
lnkqdsvwjyteogambzkchgrkpx
tnfqdsvwjyteoiamhzuchirkpx
lnfqdsvwjyteogamllschirkpx
lnfqdsvwjmthogamizuchirkpx
lnfqdbvwjyteogafbzuchirkpb
lnfxosvwjyteogahbzuchirkpx
lnmqdsvwjyzeogambzuchirkcx
lnfqdevbjytxogambzuchirkpx
lnfqdsvwjyteogamzzudhipkpx
lnfqdszwjyteoqambzuchirkpp
lffqdsvwjyteogamtouchirkpx
lnfqdsvhjytfogambzucharkpx
hnfqdsvwjyteogembzschirkpx
lnfqdsvwjateogambzuchirmpa
lnfqdsvcjyteogambzocairkpx
lnfqdsvwjyteogamwzmchirkpd
lnfqzsvwjyteogdmbzuyhirkpx
lnfqdsvwjytfyglmbzuchirkpx
lnfndsvwjyteogambzuchirktf
gnfqdnvwjytevgambzuchirkpx
lnfqdsvwjyteoganbpuchorkpx
lnfpdsvwnyteogambzucqirkpx
fnfqdstejyteogambzuchirkpx
lnfqlsvwjyteowambzuchirkmx
lnfqdsvwjyteogmmdzuchtrkpx
lnfqdsvwcyteogaqbzuchirkqx
lnfqdsvwjytlogtmbzuchiwkpx
lnfqdsvwoyteogambzuczirkwx
lnfqdsvwjyteogzybzucdirkpx
lnfqdvvwjyteogumbzuchiukpx
lnfqbwvwjyteogambzuchjrkpx
lnfgdsvwjyteogambzvchirkzx
lnfqdsvwjvtjogambzuchiokpx
lnfedsvwjyteogambzuchivkph
lhfqusvwjytaogambzuchirkpx
lnfqdsvwjyteogacbzuihirkpv
lnfwdsvwjyteogambzucokrkpx
lnfqtsvwjpteognmbzuchirkpx
anfqdswwjyteogambzucairkpx
lnfqdsvwjyteorambzuchirlsx
lnfqdsvwjytgogambzychirkpc
lnfqdhvwjyteogambzachirklx
lnfwdsvwjyteogaobquchirkpx
rnfqdsvwjiteogambzuhhirkpx
lnfqdsuwjyemogambzuchirkpx
hnfqdsvwjyteogambzuchprfpx
anfqssvwjyteogambzumhirkpx
lnfkdsvwjyteogafbzqchirkpx
lnfqdsvwjyteogacqzuchirspx
lnfqdskwjyteggambzuchiakpx
lnnqdsvwjyteooambzuchihkpx
lnlqdsvjjyteogambzuchgrkpx
lnfqdsvwjyteogamszochirkex
lnfqbsvwjyteogambzqchirepx
lnfqdsbwjcteogambzhchirkpx
lnfqdwvzjyteogambzechirkpx
ynfadsvwdyteogambzuchirkpx
tnfqdsvwjytuogambzuohirkpx
lnfqdsvwjyteogambzaohivkpx
mnfqisvwjyteogagbzuchirkpx
lnfqbsvwjyueogambzuchirkhx
ynfqdsvwjyteogdmbzuchinkpx
lnfqdwhwjyteogambzuchirqpx
mnfqdsvwjyteogambzfchkrkpx
lnfqdsnwjyteogambzgchiqkpx
lnfqdsvwjytergambzuchiuklx
lnfqdqvjjyteogamtzuchirkpx
lnfqdscwjyteorambzuchzrgpx
enfqdevwjyteogaabzuchirkpx
gnfqdsvbjyteogambzuchirkph
lnfqdxvwjyteogambzubhixkpx
lnfqdsvwjyteogambojchihkpx
lnfqdsvwjytdogambzuzhilkpx
lnfqdsvwjyteogamezuqhirtpx
tnfhdsvwjyteogambzuvhirkpx
lnfzdsvwjnteogahbzuchirkpx
lnfqdsvwjyteogambzfzhirkvx
lnfqqsvwjyteogambzuchirgpo
lufqpsvwjythogambzuchirkpx
lnfqdsvwjyteogzmbzuchimkix
lnwqdspwjyteogambzcchirkpx
lnfqdsowjyteogambzuchigypx
lnfqdnvvjyteogambzucjirkpx
lnfjdsvwryteogambzuchirkcx
lnfqdsvwbyteogambzuchirfpb
lnfqdsvwjyheogambzxchprkpx
lnfqmsvwjytezgambzuchirlpx
lnaqdsvwjyteogamdzuzhirkpx
lnoqdsvwjytebgambfuchirkpx
lnfqdtvwjytvogambzuchirkpv";
    }
}
