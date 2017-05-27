using System;
using System.Linq;

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
                string play = Console.ReadLine();
                
                if(play.ToLower() == "y")
                {
                    // Show Fighters
                    Fighter fighterOne = ff.GetRandomFighter();
                    Fighter fighterTwo = ff.GetRandomFighter();
                    ShowFighters(fighterOne, fighterTwo);

                    string firstAttacker = RollInitiative(fighterOne, fighterTwo);

                    Console.WriteLine("\n### Press any ENTER to battle ###");
                    string winner = Battle(fighterOne, fighterTwo, firstAttacker);

                    Console.WriteLine("\n\n *** {0} Wins! ***\n\n", winner);
                }
                if(play.ToLower() == "n")
                {
                    Console.WriteLine("\n\n#################\n### Good Bye! ###\n#################\n\n");
                    break;
                }
            }
        }

        public void ShowFighters(Fighter fighter1, Fighter fighter2)
        {
            var fighters = new [] { fighter1, fighter2 };
            foreach(var fighter in fighters)
            {
                Console.WriteLine("----------------------------");
                Console.WriteLine("\nName: {0}\nHealth: {1}\nAtk: {2}\nDef: {3}\nInit: {4}\n", 
                    fighter.Name, fighter.Health, fighter.Atk, fighter.Def, fighter.Init);
                Console.WriteLine("----------------------------");
            }
        }

        public string Battle(Fighter fighter1, Fighter fighter2, string firstAttacker)
        {
            Console.ReadKey();

            var fighters = new [] { fighter1, fighter2 };
            var attacker = fighters.Single(f => f.Name == firstAttacker);
            var defender = fighters.Single(f => f.Name != firstAttacker);

            // Roll damage
            int roll20 = rnd.Next(1, 20);
            int damage = attacker.Atk + roll20;
            // damage step: all attacks do a min of 1 dmg.
            int totalDmg = (damage <= defender.Def) ? 1 : damage - defender.Def;
            defender.Health -= totalDmg;

            // Check for death
            if(defender.Health <= 0)
            {
                return attacker.Name;
            }
            else
            {
                Console.WriteLine("\n{0} attacks for {1} damage; {2} now has {3} health\n", 
                    attacker.Name, totalDmg, defender.Name, defender.Health);
                return Battle(fighter1, fighter2, defender.Name);
            }
        }

        public string RollInitiative(Fighter fighter1, Fighter fighter2)
        {
            int roll1 = rnd.Next(1, 20);
            int roll2 = rnd.Next(1, 20);

            int score1 = roll1 + fighter1.Init;
            int score2 = roll2 + fighter2.Init;

            if(score1 > score2)
            {
                Console.WriteLine("\n{0} Attacks First!\n", fighter1.Name);
                return fighter1.Name;
            }
            else if(score1 < score2)
            {
                Console.WriteLine("\n{0} Attacks First!\n", fighter2.Name);
                return fighter2.Name;
            }
            else
            {
                return RollInitiative(fighter1, fighter2);
            }
        }
    }
}