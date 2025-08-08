//Есть аквариум, в котором плавают рыбы. В этом аквариуме может быть максимум определенное кол-во рыб. 
//Рыб можно добавить в аквариум или рыб можно достать из аквариума. (программу делать в цикле для того, чтобы рыбы могли “жить”) 
//Все рыбы отображаются списком, у рыб также есть возраст. За 1 итерацию рыбы стареют на определенное кол-во жизней и могут умереть. 
//Рыб также вывести в консоль, чтобы можно было мониторить показатели.

using System;
using System.Collections.Generic;

public class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        new FishTank(maxFishes: 10).Run();

        Helper.WaitForKeyPress();
    }
}

class FishTank
{
    private List<Fish> _fish;

    private int _maxFishes;

    private CommandLineInterface _userInterface;

    public FishTank(int maxFishes)
    {
        _maxFishes = maxFishes;

        _fish = new List<Fish>();
        InitializeInterface();
    }

    private bool IsEmpty => _fish.Count == 0;

    public void Run()
    {
        while (_userInterface.IsExitSelected == false)
        {
            Draw();
            _userInterface.Run();
        }
    }

    private void Draw()
    {
        Helper.WriteTitle($"It's beautiful fish tank ({_fish.Count} / {_maxFishes})");

        foreach (var fish in _fish)
        {
            fish.Draw();
            Console.WriteLine();
        }
    }

    private void AddFish()
    {
        if (_fish.Count >= _maxFishes)
        {
            Helper.WriteAt($"You tried to add more fish. Unfortunately, you have too many fish already. " +
                $"You are now thinking about buying a new fish tank", foregroundColor: ConsoleColor.DarkBlue);
            return;
        }

        Fish newFish = new Fish();

        _fish.Add(newFish);
        Console.WriteLine($"Fish has been added to the fish tank! It's {newFish.Age} days old fish. You hear a positively reinforcing sound effect");
    }

    private void CatchRandomFish()
    {
        if (IsEmpty)
        {
            Console.WriteLine("You caught some water. Good job!");
            return;
        }

        Fish caughtFish = _fish[Helper.GetRandomInt(0, _fish.Count)];

        _fish.Remove(caughtFish);

        Console.WriteLine("You caught a fish. Sadly, you don't know which one. They all look the same to you.\n" +
            $"All you know, it's around {caughtFish.Age} days old. You threw it away");
    }

    private void InitializeInterface()
    {
        Dictionary<int, Command> commands = new Dictionary<int, Command>()
        {
            [1] = new Command("Add fish", AddFish),
            [2] = new Command("Catch random fish", CatchRandomFish),
            [3] = new Command("Enjoy this beautiful fish tank", delegate { })
        };

        _userInterface = new CommandLineInterface(commands, $"Control", shouldRunOnce: true);
    }
}

class Fish
{
    private bool _isAlive;

    public Fish()
    {
        Age = Helper.GetRandomInt(0, 15);
        _isAlive = true;
    }

    public int Age { get; private set; }

    public void Draw()
    {
        int daysPassed = 1;

        Age += daysPassed;

        if (_isAlive == false)
        {
            Helper.WriteAt("I am just a not alive fish. Please someone take me out of this fish tank", foregroundColor: ConsoleColor.DarkGray);
            return;
        }

        if (TrySurvive() == false)
            return;

        Console.WriteLine($"I am fish. I can swim. In fact, I am swimming right now. And I swam for {Age} days");
    }

    private bool TrySurvive()
    {
        int heartAttackRiskAge = 15;

        if (Helper.GetRandomInt(0, Age + 1) >= heartAttackRiskAge)
        {
            Helper.WriteAt($"Oh, no. I had a heart attack. It was good {Age} days", foregroundColor: ConsoleColor.DarkRed);
            _isAlive = false;
            return false;
        }

        int maxRandomValue = 20;
        bool isAccidentalDeath = Helper.GetRandomInt(0, maxRandomValue + 1) == 0;

        if (isAccidentalDeath)
        {
            Helper.WriteAt($"Oh, no. I died from accidental death. How tragic. And I was so young ({Age}) too. Oh, well", foregroundColor: ConsoleColor.Red);
            _isAlive = false;
            return false;
        }

        return true;
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
