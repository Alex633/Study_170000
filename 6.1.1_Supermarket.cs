using System;
using System.Collections.Generic;

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
    private Groceries _grocerries;
    private Queue<Client> _clients;

    private int _balance;

    public Supermarket()
    {
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
            nextClient.CollectBasket(_grocerries);

            Helper.WaitForKeyPress(false);
            Console.WriteLine();

            ProcessSale(nextClient);

            Helper.WaitForKeyPress(true);
        }

        Console.WriteLine($"Store is closing. You earned: ${_balance}");
    }

    private void ProcessSale(Client client)
    {
        string greetings = client.Name == "Mark" ? $"Oh, hi {client.Name}" : $"Hi {client.Name}";
        Helper.WriteTitle(greetings);

        if (client.TryBuy())
        {
            _balance += client.Basket.TotalPrice;
            Groceries bag = client.Basket;
            client.PickupBag(bag);
            Helper.WriteAt($"Thanks for shopping at SUPERMARKET, {client.Name}. Have a nice day", foregroundColor: ConsoleColor.Green);
        }
        else
        {
            Helper.WriteAt($"Get the hell out of here and get a job", foregroundColor: ConsoleColor.Red);
        }

        Console.WriteLine($"Just {_clients.Count} weirdos left");
    }

    private void InitializeItems()
    {
        List<Item> items = new List<Item>()
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

        _grocerries = new Groceries(items);
    }

    private void InitializeClients()
    {
        string[] _randomNames =
        {
            "Amina", "Aroha", "Aurora", "Carlos", "Chimamanda",
            "Diego", "Elena", "Freya", "Hannah", "Ivan",
            "Jordan", "Kwame", "Lars", "Luna", "Mark",
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

class Client
{
    private int _balance;
    private Groceries _bag;

    public Client(string name)
    {
        Basket = new Groceries();
        _bag = new Groceries();

        _balance = Helper.GetRandomInt(0, 40);

        Name = name;
    }

    public string Name { get; private set; }
    public Groceries Basket { get; private set; }

    public void CollectBasket(Groceries grocerries)
    {
        int minItemsNeeded = 1;
        int maxItemsNeeded = 10;
        int itemsNeededCount = Helper.GetRandomInt(minItemsNeeded, maxItemsNeeded + 1);
        Helper.WriteTitle($"My name is {Name}. I need {itemsNeededCount} things");

        for (int i = 0; i < itemsNeededCount; i++)
        {
            if (grocerries.TryPickRandomItem(out Item item))
            {
                Helper.WriteAt($"This {item.GetInfo()} looks good to me. I'll put it in my basket");
                Basket.AddItem(item);
            }
        }

        Helper.WriteAt($"\nOkay. I think it's all I need. I have {Basket.Quantity} items for ${Basket.TotalPrice}. I'll go to the cashier now");
    }

    public bool TryBuy()
    {
        if (TryMakeBasketAffordable() == false)
            return false;

        _balance -= Basket.TotalPrice;
        Helper.WriteAt($"Here is ${Basket.TotalPrice}, cashier (you saw, that he now have ${_balance})\n", foregroundColor: ConsoleColor.Green);

        return true;
    }

    public void PickupBag(Groceries bag)
    {
        _bag = bag;
    }

    private bool TryMakeBasketAffordable()
    {
        Console.WriteLine($"Looking inside his wallet (${_balance})");

        while (Basket.TotalPrice > _balance && Basket.Quantity != 0)
        {
            Helper.WriteAt($"Looks, like I can't afford it. Everything costs ${Basket.TotalPrice}", foregroundColor: ConsoleColor.DarkRed);

            if (Basket.TryDrawRandomItem(out Item itemToRemove))
                Helper.WriteAt($"I will get rid of this {itemToRemove.GetInfo()}");
        }

        bool canAfford = _balance >= Basket.TotalPrice && Basket.Quantity != 0;

        if (canAfford == false)
            Helper.WriteAt($"I'm terribly sorry, but I don't have enough money", foregroundColor: ConsoleColor.Red);

        return canAfford;
    }
}

class Groceries
{
    private List<Item> _items;

    public Groceries()
    {
        _items = new List<Item>();
    }

    public Groceries(List<Item> items)
    {
        _items = items;
    }

    public int Quantity => _items != null ? _items.Count : 0;

    public int TotalPrice
        => CalculateTotalPrice();

    public void AddItem(Item item)
    {
        _items.Add(item);
    }

    public bool TryDrawRandomItem(out Item item)
    {
        if (_items.Count == 0)
        {
            item = null;
            return false;
        }

        TryPickRandomItem(out item);
        _items.Remove(item);
        return true;
    }

    public bool TryPickRandomItem(out Item item)
    {
        if (_items.Count == 0)
        {
            item = null;
            return false;
        }

        int itemIndex = Helper.GetRandomInt(0, Quantity);
        item = _items[itemIndex];

        return true;
    }

    private int CalculateTotalPrice()
    {
        int totalPrice = 0;

        foreach (Item item in _items)
            totalPrice += item.Price;

        return totalPrice;
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

class Helper
{
    private static Random _random = new Random();

    public static int GetRandomInt(int min, int max)
    {
        return _random.Next(min, max);
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
