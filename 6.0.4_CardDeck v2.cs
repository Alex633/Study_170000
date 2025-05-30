using System;
using System.Collections.Generic;

enum Suit
{
    Clubs = 1,
    Diamonds,
    Hearts,
    Spades
}

enum Value
{
    Ace = 1,
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Six = 6,
    Seven = 7,
    Eight = 8,
    Nine = 9,
    Ten = 10,
    Jack = 11,
    Queen = 12,
    King = 13
}

public class Program
{
    static void Main()
    {
        //Database database = new Database();

        ConsoleMenu menu = new ConsoleMenu();

        menu.Run();
    }
}

class Table
{

}

class Player
{

}

class Deck
{
    public Deck()
    {
        Initialize();
    }

    private Stack<Card> _cards;

    public void Shuffle()
    {

    }

    private void Initialize()
    {
        _cards = new Stack<Card>();

        for (int i = 0; i < 4; i++)
        {
            _cards.Push(new Card(Value.Ace, Suit.Spades));
        }
    }
}

class Card
{
    public Card(Value value, Suit suit)
    {

    }

    public Value Value { get; private set; }

    public Suit Suit { get; private set; }
}

class ConsoleMenu
{
    private Dictionary<int, MenuItem> _items;

    private int _input;
    private bool _shouldRun;

    public ConsoleMenu()
    {
        _shouldRun = true;

        InitilizeItems();
    }

    public void Run()
    {
        while (_shouldRun)
        {
            OutputCommands();

            _input = Helper.ReadInt("Input command: ");
            HandleInput();
        }
    }

    private void HandleInput()
    {
        Console.Clear();

        if (_items.TryGetValue(_input, out MenuItem item))
            item.Execute();
        else
            Helper.WriteAt("Invalid command. Please try again.", foregroundColor: ConsoleColor.Red);

        Helper.ClearAfterKeyPress();
    }


    private void OutputCommands()
    {
        Helper.WriteTitle("Gamers Database commands");

        foreach (var item in _items)
            Helper.WriteAt($"{item.Key}) {item.Value.Description}");

        Console.WriteLine();
    }

    private void Exit()
    {
        _shouldRun = false;
    }

    private void InitilizeItems()
    {
        const int CommandAdd = 1;
        const int CommandRemove = 2;
        const int CommandBan = 3;
        const int CommandUnban = 4;
        const int CommandFull = 5;
        const int CommandExit = 6;

        _items = new Dictionary<int, MenuItem>()
        {
            [CommandAdd] = new MenuItem("Add gamer", _database.AddGamer),
            [CommandRemove] = new MenuItem("Remove gamer", _database.RemoveGamer),
            [CommandBan] = new MenuItem("Ban gamer", _database.BanGamer),
            [CommandUnban] = new MenuItem("Unban gamer", _database.UnbanGamer),
            [CommandFull] = new MenuItem("Output full database", () => _database.TryOutputFull()),
            [CommandExit] = new MenuItem("Exit", Exit),
        };
    }
}

class MenuItem
{
    private readonly Action _action;

    public MenuItem(string description, Action action)
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

        WriteAt(new string(' ', helpText.Length - 1), backgroundColor: primary, isNewLine: false, xPosition: fieldStartX);

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
