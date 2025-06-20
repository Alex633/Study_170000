using System;
using System.Collections.Generic;

//Диспетчер содержит все созданные поезда и перед выбором в консольном меню показать короткую информацию
// Add output of all trains before main menu in trasin system
// Add release the train in main menu
// Instead of having locomotion price - change price to one price of releasing trains
// ... so combining release of few trains would be more profitable if player have enough money for that

public enum WagonCapacity
{
    Small = 24,
    Medium = 64,
    Large = 256
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

    public void Run()
    {
        Console.WriteLine(_mainMenu.IsExitSelected);
        Console.WriteLine(_mainMenu.LastSelectedOption);

        while (_dispatcher.IsFired == false && _mainMenu.IsExitSelected == false)
            _mainMenu.Run();
    }

    private void InitializeCommandsMainMenu()
    {
        Dictionary<int, Command> menu = new Dictionary<int, Command>()
        {
            [1] = new Command("Create train", _dispatcher.CompleteOrder),
        };

        _mainMenu = new CommandLineInterface(menu, "Train system commands", shouldRunOnce: true);
    }
}

class Dispatcher
{
    private int _ticketsSold;
    private int _lastIncome;
    private int _balance;

    public Dispatcher()
    {
        IsFired = false;

        _balance = 100;
        _ticketsSold = 0;
    }

    public void CompleteOrder()
    {
        Route tempRoute = CreateRoute();
        int boughtTickets = GenerateSoldTickets();
        Train tempTrain = BuildTrain();

        AttemptDispatchTrain(tempRoute);
    }

    public bool IsFired { get; private set; }

    private Route CreateRoute()
    {
        Helper.WriteTitle("Create a route");

        Route tempRoute = new Route(Helper.ReadString("Departure: "), Helper.ReadString("Destination: "));

        Helper.WriteAt($"Route {tempRoute.Departure} - {tempRoute.Destination} created");
        Helper.ClearAfterKeyPress();

        return tempRoute;
    }

    private int GenerateSoldTickets()
    {
        Helper.WriteTitle("Tickets sold");

        int maxPassengers = 448;
        int ticketPrice = 4;
        _ticketsSold = new Random().Next(maxPassengers);
        _lastIncome = ticketPrice * _ticketsSold;
        _balance += _lastIncome;

        Helper.WriteAt($"Tickets sold: {_ticketsSold}. Income: ${_lastIncome} (total balance: ${_balance})");
        Helper.ClearAfterKeyPress();

        return _ticketsSold;

    }

    private Train BuildTrain()
    {
        Helper.WriteTitle($"Construct a train ");

        Train tempTrain = new Train();

        Dictionary<int, Command> trainConstructorCommands = new Dictionary<int, Command>()
        {
            [1] = new Command($"Add {WagonCapacity.Small} wagon ({(int)WagonCapacity.Small} seats)", () => tempTrain.AddWagon(WagonCapacity.Small)),
            [2] = new Command($"Add {WagonCapacity.Medium} wagon ({(int)WagonCapacity.Medium} seats)", () => tempTrain.AddWagon(WagonCapacity.Medium)),
            [3] = new Command($"Add {WagonCapacity.Large} wagon ({(int)WagonCapacity.Large} seats)", () => tempTrain.AddWagon(WagonCapacity.Large)),
        };

        CommandLineInterface trainConstructor = new CommandLineInterface(trainConstructorCommands,
            $"Train constructor (passengers: {_ticketsSold})");

        trainConstructor.Run();

        CalculatePenalty(tempTrain);

        Helper.WriteAt($"Train constructed (seats: {tempTrain.Capacity}). Total train cost: ${tempTrain.Cost}", foregroundColor: ConsoleColor.Green);
        Helper.ClearAfterKeyPress();

        return tempTrain;
    }

    private void CalculatePenalty(Train train)
    {
        int penalty = 0;
        int angryPassengers = _ticketsSold - train.Capacity;
        int finePerPassenger = 8;

        if (angryPassengers > 0)
        {
            penalty = angryPassengers * finePerPassenger;
            Helper.WriteAt($"There is no seats for {angryPassengers} people. You payed ${penalty} penalty", foregroundColor: ConsoleColor.Red);
        }

        int moneySpent = train.Cost + penalty;

        _lastIncome -= moneySpent;
        _balance -= moneySpent;
    }

    private void AttemptDispatchTrain(Route route)
    {
        if (_balance > 0)
        {
            string incomeStatus = _lastIncome > 0 ? "earned" : "lost";
            ConsoleColor incomeStatusColor = _lastIncome > 0 ? ConsoleColor.Green : ConsoleColor.Red;

            Helper.WriteAt($"Train to {route.Destination} leaving the station {route.Departure}. You {incomeStatus}: ${_lastIncome} (balance: {_balance})",
                foregroundColor: incomeStatusColor);

        }
        else
        {
            IsFired = true;

            Helper.WriteAt($"You’ve been fired and now owe ${_balance} to the train company. Glory to the train company.",
                foregroundColor: ConsoleColor.DarkRed);
        }

        int aLotOfMoney = 1400;

        if (_balance >= aLotOfMoney)
        {
            IsFired = true;

            Helper.WriteAt($"\"I quit!\", - you say to your train company while having ${_balance} in your pockets. " +
                $"You buy youself a house, a car, your own train and travel the world. Good job, man",
                foregroundColor: ConsoleColor.Blue);
        }
    }
}

class Route
{
    public Route(string departue, string destination)
    {
        Departure = departue;
        Destination = destination;
    }

    public string Departure { get; private set; }
    public string Destination { get; private set; }
}

class Train
{
    private List<Wagon> _wagons;

    public Train()
    {
        _wagons = new List<Wagon>();

        int locomotionPrice = 30;

        Cost = locomotionPrice;
    }

    public int Capacity { get; private set; }
    public int Cost { get; private set; }
    public int WagonsCount => _wagons.Count;

    public void AddWagon(WagonCapacity wagonCapacity)
    {
        Wagon tempWagon = new Wagon(wagonCapacity);

        _wagons.Add(tempWagon);
        Capacity += tempWagon.Capacity;

        int couplingPrice = 29;
        Cost += tempWagon.Cost + couplingPrice;

        Console.WriteLine($"Wagon added (cost: ${tempWagon.Cost} + ${couplingPrice} coupling). Train price increased to ${Cost}. Current train capacity: {Capacity}");
    }
}

class Wagon
{
    public Wagon(WagonCapacity capacity)
    {
        Capacity = (int)capacity;

        double magicNumber = 3;
        Cost = (int)(Capacity * magicNumber);
    }

    public int Cost { get; private set; }

    public int Capacity { get; private set; }
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
        Console.Clear();

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
