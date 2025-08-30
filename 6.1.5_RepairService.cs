using System;
using System.Collections.Generic;

//У вас есть автосервис, в котором будут машины для починки.
//Автосервис содержит баланс денег и склад деталей. В автосервисе стоит очередь машин.
//Машина состоит из деталей и количество поломанных будет не меньше 1 детали. Надо показывать все детали, которые поломанные.
//Поломка всегда чинится заменой детали. При починке машины за раз можно заменять только одну деталь.
//При успешной починке детали сервис получает (цена детали + цена ремонта).
//Ремонт считается завершенным, когда все детали машины исправны. 
//От ремонта можно отказаться в любой момент.
//Если отказ перед ремонтом, то платите фиксированный штраф.
//Если отказ во время ремонта, то платите штраф за каждую непочиненную деталь.
//Количество деталей на складе ограничено.
//При замене целой детали в машине, деталь пропадает из склада, но вы ничего не получаете за замену данной детали. 
//За каждую удачную починку вы получаете выплату за ремонт, которая указана в чек-листе починки.
//Класс Деталь не может содержать значение “количество”. Деталь всего одна, за количество отвечает тот, кто хранит детали.
//При необходимости можно создать дополнительный класс для конкретной детали и работе с количеством.

public class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Client client = new Client();
        RepairService repairServis = new RepairService(client);

        repairServis.Open();

        Helper.WaitForKeyPress();
    }
}

class RepairService
{
    private int _balance;

    private Queue<Car> _cars;
    private Stack<Part> _spareParts;

    private Client _client;

    public RepairService(Client client)
    {
        StockUp();

        _balance = 0;

        _client = client;
        _cars = _client.ProvideCars();
    }

    public void Open()
    {
        while (_cars.Count != 0 && _spareParts.Count > 0)
        {
            ShowHud();
            ShowCars();

            TryRepairCar(_cars.Dequeue());
        }
    }

    //public void Repair()
    //{
    //    foreach (Car car in _cars)
    //    {
    //        TryRepairCar(car);
    //        Console.WriteLine("Proceeding to the next car");
    //        Helper.WaitForKeyPress(true);
    //    }

    //    Console.WriteLine("No more cars to repair");
    //}

    public void TryRepairCar(Car car)
    {
        while (TryFindBrokenPartInCar(car, out int brokenPartIndex))
        {
            Helper.WriteTitle($"Repairing...");

            car.ShowInfo();
            ShowHud();

            if (TryRepairPart(brokenPartIndex, car) == false)
                return;
        }

        Console.WriteLine("Nothing else to repair");
        Helper.WaitForKeyPress(shouldClearAfter: true);
    }

    private bool TryRepairPart(int brokenPartIndex, Car car)
    {
        Helper.WriteAt($"Replacing part at {brokenPartIndex}...");

        if (_spareParts.Count <= 0)
        {
            Helper.WriteAt("No more spare parts left. Closing the repair service :(", foregroundColor: ConsoleColor.Red);
            Helper.WaitForKeyPress();
            return false;
        }

        car.ReplacePartAt(brokenPartIndex, _spareParts.Pop());
        Helper.WriteAt($"Success\n", foregroundColor: ConsoleColor.Green);

        Helper.WaitForKeyPress();
        return true;
    }

    private bool TryFindBrokenPartInCar(Car car, out int brokenPartIndex)
    {
        brokenPartIndex = -1;

        for (int i = 0; i < car.Parts.Count; i++)
        {
            if (car.Parts[i].IsBroken)
            {
                brokenPartIndex = i;
                return true;
            }
        }

        return false;
    }

    private void ShowCars()
    {
        Helper.WriteTitle("Garage");

        foreach (Car car in _cars)
        {
            car.ShowInfo();
        }

        Helper.WaitForKeyPress(shouldClearAfter: true);
    }

    private void ShowHud()
    {
        int xPosition = 104;

        Helper.WriteAt($"Balance: {_balance}", yPosition: 0, xPosition: xPosition);
        Helper.WriteAt($"Spare Parts: {_spareParts.Count}", yPosition: 1, xPosition: xPosition);
        Helper.WriteAt($"Cars: {_cars.Count}", yPosition: 2, xPosition: xPosition);
    }

    private void StockUp(int carsQuantity = 4, int partsQuantity = 10)
    {

        _spareParts = new Stack<Part>();

        for (int i = 0; i < partsQuantity; i++)
        {
            _spareParts.Push(PartFactory.Create(false));
        }
    }
}

static class CarFactory
{
    public static Queue<Car> CreateFew(int amount = 1)
    {
        Queue<Car> newCars = new Queue<Car>();

        for (int i = 0; i < amount; i++)
        {
            newCars.Enqueue(Create());
        }

        return newCars;
    }

    public static Car Create()
    {
        List<Part> parts = PartFactory.CreateFew(6);

        return new Car(parts);
    }
}

static class PartFactory
{
    public static List<Part> CreateFew(int amount = 1)
    {
        List<Part> parts = new List<Part>();

        int brokenPartValue = 0;
        int goodPartValue = 1;
        bool hasManufacturingDefect = false;

        for (int i = 0; i < amount; i++)
        {
            bool isBrokenPart = Convert.ToBoolean(Helper.GetRandomInt(brokenPartValue, goodPartValue + 1));

            if (isBrokenPart && hasManufacturingDefect == false)
                hasManufacturingDefect = true;

            if (hasManufacturingDefect == false && amount == i + 1)
                isBrokenPart = true;

            parts.Add(Create(isBrokenPart));
        }

        return parts;
    }

    public static Part Create(bool isBroken, int? price = null)
    {
        if (price.HasValue == false)
        {
            int minPrice = 0;
            int maxPrice = 10;
            price = Helper.GetRandomInt(minPrice, maxPrice + 1);
        }


        return new Part(isBroken, price.Value);
    }
}

class Client
{
    private int _balance;

    private Queue<Car> _cars;

    public Client()
    {
        int minBalance = 30;
        int maxBalance = 100;
        _balance = Helper.GetRandomInt(minBalance, maxBalance + 1);

        _cars = CarFactory.CreateFew(amount: 4);
    }

    public Queue<Car> ProvideCars()
    {
        return _cars;
    }
}

class Car
{
    private List<Part> _parts;

    private Guid _id;

    public Car(List<Part> parts)
    {
        _parts = parts;

        _id = Guid.NewGuid();
    }

    public IReadOnlyList<Part> Parts => _parts.AsReadOnly();

    public void ReplacePartAt(int index, Part newPart)
    {
        if (index < 0 || index >= _parts.Count)
            throw new ArgumentOutOfRangeException();

        _parts[index] = newPart;
    }

    public void ShowInfo()
    {
        Helper.WriteTitle($"Car {_id}", isSecondary: true);

        bool haveBrokenPart = false;

        for (int i = 0; i < _parts.Count; i++)
        {
            if (_parts[i].IsBroken)
            {
                Helper.WriteAt($"Broken part found ({i})", foregroundColor: ConsoleColor.DarkMagenta);
                haveBrokenPart = true;
            }
        }

        if (haveBrokenPart == false)
        {
            Helper.WriteAt("In good condition");
        }

        Console.WriteLine();
    }
}

class Part
{
    public Part(bool isBroken, int price)
    {
        IsBroken = isBroken;
        Price = price;
    }

    public int Price { get; private set; }
    public bool IsBroken { get; private set; }
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

    public static void WaitForKeyPress(bool shouldClearAfter = true)
    {
        WriteAt("Press any key", foregroundColor: ConsoleColor.Cyan);
        Console.ReadKey(true);

        if (shouldClearAfter)
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
