using System;

namespace CsRealLearning
{
    internal class Program
    {
        static void Main()
        {
            int selectedClassID1;
            int selectedClassID2;


            BattleClass[] battleClasses =
            {
                new BattleClass("Warrior", 150, 20, 15),
                new BattleClass("Mage", 75, 40, 0),
                new BattleClass("Thief", 100, 30, 5),
            };

            ClassSelector(1, out selectedClassID1);
            ClassSelector(2, out selectedClassID2);

            Console.WriteLine($"Plyaer 1 class:");
            battleClasses[selectedClassID1].DisplayStats();

            Console.WriteLine(new string('-', 35));

            Console.WriteLine($"Player 2 class:");
            battleClasses[selectedClassID2].DisplayStats();

        }

        class BattleClass
        {
            private string _name;
            private int _hp;
            private int _damage;
            private int _armor;

            public BattleClass(string name, int hp, int damage, int armor)
            {
                _name = name;
                _hp = hp;
                _damage = damage;
                _armor = armor;
            }

            public void DisplayStats()
            {
                Console.WriteLine($"Name: {_name}, HP: {_hp}, Damage: {_damage}, Armor: {_armor}");
            }

            public void TakeDamage(int damage)
            {
                _hp -= _damage - _armor;
            }
            public void ShowCurrentHP()
            {
                Console.WriteLine(_name + " HP: ", _hp);
            }
        }

        public static void ClassSelector(int playerNum, out int selectedClassID)
        {
            Console.WriteLine($"Player {playerNum}: Select Your Class:\n" +
                            "1. Warrior\n" +
                            "2. Mage\n" +
                            "3. Thief");
            while (!int.TryParse(Console.ReadLine(), out selectedClassID) || (selectedClassID > 3 || selectedClassID < 1))
            {
                Console.WriteLine("There is no such class, Player. Can you count to 3?:");
            }

            selectedClassID--;
            Console.WriteLine(new string('-', 35));
        }
    }
}
