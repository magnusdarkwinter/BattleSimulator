using System;
using System.Collections.Generic; 

namespace BattleSimulator
{
    class Fighter
    {
        public string Name;
        public int Health, Atk, Def, Init;

        public Fighter(string name, int health, int atk, int def, int init)
        {
            this.Name = name;
            this.Health = health;
            this.Atk = atk;
            this.Def = def;
            this.Init = init;
        }
    }

    class FighterFactory
    {
        private Random rnd;

        public FighterFactory()
        {
            rnd = new Random();
        }

        public Fighter GetRandomFighter()
        {
            Dictionary<string, int> stats = GetRandomStats();
            return new Fighter(GetRandomName(), stats["health"], stats["atk"], stats["def"], stats["init"]);
        }

        private Dictionary<string, int> GetRandomStats()
        {
            Dictionary<string, int> stats = new Dictionary<string, int>();
            stats.Add("health", rnd.Next(25, 100));
            stats.Add("atk", rnd.Next(1, 10));
            stats.Add("def", rnd.Next(1, 10));
            stats.Add("init", rnd.Next(1, 7));
            return stats;
        }

        private string GetRandomName()
        {
            string[] firstNames = new string[] { "Razol", "Ne-pik", "Dirako", "Renra", "Korta", "Kotor", "Akton", "Thiak", "Keje", "Lelou", "Jadpox","Neren", "Urlak", "Bugella", "Rysra", "Urzuc", "Virthar", "Talhat", "Dashdrex", "Lennpal" };
            string[] lastNames = new string[] { "Oakforge", "Silverwater", "Bornmountain", "Landgazer", "Plainsblade", "Mazeleaf", "Arrowhalf", "Hunterbattle", "Roadgazer", "Staffbull", "Busheyes", "Furywalk", "Wargred", "Manegold", "Shadelven", "Landersheep", "Oakhame", "Bushmaze", "Axebolt", "Wararrow" };
            return string.Format("{0} {1}", firstNames[rnd.Next(0, 19)], lastNames[rnd.Next(0, 19)]);
        }
    }
}