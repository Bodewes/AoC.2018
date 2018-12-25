using System;
using System.Collections.Generic;
using System.Linq;

namespace Day24
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = new Solver();
            //s.Part1(Input.ImmuneSystemGroupTest, Input.InfectionGroupTest);
            s.Part1(Input.ImmuneSystemGroup(), Input.InfectionGroup());

            //s.Part2();
        }
    }


    public class Solver{

        public void Part2(){
            var boost = 0;
            // TEST
            // while(!Part1(Input.ImmuneSystemGroupTest(), Input.InfectionGroupTest(), boost) && boost < 1575){
            //     boost++;
            // }
            while(!Part1(Input.ImmuneSystemGroup(), Input.InfectionGroup(), boost) && boost < 10_000){
                boost++;
            }

        }

        public bool Part1(List<Group> immuneSystem, List<Group> infection, int boost = 0){

            immuneSystem.ForEach(g => g.attack += boost);


            List<Group> all = new List<Group>();
            all.AddRange(immuneSystem);
            all.AddRange(infection);

            int total_units = all.Sum(g => g.units);

            bool moreRounds = true;
            while(moreRounds){
                // List current armies
                //all.OrderBy(x=>x.faction).ThenBy(x=>x.id).ToList().ForEach(x => Console.WriteLine($" {x.faction}-{x.id} => {x.units} x {x.hp} = power {x.power}; attack {x.attack}"));

                // Target selection
                //Console.WriteLine("Selecting Targets");
                
                // >> In decreasing order of effective power, groups choose their targets; in a tie, the group with the higher initiative chooses first.
                all = all.OrderByDescending(x=>x.power).ThenByDescending(x=>x.initiative).ToList();

                //all.ForEach(x => Console.WriteLine($" {x.faction}-{x.id} => {x.power}"));

                List<(Group a, Group d)> attackOrder = new List<(Group, Group)>(); // attack-defender.

                foreach(var g in all){
                    //Console.WriteLine($"  selecting enemy for {g.faction}-{g.id} (power: {g.power})");
                    var enemyGroups = all.Where(x => x.faction != g.faction ) // enemies
                                         .Where(x => !attackOrder.Select(a => a.d).Contains(x) )// filter out already picked
                                         .Where(x => g.potentialDmg(x) > 0) // >> If it cannot deal any defending groups damage, it does not choose a target.
                                         .OrderByDescending(x => g.potentialDmg(x)).ThenByDescending(x =>x.power).ThenByDescending(x => x.initiative)
                                         .ToList();
                    //var ee = string.Join(", ", enemyGroups.Select(e => $"{e.id} pot-dmg:{ g.potentialDmg(e)}"));
                    //Console.WriteLine($"    Potential enemies {enemyGroups.Count()}: {ee} ");

                    var selectedEnemy = enemyGroups.FirstOrDefault();
                    if (selectedEnemy != null){
                        attackOrder.Add( (g , selectedEnemy) );
                    }
                }

                // foreach(var (a,d) in attackOrder.OrderByDescending( ao => ao.a.initiative)){
                //     Console.WriteLine($" {a.faction}-{a.id}\t==>\t{d.faction}-{d.id}");
                // }

                // Attack!
                //Console.WriteLine("Attack!");
                foreach(var (a,d) in attackOrder.OrderByDescending( ao => ao.a.initiative)){
                    //Console.WriteLine($" {a.faction}-{a.id}\t==>\t{d.faction}-{d.id}");
                    d.takeDmg(  a.potentialDmg(d) );

                }
                // remove dead groups
                all.RemoveAll( g => g.units <= 0);

                moreRounds = all.GroupBy(g => g.faction).Count() != 1; // meer dan 1 army over.


                if ( all.Sum(g => g.units) == total_units){
                    moreRounds = false; // no casualties, tied.
                }
                total_units = all.Sum(g => g.units);

                //Console.WriteLine($"Next round? {moreRounds}");
                //Console.ReadLine();
            }

            Console.WriteLine($"Remaining units:{all.Sum(g => g.units)} - {boost}  {(all.GroupBy(g=>g.faction).Count()==2?"TIE":all.First().faction)} WIN");
            if (all.All(g => g.faction == "s")) // all are imunne system.
                return true;
            else 
                return false;
        }
    }

    public class Group{
        public int id;
        public string faction;
        public int units;
        public int hp;
        public List<AttackTypes> weak;
        public List<AttackTypes> immune;
        public int attack;
        public AttackTypes attackType;
        public int initiative;

        public int power => units*attack;

        public int potentialDmg(Group other){
            if (other.immune.Contains(this.attackType))
                return 0;
            if (other.weak.Contains(this.attackType))
                return power*2;
            return power;
        }

        public void takeDmg(int dmg){
            var units_lost = dmg/hp;
            //Console.WriteLine($"\tTaking {dmg}, Losing {units_lost} ({units})");
            units -= units_lost;
            if (units <= 0){
                units = 0;
            }
        }

    }

    public enum AttackTypes{
        cold,
        slashing,
        radiation,
        bludgeoning,
        fire
    }



    public static class Input{
        public const string TestData = @"Immune System:
17 units each with 5390 hit points (weak to radiation, bludgeoning) with
 an attack that does 4507 fire damage at initiative 2
989 units each with 1274 hit points (immune to fire; weak to bludgeoning,
 slashing) with an attack that does 25 slashing damage at initiative 3

Infection:
801 units each with 4706 hit points (weak to radiation) with an attack
 that does 116 bludgeoning damage at initiative 1
4485 units each with 2961 hit points (immune to radiation; weak to fire,
 cold) with an attack that does 12 slashing damage at initiative 4";

        public static List<Group> ImmuneSystemGroupTest() {return  new List<Group>{
            new Group{id=1, faction ="s", units = 17, hp = 5390, weak=new List<AttackTypes>{AttackTypes.radiation, AttackTypes.bludgeoning}, immune=new List<AttackTypes>(), attack=4507,  attackType = AttackTypes.fire, initiative=2},
            new Group{id=2, faction ="s",units = 989, hp = 1274, weak= new List<AttackTypes>{AttackTypes.bludgeoning, AttackTypes.slashing}, immune=new List<AttackTypes>{AttackTypes.fire}, attack=25, attackType=AttackTypes.slashing, initiative=3}
        };}

        public static List<Group> InfectionGroupTest() { return new List<Group>{
            new Group{id=1, faction ="i", units = 801, hp = 4706, weak=new List<AttackTypes>{AttackTypes.radiation}, immune=new List<AttackTypes>(), attack=116,  attackType = AttackTypes.bludgeoning, initiative=1},
            new Group{id=2, faction ="i", units = 4485, hp = 2961, weak= new List<AttackTypes>{AttackTypes.fire, AttackTypes.cold}, immune=new List<AttackTypes>{AttackTypes.radiation}, attack=12, attackType=AttackTypes.slashing, initiative=4}
        };}

        public const string Data = @"Immune System:
2785 units each with 4474 hit points (weak to cold) with an attack that does 14 fire damage at initiative 20
4674 units each with 7617 hit points (immune to slashing, bludgeoning; weak to fire) with an attack that does 15 slashing damage at initiative 15
1242 units each with 1934 hit points (weak to fire) with an attack that does 15 bludgeoning damage at initiative 6
1851 units each with 9504 hit points (weak to bludgeoning) with an attack that does 47 slashing damage at initiative 2
846 units each with 9124 hit points (weak to bludgeoning; immune to radiation) with an attack that does 99 bludgeoning damage at initiative 4

338 units each with 1378 hit points (immune to radiation) with an attack that does 39 cold damage at initiative 10
3308 units each with 5087 hit points (weak to radiation) with an attack that does 12 fire damage at initiative 3
2668 units each with 8316 hit points (weak to bludgeoning, radiation) with an attack that does 28 slashing damage at initiative 9
809 units each with 1756 hit points (immune to bludgeoning) with an attack that does 21 cold damage at initiative 1
4190 units each with 8086 hit points (immune to cold) with an attack that does 18 cold damage at initiative 5";
        public static List<Group> ImmuneSystemGroup(){return new List<Group>{
            new Group{id=1, faction ="s", units = 2785 , hp =4474 , weak=new List<AttackTypes>{ AttackTypes.cold}, immune=new List<AttackTypes>(){}, 
                        attack= 14,  attackType = AttackTypes.fire, initiative=20},
            new Group{id=2, faction ="s", units = 4674, hp = 7617, weak=new List<AttackTypes>{AttackTypes.fire}, immune=new List<AttackTypes>(){AttackTypes.slashing, AttackTypes.bludgeoning}, 
                        attack= 15,  attackType = AttackTypes.slashing, initiative=15},
            new Group{id=3, faction ="s", units = 1242, hp = 1934, weak=new List<AttackTypes>{AttackTypes.fire}, immune=new List<AttackTypes>(){},
                        attack=15,  attackType = AttackTypes.bludgeoning, initiative=6},
            new Group{id=4, faction ="s", units = 1851, hp = 9504, weak=new List<AttackTypes>{AttackTypes.bludgeoning}, immune=new List<AttackTypes>(){},
                        attack=47,  attackType = AttackTypes.slashing, initiative=2},
            new Group{id=5, faction ="s", units = 846, hp = 9124, weak=new List<AttackTypes>{AttackTypes.bludgeoning}, immune=new List<AttackTypes>(){AttackTypes.radiation},
                        attack=99,  attackType = AttackTypes.bludgeoning, initiative=4},

            new Group{id=6, faction ="s", units = 338, hp = 1378, weak=new List<AttackTypes>{}, immune=new List<AttackTypes>(){AttackTypes.radiation},
                        attack=39,  attackType = AttackTypes.cold, initiative=10},
            new Group{id=7, faction ="s", units = 3308, hp = 5087, weak=new List<AttackTypes>{AttackTypes.radiation}, immune=new List<AttackTypes>(){}, 
                        attack=12,  attackType = AttackTypes.fire, initiative=3},
            new Group{id=8, faction ="s", units = 2668, hp = 8316, weak=new List<AttackTypes>{AttackTypes.bludgeoning, AttackTypes.radiation}, immune=new List<AttackTypes>(){},
                        attack=28,  attackType = AttackTypes.slashing, initiative=9},
            new Group{id=9, faction ="s", units = 809, hp = 1756, weak=new List<AttackTypes>{}, immune=new List<AttackTypes>(){AttackTypes.bludgeoning}, 
                        attack=21,  attackType = AttackTypes.cold, initiative=1},
            new Group{id=10, faction ="s", units = 4190, hp =8086 , weak=new List<AttackTypes>{}, immune=new List<AttackTypes>(){AttackTypes.cold}, 
                        attack=18,  attackType = AttackTypes.cold, initiative=5},
        };}

 public const string Data2 = @"
Infection:
2702 units each with 10159 hit points with an attack that does 7 fire damage at initiative 7
73 units each with 14036 hit points (weak to fire) with an attack that does 384 radiation damage at initiative 18
4353 units each with 35187 hit points with an attack that does 15 slashing damage at initiative 14
370 units each with 9506 hit points (weak to bludgeoning, radiation) with an attack that does 46 slashing damage at initiative 12
4002 units each with 22582 hit points (weak to radiation, cold) with an attack that does 11 fire damage at initiative 8

1986 units each with 24120 hit points (immune to fire) with an attack that does 22 radiation damage at initiative 11
1054 units each with 17806 hit points with an attack that does 25 cold damage at initiative 16
124 units each with 37637 hit points with an attack that does 589 cold damage at initiative 19
869 units each with 11019 hit points (weak to fire) with an attack that does 24 cold damage at initiative 17
3840 units each with 38666 hit points (immune to slashing, fire, bludgeoning) with an attack that does 19 bludgeoning damage at initiative 13";
        public static List<Group> InfectionGroup() {return new List<Group>{
            new Group{id=1, faction ="i", units = 2702, hp = 10159, weak=new List<AttackTypes>{}, immune=new List<AttackTypes>(){}, 
                    attack=7,  attackType = AttackTypes.fire, initiative=7},
            new Group{id=2, faction ="i", units = 73  , hp = 14036, weak=new List<AttackTypes>{AttackTypes.fire}, immune=new List<AttackTypes>(){}, 
                    attack= 384,  attackType = AttackTypes.radiation, initiative=18},
            new Group{id=3, faction ="i", units = 4353, hp = 35187, weak=new List<AttackTypes>{}, immune=new List<AttackTypes>(){}, 
                    attack= 15,  attackType = AttackTypes.slashing, initiative=14},
            new Group{id=4, faction ="i", units = 370 , hp = 9506, weak=new List<AttackTypes>{AttackTypes.bludgeoning, AttackTypes.radiation}, immune=new List<AttackTypes>(){},
                    attack=46,  attackType = AttackTypes.slashing, initiative=12},
            new Group{id=5, faction ="i", units = 4002, hp = 22582, weak=new List<AttackTypes>{AttackTypes.radiation, AttackTypes.cold}, immune=new List<AttackTypes>(){}, 
                    attack=11,  attackType = AttackTypes.fire, initiative=8},

            new Group{id=6, faction ="i", units = 1986, hp = 24120, weak=new List<AttackTypes>{}, immune=new List<AttackTypes>(){AttackTypes.fire},
                    attack=22,  attackType = AttackTypes.radiation, initiative=11},
            new Group{id=7, faction ="i", units = 1054, hp = 17806, weak=new List<AttackTypes>{}, immune=new List<AttackTypes>(){},
                    attack=25,  attackType = AttackTypes.cold, initiative=16},
            new Group{id=8, faction ="i", units = 124 , hp = 37637, weak=new List<AttackTypes>{}, immune=new List<AttackTypes>(){},
                    attack=589,  attackType = AttackTypes.cold, initiative=19},
            new Group{id=9, faction ="i", units = 869 , hp = 11019, weak=new List<AttackTypes>{AttackTypes.fire}, immune=new List<AttackTypes>(){}, 
                    attack=24,  attackType = AttackTypes.cold, initiative=17},
            new Group{id=10, faction ="i", units =3840, hp = 38666, weak=new List<AttackTypes>{}, immune=new List<AttackTypes>(){AttackTypes.slashing, AttackTypes.fire, AttackTypes.bludgeoning}, 
                    attack=19,  attackType = AttackTypes.bludgeoning, initiative=13},
        };}

    }
}
