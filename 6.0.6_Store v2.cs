using System;
using System.Collections.Generic;

public class Program
{
    static void Main()
    {
        Game game = new Game();

        game.Run();

        Helper.ClearAfterKeyPress();
    }
}

class Game
{
    private Stranger _stranger;
    private Merchant _merchant;

    public Game()
    {
        _stranger = new Stranger();
        _merchant = new Merchant();
    }

    public void Run()
    {
        bool shouldRun = true;

        while (shouldRun)
        {
            _stranger.ShowItems();
            Console.WriteLine();
            Console.WriteLine();
            _merchant.ShowItems();

            if (_merchant.TrySelectItem(Helper.ReadInt("Input an item number: "), out Item selectedItem))
            {
                if (_stranger.TryBuy(selectedItem))
                    _merchant.SellItem(selectedItem);
            }

            Helper.ClearAfterKeyPress();
        }
    }
}

class Merchant : Character
{
    public Merchant() : base(name: $"{nameof(Merchant)}")
    {
        InitializeItems();
    }

    public bool TrySelectItem(int itemNumber, out Item selectedItem)
    {
        if (items.Count < itemNumber)
        {
            Helper.WriteAt($"There is no such item ({itemNumber}), stranger", foregroundColor: ConsoleColor.Red);
            selectedItem = null;
            return false;
        }

        selectedItem = items[itemNumber - 1];
        return true;
    }

    public void SellItem(Item item)
    {
        balance += item.Price;

        items.Remove(item);

        Helper.WriteAt($"Thank you :)", foregroundColor: ConsoleColor.Green);
    }

    public override void ShowItems()
    {
        Console.WriteLine("What are you buying?\n");
        base.ShowItems();
    }

    private void InitializeItems()
    {
        items.AddRange(new List<Item>
        {
            new Item(8000, "Handgun"),
            new Item(20000, "Shotgun"),
            new Item(500, "Green Herb")
        });
    }
}

class Stranger : Character
{
    public Stranger() : base(name: $"Leon")
    {
        balance = 25000;
    }

    public bool TryBuy(Item item)
    {
        if (balance < item.Price)
        {
            Helper.WriteAt($"Not enough cash, stranger");
            return false;
        }

        items.Add(item);
        balance -= item.Price;

        return true;
    }
}

class Character
{
    protected List<Item> items;
    protected int balance;

    private string _name;

    public Character(string name)
    {
        _name = name;
        balance = 0;

        items = new List<Item>();
    }

    public virtual void ShowItems()
    {
        Helper.WriteTitle($"{_name}'s inventory (${balance})");

        if (items.Count <= 0)
        {
            Helper.WriteAt($"Empty", foregroundColor: ConsoleColor.DarkGray);
            return;
        }

        for (int i = 0; i < items.Count; i++)
            Console.WriteLine($"{i + 1} - {items[i].GetInfo()}");
    }
}

class Item
{
    public Item(int price, string name)
    {
        Price = price;
        Name = name;
    }
    public int Price { get; private set; }
    public string Name { get; private set; }

    public string GetInfo()
    {
        return $"{Name} ({Price} pesetas)";
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
