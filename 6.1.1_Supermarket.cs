using System;
using System.Collections.Generic;
using System.Linq;

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
    private List<Item> _storeItems;
    private Queue<Client> _clients;

    private int _balance;

    public Supermarket()
    {
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
            nextClient.CollectBasket(_storeItems);

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

        if (client.TryBuy(out int income))
        {
            _balance += income;
            Helper.WriteAt($"Thanks for shopping at SUPERMARKET, {client.Name}. Have a nice day", foregroundColor: ConsoleColor.Green);
        }
        else
        {
            Helper.WriteAt($"Get the hell out of here and get a job", foregroundColor: ConsoleColor.Red);
        }

        Helper.WaitForKeyPress();
        Console.WriteLine($"Just {_clients.Count} weirdos left");
    }

    private void InitializeItems()
    {
        _storeItems = new List<Item>()
        {
            new Item(3, "Milk"),
            new Item(2, "Bread"),
            new Item(5, "Eggs"),
            new Item(4, "Apples"),
            new Item(6, "Chicken Breast"),
            new Item(2, "Pasta"),
            new Item(3, "Tomato Sauce"),
            new Item(1, "Bananas"),
            new Item(7, "Cheese"),
            new Item(2, "Rice")
        };
    }

    private void InitializeClients()
    {
        _clients = new Queue<Client>();
        int minClients = 3;
        int maxClients = 10;
        int clientCount = Helper.GetRandomInt(minClients, maxClients + 1);

        string[] randomNames =
        {
            "Amina", "Aroha", "Aurora", "Carlos", "Chimamanda",
            "Diego", "Elena", "Freya", "Hannah", "Ivan",
            "Jordan", "Kwame", "Lars", "Luna", "Mark",
            "Mei", "Ngozi", "Orion", "Phoenix", "Raj",
            "Riley", "Sakura", "Sofia", "Tama", "Yuki"
        };

        for (int i = 0; i < clientCount; i++)
        {
            int nameIndex = Helper.GetRandomInt(0, randomNames.Length);
            string name = randomNames[nameIndex];
            Client client = new Client(name);
            _clients.Enqueue(client);
        }
    }
}

class Client
{
    private int _balance;
    private List<Item> _basket;
    private List<Item> _bag;

    public Client(string name)
    {
        _basket = new List<Item>();

        int maxMoney = 40;
        _balance = Helper.GetRandomInt(0, maxMoney + 1);

        Name = name;
    }

    public string Name { get; private set; }

    private int TotalBasketPrice
        => CalculateTotalBasketPrice();

    public void CollectBasket(IEnumerable<Item> storeItems)
    {
        List<Item> items = storeItems.ToList();
        int minItemsNeeded = 1;
        int maxItemsNeeded = 10;
        int itemsNeededCount = Helper.GetRandomInt(minItemsNeeded, maxItemsNeeded + 1);
        Helper.WriteTitle($"My name is {Name}. I need {itemsNeededCount} things");

        for (int i = 0; i < itemsNeededCount; i++)
        {
            Item item = PickRandomItem(items);

            if (item != null) 
                _basket.Add(item);
        }

        Helper.WriteAt($"\nOkay. I think it's all I need. I have {_basket.Count} items for ${TotalBasketPrice}. I'll go to the cashier now");
        Helper.WaitForKeyPress(false);
    }

    public bool TryBuy(out int purchasePrice)
    {
        if (TryMakeBasketAffordable() == false)
        {
            purchasePrice = 0;
            return false;
        }

        purchasePrice = TotalBasketPrice;
        _balance -= purchasePrice;
        Helper.WriteAt($"Here is ${purchasePrice}, cashier (you saw, that he now have ${_balance})\n", foregroundColor: ConsoleColor.Green);
        _bag = _basket;
        _basket.Clear();

        return true;
    }

    private bool TryMakeBasketAffordable()
    {
        Console.WriteLine($"Looking inside his wallet (${_balance})");

        while (TotalBasketPrice > _balance && _basket.Count != 0)
        {
            Helper.WriteAt($"Looks like I can't afford it. Everything costs ${TotalBasketPrice}", foregroundColor: ConsoleColor.DarkRed);
            RemoveRandomItemFromBasket();
            Helper.WaitForKeyPress(false);
        }

        bool canAfford = _balance >= TotalBasketPrice && _basket.Count != 0;

        if (canAfford == false)
            Helper.WriteAt($"I'm terribly sorry, but I don't have enough money", foregroundColor: ConsoleColor.DarkRed);

        return canAfford;
    }

    private Item PickRandomItem(List<Item> items)
    {
        if (items.Count == 0)
            return null;

        int itemIndex = Helper.GetRandomInt(0, items.Count);
        Item item = items[itemIndex];
        Helper.WriteAt($"I pick this {item.GetInfo()}.");

        return item;
    }

    private void RemoveRandomItemFromBasket()
    {
        if (_basket.Count == 0)
            return;

        Item item = PickRandomItem(_basket);
        Helper.WriteAt($"I didn't need it anyway");

        _basket.Remove(item);
    }

    private int CalculateTotalBasketPrice()
    {
        int totalPrice = 0;

        if (_basket == null || _basket.Count == 0)
            return totalPrice;

        foreach (Item item in _basket)
            totalPrice += item.Price;

        return totalPrice;
    }
}

class Item
{
    private string _name;

    public Item(int price, string name)
    {
        Price = price;
        _name = name;
    }

    public int Price { get; private set; }

    public string GetInfo()
    {
        return $"{_name} for ${Price}";
    }
}

class Helper
{
    private static Random s_random = new Random();

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
