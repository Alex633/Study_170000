using System;
using System.Collections.Generic;

public class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        RepairService repairServis = new RepairService();

        repairServis.Open();

        Helper.WaitForKeyPress();
    }
}

class RepairService
{
    private int _balance;

    private List<Car> _cars;

    public RepairService()
    {
        _cars = CarFactory.CreateFew(5);

        _balance = 0;
    }

    public void Open()
    {
        while (true)
        {
            ShowCars();

        }
    }

    private void ShowCars()
    {
        foreach (Car car in _cars)
        {
            car.ShowInfo();
        }

        Helper.WaitForKeyPress(shouldClear: true);
    }
}

static class CarFactory
{
    public static List<Car> CreateFew(int amount = 1)
    {
        List<Car> newCars = new List<Car>();

        for (int i = 0; i < amount; i++)
        {
            newCars.Add(new Car());
        }

        return newCars;
    }

    public static Car Create()
    {
        return new Car();
    }
}

class Car
{
    private List<Part> _parts;

    public Car()
    {
        Build();
    }

    public void ShowInfo()
    {
        Helper.WriteTitle("Car");

        bool haveBrokenPart = false;

        for (int i = 0; i < _parts.Count; i++)
        {
            if (_parts[i].IsBroken)
            {
                Helper.WriteAt($"Broken part found ({i})", foregroundColor: ConsoleColor.DarkRed);
                haveBrokenPart = true;
            }
        }

        if (haveBrokenPart == false)
        {
            Helper.WriteAt("In good condition");
        }

        Console.WriteLine();
    }

    private void Build(int partsAmount = 5)
    {
        _parts = new List<Part>();

        int brokenPartValue = 0;
        int goodPartValue = 1;
        bool haveBrokenPart = false;

        for (int i = 0; i < partsAmount; i++)
        {
            bool isBrokenPart = Convert.ToBoolean(Helper.GetRandomInt(brokenPartValue, goodPartValue + 1));

            if (isBrokenPart && haveBrokenPart == false)
                haveBrokenPart = true;

            if (haveBrokenPart == false && partsAmount == i + 1)
                isBrokenPart = true;

            _parts.Add(new Part(isBrokenPart));
        }
    }
}

class Part
{
    public bool IsBroken { get; private set; }

    public Part(bool isBroken)
    {
        IsBroken = isBroken;
    }
}

class CommandLineInterface
{
    private readonly Dictionary<int, Command> _items;

    private bool _shouldRun;
    private readonly bool _shouldRunOnce;

    private readonly string _title;

    public CommandLineInterface(Dictionary<int, Command> items, string title, bool shouldRunOnce = false)
    {
        _shouldRun = true;
        _shouldRunOnce = shouldRunOnce;
        _title = title;

        _items = items;
        _items.Add(_items.Count + 1, new Command("Exit", Exit));
    }

    public int LastSelectedOption { get; private set; }
    public bool IsExitSelected => LastSelectedOption == _items.Count;

    public void Run()
    {
        _shouldRun = true;

        while (_shouldRun)
        {
            OutputCommands();

            LastSelectedOption = Helper.ReadInt("Input command: ");
            HandleInput();

            if (_shouldRunOnce)
                _shouldRun = false;
        }
    }

    private void HandleInput()
    {
        if (_items.TryGetValue(LastSelectedOption, out Command item))
            item.Execute();
        else
            Helper.WriteAt("Invalid command. Please try again.", foregroundColor: ConsoleColor.Red);

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

        Helper.WriteAt($"Closing the {_title}");
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
    private static readonly Random s_random = new Random();

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

    public static int ReadInt(string text)
    {
        int result;

        while (int.TryParse(ReadString(text), out result) == false) ;

        return result;
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
