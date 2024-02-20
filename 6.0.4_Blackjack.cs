using System;
using System.Collections.Generic;
using static CsRealLearning.Card;

//Есть колода с картами. Игрок достает карты, пока не решит, что ему хватит карт
//(может быть как выбор пользователя, так и количество сколько карт надо взять).
//После выводиться вся информация о вытянутых картах.
//Возможные классы: Карта, Колода, Игрок.

//todo: render hand in the row below

namespace CsRealLearning
{
    internal class Program
    {
        static void Main()
        {
            Game game = new Game();

            game.Start();
        }
    }

    class Game
    {
        Deck deck = new Deck();

        private int winningNumber = 21;

        public void Start()
        {
            Console.CursorVisible = false;
            deck.Shuffle();


            Player player1 = new Player(deck, "PLAYER 1");
            Player player2 = new Player(deck, "PLAYER 2");

            Turn(player1);


        }

        public void Turn(Player player)
        {
            bool isPlayerTurnOver = false;

            while (!isPlayerTurnOver)
            {
                ConsoleTwo.WriteLine($"Your Cards (value: {player.GetHandSumValue()}):", ConsoleColor.Gray);
                player.RenderHand();
                ConsoleTwo.WriteLine($"{player.Name}\nDraw more? (y or n)", ConsoleColor.Blue);

                switch (GetPlayerChoice())
                {
                    case 'y':
                        player.DrawCard(deck);
                        break;
                    case 'n':
                        isPlayerTurnOver = true;
                        break;
                }

                Console.Clear();
            }
        }

        public char GetPlayerChoice(int outputYPos = 12)
        {
            ConsoleKeyInfo playerInput;

            while (true)
            {
                playerInput = Console.ReadKey(true);

                switch (playerInput.KeyChar)
                {
                    case 'y':
                    case 'n':
                        return playerInput.KeyChar;
                    default:
                        Console.SetCursorPosition(0, outputYPos);
                        ConsoleTwo.WriteLine("Uknown Command. Try again");
                        break;
                }
            }
        }
    }

    class Deck
    {
        public Stack<Card> Cards { get; private set; } = new Stack<Card>();

        public Deck()
        {
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Value value in Enum.GetValues(typeof(Value)))
                {
                    Cards.Push(new Card(suit, value));
                }
            }
        }

        public void Shuffle()
        {
            var tempDeck = new List<Card>(Cards);
            Random rnd = new Random();

            for (int i = tempDeck.Count - 1; i > 0; i--)
            {
                int randomIndex = rnd.Next(0, i + 1);

                Card tempCard = tempDeck[i];
                tempDeck[i] = tempDeck[randomIndex];
                tempDeck[randomIndex] = tempCard;
            }

            Cards.Clear();
            foreach (var card in tempDeck)
            {
                Cards.Push(card);
            }
        }

        private void ShowDeckInfo()
        {
            int xPos = 0;
            int yPos = 1;

            foreach (Card card in Cards)
            {
                card.ShowCardInfo(xPos, yPos);
                xPos += 16;
            }
        }
    }

    class Card
    {
        public static Random rnd { get; } = new Random();

        private static int lastID = 1;
        public int Id { get; private set; }

        public Suit CardSuit { get; private set; }
        public Value CardValue { get; private set; }

        public Card(Suit suit, Value value)
        {
            Id = lastID++;
            this.CardSuit = suit;
            this.CardValue = value;
        }

        public enum Suit { Hearts = 1, Diamonds, Clubs, Spades }

        public enum Value { Two = 2, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace }

        public void RenderCard(char suit, Value cardValue, int xPos, int yPos)
        {
            var suitColors = new Dictionary<Suit, ConsoleColor>()
            {
                { Suit.Hearts, ConsoleColor.DarkRed },
                { Suit.Diamonds, ConsoleColor.DarkRed },
                { Suit.Clubs, ConsoleColor.DarkGray },
                { Suit.Spades, ConsoleColor.DarkGray },
            };

            var cardValueIcons = new Dictionary<Value, String>()
            {
                { Value.Two, "2" },
                { Value.Three, "3" },
                { Value.Four, "4" },
                { Value.Five, "5" },
                { Value.Six, "6" },
                { Value.Seven, "7" },
                { Value.Eight, "8" },
                { Value.Nine, "9" },
                { Value.Ten, "10" },
                { Value.Jack, "J" },
                { Value.Queen, "Q" },
                { Value.King, "K" },
                { Value.Ace, "A" },
            };

            string topBorder = "╔═════════╗";
            string bottomBorder = "╚═════════╝";
            string wall = "║         ║";


            Console.ForegroundColor = suitColors[CardSuit];
            Console.SetCursorPosition(xPos, yPos);
            Console.WriteLine(topBorder);
            Console.SetCursorPosition(xPos, ++yPos);
            Console.WriteLine($"║{cardValueIcons[cardValue],-8} ║");
            Console.SetCursorPosition(xPos, ++yPos);
            Console.WriteLine($"║{suit,-8} ║");
            Console.SetCursorPosition(xPos, ++yPos);
            Console.WriteLine(wall);
            Console.SetCursorPosition(xPos, ++yPos);
            Console.WriteLine(wall);
            Console.SetCursorPosition(xPos, ++yPos);
            Console.WriteLine($"║{' ',8}{suit}║");
            Console.SetCursorPosition(xPos, ++yPos);

            if (cardValueIcons[cardValue] != "10")
                Console.WriteLine($"║{' ',8}{cardValueIcons[cardValue]}║");
            else
                Console.WriteLine($"║{' ',7}{cardValueIcons[cardValue]}║");

            Console.SetCursorPosition(xPos, ++yPos);
            Console.WriteLine(bottomBorder);
            Console.ResetColor();
        }

        public void ShowCardInfo(int xPos, int yPos)
        {
            switch (CardSuit)
            {
                case Suit.Hearts:
                    RenderCard('\u2665', CardValue, xPos, yPos);
                    break;
                case Suit.Diamonds:
                    RenderCard('\u2666', CardValue, xPos, yPos);
                    break;
                case Suit.Clubs:
                    RenderCard('\u2663', CardValue, xPos, yPos);
                    break;
                case Suit.Spades:
                    RenderCard('\u2660', CardValue, xPos, yPos);
                    break;
            }
        }
    }

    class Player
    {
        List<Card> hand = new List<Card>();
        public string Name {  get; private set; }
        
        public Player(Deck deck, string name)
        {
            hand.Add(deck.Cards.Pop());
            hand.Add(deck.Cards.Pop());
            Name = name;
        }

        public void DrawCard(Deck deck)
        {
            hand.Add(deck.Cards.Pop());
        }

        public void RenderHand()
        {
            int xPos = 0;
            int yPos = 1;
            int firstRow = 120;

            foreach (Card card in hand)
            {
                card.ShowCardInfo(xPos, yPos);
                xPos += 12;

                if (xPos >= firstRow)
                {
                    yPos += 8;
                    xPos = 0;
                }
            }
        }

        public int GetHandSumValue()
        {
            int sumHandValue = 0;

            foreach (Card card in hand)
            {
                sumHandValue += (int)card.CardValue;
            }

            return sumHandValue;
        }
    }
}

class ConsoleTwo
{
    public static void WriteLine(string style, ConsoleColor color = ConsoleColor.Red)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(style);
        Console.ResetColor();
    }

    public static void PressAnything(string text = "\npress anything to continue")
    {
        ConsoleTwo.WriteLine(text, ConsoleColor.DarkYellow);
        Console.ReadKey();
        Console.Clear();
    }
}
