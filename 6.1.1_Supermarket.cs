using System;
using System.Collections.Generic;
using System.Linq;

interface IDamageable
{
    void TakeDamage(int damage);
}

public class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Supermarket supermarket = new Supermarket();
        supermarket.Run();

        Helper.WaitForKeyPress();
    }
}

class Supermarket
{
    private List<Item> _items;
    private Queue<Client> _clients;

    private int _balance;

    public Supermarket()
    {
        _items = new List<Item>();
        _clients = new Queue<Client>();

        _balance = 0;

        InitializeItems();
        InitializeClients();
    }

    public void Run()
    {
        Console.WriteLine($"Today there are {_clients.Count} clients");
        Helper.WaitForKeyPress();

        while (_clients.Count > 0)
        {
            Helper.WriteAt($"Balance: ${_balance}", 0, 100);

            Client nextClient = _clients.Dequeue();
            nextClient.CollectBasket(_items);

            Helper.WaitForKeyPress(false);
            Console.WriteLine();

            ProcessSale(nextClient);

            Helper.WaitForKeyPress(true);
        }

        Console.WriteLine($"Store is closing. You earned: ${_balance}");
    }

    private void ProcessSale(Client client)
    {
        Helper.WriteTitle($"Hi {client.Name}");

        Item item = new Item(3, "Milk", 1000, "ml");
        _balance += item.Price;

        Console.WriteLine($"That's it with him/her. Just {_clients.Count} weirdos left. Wait, did I said that out loud?");
    }

    private void InitializeItems()
    {
        _items = new List<Item>
        {
            new Item(3, "Milk", 1000, "ml"),
            new Item(2, "Bread", 1, "count"),
            new Item(5, "Eggs", 12, "count"),
            new Item(4, "Apples", 1, "kg"),
            new Item(6, "Chicken Breast", 1, "kg"),
            new Item(2, "Pasta", 500, "g"),
            new Item(3, "Tomato Sauce", 500, "ml"),
            new Item(1, "Bananas", 1, "kg"),
            new Item(7, "Cheese", 200, "g"),
            new Item(2, "Rice", 1, "kg")
        };
    }

    private void InitializeClients()
    {
        string[] _randomNames = 
        {
            "Amina", "Aroha", "Aurora", "Carlos", "Chimamanda",
            "Diego", "Elena", "Freya", "Hannah", "Ivan",
            "Jordan", "Kwame", "Lars", "Luna", "Maria",
            "Mei", "Ngozi", "Orion", "Phoenix", "Raj",
            "Riley", "Sakura", "Sofia", "Tama", "Yuki"
        };

        for (int i = 0; i < Helper.GetRandomInt(3, 11); i++)
        {
            int nameIndex = Helper.GetRandomInt(0, _randomNames.Length);
            string name = _randomNames[nameIndex];
            Client client = new Client(name);
            _clients.Enqueue(client);
        }
    }
}

class Item
{
    private string _name;
    private int _quantity;
    private string _quantityType;

    public Item(int price, string name, int quantity, string quantityType)
    {
        Price = price;
        _name = name;
        _quantity = quantity;
        _quantityType = quantityType;
    }

    public int Price { get; private set; }

    public string GetInfo()
    {
        return $"{_name} - {_quantity} ({_quantityType}) for ${Price}";
    }
}

class Client
{
    private List<Item> _basket;
    private List<Item> _bag;

    private int _balance;

    public Client(string name)
    {
        _basket = new List<Item>();
        _bag = new List<Item>();

        _balance = Helper.GetRandomInt(0, 101);

        Name = name;
    }

    public string Name { get; private set; }
    public IEnumerable<Item> Basket => _basket;
    public IEnumerable<Item> Bag => _bag;

    public void CollectBasket(IEnumerable<Item> storeItems)
    {
        List<Item> items = storeItems.ToList();

        int minItemsNeeded = 1;
        int maxItemsNeeded = 10;
        int itemsNeededCount = Helper.GetRandomInt(minItemsNeeded, maxItemsNeeded + 1);
        Helper.WriteTitle($"My name is {Name}. I need {itemsNeededCount} things");

        for (int i = 0; i < itemsNeededCount; i++)
        {
            int itemIndex = Helper.GetRandomInt(0, items.Count());
            Item item = items[itemIndex];
            Helper.WriteAt($"This {item.GetInfo()} looks good to me. I'll put it in my basket");

            _basket.Add(item);
        }

        Helper.WriteAt($"\nOkay. I think it's all I need ({_basket.Count}). I'll go to the cashier now");
    }

    public void PickupBag(List<Item> bag)
    {
        _bag = bag;
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

    public static int GetRandomInt(int min, int max)
    {
        return _random.Next(min, max);
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
