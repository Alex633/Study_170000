using System;
using System.Collections.Generic;
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

        Table table = new Table();

        table.Render();

        table.DealCardsToPlayer(Helper.ReadInt("\nHow many cards to draw?"));
        Console.Clear();

        table.Render();

        Helper.ClearAfterKeyPress();
    }
}

class Table
{
    private Player _player;
    private Deck _deck;

    public Table()
    {
        _deck = new Deck();
        _player = new Player();
    }

    public void Render()
    {
        _deck.Render();
        Console.WriteLine("\n");
        _player.RenderHand();
    }

    public void DealCardsToPlayer(int amount)
    {
        List<Card> cards = _deck.DrawCards(amount);

        _player.ReceiveCards(cards);
    }
}

class Player
{
    private List<Card> _hand;

    public Player()
    {
        _hand = new List<Card>();
    }

    public void RenderHand() => Renderer.RenderCards("player's hand", _hand);

    public void ReceiveCards(List<Card> cards)
    {
        _hand.AddRange(cards);
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

    public void Render() => Renderer.RenderCards("deck", _cards);

    public List<Card> DrawCards(int amount)
    {
        HandleDrawErrors(amount);

        List<Card> cards = new List<Card>();

        for (int i = 0; i < amount; i++)
        {
            if (_cards.Count == 0)
                return cards;

            cards.Add(_cards.Pop());
        }

        return cards;
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

    private void HandleDrawErrors(int amount)
    {
        if (_cards.Count == 0)
            throw new Exception("The deck is empty.");

        if (amount < 0)
            throw new Exception("Amount can't be negative");
    }
}

class Card
{
    public Card(ValueName value, SuitName suit)
    {
        Value = value;
        Suit = new Suit(suit);
    }

    public ValueName Value { get; private set; }
    public Suit Suit { get; private set; }

    public ConsoleColor Color => Suit.Color;
}

class Suit
{
    public Suit(SuitName suit)
    {
        Symbol = ConvertToSymbol(suit);

        Color = suit == SuitName.Clubs || suit == SuitName.Spades ?
            ConsoleColor.White : ConsoleColor.DarkRed;
    }

    public string Symbol { get; private set; }
    public ConsoleColor Color { get; private set; }

    private string ConvertToSymbol(SuitName suit)
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

class Renderer
{
    public static void RenderCards(string title, IEnumerable<Card> cards, int rowSize = 4)
    {
        Helper.WriteTitle(title);

        int cardCount = 0;

        foreach (Card card in cards)
        {
            RenderCard(card);
            Console.Write(" ");

            cardCount++;

            if (cardCount % rowSize == 0)
                Console.WriteLine();
        }
    }

    private static void RenderCard(Card card)
    {
        int maxValueLength = 5;
        int currentValueLength = card.Value.ToString().Length;

        int length = maxValueLength - currentValueLength;
        string emptySpace = new string(' ', length);

        string sprite = $"[ {card.Suit.Symbol} {card.Value} {emptySpace}]";

        Helper.WriteAt(sprite, foregroundColor: card.Color, isNewLine: false);
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
