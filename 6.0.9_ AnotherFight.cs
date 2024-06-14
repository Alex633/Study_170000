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
    //      demons into the list - not just demon count
    //      duelist can kill the demons
    //      name generator for demons that are random


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
                    new Duelist(),
                    new DemonLord(),
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
                Fighter tempFighter = null;
                int fighter1DiceValue;
                int fighter2DiceValue;

                Custom.WriteLineInColor("Determing who is going first:\n", ConsoleColor.DarkGray);

                fighter1DiceValue = _fighter1.RollTheDice();
                Console.WriteLine($"{_fighter1.GetType().Name} rolling the dice... {fighter1DiceValue}");
                Custom.PressAnythingToContinue(ConsoleColor.DarkYellow, false, 0, 26, "press anything to continue\n", false);
                fighter2DiceValue = _fighter2.RollTheDice();
                Console.WriteLine($"{_fighter2.GetType().Name} rolling the dice... {fighter2DiceValue}");
                Custom.PressAnythingToContinue(ConsoleColor.DarkYellow, false, 0, 26, "press anything to continue\n", false);

                if (fighter1DiceValue > fighter2DiceValue)
                {
                    Custom.WriteLineInColor($"\n{_fighter1.GetType().Name} going first 🚩", ConsoleColor.Blue);
                }
                else
                {
                    Custom.WriteLineInColor($"\n{_fighter2.GetType().Name} going first 🚩", ConsoleColor.Blue);
                    tempFighter = _fighter1;
                    _fighter1 = _fighter2;
                    _fighter2 = tempFighter;
                }

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
                    Custom.WriteLineInColor($"\n{_fighter1.GetType().Name} is Victorious! 😎👌🔥\n", ConsoleColor.Cyan);
                else if (_fighter2.IsAlive())
                    Custom.WriteLineInColor($"\n{_fighter2.GetType().Name} is Victorious 😎👌🔥!\n", ConsoleColor.Cyan);
                else
                    Custom.WriteLineInColor($"\nIt's just two corpses. Bummer\n", ConsoleColor.Red);
            }
        }

        abstract class Fighter
        {
            public int Damage { get; protected set; }

            public int Health { get; protected set; }

            public Fighter()
            {
                Health = 30;
                Damage = 2;
            }

            public Fighter(int health, int damage)
            {
                Health = health;
                Damage = damage;
            }

            public int RollTheDice()
            {
                Random _random = new Random();
                return _random.Next(1, 101);
            }

            public virtual void DisplayStats()
            {
                Custom.WriteLineInColor($"{GetType().Name}", ConsoleColor.Blue);
                Console.WriteLine($"HP: {Health}\n" +
                    $"Damage: {Damage}");
            }

            public virtual void Attack(Fighter target)
            {
                Console.Write($"[❤️{Health}] {GetType().Name} ⚔︎{Damage} attacks ");
                target.TakeDamage(this);
            }

            public virtual void TakeDamage(Fighter opponent)
            {
                Health -= opponent.Damage;
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
                _blockFrequency = 2;
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
            private int _critDamageModifier;

            public Thief() : base(26, 2)
            {
                _crit = 36;
                _critDamageModifier = 2;
            }

            public override void DisplayStats()
            {
                base.DisplayStats();
                Console.WriteLine($"Critical Chance: {_crit}%");
            }

            public override void Attack(Fighter target)
            {
                int defaultDamage;

                if (RollTheDice() <= _crit)
                {
                    defaultDamage = Damage;
                    Damage = _critDamageModifier * Damage;
                    Console.Write($"[❤️{Health}] {GetType().Name} ");
                    Custom.WriteInColor($"🏹{Damage} critically strikes ", ConsoleColor.DarkRed);
                    target.TakeDamage(this);
                    Damage = defaultDamage;
                }
                else
                {
                    base.Attack(target);
                }
            }
        }


        class Duelist : Fighter
        {
            private int _parryFrequency;
            private int _attackedCount;

            public Duelist() : base(22, 2)
            {
                _parryFrequency = 3;
            }
            public override void DisplayStats()
            {
                base.DisplayStats();
                Console.WriteLine($"Parries every {_parryFrequency}rd strike");
            }

            public override void TakeDamage(Fighter opponent)
            {
                _attackedCount++;

                if (_attackedCount == _parryFrequency)
                {
                    _attackedCount = 0;
                    Custom.WriteLineInColor($"but [❤️{Health}] {GetType().Name} ↩ parries the attack!", ConsoleColor.Blue);
                    Attack(opponent);
                }
                else
                {
                    base.TakeDamage(opponent);

                }
            }
        }

        class DemonLord : Fighter
        {
            private Demon _demon = new Demon();

            private int _mana;
            private int _manaCostToSummon;
            private int _manaPerChanelling;
            private int _demonsCount;

            public DemonLord() : base(18, 0)
            {
                _mana = 2;
                _manaCostToSummon = 5;
                _manaPerChanelling = 3;
                _demonsCount = 0;
            }

            public override void DisplayStats()
            {
                base.DisplayStats();
                Console.WriteLine($"Mana cost to summon the demon: {_manaCostToSummon} (Demon's HP: {_demon.Health}, damage {_demon.Damage})");
                Console.WriteLine($"Mana per chanelling: {_manaPerChanelling}");
            }

            public override void Attack(Fighter target)
            {
                //if (!_demon.IsAlive)
                //{
                //    _demonsCount--;
                //}    

                if (_mana >= _manaCostToSummon)
                {
                    _mana -= _manaCostToSummon;
                    _demonsCount++;
                    Console.WriteLine($"[🔥{_mana}⬇] {GetType().Name} summoning a demon {_demon.Name} from the hell itself (-{_manaCostToSummon})");
                }
                else
                {
                    _mana += _manaPerChanelling;
                    Console.WriteLine($"[🔥{_mana}↑] {GetType().Name} channeling the forces of hell (+{_manaPerChanelling})");
                }

                for (int i = 0; i < _demonsCount; i++)
                {
                    _demon.Attack(target);
                }
            }
        }

        class Demon : Fighter
        {
            public string Name { get; private set; }
            Random random = new Random();


            public Demon() : base(1, 2)
            {
                Name = SelectRandomName();
            }

            public override void Attack(Fighter target)
            {
                Console.Write($"[❤️{Health}] {GetType().Name} {Name} ⚔︎{Damage} attacks ");
                target.TakeDamage(this);
            }

            private string SelectRandomName()
            {
                int index;

                string[] names = { "Bob", "Greg", "John", "Mike", "Steve", "Tom", "Bill", "Dave", "Jim", "Larry" };
                index = random.Next(names.Length);
                return names[index];
            }
        }

        class BloodHunter : Fighter
        {
            private int _vamp;
            int maxHealth;

            public BloodHunter() : base(20, 1)
            {
                _vamp = 200;
                maxHealth = Health;
            }

            public override void DisplayStats()
            {
                base.DisplayStats();
                Console.WriteLine($"Vamp: {_vamp}%");
            }

            public override void Attack(Fighter target)
            {
                if (Health + Damage * (_vamp / 100) <= maxHealth)
                {
                    int targetCurrentHealth = target.Health;

                    Console.Write($"[❤️{Health}] {GetType().Name} ⚔︎{Damage} attacks ");
                    target.TakeDamage(this);

                    if (target.Health < targetCurrentHealth)
                    {
                        Health += Damage * (_vamp / 100);
                        Custom.WriteLineInColor($"[❤️{Health}↑] {GetType().Name} tastes the blood. It tasted delicious (for him)", ConsoleColor.Green);
                    }
                }
                else
                {
                    base.Attack(target);
                }
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
