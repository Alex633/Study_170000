using System;

namespace CsRealLearning
{
    internal class Program
    {
        static void Main()
        {
            int selectedClassID1;
            int selectedClassID2;
            int count = 0;

            BattleClass[] battleClasses =
            {
                new BattleClass("Warrior", 260, 20, 30),
                new BattleClass("Mage", 150, 60, 5),
                new BattleClass("Thief", 240, 35, 15),
            };

            Console.WriteLine();

            //displaing stats for each battle class
            foreach (BattleClass battleClass in battleClasses)
            {
                Console.Write("\n" + ++count + ". ");
                battleClass.DisplayStats();
            }

            count = 0;

            //character selector
            ClassSelector(1, out selectedClassID1);
            ClassSelector(2, out selectedClassID2);

            Console.WriteLine($"Plyaer 1 class:");
            battleClasses[selectedClassID1].DisplayStats();

            Console.WriteLine($"Player 2 class:");
            battleClasses[selectedClassID2].DisplayStats();

            //title
            Console.Clear();
            Console.WriteLine("FIGHT");

            //fight
            while (battleClasses[selectedClassID1].HP > 0 && battleClasses[selectedClassID2].HP > 0)
            {
                Console.WriteLine($"\nRound {++count}");
                battleClasses[selectedClassID2].TakeDamage(battleClasses[selectedClassID1].Damage);
                battleClasses[selectedClassID1].TakeDamage(battleClasses[selectedClassID2].Damage);

            }

            Console.Write("\nFight Ended. Winner is this guy: ");
            if (battleClasses[selectedClassID1].HP > 0)
                Console.WriteLine($"Player 1 ({battleClasses[selectedClassID1].Name}) Good Job.\nWhat do you want now? Cookie?");
            else
                Console.WriteLine($"Player 2 ({battleClasses[selectedClassID2].Name}). Good Job.\nWhat do you want now? Cookie?");

            Console.ReadKey();
            Console.Clear();
            Console.WriteLine("Well, I don't have a cookie... BLINDING ATTACK");
            Console.ReadKey();
            Console.BackgroundColor = ConsoleColor.White;
            Console.Clear();

        }

        class BattleClass
        {
            private int _armor;

            public int HP { get; private set; }
            public int Damage { get; private set; }
            public string Name { get; private set; }



            public BattleClass(string name, int hp, int damage, int armor)
            {
                Name = name;
                HP = hp;
                Damage = damage;
                _armor = armor;
            }

            public void DisplayStats()
            {
                Console.WriteLine($"{Name}\n" +
                    $"HP: {HP}, Damage: {Damage}, Armor: {_armor}\n");
            }

            public void TakeDamage(int opponentDamage)
            {
                int actualDamage = opponentDamage - _armor;
                HP -= actualDamage;

                if (HP < 0)
                {
                    HP = 0;
                }

                Console.WriteLine($"{Name} took {actualDamage} damage. Current HP: {HP}");
            }
        }

        public static void ClassSelector(int playerNum, out int selectedClassID)
        {
            Console.WriteLine();
            Console.WriteLine(new string('-', 35));

            Console.WriteLine($"\nPlayer {playerNum}: Select Your Class:");
            while (!int.TryParse(Console.ReadLine(), out selectedClassID) || (selectedClassID > 3 || selectedClassID < 1))
            {
                Console.WriteLine("There is no such class, Player. Can you count to 3?:");
            }

            selectedClassID--;
        }
    }
}
