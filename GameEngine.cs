using System;
using System.Linq;
using System.Collections.Generic;

namespace BattleSimulator
{
    class GameEngine
    {
        private Random rnd;
        private FighterFactory ff;

        public GameEngine()
        {
            rnd = new Random();
            ff = new FighterFactory();
        }

        public void Run()
        {
            while(true)
            {
                // Main Menu
                Console.Write("\n\nReady to Play? (n/y) >>>");
                var play = Console.ReadLine();
                
                if(play.ToLower() == "y")
                {
                    var fighters = new List<Fighter>();
                    foreach(var fighter in Enumerable.Range(0, 5))
                    {
                        fighters.Add(ff.GetRandomFighter());
                    }
                    
                    ShowFighters(fighters.ToArray());

                    Console.WriteLine("\n### Press any ENTER to battle ###");
                    var winner = Battle(GetFirstAttacker(fighters.ToArray()), fighters.ToArray());

                    Console.WriteLine("\n\n *** {0} Wins! ***\n\n", winner);
                }
                if(play.ToLower() == "n")
                {
                    Console.WriteLine("\n\n#################\n### Good Bye! ###\n#################\n\n");
                    break;
                }
            }
        }

        public void ShowFighters(params Fighter[] fighters)
        {
            foreach(var fighter in fighters)
            {
                Console.WriteLine("----------------------------");
                Console.WriteLine("\nName: {0}\nHealth: {1}\nAtk: {2}\nDef: {3}\nInit: {4}\n", 
                    fighter.Name, fighter.Health, fighter.Atk, fighter.Def, fighter.Init);
                Console.WriteLine("----------------------------");
            }
        }

        public string Battle(string firstAttacker, params Fighter[] fighters)
        {
            Console.ReadKey();

            // Would need to have an array of defenders? or some mech to determin who attacks who for 3+ fighters
            var attacker = fighters.Single(f => f.Name == firstAttacker);
            var defenders = fighters.Where(f => f.Name != firstAttacker);
            var defender = defenders.ToList()[rnd.Next(0, defenders.ToList().Count - 1)]; // Pick random fighter to attack

            // Damage Step
            var roll = attacker.RollDamage();
            var damage = (roll <= defender.Def) ? 1 : roll - defender.Def; // all attacks do a min of 1 dmg.
            defender.Health -= damage;

            var remainingFighters = new List<Fighter>();

            // Check for death
            foreach(var checkDef in defenders.ToList())
            {
                if(checkDef.Health > 0) 
                {
                    remainingFighters.Add(checkDef);
                }
            }

            if(remainingFighters.Count < 1)
            {
                return attacker.Name;
            }
            else
            {
                remainingFighters.Add(attacker);
                var nextAttacker = "";
                if(defender.Health < 1)
                {
                    Console.WriteLine("\n{0} attacks {1} for {2} damage; {1} Has Died!\n",
                        attacker.Name, defender.Name, damage);
                    nextAttacker = remainingFighters.First().Name;
                }
                else
                {
                    Console.WriteLine("\n{0} attacks {1} for {2} damage; {1} now has {3} health\n", 
                        attacker.Name, defender.Name, damage, defender.Health);
                    nextAttacker = defender.Name;
                }
                
                return Battle(nextAttacker, remainingFighters.ToArray());
            }
        }

        public string GetFirstAttacker(params Fighter[] fighters)
        {
            var winner = "";
            var highestRoll = 0;

            foreach(var fighter in fighters)
            {
                var roll = fighter.RollInitiative();
                if(roll >= highestRoll)
                {
                    winner = fighter.Name;
                    highestRoll = roll;
                }
            }

            Console.WriteLine("\n{0} Attacks First!\n", winner);
            return winner;
        }
    }
}