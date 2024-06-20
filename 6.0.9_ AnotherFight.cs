namespace millionDollarsCourses
{
    using System;
    using System.Collections.Generic;

    //Создать 5 классов, пользователь выбирает 2 воина и они сражаются друг с другом до смерти. У каждого класса могут быть свои статы.
    //Каждый класс должен иметь особую способность для атаки, которая свойственна только его типу класса!
    //Если можно выбрать одинаковых бойцов, то это не должна быть одна и та же ссылка на одного бойца, чтобы он не атаковал сам себя.
    //Пример, что может быть уникальное у бойцов. Кто-то каждый 3 удар наносит удвоенный урон, другой имеет 30% увернуться от полученного урона,
    //кто-то при получении урона немного себя лечит. Будут новые поля у наследников. У кого-то может быть мана и это только его особенность.

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
            private readonly List<Fighter> _fighters;

            public BattleSystem()
            {
                _fighters = new List<Fighter>
                {
                    new Knight(),
                    new Thief(),
                    new Duelist(),
                    new DemonLord(),
                    new BloodHunter()
                };
            }

            public void Fight()
            {
                Text.WriteLineInColor("Battle\n", ConsoleColor.DarkGray);

                while (_fighter1.IsAlive && _fighter2.IsAlive)
                {
                    StartRound();

                    if (_round % 2 != 0)
                        _fighter1.Attack(_fighter2);
                    else
                        _fighter2.Attack(_fighter1);

                    Utils.PressAnythingToContinue(ConsoleColor.DarkYellow, false, 0, 26, "press anything to continue\n", false);
                }
            }

            public void SelectFighter()
            {
                int fighterNumOne;
                int fighterNumTwo;
                bool isUniqueFighter = false;

                DisplayFighters();
                fighterNumOne = Utils.GetUserNumberInRange("Select a Fighter: ", _fighters.Count);
                _fighter1 = _fighters[fighterNumOne - 1];

                while (!isUniqueFighter)
                {
                    fighterNumTwo = Utils.GetUserNumberInRange($"Select an Opponent for {_fighter1.GetType().Name}: ", _fighters.Count);

                    if (fighterNumTwo != fighterNumOne)
                    {
                        _fighter2 = _fighters[fighterNumTwo - 1];
                        isUniqueFighter = true;
                    }
                    else
                    {
                        Text.WriteLineInColor($"{_fighter1.GetType().Name} can't fight himself, can he?");
                        Utils.PressAnythingToContinue();
                        DisplayFighters();
                    }
                }

                Text.WriteLineInColor($"\n{_fighter1.ClassName} vs {_fighter2.ClassName}", ConsoleColor.Blue);

                Utils.PressAnythingToContinue();
            }

            private void DisplayFighters()
            {
                int count = 1;

                Text.WriteLineInColor("Fighter Selection\n", ConsoleColor.DarkGray);

                foreach (Fighter fighter in _fighters)
                {
                    Console.Write(count + ". ");
                    count++;
                    fighter.DisplayStats();
                    Console.WriteLine();
                }
            }

            public void DetermineInitiative()
            {
                Fighter tempFighter;
                int fighter1DiceValue;
                int fighter2DiceValue;
                bool initiativeDetermined = false;

                Text.WriteLineInColor("Determining who goes first\n", ConsoleColor.DarkGray);

                while (!initiativeDetermined)
                {
                    fighter1DiceValue = Utils.GenerateRandomNumber(101);
                    Console.WriteLine($"{_fighter1.GetType().Name} rolling the dice... {fighter1DiceValue}");
                    Utils.PressAnythingToContinue(ConsoleColor.DarkYellow, false, 0, 26, "press anything to continue\n", false);
                    fighter2DiceValue = Utils.GenerateRandomNumber(101);
                    Console.WriteLine($"{_fighter2.GetType().Name} rolling the dice... {fighter2DiceValue}");
                    Utils.PressAnythingToContinue(ConsoleColor.DarkYellow, false, 0, 26, "press anything to continue\n", false);

                    if (fighter1DiceValue > fighter2DiceValue)
                    {
                        Text.WriteLineInColor($"\n{_fighter1.GetType().Name} goes  first 🚩", ConsoleColor.Blue);
                        initiativeDetermined = true;
                    }
                    else if (fighter1DiceValue < fighter2DiceValue)
                    {
                        Text.WriteLineInColor($"\n{_fighter2.GetType().Name} goes  first 🚩", ConsoleColor.Blue);
                        tempFighter = _fighter1;
                        _fighter1 = _fighter2;
                        _fighter2 = tempFighter;
                        initiativeDetermined = true;
                    }
                    else
                    {
                        Text.WriteLineInColor($"\nSame Values. Rerolling...", ConsoleColor.Blue);
                        Utils.PressAnythingToContinue(ConsoleColor.DarkYellow, false, 0, 26, "press anything to continue\n", false);
                    }
                }

                Utils.PressAnythingToContinue();
            }

            private void StartRound()
            {
                _round++;
                Console.WriteLine($"Round {_round}");
            }

            public void DisplayWinner()
            {
                if (_fighter1.IsAlive)
                    Text.WriteLineInColor($"\n{_fighter1.GetType().Name} is Victorious! 😎👌🔥\n", ConsoleColor.Cyan);
                else if (_fighter2.IsAlive)
                    Text.WriteLineInColor($"\n{_fighter2.GetType().Name} is Victorious 😎👌🔥!\n", ConsoleColor.Cyan);
                else
                    Text.WriteLineInColor($"\nIt's just two corpses. Bummer\n", ConsoleColor.Red);
            }
        }

        abstract class Fighter
        {
            public int Damage { get; protected set; }
            public int Health { get; protected set; }
            public string ClassName { get; protected set; }
            public bool IsAlive
            {
                get { return Health > 0; }
                protected set { }
            }

            public Fighter(int health, int damage)
            {
                Health = health;
                Damage = damage;
                ClassName = GetType().Name;
            }

            public virtual void DisplayStats()
            {
                Text.WriteLineInColor($"{ClassName}", ConsoleColor.Blue);
                Console.WriteLine($"HP: ❤️{Health}\n" +
                                  $"Damage: ⚔︎{Damage}");
            }

            public virtual void Attack(Fighter target)
            {
                Console.Write($"[❤️{Health}] {ClassName} ⚔︎{Damage} attacks ");
                target.TakeDamage(this);
            }

            public virtual void TakeDamage(Fighter opponent)
            {
                Health -= opponent.Damage;
                Text.WriteLineInColor($"{ClassName} [❤️{Health}⬇]", ConsoleColor.Red);
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
                    BlockDamage();
                }
                else
                {
                    base.TakeDamage(opponent);
                }
            }

            private void BlockDamage()
            {
                _attackedCount = 0;
                Text.WriteLineInColor($"but {ClassName} ⛊ blocks all the damage! Wow...", ConsoleColor.Blue);
            }
        }

        class Thief : Fighter
        {
            private readonly int _critChancePercent;
            private readonly int _critDamageModifier;

            public Thief() : base(26, 2)
            {
                _critChancePercent = 36;
                _critDamageModifier = 2;
            }

            public override void DisplayStats()
            {
                base.DisplayStats();
                Console.WriteLine($"Critical Chance: {_critChancePercent}%");
            }

            public override void Attack(Fighter target)
            {
                if (Utils.GenerateRandomNumber(101) <= _critChancePercent)
                    CriticallyStrike(target, Damage);
                else
                    base.Attack(target);
            }

            private void CriticallyStrike(Fighter target, int defaultDamage)
            {
                Damage *= _critDamageModifier;
                Console.Write($"[❤️{Health}] {ClassName} ");
                Text.WriteInColor($"🏹{Damage} critically fires ", ConsoleColor.DarkRed);
                target.TakeDamage(this);
                Damage = defaultDamage;
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
                    ParryAttack(opponent);
                else
                    base.TakeDamage(opponent);
            }

            private void ParryAttack(Fighter opponent)
            {
                _attackedCount = 0;
                Text.WriteLineInColor($"but {ClassName} ↩ parries the attack!", ConsoleColor.Blue);
                Attack(opponent);
            }
        }

        class DemonLord : Fighter
        {
            private int _startingMana;
            private int _manaCostToSummon;
            private int _manaPerChanelling;
            private readonly List<Demon> _demons = new List<Demon>();
            private readonly Demon _demonModel = new Demon();
            private Demon _newBornDemon;

            public DemonLord() : base(18, 0)
            {
                _startingMana = 2;
                _manaCostToSummon = 5;
                _manaPerChanelling = 3;
            }

            public override void DisplayStats()
            {
                base.DisplayStats();
                Console.WriteLine($"Starting mana: 🔥{_startingMana}. Mana per chanelling: 🔥{_manaPerChanelling}");
                Console.WriteLine($"Mana cost to summon a demon: 🔥{_manaCostToSummon} (Demon's HP: ❤️{_demonModel.Health}, damage: ⚔︎{_demonModel.Damage}). Other fighters can't attack demons directly");
            }

            public override void Attack(Fighter target)
            {

                if (_startingMana >= _manaCostToSummon)
                    SummonDemon();
                else
                    ChannelMana();

                CommandDemonsToAttack(target);
            }

            private void ChannelMana()
            {
                _startingMana += _manaPerChanelling;
                Console.WriteLine($"[🔥{_startingMana}↑] {GetType().Name} channeling the forces of hell (+🔥{_manaPerChanelling})");
            }

            private void SummonDemon()
            {
                _newBornDemon = new Demon();
                _startingMana -= _manaCostToSummon;
                _demons.Add(_newBornDemon);
                Console.WriteLine($"[🔥{_startingMana}⬇] {GetType().Name} summoning a demon {_newBornDemon.PersonalName} from the hell itself (-🔥{_manaCostToSummon})");
            }

            private void CommandDemonsToAttack(Fighter target)
            {
                foreach (Demon demon in _demons)
                {
                    if (demon.IsAlive)
                        demon.Attack(target);
                }
            }
        }

        class Demon : Fighter
        {
            private List<string> _helishNames;

            public string PersonalName { get; private set; }

            public Demon() : base(1, 2)
            {
                _helishNames = new List<string>(new[] { "Greg", "Mike", "Steve", "Tom", "Bill", "Jimmy", "Larry", "Jack", "Dan", "Mark", "Ben", "Fred", "Jake", "Karl", "Luke", "Matt", "Nick", "Paul", 
                                                        "Phil", "Rob", "Sam", "Ted", "Will", "Zach", "Brandon", "Derek", "Eric", "Frank", "George", "Kevin", "Pete", "Ryan", "Tommy", "Vincent", "Walt" });
                PersonalName = SelectHelishName();
            }

            public override void Attack(Fighter target)
            {
                Console.Write($"[❤️{Health}] {PersonalName} the {GetType().Name} ⚔︎{Damage} attacks ");
                target.TakeDamage(this);
            }

            private string SelectHelishName()
            {
                return _helishNames[Utils.GenerateRandomNumber(_helishNames.Count)];
            }

            public override void TakeDamage(Fighter opponent)
            {
                base.TakeDamage(opponent);
                if (!IsAlive)
                    Text.WriteLineInColor($"{PersonalName} the {ClassName} is dead, man. {PersonalName} is dead...", ConsoleColor.Red);
            }
        }

        class BloodHunter : Fighter
        {
            private int _vampirismPercentage;

            public BloodHunter() : base(20, 1)
            {
                _vampirismPercentage = 200;
            }

            public override void DisplayStats()
            {
                base.DisplayStats();
                Console.WriteLine($"Vampirism: {_vampirismPercentage}%. Can overheal");
            }

            public override void Attack(Fighter target)
            {
                int targetHealthAtRoundStart = target.Health;

                base.Attack(target);

                if (targetHealthAtRoundStart > target.Health)
                    Vampirize();
            }

            private void Vampirize()
            {
                int healthToRestore;

                healthToRestore = Damage * (_vampirismPercentage / 100);
                Health += healthToRestore;
                Text.WriteLineInColor($"[❤️{Health}↑] {ClassName} tastes the blood (+❤️{healthToRestore}). It tasted delicious (for him)", ConsoleColor.Green);
            }
        }
    }

    class Utils
    {
        public static void PressAnythingToContinue(ConsoleColor color = ConsoleColor.DarkYellow, bool isCustomPosition = false, int xPosition = 0, int yPosition = 0, string text = "press anything to continue", bool isConsoleClear = true)
        {
            if (isCustomPosition)
                Console.SetCursorPosition(xPosition, yPosition);

            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
            Console.ReadKey(true);

            if (isConsoleClear)
                Console.Clear();
        }

        public static int GenerateRandomNumber(int maxValue, int minValue = 1)
        {
            Random random = new Random();

            return random.Next(minValue, maxValue);
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
                        Text.WriteInColor($"\nPlease enter a number between 1 and {maxInput}: ", ConsoleColor.Red);
                }
                else
                {
                    Text.WriteInColor("\nInvalid input. Please enter a number instead: ", ConsoleColor.Red);
                }
            }

            return userInput;
        }
    }

    class Text
    {
        public static void WriteLineInColor(string text, ConsoleColor color = ConsoleColor.DarkRed, bool isCustomPosition = false, int xPosition = 0, int yPosition = 0)
        {
            Console.ForegroundColor = color;

            if (isCustomPosition)
                WriteLineAtPosition(xPosition, yPosition, text);
            else
                Console.WriteLine(text);

            Console.ResetColor();
        }

        public static void WriteInColor(string text, ConsoleColor color = ConsoleColor.DarkRed, bool isCustomPosition = false, int xPosition = 0, int yPosition = 0)
        {
            Console.ForegroundColor = color;

            if (isCustomPosition)
                WriteAtPosition(xPosition, yPosition, text);
            else
                Console.Write(text);

            Console.ResetColor();
        }

        public static void WriteLineAtPosition(int xPosition, int yPosition, string text)
        {
            Console.SetCursorPosition(xPosition, yPosition);
            Console.WriteLine(text);
        }

        public static void WriteAtPosition(int xPosition, int yPos, string text)
        {
            Console.SetCursorPosition(xPosition, yPos);
            Console.Write(text);
        }
    }
}
