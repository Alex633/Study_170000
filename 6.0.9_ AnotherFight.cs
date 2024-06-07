namespace millionDollarsCourses
{
    using System;

    //Создать 5 классов, пользователь выбирает 2 воина и они сражаются друг с другом до смерти. У каждого класса могут быть свои статы.
    //Каждый класс должен иметь особую способность для атаки, которая свойственна только его типу класса!
    //Если можно выбрать одинаковых бойцов, то это не должна быть одна и та же ссылка на одного бойца, чтобы он не атаковал сам себя.
    //Пример, что может быть уникальное у бойцов. Кто-то каждый 3 удар наносит удвоенный урон, другой имеет 30% увернуться от полученного урона,
    //кто-то при получении урона немного себя лечит. Будут новые поля у наследников. У кого-то может быть мана и это только его особенность.

    //todo: 
    //      -
    //      -
    //      -

    internal class Program
    {
        static void Main()
        {
            BattleSystem battleSystem = new BattleSystem();

            battleSystem.Fight();
        }

        class BattleSystem
        {
            private int _round = 0;

            public BattleSystem()
            {

            }

            public void Fight()
            {
                while (true)
                {
                    RoundStart();
                    RoundEnd();
                }
            }

            private void RoundStart()
            {
                int textXPos = 0;
                int textYPos = 0;

                _round++;
                Custom.WriteAtPosition(textXPos, textYPos, $"Round: {_round}");
            }

            private void RoundEnd()
            {
                Custom.PressAnythingToContinue();
            }
        }

        abstract class Warrior
        {
            protected int _health;
            protected int _damage;

            public Warrior(int health, int damage)
            {
                _health = health;
                _damage = damage;
            }

            public virtual void DisplayStats()
            {
                Console.WriteLine($"{GetType().Name} stats:\n" +
                    $"HP: {_health}\n" +
                    $"Damage: {_damage}");
            }

            protected virtual void Attack()
            {

            }

            protected virtual void TakeDamage()
            {

            }

            protected bool isAlive()
            {
                return _health > 0;
            }


        }

        class Knight : Warrior
        {
            private int _block;

            public Knight(int health, int damage, int block) : base(health, damage)
            {
                _block = block;
            }

            public override void DisplayStats()
            {
                base.DisplayStats();
                Console.WriteLine($"Block: {_block}");
            }
        }

        class Thief : Warrior
        {
            private int _dodge;
            private int _crit;

            public Thief(int health, int damage, int dodge, int crit) : base(health, damage)
            {
                _dodge = dodge;
                _crit = crit;
            }
            public override void DisplayStats()
            {
                base.DisplayStats();
                Console.WriteLine($"Dodge: {_dodge}");
                Console.WriteLine($"Critical Change: {_crit}");
            }
        }

        class Dualist : Warrior
        {
            private int _parry;

            public Dualist(int health, int damage, int parry) : base(health, damage)
            {
                _parry = parry;
            }
            public override void DisplayStats()
            {
                base.DisplayStats();
                Console.WriteLine($"Parry: {_parry}");
            }
        }

        class Summoner : Warrior
        {
            private int _petDamage;
            private int _petHealth;

            public Summoner(int health, int damage, int petDamage, int petHealth) : base(health, damage)
            {
                _petDamage = petDamage;
                _petHealth = petHealth;
            }

            public override void DisplayStats()
            {
                base.DisplayStats();
                Console.WriteLine($"Pet Health: {_petHealth}");
                Console.WriteLine($"Pet Damage: {_petDamage}");
            }
        }

        class BloodHunter : Warrior
        {
            private int _vamp;

            public BloodHunter(int health, int damage, int vamp) : base(health, damage)
            {
                _vamp = vamp;
            }

            public override void DisplayStats()
            {
                base.DisplayStats();
                Console.WriteLine($"Vamp: {_vamp}");
            }
        }

    }

    class Custom
    {
        public static void WriteInColor(string text, ConsoleColor color = ConsoleColor.DarkRed, bool customPos = false, int xPos = 0, int YPos = 0)
        {
            if (customPos)
                Console.SetCursorPosition(xPos, YPos);

            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public static void PressAnythingToContinue(ConsoleColor color = ConsoleColor.DarkYellow, bool customPos = false, int xPos = 0, int YPos = 0, string text = "press anything to continue")
        {
            if (customPos)
                Console.SetCursorPosition(xPos, YPos);

            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
            Console.ReadKey();
            Console.Clear();
        }

        public static void WriteFilled(string text, ConsoleColor color = ConsoleColor.DarkYellow, bool customPos = false, int xPos = 0, int yPos = 0)
        {
            int borderLength = text.Length + 2;
            string filler = new string('═', borderLength);
            string topBorder = "╔" + filler + "╗";
            string line = $"║ {text} ║";
            string bottomBorder = "╚" + filler + "╝";

            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = color;

            WriteAtPosition(xPos, yPos, topBorder);
            WriteAtPosition(xPos, yPos + 1, line);
            WriteAtPosition(xPos, yPos + 2, bottomBorder);

            Console.ResetColor();
        }

        public static void WriteAtPosition(int xPos, int yPos, string text)
        {
            Console.SetCursorPosition(xPos, yPos);
            Console.WriteLine(text);
        }

        public static int GetUserNumberInRange(string startMessage = "The station number", int maxInput = 100)
        {
            int userInput = 0;
            bool isValidInput = false;

            Console.WriteLine(startMessage);

            while (!isValidInput)
            {
                if (int.TryParse(Console.ReadLine(), out userInput))
                {
                    if (userInput > 0 && userInput <= maxInput)
                        isValidInput = true;
                    else
                        Custom.WriteInColor($"\nPlease enter a number between 1 and {maxInput}:", ConsoleColor.Red);

                }
                else
                {
                    Custom.WriteInColor("\nInvalid input. Please enter a number:", ConsoleColor.Red);
                }
            }

            return userInput - 1;
        }
    }
}
