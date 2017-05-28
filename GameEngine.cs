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
                var play = Console.ReadLine();
                
                if(play.ToLower() == "y")
                {
                    var fighter1 = ff.GetRandomFighter();
                    var fighter2 = ff.GetRandomFighter();

                    ShowFighters(fighter1, fighter2);

                    Console.WriteLine("\n### Press any ENTER to battle ###");
                    var winner = Battle(GetFirstAttacker(fighter1, fighter2), fighter1, fighter2);

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
            var defender = fighters.Single(f => f.Name != firstAttacker);

            // Damage Step
            var damage = attacker.RollDamage(defender.Def);
            defender.Health -= damage;

            // Check for death
            if(defender.Health <= 0)
            {
                return attacker.Name;
            }
            else
            {
                Console.WriteLine("\n{0} attacks for {1} damage; {2} now has {3} health\n", 
                    attacker.Name, damage, defender.Name, defender.Health);
                return Battle(defender.Name, attacker, defender);
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