using System;
using System.Collections.Generic;
using System.Security.Policy;
using static CsRealLearning.Card;

//Есть колода с картами. Игрок достает карты, пока не решит, что ему хватит карт
//(может быть как выбор пользователя, так и количество сколько карт надо взять).
//После выводиться вся информация о вытянутых картах.
//Возможные классы: Карта, Колода, Игрок.

//todo: 
//      - 
//      - how dealer takes money from player to the pot if player's money and the pot private
//      - money betting
//      - Double Down: (Available on initial two cards only) Double the bet, receive one more card, and automatically stand.
//      - custom player names

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
        Renderer renderer = new Renderer();
        Dealer dealer = new Dealer();
        Player player1 = new Player("Player 1", 0);
        Player player2 = new Player("Player 2", 60);

        private bool _isRoundOn = false;
        private int _round = 0;
        static public int Bet { get; private set; }

        static public int BlackjackNum { get; private set; }

        public BlackjackGame()
        {
            BlackjackNum = 21;
        }

        public void Start()
        {
            Console.CursorVisible = false;


            while (player1.Money > 0 && player2.Money > 0)
            {
                RoundStarts();
                Custom.WriteAtPosition(50, 16, $"{_round}");

                if (_round % 2 != 0)
                {
                    if (!player2.IsBust)
                        Turn(player1, player2);

                    if (!player1.IsBust)
                        Turn(player2, player1);
                }
                else
                {
                    if (!player1.IsBust)
                        Turn(player2, player1);

                    if (!player2.IsBust)
                        Turn(player1, player2);
                }

                CheckIfOver();
            }
        }

        private void Turn(Player player, Player opponent)
        {
            if (player.IsPlaying)
            {
                renderer.RenderTable(player, opponent);
                HandleInput(player);
                renderer.RenderTable(player, opponent);
                player.CheckIfAlive();
            }
        }

        private void RoundStarts()
        {
            if (!_isRoundOn)
            {
                _round++;
                _isRoundOn = true;
                player1.IsPlaying = true;
                player2.IsPlaying = true;
                player1.IsBust = false;
                player2.IsBust = false;
                dealer.CollectCardsFromPlayer(player1);
                dealer.CollectCardsFromPlayer(player2);
                dealer.Shuffle();
                dealer.DealStartingHands(player1, player2);
            }
        }

        private void CheckIfOver()
        {
            if (player1.IsBust || player2.IsBust)
                _isRoundOn = false;

            if (!player1.IsPlaying && !player2.IsPlaying)
                _isRoundOn = false;

            if (!_isRoundOn)
            {
                if (player1.IsBust || player2.IsBust)
                {
                    if (player1.IsBust)
                    {
                        dealer.GivePotToPlayer(player2);
                        renderer.RenderTable(player1, player2);
                        Custom.WriteFilled($"{player2.Name} is victorius because {player1.Name} can't count to 21", ConsoleColor.DarkYellow, true, player2.XPos);
                        Custom.PressAnythingToContinue(ConsoleColor.DarkYellow, true, player2.XPos, player2.CmdYPos + 2);
                    }

                    if (player2.IsBust)
                    {
                        dealer.GivePotToPlayer(player1);
                        renderer.RenderTable(player1, player2);
                        Custom.WriteFilled($"{player1.Name} is victorius because {player2.Name} can't count to 21");
                        Custom.PressAnythingToContinue();
                    }
                }
                else if (player1.GetHandSumValue() > player2.GetHandSumValue())
                {
                    dealer.GivePotToPlayer(player2);
                    renderer.RenderTable(player1, player2);
                    Custom.WriteFilled($"{player1.Name} is victorius with {player1.GetHandSumValue()}");
                    Custom.PressAnythingToContinue();
                }
                else if (player2.GetHandSumValue() > player1.GetHandSumValue())
                {
                    dealer.GivePotToPlayer(player2);
                    renderer.RenderTable(player1, player2);
                    Custom.WriteFilled($"{player2.Name} is victorius with {player2.GetHandSumValue()}", ConsoleColor.DarkYellow, true, player2.XPos);
                    Custom.PressAnythingToContinue(ConsoleColor.DarkYellow, true, player2.XPos, player2.CmdYPos + 2);
                }
                else if (player1.GetHandSumValue() == player2.GetHandSumValue())
                {
                    Custom.WriteFilled($"     Standoff with {player1.GetHandSumValue()}     ", ConsoleColor.DarkYellow, true, 27);
                    Custom.PressAnythingToContinue(ConsoleColor.DarkYellow, true, 27, player1.CmdYPos + 2);
                }

            }

        }

        private void HandleInput(Player player)
        {
            Custom.WriteInColor($"--- {player.Name} ({player.GetHandSumValue()} (${player.Money})) Turn ---", ConsoleColor.Cyan, true, player.XPos, 0);
            Custom.WriteInColor("Commands:", ConsoleColor.Cyan, true, player.XPos, player.CmdYPos);
            Custom.WriteInColor("[h] Hit", ConsoleColor.Cyan, true, player.XPos, player.CmdYPos + 1);
            Custom.WriteInColor("[s] Stand", ConsoleColor.Cyan, true, player.XPos, player.CmdYPos + 2);

            switch (GetPlayerChoice(player))
            {
                case 'h':
                    dealer.DealCardToPlayer(player);
                    break;
                case 's':
                    player.IsPlaying = false;
                    break;
            }

            Console.Clear();
        }

        private char GetPlayerChoice(Player player)
        {
            ConsoleKeyInfo playerInput;

            while (true)
            {
                playerInput = Console.ReadKey(true);

                switch (playerInput.KeyChar)
                {
                    case 'h':
                    case 's':
                        return playerInput.KeyChar;
                    default:
                        Custom.WriteInColor("Unknown Command. Try again:", ConsoleColor.Red, true, player.XPos, player.CmdYPos);
                        break;
                }
            }
        }
    }


    class Renderer
    {
        public void RenderCard(char suitChar, Value cardValue, int xPos, int yPos)
        {
            var cardSuitIcons = new Dictionary<char, ConsoleColor>()
            {
                { '♥', ConsoleColor.Red },
                { '♦', ConsoleColor.Red },
                { '♣', ConsoleColor.DarkGray },
                { '♠', ConsoleColor.DarkGray },
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


            Console.ForegroundColor = cardSuitIcons[suitChar];
            Console.SetCursorPosition(xPos, yPos);
            Console.WriteLine(topBorder);
            Console.SetCursorPosition(xPos, ++yPos);
            Console.WriteLine($"║{cardValueIcons[cardValue],-8} ║");
            Console.SetCursorPosition(xPos, ++yPos);
            Console.WriteLine($"║{suitChar,-8} ║");
            Console.SetCursorPosition(xPos, ++yPos);
            Console.WriteLine(wall);
            Console.SetCursorPosition(xPos, ++yPos);
            Console.WriteLine(wall);
            Console.SetCursorPosition(xPos, ++yPos);
            Console.WriteLine($"║{' ',8}{suitChar}║");
            Console.SetCursorPosition(xPos, ++yPos);

            if (cardValueIcons[cardValue] != "10")
                Console.WriteLine($"║{' ',8}{cardValueIcons[cardValue]}║");
            else
                Console.WriteLine($"║{' ',7}{cardValueIcons[cardValue]}║");

            Console.SetCursorPosition(xPos, ++yPos);
            Console.WriteLine(bottomBorder);
            Console.ResetColor();
        }

        public void RenderHand(Player player, int maxCardsInTheRow = 4)
        {
            int cardsInTheRow = 0;
            int currentCardXPos = player.XPos;
            int currentCardYPos = player.FirstRowCardsYPos;

            Console.SetCursorPosition(currentCardXPos, currentCardYPos);

            foreach (Card card in player.Hand)
            {
                card.DisplayCard(currentCardXPos, currentCardYPos);
                currentCardXPos += 12;
                cardsInTheRow++;

                if (cardsInTheRow == maxCardsInTheRow)
                {
                    currentCardXPos = player.XPos;
                    currentCardYPos += 8;
                    cardsInTheRow = 0;
                }
            }
        }

        public void RenderTable(Player player1, Player player2)
        {
            int player1XPos = player1.XPos;
            int player1YPos = 0;
            int player2XPos = player2.XPos;
            int player2YPos = 0;

            Custom.WriteInColor($"{player1.Name} ({player1.GetHandSumValue()}) (${player1.Money})", ConsoleColor.DarkGray, true, player1XPos, player1YPos);
            RenderHand(player1);

            Custom.WriteInColor($"{player2.Name} ({player2.GetHandSumValue()}) (${player2.Money})", ConsoleColor.DarkGray, true, player2XPos, player2YPos);
            RenderHand(player2);
        }
    }

    class Dealer
    {
        private Stack<Card> _deck52 = new Stack<Card>();

        public int Pot { get; private set; }

        public Dealer()
        {
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Value value in Enum.GetValues(typeof(Value)))
                {
                    _deck52.Push(new Card(suit, value));
                }
            }
        }

        public void Shuffle()
        {
            var tempDeck = new List<Card>(_deck52);
            Random rnd = new Random();

            for (int i = tempDeck.Count - 1; i > 0; i--)
            {
                int randomIndex = rnd.Next(0, i + 1);

                Card tempCard = tempDeck[i];
                tempDeck[i] = tempDeck[randomIndex];
                tempDeck[randomIndex] = tempCard;
            }

            _deck52.Clear();
            foreach (var card in tempDeck)
            {
                _deck52.Push(card);
            }
        }


        public void DealCardToPlayer(Player player)
        {
            player.Hand.Add(_deck52.Pop());
        }

        public void DealStartingHands(Player player1, Player player2)
        {
            int startingHandNum = 2;

            for (int i = 0; i < startingHandNum; i++)
                player1.Hand.Add(_deck52.Pop());

            for (int i = 0; i < startingHandNum; i++)
                player2.Hand.Add(_deck52.Pop());
        }

        public void CollectCardsFromPlayer(Player player)
        {
            foreach (Card card in player.Hand)
            {
                _deck52.Push(card);
            }

            player.Hand.Clear();
        }             

        public void GivePotToPlayer(Player player)
        {

        }

        private void ShowDeckInfo()
        {
            int xPos = 0;
            int yPos = 1;

            foreach (Card card in _deck52)
            {
                card.DisplayCard(xPos, yPos);
                xPos += 16;
            }
        }
    }

    class Card
    {
        Renderer renderer = new Renderer();
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

        public enum Suit
        {
            Hearts = 1,
            Diamonds,
            Clubs,
            Spades
        }

        public enum Value { Ace = 1, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King }

        public void DisplayCard(int xPos, int yPos)
        {
            char suitSym = '.';
            switch (CardSuit)
            {
                case Suit.Hearts:
                    suitSym = '♥';
                    break;
                case Suit.Diamonds:
                    suitSym = '♦';
                    break;
                case Suit.Clubs:
                    suitSym = '♣';
                    break;
                case Suit.Spades:
                    suitSym = '♠';
                    break;
            }

            renderer.RenderCard(suitSym, CardValue, xPos, yPos);
        }
    }

    class Player
    {
        public List<Card> Hand = new List<Card>();

        public int CmdYPos;
        public int FirstRowCardsYPos;
        public int NotificationsYPos;
        public bool IsPlaying;
        public bool IsBust;

        public int XPos { get; private set; }
        public int Money { get; private set; }
        public string Name { get; private set; }
        public bool IsBlackjack { get; private set; }


        public Player(string name, int xPos)
        {
            Name = name;
            XPos = xPos;
            IsBlackjack = false;
            IsBust = false;
            IsPlaying = true;

            CmdYPos = 1;
            FirstRowCardsYPos = 4;
            NotificationsYPos = 0;
            Money = 500;

        }

        public void CheckBlackjack()
        {
            if (GetHandSumValue() == BlackjackGame.BlackjackNum)
            {
                IsBlackjack = true;
                Custom.WriteFilled($"    {Name} just got blackjack!    ", ConsoleColor.DarkYellow, true, XPos, NotificationsYPos);
                Custom.PressAnythingToContinue(ConsoleColor.DarkGray, true, XPos, NotificationsYPos + 3);
            }
        }

        public void CheckIfBust()
        {
            if (GetHandSumValue() > BlackjackGame.BlackjackNum)
            {
                IsBust = true;
                Custom.WriteFilled($"          {Name} bust!           ", ConsoleColor.DarkRed, true, XPos, NotificationsYPos);
                Custom.PressAnythingToContinue(ConsoleColor.DarkGray, true, XPos, NotificationsYPos + 3);
            }
        }

        public void CheckIfAlive()
        {
            CheckIfBust();
            CheckBlackjack();

            if (IsBlackjack || IsBust)
            {
                IsPlaying = false;
            }
        }

        public int GetHandSumValue()
        {
            int sumHandValue = 0;
            bool hasAce = false;

            foreach (Card card in Hand)
            {
                sumHandValue += card.BlackjackValue;

                if (card.CardValue == Value.Ace)
                    hasAce = true;
            }

            if (hasAce && sumHandValue + 10 <= 21)
                sumHandValue += 10;

            return sumHandValue;
        }

        public void MakeBet(Dealer dealer)
        {
            if (Money > BlackjackGame.Bet)
            {
                Money -= BlackjackGame.Bet;
                dealer.Pot += BlackjackGame.Bet;
            }
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

    public static void PressAnythingToContinue(ConsoleColor color = ConsoleColor.DarkYellow, bool customPos = false, int xPos = 0, int YPos = 0, string text = "press anything to continue")
    {
        if (customPos)
            Console.SetCursorPosition(xPos, YPos);

        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
        Console.ReadKey();
        Console.Clear();
    }

    public static void WriteFilled(string text, ConsoleColor color = ConsoleColor.DarkYellow, bool customPos = false, int xPos = 0, int yPos = 0)
    {
        int borderLength = text.Length + 2;
        string filler = new string('═', borderLength);
        string topBorder = "╔" + filler + "╗";
        string line = $"║ {text} ║";
        string bottomBorder = "╚" + filler + "╝";

        Console.ForegroundColor = ConsoleColor.Black;
        Console.BackgroundColor = color;

        WriteAtPosition(xPos, yPos, topBorder);
        WriteAtPosition(xPos, yPos + 1, line);
        WriteAtPosition(xPos, yPos + 2, bottomBorder);

        Console.ResetColor();
    }

    public static void WriteAtPosition(int xPos, int yPos, string text)
    {
        Console.SetCursorPosition(xPos, yPos);
        Console.WriteLine(text);
    }
}
