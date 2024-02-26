using System;
using System.Collections.Generic;
using static CsRealLearning.Card;

//Есть колода с картами. Игрок достает карты, пока не решит, что ему хватит карт
//(может быть как выбор пользователя, так и количество сколько карт надо взять).
//После выводиться вся информация о вытянутых картах.
//Возможные классы: Карта, Колода, Игрок.

//todo: 1. press anything to continue at wrong position!!!!
//      3. other blackjack commands
//      4. score
//      5. money betting
//      6. custom player names
//      7. change sits after every round

namespace CsRealLearning
{
    internal class Program
    {
        static void Main()
        {
            BlackjackGame game = new BlackjackGame();

            game.Start();
        }
    }

    class BlackjackGame
    {
        Deck deck = new Deck();

        private bool isGameOn = true;

        static public int BlackjackNum { get; private set; }

        public BlackjackGame()
        {
            BlackjackNum = 21;
        }


        enum playerColor
        {
            Cyan = 1,
            Green
        }

        public void Start()
        {
            Console.CursorVisible = false;
            deck.Shuffle();


            Player player1 = new Player(deck, "Player 1", 0);
            Player player2 = new Player(deck, "Player 2", 60);

            while (isGameOn)
            {
                if (!player2.IsBust)
                    Turn(player1, player2);

                if (!player1.IsBust)
                    Turn(player2, player1);

                CheckIfOver(player1, player2, ref isGameOn);
            }
        }

        private void Turn(Player player1, Player player2)
        {
            if (player1.IsAlive)
            {
                RenderTable(player1, player2);
                HandleInput(player1);
                RenderTable(player1, player2);
                player1.CheckIfAlive();
            }
        }

        private void CheckIfOver(Player player1, Player player2, ref bool isGameOn)
        {
            if (player1.IsBust || player2.IsBust)
            {
                if (player1.IsBust)
                    Custom.WriteFilled($"{player2.Name} victorius with {player2.GetHandSumValue()}");

                if (player2.IsBust)
                    Custom.WriteFilled($"{player1.Name} victorius with {player1.GetHandSumValue()}");

                isGameOn = false;
                return;
            }

            if (!player1.IsAlive && !player2.IsAlive)
            {
                if (player1.GetHandSumValue() > player2.GetHandSumValue())
                    Custom.WriteFilled($"{player1.Name} victorius with {player1.GetHandSumValue()}");

                if (player2.GetHandSumValue() > player1.GetHandSumValue())
                    Custom.WriteFilled($"{player2.Name} victorius with {player2.GetHandSumValue()}");

                isGameOn = false;
            }
        }
        private void RenderTable(Player player1, Player player2)
        {
            int player1XPos = player1.XPos;
            int player1YPos = 0;
            int player2XPos = player2.XPos;
            int player2YPos = 0;

            Custom.WriteInColor($"{player1.Name} ({player1.GetHandSumValue()})", ConsoleColor.Cyan, true, player1XPos, player1YPos);
            player1.RenderHand(ConsoleColor.Cyan);

            Custom.WriteInColor($"{player2.Name} ({player2.GetHandSumValue()})", ConsoleColor.Green, true, player2XPos, player2YPos);
            player2.RenderHand(ConsoleColor.Green);
        }

        private void HandleInput(Player player)
        {

            Custom.WriteInColor($"\nDo you want me to hit you? (y or n)", ConsoleColor.DarkBlue, true, player.XPos, player.CmdYPos);

            switch (GetPlayerChoice(player))
            {
                case 'y':
                    player.DrawCard(deck);
                    break;
                case 'n':
                    break;
            }
        }

        private char GetPlayerChoice(Player player)
        {
            ConsoleKeyInfo playerInput;
            int yPos = 2;

            while (true)
            {
                playerInput = Console.ReadKey(true);

                switch (playerInput.KeyChar)
                {
                    case 'y':
                    case 'n':
                        return playerInput.KeyChar;
                    default:
                        Custom.WriteInColor("Unknown Command.", ConsoleColor.Red, true, player.XPos, yPos);
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

        public void GotBlackjack()
        {

        }

        public void RenderCard(char suit, Value cardValue, int xPos, int yPos)
        {
            var suitColors = new Dictionary<Suit, ConsoleColor>()
            {
                { Suit.Hearts, ConsoleColor.Red },
                { Suit.Diamonds, ConsoleColor.Red },
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

        public int CmdYPos;
        private int _firstCardsRowYPos;
        private int _notificationsYPos;

        public int XPos { get; private set; }
        public string Name { get; private set; }
        public bool IsAlive { get; private set; }
        public bool IsBust { get; private set; }
        public bool IsBlackjack { get; private set; }


        public Player(Deck deck, string name, int xPos)
        {
            hand.Add(deck.Cards.Pop());
            hand.Add(deck.Cards.Pop());
            Name = name;
            XPos = xPos;
            IsBlackjack = false;
            IsBust = false;
            IsAlive = true;

            CmdYPos = 0;
            _firstCardsRowYPos = 0;
            _notificationsYPos = 24;

        }

        public void CheckBlackjack()
        {
            if (GetHandSumValue() == BlackjackGame.BlackjackNum)
            {
                IsBlackjack = true;
                Custom.WriteFilled($"{Name} just got blackjack!", ConsoleColor.Yellow, true, XPos, _notificationsYPos);
                Custom.PressAnything(true, XPos, _notificationsYPos);
            }
        }
        public void CheckIfBust()
        {
            if (GetHandSumValue() > BlackjackGame.BlackjackNum)
            {
                IsBust = true;
                Custom.WriteFilled($"{Name} bust!", ConsoleColor.DarkRed, true, XPos, _notificationsYPos);
                Custom.PressAnything(true, XPos, _notificationsYPos);
            }
        }

        public void CheckIfAlive()
        {
            CheckIfBust();
            CheckBlackjack();

            if (IsBlackjack || IsBust)
            {
                IsAlive = false;
            }
        }

        public void DrawCard(Deck deck)
        {
            hand.Add(deck.Cards.Pop());
        }

        public void RenderHand(ConsoleColor playerColor = ConsoleColor.Gray, int maxCardsInTheRow = 4)
        {
            int cardsInTheRow = 0;
            int currentCardXPos = XPos;
            int currentCardYPos = _firstCardsRowYPos;

            Console.SetCursorPosition(currentCardXPos, currentCardYPos);

            foreach (Card card in hand)
            {
                card.ShowCardInfo(currentCardXPos, currentCardYPos + 3);
                currentCardXPos += 12;
                cardsInTheRow++;

                if (cardsInTheRow == maxCardsInTheRow)
                {
                    currentCardXPos = XPos;
                    currentCardYPos += 8;
                    cardsInTheRow = 0;
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
    public static void WriteInColor(string text, ConsoleColor color = ConsoleColor.DarkRed, bool customPos = false, int xPos = 0, int YPos = 0)
    {
        if (customPos)
            Console.SetCursorPosition(xPos, YPos);

        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    public static void WriteFilled(string text, ConsoleColor color = ConsoleColor.DarkYellow, bool customPos = false, int xPos = 0, int YPos = 0)
    {
        if (customPos)
            Console.SetCursorPosition(xPos, YPos);

        Console.BackgroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    public static void PressAnything(bool customPos = false, int xPos = 0, int YPos = 0, string text = "\npress anything to continue")
    {
        if (customPos)
            Console.SetCursorPosition(xPos, YPos);

        Custom.WriteInColor(text, ConsoleColor.DarkGray);
        Console.ReadKey();
        Console.Clear();
    }
}
