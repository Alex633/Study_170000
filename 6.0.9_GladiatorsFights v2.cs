using System;
using System.Collections.Generic;

interface IDamageable
{
    void TakeDamage(int damage);
}


public class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Arena arena = new Arena();
        arena.Run();

        Helper.WaitForKeyPress();
    }
}

class Arena
{
    private Fighter _firstFighter;
    private Fighter _secondFighter;

    private List<Fighter> _fighterClasses;

    private CommandLineInterface _commandInterface;

    public Arena()
    {
        InitializeFighters();
        InitializeCommands();
    }

    private bool IsFightOn => _firstFighter.IsAlive && _secondFighter.IsAlive;

    public void Run()
    {
        SayHi();
        Console.WriteLine();
        _commandInterface.Run();
    }

    private void Open()
    {
        SelectFighters();
        Fight();
        ShowFightResult();
    }

    private void SelectFighters()
    {
        Helper.WriteTitle($"Выбор воинов");

        OutputFighters();

        Console.WriteLine();
        int firstFighter = Helper.ReadIntInRange("Выберите первого воина: ", _fighterClasses.Count) - 1;
        int secondFighter = Helper.ReadIntInRange("Выберите второго воина: ", _fighterClasses.Count) - 1;

        DetermineInitiative(firstFighter, secondFighter);
        Helper.WaitForKeyPress();
    }

    private void SayHi()
    {
        Console.WriteLine("Привет!");
    }

    private void DetermineInitiative(int firstFighter, int secondFighter)
    {
        Helper.WriteTitle("Определение инициативы", true);

        int firstFighterInitiative = 0;
        int secondFighterInitiative = 0;

        while (firstFighterInitiative == secondFighterInitiative)
        {
            firstFighterInitiative = Helper.GetDiceValue();
            secondFighterInitiative = Helper.GetDiceValue();

            Console.WriteLine($"Первый воин выкинул: {firstFighterInitiative}");
            Console.WriteLine($"Второй воин выкинул: {secondFighterInitiative}");

            if (firstFighterInitiative == secondFighterInitiative)
                Console.WriteLine("Одинаковые значения. Перекидываем кубики");
        }

        if (firstFighterInitiative > secondFighterInitiative)
        {
            _firstFighter = _fighterClasses[firstFighter].Clone();
            _secondFighter = _fighterClasses[secondFighter].Clone();
        }
        else
        {
            _firstFighter = _fighterClasses[secondFighter].Clone();
            _secondFighter = _fighterClasses[firstFighter].Clone();
        }

        Console.WriteLine($"Первый ходит: {_firstFighter.Name}");
    }

    private void Fight()
    {
        int round = 1;

        while (IsFightOn)
        {
            Helper.WriteTitle($"Раунд {round}");

            CompleteTurn(_firstFighter, _secondFighter);

            if (IsFightOn)
            {
                Console.WriteLine();
                CompleteTurn(_secondFighter, _firstFighter);

                round++;
            }

            Console.Clear();
        }
    }

    private void CompleteTurn(Fighter fighter, Fighter target)
    {
        Helper.WriteTitle($"Ход {fighter.Name}", true);
        fighter.Attack(target);
        Helper.WaitForKeyPress(false);
    }

    private void ShowFightResult()
    {
        Helper.WriteTitle($"Победитель");

        if (_firstFighter.IsAlive == false && _secondFighter.IsAlive == false)
            Helper.WriteAt("Дружба", foregroundColor: ConsoleColor.DarkRed);
        else if (_firstFighter.IsAlive)
            Helper.WriteAt(_firstFighter.Name);
        else if (_secondFighter.IsAlive)
            Helper.WriteAt(_secondFighter.Name);
    }

    private void OutputFighters()
    {
        Helper.WriteTitle("Классы", true);

        for (int i = 0; i < _fighterClasses.Count; i++)
        {
            int number = i + 1;
            Console.Write(number + ") ");

            _fighterClasses[i].ShowInfo();
        }
    }

    private void InitializeFighters()
    {
        _fighterClasses = new List<Fighter>()
        {
            new Assassin(),
            new Thief(),
            new Dualist(),
            new Berserk(),
            new Paladin(),
            new Wizard(),
        };
    }

    private void InitializeCommands()
    {
        Dictionary<int, Command> commands = new Dictionary<int, Command>()
        {
            [1] = new Command("Сказать привет еще раз", SayHi),
            [2] = new Command("Начать бой", Open)
        };

        _commandInterface = new CommandLineInterface(commands, "Арена");

    }
}

class Assassin : Fighter
{
    private int _criticalChancePercentage;
    private double _criticalMultiplier;

    public Assassin() : base("Убийца", 22, 2, 1)
    {
        _criticalMultiplier = 6;
        _criticalChancePercentage = 45;
    }

    public override void Attack(IDamageable subject)
    {
        bool isCriticalStrike = Helper.PerformDiceCheck($"{Name} пытается пройти на критическую атаку", _criticalChancePercentage);
        int damage = isCriticalStrike == false ? Damage : (int)(Damage * _criticalMultiplier);
        string strikeAdditionalInfo = isCriticalStrike ? "критически " : "";

        Helper.WriteAt($"({Health}) {Name} {strikeAdditionalInfo}атакует на {damage} урона");

        subject.TakeDamage(damage);
    }

    public override void ShowInfo()
    {
        base.ShowInfo();
        Console.WriteLine($", крит. шанс: {_criticalChancePercentage}%, крит. урон множитель: {_criticalMultiplier}");
    }

    public override Fighter Clone()
        => new Assassin();
}

class Thief : Fighter
{
    private int _dodgeChancePercentage;

    public Thief() : base("Вор", 7, 4, 1)
    {
        _dodgeChancePercentage = 75;
    }

    public override void TakeDamage(int damage)
    {
        bool isDodge = Helper.PerformDiceCheck($"{Name} пытается пройти проверку на уворот", _dodgeChancePercentage);

        if (isDodge == false)
            base.TakeDamage(damage);
        else
            Helper.WriteAt($"({Health}) {Name} уворачивается от атаки");
    }

    public override void ShowInfo()
    {
        base.ShowInfo();
        Console.WriteLine($", уворот: {_dodgeChancePercentage}%");
    }

    public override Fighter Clone()
        => new Thief();
}

class Dualist : Fighter
{
    private int _extraAttackFrequency;
    private int _extraAttacks;
    private int _attackCount;

    public Dualist() : base("Дуалист", damage: 3)
    {
        _extraAttackFrequency = 3;
        _extraAttacks = 3;
    }

    public override void Attack(IDamageable subject)
    {
        base.Attack(subject);
        _attackCount++;

        if (_attackCount >= _extraAttackFrequency)
        {
            Helper.WriteAt($"{Name} атакует еще {_extraAttacks} раза, так как это {_attackCount} атака");

            for (int i = 0; i < _extraAttacks; i++)
                base.Attack(subject);

            _attackCount = 0;
        }
    }

    public override void ShowInfo()
    {
        base.ShowInfo();
        Console.WriteLine($", атакует {_extraAttacks} дополнительных раза каждую {_extraAttackFrequency} атаку");
    }

    public override Fighter Clone()
        => new Dualist();
}

class Berserk : Fighter
{
    private int _rageCurrent;
    private int _rageToDoubleDamage;
    private int _ragePerHit;
    private int _rageDamageModifier;

    public Berserk() : base("Берсерк", 100, 1, -8)
    {
        _rageDamageModifier = 5;
        _rageCurrent = 0;
        _rageToDoubleDamage = 8;
        _ragePerHit = 2;
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        ApplyRage();
    }

    public override void ShowInfo()
    {
        base.ShowInfo();
        Console.WriteLine($", накапливает {_ragePerHit} ярости при получении урона. Ярость увеличивает атаку в {_rageDamageModifier} раз каждые {_rageToDoubleDamage} очков ярости");
    }

    public override Fighter Clone()
    => new Berserk();

    private void ApplyRage()
    {
        if (IsAlive == false)
            return;

        _rageCurrent += _ragePerHit;
        Helper.WriteAt($"{Name} накапливает ярость до {_rageCurrent}");

        if (_rageCurrent % _rageToDoubleDamage == 0)
        {
            Damage *= _rageDamageModifier;
            Helper.WriteAt($"{Name} в ярости. Атака увеличена до {Damage}", foregroundColor: ConsoleColor.Yellow);
        }
    }
}

class Paladin : Fighter
{
    private int _blessingCurrent;
    private int _maxBlessing;
    private int _blessingPerHit;
    private int _blessingHealthRestoration = 8;

    public Paladin() : base("Паладин", damage: 2, armor: 2)
    {
        _blessingCurrent = 0;
        _maxBlessing = 15;
        _blessingPerHit = 5;
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        ApplyBlessing();
    }

    public override void ShowInfo()
    {
        base.ShowInfo();
        Console.WriteLine($", накапливает {_blessingPerHit} благославления при получении урона. " +
            $"При достижении {_maxBlessing} восстанавливает {_blessingHealthRestoration} ХП");
    }

    public override Fighter Clone()
    => new Paladin();

    private void ApplyBlessing()
    {
        if (IsAlive == false)
            return;

        _blessingCurrent += _blessingPerHit;
        Helper.WriteAt($"{Name} накапливает благославление до {_blessingCurrent}");

        if (_blessingCurrent >= _maxBlessing)
        {
            _blessingCurrent = 0;

            Health += _blessingHealthRestoration;
            Helper.WriteAt($"{Name} восстанавливает {_blessingHealthRestoration} ХП", foregroundColor: ConsoleColor.Yellow);
        }
    }
}

class Wizard : Fighter
{
    private int _manaCurrent;
    private int _maxMana;
    private int _manaRestoration;

    private int _fireBallDamage;
    private int _fireBallCost;

    public Wizard() : base("Волшебник", health: 17, damage: 1, armor: 0)
    {
        _maxMana = 7;
        _manaCurrent = _maxMana;
        _manaRestoration = 1;

        _fireBallDamage = 14;
        _fireBallCost = 4;
    }

    public override void Attack(IDamageable subject)
    {
        RestoreMana();

        if (_manaCurrent >= _fireBallCost)
            UseFireball(subject);
        else
            base.Attack(subject);
    }

    public override void ShowInfo()
    {
        base.ShowInfo();
        Console.WriteLine($", мана: {_maxMana}. Использует огненный шар (наносит {_fireBallDamage} урона, требует {_fireBallCost} маны). Восстанавливает {_manaRestoration} маны в ход");
    }

    public override Fighter Clone()
    => new Wizard();

    private void UseFireball(IDamageable subject)
    {
        _manaCurrent -= _fireBallCost;

        Helper.WriteAt($"({Health}) {Name} кидает огненный шар и наносит {_fireBallDamage} урона");

        subject.TakeDamage(_fireBallDamage);
    }

    private void RestoreMana()
    {
        if (IsAlive == false)
            return;

        _manaCurrent += _manaRestoration;

        if (_manaCurrent > _maxMana)
            _manaCurrent = _maxMana;
    }
}

abstract class Fighter : IDamageable
{
    protected int _maxHealth;
    private int _currentHealth;

    public Fighter(string name, int health = 30, int damage = 4, int armor = 1)
    {
        Name = name;

        _maxHealth = health;
        _currentHealth = health;

        Damage = damage;
        Armor = armor;
    }

    public int Damage { get; protected set; }
    public string Name { get; private set; }
    public int Armor { get; private set; }

    public int Health
    {
        get => _currentHealth;
        protected set
        {
            _currentHealth = value > _maxHealth ? _maxHealth : value;

            if (value < 0)
                _currentHealth = 0;
        }
    }

    public bool IsAlive => Health > 0;

    public abstract Fighter Clone();

    public virtual void Attack(IDamageable subject)
    {
        Helper.WriteAt($"({Health}) {Name} атакует на {Damage} урона");

        subject.TakeDamage(Damage);
    }

    public virtual void TakeDamage(int damage)
    {
        if (IsAlive == false)
        {
            Helper.WriteAt($"(X_X) {Name} уже мертв. Оставьте его в покое");
            return;
        }

        int actualDamage = Math.Max(0, damage - Armor);
        string damageRedaction = Armor > 0 ? $"(-{Armor}) " : "";

        Health -= actualDamage;

        if (actualDamage > 0)
            Helper.WriteAt($"({Health}) {Name} получает {actualDamage} {damageRedaction}урона");
        else
            Helper.WriteAt($"({Health}) {Name} поглащает весь урон");

        FallDramaticallyIfDead();
    }

    public virtual void ShowInfo()
    {
        Console.Write($"{Name}: {Health} ХП, {Damage} урона, {Armor} брони");
    }

    private void FallDramaticallyIfDead()
    {
        if (IsAlive == false)
            Helper.WriteAt($"({Health}) {Name} драматически падает на землю (X_X)");
    }
}

class CommandLineInterface
{
    private Dictionary<int, Command> _items;

    private bool _shouldRun;
    private bool _shouldRunOnce;

    private string _title;

    public CommandLineInterface(Dictionary<int, Command> items, string title, bool shouldRunOnce = false)
    {
        _shouldRun = true;
        _shouldRunOnce = shouldRunOnce;
        _title = title;

        _items = items;
        _items.Add(_items.Count + 1, new Command("Выход", Exit));
    }

    public int LastSelectedOption { get; private set; }
    public bool IsExitSelected => LastSelectedOption == _items.Count;

    public void Run()
    {
        _shouldRun = true;

        while (_shouldRun)
        {
            OutputCommands();

            LastSelectedOption = Helper.ReadInt("Введите команду: ");
            HandleInput();

            if (_shouldRunOnce)
                _shouldRun = false;
        }
    }

    private void HandleInput()
    {
        Console.Clear();

        if (_items.TryGetValue(LastSelectedOption, out Command item))
            item.Execute();
        else
            Helper.WriteAt("Неверная команда. Пожалуйста, попробуйте снова.", foregroundColor: ConsoleColor.Red);

        if (_shouldRun)
            Helper.WaitForKeyPress();
    }

    private void OutputCommands()
    {
        Helper.WriteTitle(_title);

        foreach (var item in _items)
            Helper.WriteAt($"{item.Key}) {item.Value.Description}");

        Console.WriteLine();
    }

    private void Exit()
    {
        _shouldRun = false;

        Helper.WriteAt($"Закрытие {_title}");
    }
}

class Command
{
    private readonly Action _action;

    public Command(string description, Action action)
    {
        Description = description;
        _action = action;
    }

    public string Description { get; private set; }

    public void Execute()
    {
        Helper.WriteTitle(Description);
        _action();
    }
}

class Helper
{
    private static Random _random = new Random();

    public static bool PerformDiceCheck(string prompt, int chancePercentage)
    {
        int roll = GetDiceValue();
        int minimalDiceForSuccess = 101 - chancePercentage;

        bool isSuccess = roll >= minimalDiceForSuccess;
        string status = isSuccess ? "Успех" : "Провал";

        WriteAt($"{prompt} (требуется {minimalDiceForSuccess} или выше): {status} ({roll})",
            foregroundColor: isSuccess ? ConsoleColor.Yellow : ConsoleColor.DarkRed);

        return isSuccess;
    }

    public static int GetDiceValue()
    {
        int maxRoll = 100;

        return _random.Next(1, maxRoll + 1);
    }

    public static string ReadString(string helpText, ConsoleColor primary = ConsoleColor.Cyan, ConsoleColor secondary = ConsoleColor.Black)
    {
        ConsoleColor backgroundColor = Console.BackgroundColor;

        WriteAt(helpText, foregroundColor: primary, isNewLine: true);

        int fieldStartY = Console.CursorTop;
        int fieldStartX = Console.CursorLeft;

        char emptiness = ' ';
        WriteAt(new string(emptiness, helpText.Length - 1), backgroundColor: primary, isNewLine: false, xPosition: fieldStartX);

        Console.SetCursorPosition(fieldStartX + 1, fieldStartY);

        Console.BackgroundColor = primary;
        Console.ForegroundColor = secondary;

        string input = Console.ReadLine();

        Console.BackgroundColor = backgroundColor;

        Console.WriteLine();

        return input;
    }

    public static int ReadIntInRange(string prompt, int max, int min = 1)
    {
        int result = 0;

        bool isNumberInRangeInputed = false;

        while (isNumberInRangeInputed == false)
        {
            result = ReadInt(prompt);

            if (result > max || result < min)
                WriteAt($"Неверный ввод ({result}). Введите число от {min} до {max}");
            else
                isNumberInRangeInputed = true;
        }

        return result;
    }

    public static int ReadInt(string prompt)
    {
        int result = 0;

        bool isNumberInputed = false;

        while (isNumberInputed == false)
        {
            isNumberInputed = int.TryParse(ReadString(prompt), out result);

            if (isNumberInputed == false)
                WriteAt($"Неверный ввод ({result}). Введите число");
        }

        return result;
    }

    public static void WriteTitle(string title, bool isSecondary = false)
    {
        ConsoleColor backgroundColor = isSecondary ? ConsoleColor.DarkGray : ConsoleColor.Gray;

        WriteAt($" {title} ", foregroundColor: ConsoleColor.Black, backgroundColor: backgroundColor);

        if (isSecondary == false)
            Console.WriteLine();
    }

    public static void WaitForKeyPress(bool shouldClear = true)
    {
        Console.WriteLine();
        WriteAt("Нажмите любую клавишу", foregroundColor: ConsoleColor.Cyan);
        Console.ReadKey(true);

        if (shouldClear)
            Console.Clear();
    }

    public static void WriteAt(object element, int? yPosition = null, int? xPosition = null,
    ConsoleColor foregroundColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black,
    bool isNewLine = true)
    {
        Console.ForegroundColor = foregroundColor;
        Console.BackgroundColor = backgroundColor;

        int yStart = Console.CursorTop;
        int xStart = Console.CursorLeft;

        bool isCustomPosition = yPosition.HasValue || xPosition.HasValue;

        if (isCustomPosition)
            Console.SetCursorPosition(xPosition ?? xStart, yPosition ?? yStart);

        if (isNewLine)
            Console.WriteLine(element);
        else
            Console.Write(element);

        Console.ResetColor();

        if (isCustomPosition)
        {
            Console.CursorTop = yStart;
            Console.CursorLeft = xStart;
        }
    }
}
