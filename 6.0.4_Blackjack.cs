using System;
using System.Collections.Generic;
using static CsRealLearning.Card;

//Есть колода с картами. Игрок достает карты, пока не решит, что ему хватит карт
//(может быть как выбор пользователя, так и количество сколько карт надо взять).
//После выводиться вся информация о вытянутых картах.
//Возможные классы: Карта, Колода, Игрок.

//todo: 1. render at specific position for player 2
//      2. order of turns
//      3. other blackjack commands
//      4. score

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

        private int _jackBlack = 21;
        private bool _isGameFinished = false;

        enum playerColor
        {
            Cyan = 1,
            Green
        }

        public void Start()
        {
            Console.CursorVisible = false;
            deck.Shuffle();


            Player player1 = new Player(deck, "PLAYER 1");
            Player player2 = new Player(deck, "PLAYER 2");

            while (!_isGameFinished)
            {
                Turn(player1, player2);
                //Turn(player2, player1);
            }

            Console.WriteLine("The End");


        }

        private void Turn(Player player, Player opponent)
        {
            Custom.WriteLine($"{player.Name} turn", ConsoleColor.Cyan);
            player.RenderHand(ConsoleColor.Cyan);
            opponent.RenderHand(ConsoleColor.DarkGray, 50);
            CheckIfEnd(player, opponent, ref _isGameFinished);

            if (_isGameFinished)
            {
                Console.Clear();
                return;
            }

            Custom.WriteLine($"\nDraw more? (y or n)", ConsoleColor.Cyan, true, 1, 1);

            switch (GetPlayerChoice())
            {
                case 'y':
                    player.DrawCard(deck);
                    break;
                case 'n':
                    break;
            }

            Console.Clear();
        }

        private void CheckIfEnd(Player player, Player opponent, ref bool _isGameFinished)
        {
            if (player.GetHandSumValue() == _jackBlack)
            {
                Custom.WriteLine($"{player.Name} blackjack! Giving turn", ConsoleColor.Yellow);
                Console.ReadKey();
                _isGameFinished = true;
            }
            else if (player.GetHandSumValue() > _jackBlack)
            {
                Custom.WriteLine($"{player.Name} bust!");
                Custom.WriteLine($"{opponent.Name} won!", ConsoleColor.Yellow);
                Console.ReadKey();
                _isGameFinished = true;
            }
        }

        private char GetPlayerChoice(int outputXPos = 0, int outputYPos = 1)
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
                        Console.SetCursorPosition(outputXPos, outputYPos);
                        Custom.WriteLine("Uknown Command. Try again", ConsoleColor.Red, true, 0, 3);
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

        public Suit CardSuit { get; private set; }
        public Value CardValue { get; private set; }

        public int BlackjackValue
        {
            get
            {
                if ((int)CardValue > 10)
                    return 10;
                else
                    return (int)CardValue;
            }
            private set { }
        }

        public Card(Suit suit, Value value)
        {
            CardSuit = suit;
            CardValue = value;
        }

        public enum Suit { Hearts = 1, Diamonds, Clubs, Spades }

        public enum Value { Ace = 1, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King }

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
        public string Name { get; private set; }

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

        public void RenderHand(ConsoleColor playerColor = ConsoleColor.Gray, int xStartPos = 0, int yStartPos = 1, int rowMaxSym = 40)
        {
            Console.SetCursorPosition(xStartPos, yStartPos);

            Custom.WriteLine($"(value: {GetHandSumValue()})", playerColor);

            foreach (Card card in hand)
            {
                card.ShowCardInfo(xStartPos, yStartPos + 3);
                xStartPos += 12;

                if (xStartPos >= rowMaxSym)
                {
                    yStartPos += 8;
                    xStartPos = 0;
                }
            }
        }

        public int GetHandSumValue()
        {
            int sumHandValue = 0;

            foreach (Card card in hand)
            {
                if (card.CardValue == Value.Ace && sumHandValue < 10)
                    sumHandValue += 11;

                sumHandValue += card.BlackjackValue;
            }

            return sumHandValue;
        }
    }
}

class Custom
{
    public static void WriteLine(string text, ConsoleColor color = ConsoleColor.Red, bool customPos = false, int xPos = 0, int YPos = 0)
    {
        if (customPos)
            Console.SetCursorPosition(xPos, YPos);

        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    public static void PressAnything(string text = "\npress anything to continue")
    {
        Custom.WriteLine(text, ConsoleColor.DarkYellow);
        Console.ReadKey();
        Console.Clear();
    }
}
