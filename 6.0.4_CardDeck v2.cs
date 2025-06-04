using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

enum SuitName
{
    Clubs = 1,
    Spades,
    Diamonds,
    Hearts,
}

enum ValueName
{
    Ace = 1,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
    Jack,
    Queen,
    King
}

public class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        //Database database = new Database();

        ConsoleMenu menu = new ConsoleMenu();
        Deck deck = new Deck();

        //menu.Run();


        deck.Render("Deck");
        Console.ReadKey();
    }
}

class Table
{
    private Deck _deck;

    private Player _player;

    public Table()
    {
        _deck = new Deck();
        _player = new Player();
    }

    public void DealCards()
    {

    }
}

class Player
{
    private List<Card> _hand;

    public Player()
    {
        _hand = new List<Card>();
    }
}

class Deck
{
    public const int Size = 52;

    private Stack<Card> _cards;

    public Deck()
    {
        Populate();
        Shuffle();
    }

    public Card DrawOne()
    {
        if (_cards.Count == 0)
            throw new Exception("The deck is empty.");

        return _cards.Pop();
    }

    public void Render(string title, int rowSize = 4)
    {
        Helper.WriteTitle(title);

        int cardCount = 0;

        foreach (Card card in _cards)
        {
            card.Draw();
            Console.Write(" ");

            cardCount++;

            if (cardCount % rowSize == 0)
                Console.WriteLine();
        }
    }

    public void Shuffle()
    {
        var tempDeck = new List<Card>(Size);

        for (int i = 0; i < Size; i++)
            tempDeck.Add(_cards.Pop());

        for (int i = Size - 1; i > 0; i--)
        {
            int randomIndex = Helper.GetRandomInt(0, (i + 1));

            Card tempCard = tempDeck[i];

            tempDeck[i] = tempDeck[randomIndex];
            tempDeck[randomIndex] = tempCard;
        }

        _cards = new Stack<Card>(tempDeck);
    }

    private void Populate()
    {
        _cards = new Stack<Card>(Size);
        
        foreach (ValueName value in Enum.GetValues(typeof(ValueName)))
            foreach (SuitName suit in Enum.GetValues(typeof(SuitName)))
                _cards.Push(new Card(value, suit));
    }
}

class Card
{
    private ConsoleColor _color => Suit.Color;

    public Card(ValueName value, SuitName suit)
    {
        Value = new Value(value);
        Suit = new Suit(suit);
    }

    public Value Value { get; private set; }
    public Suit Suit { get; private set; }

    public void Draw()
    {
        Helper.WriteAt($"[ {Suit.Symbol} {Value.Symbol} ]",
            foregroundColor: _color, isNewLine: false);
    }
}

class Suit
{
    private string _name;

    public Suit(SuitName suit)
    {
        _name = suit.ToString();
        Symbol = ToSymbol(suit);

        Color = suit == SuitName.Clubs || suit == SuitName.Spades ? 
            ConsoleColor.White : ConsoleColor.DarkRed;
    }

    public string Symbol { get; private set; }
    public ConsoleColor Color { get; private set; }


    private string ToSymbol(SuitName suit)
    {
        switch (suit)
        {
            case SuitName.Clubs:
                return "♣";

            case SuitName.Diamonds:
                return "♦";

            case SuitName.Hearts:
                return "♥";

            case SuitName.Spades:
                return "♠";

            default:
                throw new Exception("Unknown suit");
        }
    }
}

class Value
{
    private string _name;

    public Value(ValueName value)
    {
        _name = value.ToString();
        Symbol = ToSymbol(value);
        Weight = (int)value;
    }

    public string Symbol { get; private set; }

    public int Weight { get; private set; }

    public string ToSymbol(ValueName value)
    {
        switch (value)
        {
            case ValueName.Ace:
                return "A ";

            case ValueName.Two:
                return "2 ";

            case ValueName.Three:
                return "3 ";

            case ValueName.Four:
                return "4 ";

            case ValueName.Five:
                return "5 ";

            case ValueName.Six:
                return "6 ";

            case ValueName.Seven:
                return "7 ";

            case ValueName.Eight:
                return "8 ";

            case ValueName.Nine:
                return "9 ";

            case ValueName.Ten:
                return "10";

            case ValueName.Jack:
                return "J ";

            case ValueName.Queen:
                return "Q ";

            case ValueName.King:
                return "K ";

            default:
                throw new Exception("Unknown value");
        }
    }
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

        //_items = new Dictionary<int, MenuItem>()
        //{
        //    [CommandAdd] = new MenuItem("Add gamer", _database.AddGamer),
        //    [CommandRemove] = new MenuItem("Remove gamer", _database.RemoveGamer),
        //    [CommandBan] = new MenuItem("Ban gamer", _database.BanGamer),
        //    [CommandUnban] = new MenuItem("Unban gamer", _database.UnbanGamer),
        //    [CommandFull] = new MenuItem("Output full database", () => _database.TryOutputFull()),
        //    [CommandExit] = new MenuItem("Exit", Exit),
        //};
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
    private static readonly Random _rng = new Random();

    public static int GetRandomInt(int minValue, int maxValue)
    {
        return _rng.Next(minValue, maxValue);
    }

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
