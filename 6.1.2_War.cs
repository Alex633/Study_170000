using System;
using System.Collections.Generic;

//why everybody's dead

interface IDamageble
{
    void TakeDamage(double damage);
}

//interface IAttack
//{
//    void Attack(int target);
//}

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
    private Squad SquadA;
    private Squad SquadB;

    public Battlefield()
    {
        SquadA = BuildSquad("A");
        SquadB = BuildSquad("B");
    }

    public bool IsInFight => SquadA.IsAlive && SquadB.IsAlive;

    public void Fight()
    {
        Helper.WriteTitle($"Fighting");

        Console.WriteLine($"{SquadA.IsAlive}, {SquadB.IsAlive}");

        while (IsInFight)
        {
            SquadA.Attack(SquadB);
            SquadB.Attack(SquadA);
        }

        Console.WriteLine();

        ShowResult();
    }

    private void ShowResult()
    {
        Helper.WriteTitle($"Winner:");

        if (SquadA.IsAlive)
            SquadA.ShowInfo();
        else if (SquadB.IsAlive)
            SquadB.ShowInfo();
        else
            Helper.WriteAt($"There are only dead left");
    }

    private Squad BuildSquad(string name)
    {
        Squad squad = new Squad(name);

        Soldier defaultSoldier = new Soldier();
        List<Unit> defaultSoldiers = new List<Unit>();

        int minClones = 5;
        int maxClones = 10;
        int clonesCount = Helper.GetRandomInt(minClones, maxClones);

        for (int i = 0; i < clonesCount; i++)
            defaultSoldiers.Add(defaultSoldier.Clone());

        squad.AddUnit(defaultSoldiers);

        SoloSoldier soloSoldier = new SoloSoldier();
        List<Unit> soloSoldiers = new List<Unit>();

        minClones = 1;
        maxClones = 3;
        clonesCount = Helper.GetRandomInt(minClones, maxClones);

        for (int i = 0; i < clonesCount; i++)
            soloSoldiers.Add(soloSoldier.Clone());

        squad.AddUnit(soloSoldiers);

        return squad;
    }
}

class Squad : IDamageble
{
    private List<Unit> _units;

    private string _name;

    public Squad(string name)
    {
        _units = new List<Unit>();
        _name = name;
    }

    public bool IsAlive => CollectAliveData();

    public void AddUnit(Unit unit)
    {
        _units.Add(unit);
    }

    public void AddUnit(List<Unit> units)
    {
        _units.AddRange(units);
    }

    public void Attack(IDamageble target)
    {
        if (IsAlive == false)
            return;

        Helper.WriteTitle($"Squad {_name} attacks", isSecondary: true);

        //target.TakeDamage(Damage);
        Helper.WaitForKeyPress(false);
    }

    public void TakeDamage(double damage)
    {
        if (IsAlive == false)
            return;
    }

    public void ShowInfo()
    {
        Helper.WriteTitle($"Squad {_name}", isSecondary: true);

        foreach (var unit in _units)
            unit.ShowInfo();
    }

    private bool CollectAliveData()
    {
        foreach (Unit unit in _units)
        {
            if (unit.IsAlive)
                return true;
        }

        return false;
    }
}

class Soldier : Unit
{
    public Soldier() : base(5, 0.4, 0.1, "Soldier")
    { }

    public override Unit Clone()
        => new Soldier();

    public override void Attack(IDamageble target)
    {
        target.TakeDamage(Damage);
    }
}

class SoloSoldier : Unit
{
    private double _damageModificator;

    public SoloSoldier() : base(10, 0.8, 0.2, "Solo Soldier")
    {
        _damageModificator = 2.5;
    }

    public override Unit Clone()
        => new SoloSoldier();

    public override void Attack(IDamageble target)
    {
        target.TakeDamage(Damage * _damageModificator);
    }
}

//class AreaOfEffectSoldier : Unit
//{

//}

//class MultipleAttacksSoldier : Unit
//{

//}

abstract class Unit : IDamageble
{
    private double _health;
    private double _damage;
    private string _name;

    public Unit(double health, double damage, double armor, string name)
    {
        Health = health;
        Damage = damage;
        Armor = armor;
        _name = name;
    }

    public bool IsAlive => Health > 0;

    public double Armor { get; private set; }

    public double Health
    {
        get => _health;
        private set => _ = Math.Min(_health, 0);
    }

    public double Damage
    {
        get => _damage;
        private set => _ = Math.Min(_damage, 0);
    }
    public abstract Unit Clone();

    public abstract void Attack(IDamageble target);

    public void TakeDamage(double damage)
    {
        if (IsAlive == false)
            return;

        double actualDamage = Math.Min(damage - Armor, 0);

        Health -= actualDamage;
    }

    public void ShowInfo()
    {
        if (IsAlive == false)
            return;

        Helper.WriteAt($"({Health} HP) {_name}");
    }
}

class Helper
{
    private static readonly Random s_random = new Random();

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
        Console.WriteLine();
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
