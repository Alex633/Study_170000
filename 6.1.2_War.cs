using System;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

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


        Helper.WaitForKeyPress();
    }
}

class Battlefield
{
    private Squid SquidA;
    private Squid SquidB;

    public Battlefield()
    {
        SquidA = new Squid();
        SquidB = new Squid();
    }
}

class Squid : IDamageble
{
    private List<Unit> _units;

    public Squid()
    {
        _units = new List<Unit>();
    }

    public void Attack(IDamageble target)
    {
        //target.TakeDamage(Damage);
    }

    public void TakeDamage(double damage)
    {

    }
}

class Soldier : Unit
{
    public Soldier() : base(5, 0.4, 0.1)
    { }

    public override void Attack(IDamageble target)
    {
        target.TakeDamage(Damage);
    }
}

class SoloSoldier : Unit
{
    private double _damageModificator;

    public SoloSoldier() : base(10, 0.8, 0.2)
    {
        _damageModificator = 2.5;
    }

    public override void Attack(IDamageble target)
    {
        target.TakeDamage(Damage * _damageModificator);
    }
}

class AreaOfEffectSoldier : Unit
{

}

class MultipleAttacksSoldier : Unit
{

}

abstract class Unit : IDamageble
{
    private double _health;
    private double _damage;

    public Unit(double health, double damage, double armor)
    {
        Health = health;
        Damage = damage;
        Armor = armor;
    }

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

    public abstract void Attack(IDamageble target);

    public void TakeDamage(double damage)
    {
        Health -= damage;
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
