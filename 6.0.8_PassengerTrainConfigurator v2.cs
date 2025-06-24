using System;
using System.Collections.Generic;

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
        Console.OutputEncoding = System.Text.Encoding.UTF8;

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
        while (_dispatcher.IsFired == false && _mainMenu.IsExitSelected == false)
        {
            _dispatcher.ShowBalance();
            _dispatcher.AttemptShowOrders();
            _mainMenu.Run();
            EvaluateDispatcherFiringStatus();
        }
    }

    private void EvaluateDispatcherFiringStatus()
    {
        int aLotOfMoney = -3000;

        if (_dispatcher.Balance <= aLotOfMoney)
        {
            _dispatcher.IsFired = true;

            Helper.WriteAt($"You’ve been fired and now owe ${_dispatcher.Balance} to the train company. " +
                $"Glory to the train company.", foregroundColor: ConsoleColor.DarkRed);
        }
    }

    private void InitializeCommandsMainMenu()
    {
        Dictionary<int, Command> menu = new Dictionary<int, Command>()
        {
            [1] = new Command("Create order", _dispatcher.CreateOrder),
            [2] = new Command("Dispatch trains", _dispatcher.DispatchTrains)
        };

        _mainMenu = new CommandLineInterface(menu, "Train system commands", shouldRunOnce: true);
    }
}

class Dispatcher
{
    public bool IsFired;

    private List<Order> _orders;
    private int _lastSoldTickets;

    public Dispatcher()
    {
        IsFired = false;

        Balance = 100;

        _orders = new List<Order>();
    }

    public int Balance { get; private set; }

    public void ShowBalance()
    {
        bool isNegative = Balance < 0;

        int xPosition = 100;

        ConsoleColor color = isNegative ? ConsoleColor.Red : ConsoleColor.Green;

        Helper.WriteAt($"Balance: ${Balance}", 0, xPosition, color);
    }

    public void DispatchTrains()
    {
        int dispatchCost = 110;

        Balance -= dispatchCost;

        Console.WriteLine($"You paid ${dispatchCost} for dispatching...\n");

        foreach (Order order in _orders)
            CompleteOrder(order);

        _orders.Clear();
        EvaluateRetirement();
    }

    public void AttemptShowOrders()
    {
        if (_orders.Count <= 0)
            return;

        Helper.WriteTitle("Current orders");

        foreach (Order order in _orders)
        {
            order.ShowInfo();
            Console.WriteLine();
        }
    }

    public void CreateOrder()
    {
        _orders.Add(new Order(CreateRoute(), GenerateSoldTickets(), BuildTrain()));
    }

    private Route CreateRoute()
    {
        Helper.WriteTitle("Create route");

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

        _lastSoldTickets = new Random().Next(maxPassengers);
        int income = ticketPrice * _lastSoldTickets;

        Helper.WriteAt($"Tickets sold: {_lastSoldTickets}. Income (locked until order complete): ${income}");
        Helper.ClearAfterKeyPress();

        return _lastSoldTickets;
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
            $"Train constructor (tickets sold: {_lastSoldTickets})");

        trainConstructor.Run();

        Helper.WriteAt($"Train constructed (seats: {tempTrain.Capacity}). Total train cost: ${tempTrain.Cost}", foregroundColor: ConsoleColor.Green);

        Balance -= tempTrain.Cost;

        return tempTrain;
    }

    private void CompleteOrder(Order order)
    {
        order.Complete();

        string incomeStatus = order.IsProfitable ? "earned" : "lost";
        ConsoleColor incomeStatusColor = order.IsProfitable ? ConsoleColor.Green : ConsoleColor.Red;

        Balance += order.Sales;
        Balance -= order.Penalty;

        Helper.WriteAt($"You {incomeStatus}: ${order.Income} (balance: ${Balance})\n", foregroundColor: incomeStatusColor);
    }

    private void EvaluateRetirement()
    {
        int aLotOfMoney = 1400;

        if (Balance >= aLotOfMoney)
        {
            IsFired = true;

            Helper.WriteAt($"\"I quit!\", - you say to your train company while having ${Balance} in your pockets. " +
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

        int locomotionPrice = 28;
        Console.WriteLine($"New train created. Locomotion price: {locomotionPrice}\n");

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

        int couplingPrice = 16;
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

class Order
{
    private int _soldTickets;
    private int _angryPassengers;

    public Order(Route route, int soldTickets, Train train)
    {
        _route = route;

        _soldTickets = soldTickets;
        _train = train;

        CalculateSales();
        CalculatePenalty();
        CalculateIncome();
    }

    private Route _route;
    private Train _train;

    public int Penalty { get; private set; }
    public int Income { get; private set; }
    public int Sales { get; private set; }

    public bool IsProfitable { get; private set; }

    public void Complete()
    {
        Helper.WriteAt($"Train (${_train.Cost}) to {_route.Destination} leaving the station {_route.Departure} (sales: ${Sales})");

        if (_angryPassengers > 0)
        {
            Helper.WriteAt($"However, there were not enough seats. {_angryPassengers} passengers were very angry " +
                $"(penalty: ${Penalty})",
                foregroundColor: ConsoleColor.Red);
        }
    }

    public void ShowInfo()
    {
        Console.WriteLine($"Train (${_train.Cost}) from {_route.Departure} to {_route.Destination}. Sold tickets: {_soldTickets}. " +
            $"Train capacity: {_train.Capacity}");
    }

    private void CalculateSales()
    {
        int ticketPrice = 4;

        Sales = ticketPrice * _soldTickets;
    }

    private void CalculateIncome()
    {
        Income -= (_train.Cost + Penalty);
        Income += Sales;

        IsProfitable = Income >= 0;
    }

    private void CalculatePenalty()
    {
        if (_soldTickets > _train.Capacity)
        {
            int finePerPassenger = 8;

            _angryPassengers = _soldTickets - _train.Capacity;
            Penalty = _angryPassengers * finePerPassenger;
        }
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
