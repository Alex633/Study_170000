namespace millionDollarsCourses
{
    using System;
    using System.Collections.Generic;

    //Создать 5 классов, пользователь выбирает 2 воина и они сражаются друг с другом до смерти. У каждого класса могут быть свои статы.
    //Каждый класс должен иметь особую способность для атаки, которая свойственна только его типу класса!
    //Если можно выбрать одинаковых бойцов, то это не должна быть одна и та же ссылка на одного бойца, чтобы он не атаковал сам себя.
    //Пример, что может быть уникальное у бойцов. Кто-то каждый 3 удар наносит удвоенный урон, другой имеет 30% увернуться от полученного урона,
    //кто-то при получении урона немного себя лечит. Будут новые поля у наследников. У кого-то может быть мана и это только его особенность.

    //todo:
    //      random in determineinitiative are not random
    //      actually apply who is going first
    //      make constructor out of attack and take damage for text
    //      (exm:
    //      attack method outputs
    //      [[hp]thief attacks for 2]
    //      and take damage outputs
    //      [[hp]knight])

    internal class Program
    {
        static void Main()
        {
            BattleSystem battleSystem = new BattleSystem();

            battleSystem.SelectFighter();
            battleSystem.DetermineInitiative();
            battleSystem.Fight();
            battleSystem.DisplayWinner();
        }

        class BattleSystem
        {
            private int _round = 0;
            private Fighter _fighter1;
            private Fighter _fighter2;
            List<Fighter> fighters = new List<Fighter>
                {
                    new Knight(),
                    new Thief(),
                    new Dualist(),
                    new Summoner(),
                    new BloodHunter()
                };

            public BattleSystem()
            {

            }

            public void Fight()
            {
                while (_fighter1.IsAlive() && _fighter2.IsAlive())
                {
                    RoundStart();

                    if (_round % 2 != 0)
                        _fighter1.Attack(_fighter2);
                    else
                        _fighter2.Attack(_fighter1);


                    Custom.PressAnythingToContinue(ConsoleColor.DarkYellow, false, 0, 26, "press anything to continue", false);
                }
            }

            public void SelectFighter()
            {
                int fighterNumOne;
                int fighterNumTwo;
                bool isUniqueFighter = false;

                DisplayFighters();
                fighterNumOne = Custom.GetUserNumberInRange("Select a fighter: ", fighters.Count);
                _fighter1 = fighters[fighterNumOne - 1];

                while (!isUniqueFighter)
                {
                    fighterNumTwo = Custom.GetUserNumberInRange($"Select opponent for {_fighter1.GetType().Name}: ", fighters.Count);

                    if (fighterNumTwo != fighterNumOne)
                    {
                        _fighter2 = fighters[fighterNumTwo - 1];
                        isUniqueFighter = true;
                    }
                    else
                    {
                        Custom.WriteLineInColor($"{_fighter1.GetType().Name} can't fight himself, can't he?");
                        Custom.PressAnythingToContinue();
                        DisplayFighters();
                    }
                }

                Custom.WriteLineInColor($"\n{_fighter1.GetType().Name} vs {_fighter2.GetType().Name}", ConsoleColor.Blue);

                Custom.PressAnythingToContinue();
            }

            public void DisplayFighters()
            {
                Custom.WriteLineInColor("Fighters:\n", ConsoleColor.DarkGray);

                int count = 1;

                foreach (Fighter fighter in fighters)
                {
                    Console.Write(count + ". ");
                    count++;
                    fighter.DisplayStats();
                    Console.WriteLine();
                }
            }

            public void DetermineInitiative()
            {
                Custom.WriteLineInColor("Determing who is going first:\n", ConsoleColor.DarkGray);

                int fighter1DiceValue = _fighter1.RollTheDice();
                Console.WriteLine($"{_fighter1.GetType().Name} rolling the dice... {fighter1DiceValue}");
                Custom.PressAnythingToContinue(ConsoleColor.DarkYellow, false, 0, 26, "press anything to continue\n", false);
                int fighter2DiceValue = _fighter2.RollTheDice();
                Console.WriteLine($"{_fighter2.GetType().Name} rolling the dice... {fighter2DiceValue}");
                Custom.PressAnythingToContinue(ConsoleColor.DarkYellow, false, 0, 26, "press anything to continue\n", false);

                if (fighter1DiceValue > fighter2DiceValue)
                    Custom.WriteLineInColor($"\n{_fighter1.GetType().Name} going first 🚩", ConsoleColor.Blue);
                else
                    Custom.WriteLineInColor($"\n{_fighter2.GetType().Name} going first 🚩", ConsoleColor.Blue);

                Custom.PressAnythingToContinue();
            }

            private void RoundStart()
            {
                _round++;
                Console.WriteLine($"\nRound {_round}");
            }

            public void DisplayWinner()
            {
                if (_fighter1.IsAlive())
                    Custom.WriteLineInColor($"\n{_fighter1.GetType().Name} is Victorious! 😎👌🔥\n", ConsoleColor.Green);
                else if (_fighter2.IsAlive())
                    Custom.WriteLineInColor($"\n{_fighter2.GetType().Name} is Victorious 😎👌🔥!\n", ConsoleColor.Green);
                else
                    Custom.WriteLineInColor($"\nIt's just two corpses. Bummer\n", ConsoleColor.Red);
            }
        }

        abstract class Fighter
        {
            protected int _damage;
            private Random _random = new Random();
            public int Health { get; protected set; }

            public Fighter()
            {
                Health = 30;
                _damage = 2;
            }

            public Fighter(int health, int damage)
            {
                Health = health;
                _damage = damage;
            }

            public int RollTheDice()
            {
                return _random.Next(1, 101);
            }

            public virtual void DisplayStats()
            {
                Custom.WriteLineInColor($"{GetType().Name}", ConsoleColor.Blue);
                Console.WriteLine($"HP: {Health}\n" +
                    $"Damage: {_damage}");
            }

            public virtual void Attack(Fighter target)
            {
                target.TakeDamage(this);
                Console.Write($"[❤️{Health}] {GetType().Name} ⚔︎{_damage} attacks ");
            }

            public virtual void TakeDamage(Fighter opponent)
            {
                Health -= opponent._damage;
                Custom.WriteLineInColor($"{GetType().Name} [❤️{Health}⬇]", ConsoleColor.Red);
            }

            public bool IsAlive()
            {
                return Health > 0;
            }
        }


        class Knight : Fighter
        {
            private int _blockFrequency;
            private int _attackedCount;

            public Knight() : base(40, 1)
            {
                _blockFrequency = 2; //every [*] attack is blocked
                _attackedCount = 0;
            }

            public override void DisplayStats()
            {
                base.DisplayStats();
                Console.WriteLine($"Blocks every {_blockFrequency}nd strike");
            }

            public override void TakeDamage(Fighter opponent)
            {
                _attackedCount++;

                if (_attackedCount == _blockFrequency)
                {
                    _attackedCount = 0;
                    Custom.WriteLineInColor($"but [❤️{Health}] {GetType().Name} ⛊ blocks all the damage! Wow...", ConsoleColor.Blue);
                }
                else
                {
                    base.TakeDamage(opponent);

                }
            }
        }

        class Thief : Fighter
        {
            private int _crit;

            public Thief() : base(25, 2)
            {
                _crit = 25;
            }

            public override void DisplayStats()
            {
                base.DisplayStats();
                Console.WriteLine($"Critical Chance: {_crit}%");
            }

            public override void Attack(Fighter target)
            {
                

                if (RollTheDice() <= _crit)
                {
                    target.TakeDamage(this);
                    Custom.WriteLineInColor($"[❤️{Health}] {GetType().Name} 🏹{_damage * 2} critically strikes ", ConsoleColor.DarkRed);
                }
                else
                {
                    base.Attack(target);
                }
            }
        }


        class Dualist : Fighter
        {
            private int _parry;

            public Dualist() : base()
            {
                _parry = 4; //every fourth attack is parried
            }
            public override void DisplayStats()
            {
                base.DisplayStats();
                Console.WriteLine($"Parry: Every {_parry} strike");
            }
        }

        class Summoner : Fighter
        {
            private int _petDamage;
            private int _petHealth;

            public Summoner() : base(20, 1)
            {
                _petDamage = 2;
                _petHealth = 15;
            }

            public override void DisplayStats()
            {
                base.DisplayStats();
                Console.WriteLine($"Pet Health: {_petHealth}");
                Console.WriteLine($"Pet Damage: {_petDamage}");
            }
        }

        class BloodHunter : Fighter
        {
            private int _vamp;

            public BloodHunter() : base(20, 1)
            {
                _vamp = 100;
            }

            public override void DisplayStats()
            {
                base.DisplayStats();
                Console.WriteLine($"Vamp: {_vamp}%");
            }
        }
    }

    class Custom
    {
        public static void WriteLineInColor(string text, ConsoleColor color = ConsoleColor.DarkRed, bool customPos = false, int xPos = 0, int YPos = 0)
        {
            if (customPos)
                Console.SetCursorPosition(xPos, YPos);

            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public static void WriteInColor(string text, ConsoleColor color = ConsoleColor.DarkRed, bool customPos = false, int xPos = 0, int YPos = 0)
        {
            if (customPos)
                Console.SetCursorPosition(xPos, YPos);

            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }

        public static void PressAnythingToContinue(ConsoleColor color = ConsoleColor.DarkYellow, bool customPos = false, int xPos = 0, int YPos = 0, string text = "press anything to continue", bool consoleClear = true)
        {
            if (customPos)
                Console.SetCursorPosition(xPos, YPos);

            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
            Console.ReadKey(true);

            if (consoleClear)
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

        public static int GetUserNumberInRange(string startMessage = "Select Number: ", int maxInput = 100)
        {
            int userInput = 0;
            bool isValidInput = false;

            Console.Write(startMessage);

            while (!isValidInput)
            {
                if (int.TryParse(Console.ReadLine(), out userInput))
                {
                    if (userInput > 0 && userInput <= maxInput)
                        isValidInput = true;
                    else
                        Custom.WriteInColor($"\nPlease enter a number between 1 and {maxInput}: ", ConsoleColor.Red);

                }
                else
                {
                    Custom.WriteInColor("\nInvalid input. Please enter a number instead: ", ConsoleColor.Red);
                }
            }

            return userInput;
        }
    }
}
