using System;
using System.Collections.Generic;

public enum VagonCapacity
{
    Small = 10,
    Medium = 25,
    Large = 100
}

public class Program
{
    static void Main()
    {
        TrainSystem trainSystem = new TrainSystem();

        trainSystem.Run();

        Helper.ClearAfterKeyPress();
    }
}

class TrainSystem
{
    private Dispatcher _dispatcher;

    private CommandLineInterface _mainMenu;

    public TrainSystem()
    {
        _dispatcher = new Dispatcher();

        InitializeCommandsMainMenu();
    }

    public void Run() =>
        _mainMenu.Run();

    private void InitializeCommandsMainMenu()
    {
        Dictionary<int, Command> menu = new Dictionary<int, Command>()
        {
            [1] = new Command("Create train", _dispatcher.CreateTrainRoutePair),
        };

        _mainMenu = new CommandLineInterface(menu, "Train system commands");
    }


}

class Dispatcher
{
    private List<Command> _trainConstractionSequence;

    private Dictionary<Train, Route> _trainRoute;

    public Dispatcher()
    {
        _trainRoute = new Dictionary<Train, Route>();
    }

    public void CreateTrainRoutePair()
    {
        Route tempRoute = CreateRoute();

        int boughtTickets = GetSoldlTicketsAmount();
        Train tempTrain = BuildTrain();
    }

    private Route CreateRoute()
    {
        Helper.WriteTitle("Create a route");

        Route tempRoute = new Route(Helper.ReadString("Departure: "), Helper.ReadString("Destination: "));

        return tempRoute;
    }

    private int GetSoldlTicketsAmount()
    {
        Helper.WriteTitle("Tickets sold");

        return new Random().Next(1000);

    }

    private Train BuildTrain()
    {
        Helper.WriteTitle("Construct a train");

        Route tempRoute;

        return tempRoute;
    }
}

class Route
{
    public Route(string departue, string destination)
    {
        Departue = departue;
        Destination = destination;
    }

    public string Departue { get; private set; }
    public string Destination { get; private set; }
}

class Train
{
    private List<Vagon> _vagons;

    public Train()
    {
        _vagons = new List<Vagon>();
    }

    public int Capacity { get; private set; }

    public void AddVagon(Vagon vagon)
    {
        _vagons.Add(vagon); 
        Capacity += vagon.Capacity;
    }
}

class Vagon
{
    public Vagon(VagonCapacity capacity)
    {
        Capacity = (int)capacity;
    }

    public int Capacity { get; private set; }
}

class CommandLineInterface
{
    private Dictionary<int, Command> _items;

    private bool _shouldRun;

    private string _title;

    public CommandLineInterface(Dictionary<int, Command> items, string title)
    {
        _shouldRun = true;
        _title = title;

        _items = items;
        _items.Add(_items.Count + 1, new Command("Exit", Exit));
    }

    public int LastSelectedOption { get; private set; }

    public void Run()
    {
        Console.Clear();

        _shouldRun = true;

        while (_shouldRun)
        {
            OutputCommands();

            LastSelectedOption = Helper.ReadInt("Input command: ");
            HandleInput();
        }
    }

    private void HandleInput()
    {
        Console.Clear();

        if (_items.TryGetValue(LastSelectedOption, out Command item))
            item.Execute();
        else
            Helper.WriteAt("Invalid command. Please try again.", foregroundColor: ConsoleColor.Red);

        if (_shouldRun)
            Helper.ClearAfterKeyPress();
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

    public static void WriteTitle(string title, bool isSecondary = false)
    {
        ConsoleColor backgroundColor = isSecondary ? ConsoleColor.DarkGray : ConsoleColor.Gray;

        WriteAt($" {title} ", foregroundColor: ConsoleColor.Black, backgroundColor: backgroundColor);

        if (isSecondary == false)
            Console.WriteLine();
    }

    public static void ClearAfterKeyPress()
    {
        Console.WriteLine();
        WriteAt($"Press any key", foregroundColor: ConsoleColor.Cyan);
        Console.ReadKey(true);
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
