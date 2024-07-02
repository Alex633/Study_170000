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

            battleSystem.ChooseFighters();
            battleSystem.DetermineInitiative();
            battleSystem.Fight();
            battleSystem.DisplayWinner();
        }
    }

    class BattleSystem
    {
        private readonly List<Fighter> _fighters;
        private int _round = 0;
        private Fighter _fighter1;
        private Fighter _fighter2;

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
            int TurnAlternator = 2;

            Utility.WriteLineColored("Battle\n", ConsoleColor.DarkGray);

            while (_fighter1.IsAlive && _fighter2.IsAlive)
            {
                StartRound();

                if (_round % TurnAlternator != 0)
                    _fighter1.Attack(_fighter2);
                else
                    _fighter2.Attack(_fighter1);

                Utility.PressAnythingToContinue(ConsoleColor.DarkYellow, false, 0, 26, "press anything to continue\n", false);
            }
        }

        public void DetermineInitiative()
        {
            Fighter tempFighter;
            int firstFighterDiceValue;
            int secondFighterDiceValue;
            bool initiativeDetermined = false;

            Utility.WriteLineColored("Determining who goes first\n", ConsoleColor.DarkGray);

            while (initiativeDetermined == false)
            {
                firstFighterDiceValue = RollDiceForFighter(_fighter1);
                secondFighterDiceValue = RollDiceForFighter(_fighter2);

                if (firstFighterDiceValue > secondFighterDiceValue)
                {
                    initiativeDetermined = true;
                }
                else if (firstFighterDiceValue < secondFighterDiceValue)
                {
                    tempFighter = _fighter1;
                    _fighter1 = _fighter2;
                    _fighter2 = tempFighter;
                    initiativeDetermined = true;
                }
                else
                {
                    Utility.WriteLineColored($"\nSame Values. Rerolling...", ConsoleColor.Blue);
                    Utility.PressAnythingToContinue(ConsoleColor.DarkYellow, false, 0, 26, "press anything to continue\n", false);
                }
            }

            Utility.WriteLineColored($"\n{_fighter1.ClassName} goes first 🚩", ConsoleColor.Blue);
            Utility.PressAnythingToContinue();
        }

        public void ChooseFighters()
        {
            _fighter1 = SelectFighter();
            _fighter2 = SelectFighter($"Select an Opponent for {_fighter1.ClassName}: ");

            if (_fighter1 == _fighter2)
            {
                _fighter2 = _fighter1.Clone();
                Utility.WriteLineColored($"\n{_fighter1.ClassName} struggling with a mental health issues and fights himself", ConsoleColor.Red);
            }
            else
            {
                Utility.WriteLineColored($"\n{_fighter1.ClassName} vs {_fighter2.ClassName}", ConsoleColor.Blue);
            }

            Utility.PressAnythingToContinue();
        }

        private Fighter SelectFighter(string text = "Select a Fighter: ")
        {
            int fighterNumber;

            DisplayFighters();
            fighterNumber = Utility.GetUserNumberInRange(text, _fighters.Count);
            return _fighters[fighterNumber - 1];
        }

        private int RollDiceForFighter(Fighter fighter)
        {
            int diceValue;

            diceValue = Utility.GenerateRandomNumber();
            Console.WriteLine($"{fighter.ClassName} rolling the dice... {diceValue}");
            Utility.PressAnythingToContinue(ConsoleColor.DarkYellow, false, 0, 26, "press anything to continue\n", false);
            return diceValue;
        }

        private void DisplayFighters()
        {
            int count = 1;

            Utility.WriteLineColored("Fighter Selection\n", ConsoleColor.DarkGray);

            foreach (Fighter fighter in _fighters)
            {
                Console.Write(count + ". ");
                count++;
                fighter.DisplayStats();
                Console.WriteLine();
            }
        }

        private void StartRound()
        {
            _round++;
            Console.WriteLine($"Round {_round}");
        }

        public void DisplayWinner()
        {
            Fighter winner = null;

            if (_fighter1.IsAlive)
                winner = _fighter1;
            else if (_fighter2.IsAlive)
                winner = _fighter2;

            if (winner != null && winner.ClassName == Fighter.CloneName)
                Utility.WriteLineColored($"\n{winner.ClassName} wins. As always. Please help\n", ConsoleColor.DarkRed);
            else if (winner != null)
                Utility.WriteLineColored($"\n{winner.ClassName} is Victorious! 😎👌🔥\n", ConsoleColor.Cyan);
            else
                Utility.WriteLineColored($"\nIt's just two corpses. Bummer\n", ConsoleColor.Red);
        }
    }

    abstract class Fighter
    {
        public const string CloneName = "Depression";

        public Fighter(int health, int damage)
        {
            Health = health;
            Damage = damage;
            ClassName = GetType().Name;
        }

        public bool IsAlive
        {
            get { return Health > 0; }
            protected set { }
        }

        public int Damage { get; protected set; }
        public int Health { get; protected set; }
        public string ClassName { get; protected set; }

        public abstract Fighter Clone();

        public virtual void DisplayStats()
        {
            Utility.WriteLineColored($"{ClassName}", ConsoleColor.Blue);
            Console.WriteLine($"HP: ❤️{Health}\n" +
                              $"Damage: ⚔︎{Damage}");
        }

        public virtual void Attack(Fighter target)
        {
            Console.Write($"[❤️{Health}] {ClassName} ⚔︎{Damage} attacks ");
            target.TakeDamage(Damage);
        }

        public virtual void TakeDamage(int damage)
        {
            Health -= damage;
            Utility.WriteLineColored($"{ClassName} [❤️{Health}⬇]", ConsoleColor.Red);
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

        public override Fighter Clone()
        {
            Knight clone = new Knight
            {
                ClassName = CloneName
            };

            return clone;
        }

        public override void DisplayStats()
        {
            base.DisplayStats();
            Console.WriteLine($"Blocks every {_blockFrequency}nd strike");
        }

        public override void TakeDamage(int damage)
        {
            _attackedCount++;

            if (_attackedCount == _blockFrequency)
                BlockDamage();
            else
                base.TakeDamage(damage);
        }

        private void BlockDamage()
        {
            _attackedCount = 0;
            Utility.WriteLineColored($"but {ClassName} ⛊ blocks all the damage! Wow...", ConsoleColor.Blue);
        }
    }

    class Thief : Fighter
    {
        private readonly int _critChancePercent;
        private readonly int _critDamage;
        private readonly int _critDamageModifier;

        public Thief() : base(26, 2)
        {
            _critChancePercent = 36;
            _critDamageModifier = 2;
            _critDamage = _critDamageModifier * Damage;
        }

        public override Fighter Clone()
        {
            Thief clone = new Thief
            {
                ClassName = CloneName
            };

            return clone;
        }

        public override void DisplayStats()
        {
            base.DisplayStats();
            Console.WriteLine($"Critical Chance: {_critChancePercent}%");
        }

        public override void Attack(Fighter target)
        {
            if (Utility.GenerateRandomNumber() <= _critChancePercent)
                StrikeCritically(target);
            else
                base.Attack(target);
        }

        private void StrikeCritically(Fighter target)
        {
            Console.Write($"[❤️{Health}] {ClassName} ");
            Utility.WriteColored($"🏹{_critDamage} critically fires ", ConsoleColor.DarkRed);
            target.TakeDamage(_critDamage);
        }
    }


    class Duelist : Fighter
    {
        private int _parryFrequency;
        private int _attackedCount;
        private bool _isParriedState = false;

        public Duelist() : base(22, 2)
        {
            _parryFrequency = 3;
        }

        public override Fighter Clone()
        {
            Duelist clone = new Duelist
            {
                ClassName = CloneName
            };

            return clone;
        }

        public override void DisplayStats()
        {
            base.DisplayStats();
            Console.WriteLine($"Parries every {_parryFrequency}rd strike");
        }

        public override void Attack(Fighter target)
        {
            if (_isParriedState)
                StrikeBack(target);

            base.Attack(target);
        }

        public override void TakeDamage(int damage)
        {
            _attackedCount++;

            if (_attackedCount != _parryFrequency)
                base.TakeDamage(damage);
            else
                ParryAttack();
        }

        private void ParryAttack()
        {
            Utility.WriteLineColored($"but {ClassName} ↩ parries the attack!", ConsoleColor.Blue);
            _isParriedState = true;
            _attackedCount = 0;
        }

        private void StrikeBack(Fighter target)
        {
            Console.Write($"[❤️{Health}] {ClassName} ⚔︎{Damage} strikes back after successful parry - ");
            target.TakeDamage(Damage);
            _isParriedState = false;
        }
    }

    class DemonLord : Fighter
    {
        private readonly List<Demon> _demons = new List<Demon>();
        private readonly Demon _demonModel = new Demon();
        private int _startingMana;
        private int _manaCostToSummon;
        private int _manaPerChanelling;
        private Demon _newBornDemon;

        public DemonLord() : base(18, 0)
        {
            _startingMana = 2;
            _manaCostToSummon = 5;
            _manaPerChanelling = 3;
        }

        public override Fighter Clone()
        {
            DemonLord clone = new DemonLord
            {
                ClassName = CloneName
            };

            return clone;
        }

        public override void DisplayStats()
        {
            base.DisplayStats();
            Console.WriteLine($"Starting mana: 🔥{_startingMana}. Mana per chanelling: 🔥{_manaPerChanelling}");
            Console.WriteLine($"Mana cost to summon a demon: 🔥{_manaCostToSummon} (Demon's HP: ❤️{_demonModel.Health}, damage: ⚔︎{_demonModel.Damage}). Other fighters can't attack demons");
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
            Console.WriteLine($"[🔥{_startingMana}↑] {ClassName} channeling the forces of hell (+🔥{_manaPerChanelling})");
        }

        private void SummonDemon()
        {
            _newBornDemon = new Demon();
            _startingMana -= _manaCostToSummon;
            _demons.Add(_newBornDemon);
            Console.WriteLine($"[🔥{_startingMana}⬇] {ClassName} summoning a demon {_newBornDemon.PersonalName} from the hell itself (-🔥{_manaCostToSummon})");
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

        public Demon() : base(1, 2)
        {
            _helishNames = new List<string>(new[] { "Greg", "Mike", "Steve", "Tom", "Bill", "Jimmy", "Larry", "Jack", "Dan", "Mark", "Ben", "Fred", "Jake", "Karl", "Luke", "Matt", "Nick", "Paul",
                                                        "Phil", "Rob", "Sam", "Ted", "Will", "Zach", "Brandon", "Derek", "Eric", "Frank", "George", "Kevin", "Pete", "Ryan", "Tommy", "Vincent", "Walt" });
            PersonalName = SelectHelishName();
        }

        public override Fighter Clone()
        {
            return new Demon();
        }

        public string PersonalName { get; private set; }

        public override void Attack(Fighter target)
        {
            Console.Write($"[❤️{Health}] {PersonalName} the {GetType().Name} ⚔︎{Damage} attacks ");
            target.TakeDamage(Damage);
        }

        private string SelectHelishName()
        {
            return _helishNames[Utility.GenerateRandomNumber(_helishNames.Count)];
        }
    }

    class BloodHunter : Fighter
    {
        private int _vampirismPercentage;

        public BloodHunter() : base(20, 1)
        {
            _vampirismPercentage = 200;
        }

        public override Fighter Clone()
        {
            BloodHunter clone = new BloodHunter
            {
                ClassName = CloneName
            };

            return clone;
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
            int PercentageDivisor = 100;

            healthToRestore = Damage * (_vampirismPercentage / PercentageDivisor);
            Health += healthToRestore;
            Utility.WriteLineColored($"[❤️{Health}↑] {ClassName} tastes the blood (+❤️{healthToRestore}). It tasted delicious (for him)", ConsoleColor.Green);
        }
    }

    class Utility
    {
        private static Random _random = new Random();

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

        public static int GenerateRandomNumber(int maxValue = 101, int minValue = 1)
        {

            return _random.Next(minValue, maxValue);
        }

        public static int GetUserNumberInRange(string startMessage = "Select Number: ", int maxInput = 100)
        {
            int userInput = 0;
            bool isValidInput = false;

            Console.Write(startMessage);

            while (isValidInput == false)
            {
                if (int.TryParse(Console.ReadLine(), out userInput))
                {
                    if (userInput > 0 && userInput <= maxInput)
                        isValidInput = true;
                    else
                        Utility.WriteColored($"\nPlease enter a number between 1 and {maxInput}: ", ConsoleColor.Red);
                }
                else
                {
                    Utility.WriteColored("\nInvalid input. Please enter a number instead: ", ConsoleColor.Red);
                }
            }

            return userInput;
        }

        public static void WriteLineColored(string text, ConsoleColor color = ConsoleColor.DarkRed, bool isCustomPosition = false, int xPosition = 0, int yPosition = 0)
        {
            Console.ForegroundColor = color;

            if (isCustomPosition)
                WriteLine(xPosition, yPosition, text);
            else
                Console.WriteLine(text);

            Console.ResetColor();
        }

        public static void WriteColored(string text, ConsoleColor color = ConsoleColor.DarkRed, bool isCustomPosition = false, int xPosition = 0, int yPosition = 0)
        {
            Console.ForegroundColor = color;

            if (isCustomPosition)
                Write(xPosition, yPosition, text);
            else
                Console.Write(text);

            Console.ResetColor();
        }

        public static void WriteLine(int xPosition, int yPosition, string text)
        {
            Console.SetCursorPosition(xPosition, yPosition);
            Console.WriteLine(text);
        }

        public static void Write(int xPosition, int yPosition, string text)
        {
            Console.SetCursorPosition(xPosition, yPosition);
            Console.Write(text);
        }
    }
}
