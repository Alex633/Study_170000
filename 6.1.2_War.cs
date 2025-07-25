using System;
using System.Collections.Generic;

interface IDamageable
{
    bool IsAlive { get; }
    int Quantity { get; }

    void TakeDamageAt(int index, double damage);
}

public class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        new Battlefield().Fight();

        Helper.WaitForKeyPress();
    }
}

class Battlefield
{
    private Squad _squadA;
    private Squad _squadB;

    public Battlefield()
    {
        _squadA = BuildSquad("A");
        _squadB = BuildSquad("B");
    }

    public bool IsInFight => _squadA.IsAlive && _squadB.IsAlive;

    public void Fight()
    {
        int round = 1;

        while (IsInFight)
        {
            Helper.WriteTitle($"Round {round}");

            ShowSquadsData();

            _squadA.Attack(_squadB);

            if (IsInFight == false)
            {
                Console.Clear();
                break;
            }

            _squadB.Attack(_squadA);

            Console.Clear();
            round++;
        }

        Console.WriteLine();

        ShowResult();
    }

    private void ShowSquadsData()
    {
        _squadA.ShowInfo();
        Console.WriteLine();
        _squadB.ShowInfo();

        Console.WriteLine();
        Helper.WaitForKeyPress();
    }

    private void ShowResult()
    {
        Helper.WriteTitle($"Winner:");

        if (_squadA.IsAlive)
            _squadA.ShowInfo();
        else if (_squadB.IsAlive)
            _squadB.ShowInfo();
        else
            Helper.WriteAt($"There are only dead left");
    }

    private Squad BuildSquad(string name)
    {
        Squad squad = new Squad(name);

        Unit defaultSoldier = new Unit();
        squad.AddUnit(BuildClones(defaultSoldier, 1, 3));

        SoloSoldier solo = new SoloSoldier();
        squad.AddUnit(BuildClones(solo, 1, 3));

        RandomAreaOfEffectSoldier randomAoe = new RandomAreaOfEffectSoldier();
        squad.AddUnit(BuildClones(randomAoe, 1, 3));

        AreaOfEffectSoldier aoe = new AreaOfEffectSoldier();
        squad.AddUnit(BuildClones(aoe, 1, 3));

        return squad;
    }

    private List<Unit> BuildClones(Unit soldierType, int minClones, int maxClones)
    {
        List<Unit> soldiers = new List<Unit>();

        int clonesCount = Helper.GetRandomInt(minClones, maxClones);

        for (int i = 0; i < clonesCount; i++)
            soldiers.Add(soldierType.Clone());

        return soldiers;
    }
}

class Squad : IDamageable
{
    private List<Unit> _units;
    private string _name;

    public Squad(string name)
    {
        _units = new List<Unit>();
        _name = name;
    }

    public int Quantity => _units.Count;
    public bool IsAlive => Quantity > 0;
    private string Name => _name + $" ({Quantity})";

    public void AddUnit(List<Unit> units)
    {
        _units.AddRange(units);
    }

    public void Attack(IDamageable target)
    {
        if (IsAlive == false)
            return;

        Helper.WriteTitle($"Squad {Name} attacks", isSecondary: true);

        foreach (var unit in _units)
        {
            unit.Attack(target);

            if (target.IsAlive == false)
                break;
        }

        Helper.WaitForKeyPress(true);
        Console.WriteLine(new string('-', Console.WindowWidth));
        Console.WriteLine();
    }

    public void TakeDamageAt(int index, double damage)
    {
        if (index < 0 || index >= _units.Count)
            throw new ArgumentOutOfRangeException("index");

        Unit target = _units[index];

        target.TakeDamage(damage);

        if (target.IsAlive == false)
            _units.Remove(target);
    }

    public void ShowInfo()
    {
        Helper.WriteTitle($"Squad {Name}", isSecondary: true);

        foreach (var unit in _units)
            unit.ShowInfo();
    }
}

class SoloSoldier : Unit
{
    private double _damageModificator;

    public SoloSoldier() : base(10, 0.8, 0.2, "Solo Soldier")
    {
        _damageModificator = 2.5;

        Damage *= _damageModificator;
    }

    public override Unit Clone()
        => new SoloSoldier();
}

class AreaOfEffectSoldier : Unit
{
    public AreaOfEffectSoldier() : base(8, 1, 0, "Area of Effect Soldier")
    { }

    public override Unit Clone()
        => new AreaOfEffectSoldier();

    public override void Attack(IDamageable squad)
    {
        int attackAmount = squad.Quantity >= 5 ? 5 : squad.Quantity;
        Helper.WriteAt($"{Name} prepares to attack {attackAmount} enemies (enemy squad size: {squad.Quantity})\n", foregroundColor: ConsoleColor.Blue);

        for (int i = 0; i < attackAmount; i++)
        {
            int attackCount = i + 1;
            Helper.WriteAt($"⚔️ {Name} attacks for {Damage} damage his {attackCount} time\n");
            squad.TakeDamageAt(i, Damage);
        }
    }
}

class RandomAreaOfEffectSoldier : Unit
{
    public RandomAreaOfEffectSoldier() : base(6, 0.6, 0, "Random Area of Effect Soldier")
    { }

    public override Unit Clone()
        => new RandomAreaOfEffectSoldier();

    public override void Attack(IDamageable squad)
    {
        int minAttacks = 2;
        int maxAttacks = 5;
        int attackCount = Helper.GetRandomInt(minAttacks, maxAttacks);
        Helper.WriteAt($"{Name} prepares to attack {attackCount} times\n", foregroundColor: ConsoleColor.Blue);

        for (int i = 0; i < attackCount; i++)
        {
            base.Attack(squad);

            if (squad.IsAlive == false)
            {
                if (attackCount == i + 1)
                    Helper.WriteAt($"{Name} tries to attack, but everybody is already dead. Calm down {Name}", foregroundColor: ConsoleColor.Yellow);

                break;
            }
        }
    }
}

class Unit
{
    private double _health;
    private double _damage;

    public Unit(double health = 5, double damage = 0.4, double armor = 0.1, string name = "Regular Soldier")
    {
        Health = health;
        Damage = damage;
        Armor = armor;
        Name = $"{name} ({Helper.GetId()})";
    }

    protected string Name { get; private set; }
    public double Armor { get; private set; }

    public bool IsAlive => Health > 0;

    public double Health
    {
        get => _health;
        private set => _health = Math.Max(value, 0);
    }

    public string HealthBar => Helper.ConvertHealthToHealthBar(Health, 5);

    public double Damage
    {
        get => _damage;
        protected set => _damage = Math.Max(value, 0);
    }

    public virtual Unit Clone()
        => new Unit();

    public virtual void Attack(IDamageable squadTarget)
    {
        if (IsAlive == false || squadTarget.IsAlive == false)
            return;

        int randomUnitIndex = Helper.GetRandomInt(0, squadTarget.Quantity);

        Console.WriteLine($"⚔️ {Name} attacks for {Damage}");

        squadTarget.TakeDamageAt(randomUnitIndex, Damage);
    }

    public void TakeDamage(double damage)
    {
        if (IsAlive == false)
            return;

        double actualDamage = Math.Max(damage - Armor, 0);

        Health -= actualDamage;

        if (IsAlive)
            Helper.WriteAt($"💢 {HealthBar} {Name} takes {actualDamage} damage\n");
        else
            Helper.WriteAt($"💀 {Name} takes {actualDamage} and falls dead\n", foregroundColor: ConsoleColor.DarkRed);

    }

    public void ShowInfo()
    {
        if (IsAlive == false)
            return;

        Helper.WriteAt($"{HealthBar} {Name}");
    }
}

class Helper
{
    private static readonly Random s_random = new Random();

    private static int s_id = 0;

    public static string ConvertHealthToHealthBar(double health, int maxLength)
    {
        string healthValue = health.ToString("F1").PadLeft(maxLength);
        return $"|❤️{healthValue}|";
    }

    public static int GetId()
    {
        return s_id++;
    }

    public static int GetRandomInt(int min, int max)
    {
        return s_random.Next(min, max);
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
        WriteAt("Press any key", foregroundColor: ConsoleColor.Cyan);
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
        {
            int targetY = yPosition ?? yStart;
            int targetX = xPosition ?? xStart;

            targetY = Math.Max(0, targetY);
            targetX = Math.Max(0, targetX);

            int safeYPosition = Math.Min(targetY, Console.WindowHeight - 1);
            int safeXPosition = Math.Min(targetX, Console.WindowWidth - 1);

            Console.SetCursorPosition(safeXPosition, safeYPosition);
        }

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
