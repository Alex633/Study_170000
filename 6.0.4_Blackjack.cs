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
            deck.Shuffle();
            
            Player player1 = new Player(deck);
            Player player2 = new Player(deck);

            ConsoleTwo.WriteLine($"Your Cards (value: {player1.GetHandSumValue()}):", ConsoleColor.Gray);
            player1.RenderHand();

            ConsoleTwo.WriteLine($"Player 1\nDraw more? (y or n)", ConsoleColor.Blue);
            ConsoleKeyInfo playerInput = Console.ReadKey(true);

            switch (playerInput.KeyChar)
            {
                case 'y':
                    player1.DrawCard(deck);

                    break;
                case 'n':
                    break;
                default:
                    break;
            }

            ConsoleTwo.WriteLine($"Player 2\nDraw more? (y or n)", ConsoleColor.Blue);
            playerInput = Console.ReadKey(true);

            switch (playerInput.KeyChar)
            {
                case 'y':
                    player2.DrawCard(deck);
                    break;
                case 'n':
                    break;
                default:
                    break;
            }

            Console.Clear();
            player1.RenderHand();
            Console.ReadKey();
            Console.Clear();
            player1.DrawCard(deck);
            player1.RenderHand();
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
            foreach (Card card in Cards)
            {
                card.ShowCardInfo();
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

        public void RenderCard(char suit, Value cardValue)
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

            Console.WriteLine(topBorder);
            Console.WriteLine($"║{cardValueIcons[cardValue],-8} ║");
            Console.WriteLine($"║{suit,-8} ║");
            Console.WriteLine(wall);
            //Console.WriteLine($"║{' ',3} {suit} {' ',3}║");
            Console.WriteLine(wall);
            Console.WriteLine($"║{' ',8}{suit}║");

            if (cardValueIcons[cardValue] != "10")
                Console.WriteLine($"║{' ',8}{cardValueIcons[cardValue]}║");
            else
                Console.WriteLine($"║{' ',7}{cardValueIcons[cardValue]}║");

            Console.WriteLine(bottomBorder);
            Console.ResetColor();
        }

        public void ShowCardInfo()
        {
            switch (CardSuit)
            {
                case Suit.Hearts:
                    RenderCard('\u2665', CardValue);
                    break;
                case Suit.Diamonds:
                    RenderCard('\u2666', CardValue);
                    break;
                case Suit.Clubs:
                    RenderCard('\u2663', CardValue);
                    break;
                case Suit.Spades:
                    RenderCard('\u2660', CardValue);
                    break;
            }
        }
    }

    class Player
    {
        List<Card> hand = new List<Card>();
        public Player(Deck deck)
        {
            hand.Add(deck.Cards.Pop());
            hand.Add(deck.Cards.Pop());
        }

        public void DrawCard(Deck deck)
        {
            hand.Add(deck.Cards.Pop());
        }

        public void RenderHand()
        {
            foreach (Card card in hand)
            {
                card.ShowCardInfo();
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
