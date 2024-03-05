using System;
using System.Collections.Generic;
using static CsRealLearning.Card;

//Есть колода с картами. Игрок достает карты, пока не решит, что ему хватит карт
//(может быть как выбор пользователя, так и количество сколько карт надо взять).
//После выводиться вся информация о вытянутых картах.
//Возможные классы: Карта, Колода, Игрок.

//todo: 
//      - Double Down: (Available on initial two cards only) Double the bet, receive one more card, and automatically stand.
//      - get rid of static method NamePlayer
//      - proper pot management (private set)

namespace CsRealLearning
{
    internal class Program
    {
        static void Main()
        {
            BlackjackGame game = new BlackjackGame();         

            Player player1 = new Player(NamePlayer(1), 0);
            Player player2 = new Player(NamePlayer(2), 60);

            game.Start(player1, player2);
        }

        public static string NamePlayer(int PlayerNum)
        {
            Console.WriteLine($"Player {PlayerNum}, enter you name");
            string userInput = Console.ReadLine();
            Console.Clear();
            return userInput;
        }
    }

    class BlackjackGame
    {
        Renderer renderer = new Renderer();
        Dealer dealer = new Dealer();

        private bool _isRoundOn = false;
        private int _round = 0;
        static public int BlackjackBet { get; private set; }

        static public int BlackjackNum { get; private set; }

        public BlackjackGame()
        {
            BlackjackNum = 21;
            BlackjackBet = 25;
        }

        public void Start(Player player1, Player player2)
        {
            Console.CursorVisible = false;

            while (player1.Money >= BlackjackBet && player2.Money >= BlackjackBet)
            {
                RoundStarts(player1, player2);
                Custom.WriteInColor($"Pot: ${dealer.Pot}", ConsoleColor.Green, true, 40, 0);

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

                CheckIfOver(player1, player2);
            }

            CongrazWinner(player1, player2);
        }

        private void CongrazWinner(Player player1, Player player2)
        {
            if (player1.Money > 25)
            {
                Custom.WriteFilled($"Gz {player1.Name}. You won everything", ConsoleColor.DarkMagenta);
            }
            else if (player2.Money > 25)
            {
                Custom.WriteFilled($"Gz {player2.Name}. You won everything", ConsoleColor.DarkMagenta);
            }
            else
            {
                Custom.WriteFilled("There is no winners. How did that happened?..", ConsoleColor.DarkRed);
            }

            Custom.PressAnythingToContinue(ConsoleColor.Magenta);
        }

        private void Turn(Player player, Player opponent)
        {
            if (player.IsPlaying)
            {
                renderer.RenderTable(player, opponent);
                HandleInput(player);
                renderer.RenderTable(player, opponent);
                player.UpdateAliveStatus();
            }
        }

        private void RoundStarts(Player player1, Player player2)
        {
            if (!_isRoundOn)
            {
                _round++;
                HandleBets(player1, player2);
                _isRoundOn = true;
                player1.IsPlaying = true;
                player2.IsPlaying = true;
                player2.IsBust = false;
                dealer.CollectCards(player1);
                dealer.CollectCards(player2);
                dealer.Shuffle();
                dealer.DealStartingHands(player1, player2);
            }
        }

        private void CheckIfOver(Player player1, Player player2)
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
                        renderer.RenderTable(player1, player2);
                        Custom.WriteFilled($"   {player2.Name} is victorius. Won: ${dealer.Pot}"   , ConsoleColor.DarkYellow, true, player2.XPos);
                        player2.CollectPot(dealer);
                        Custom.PressAnythingToContinue(ConsoleColor.DarkYellow, true, player2.XPos, player2.CmdYPos + 2);
                    }

                    if (player2.IsBust)
                    {
                        renderer.RenderTable(player1, player2);
                        Custom.WriteFilled($"   {player1.Name} is victorius. Won: ${dealer.Pot}   ");
                        player1.CollectPot(dealer);
                        Custom.PressAnythingToContinue();
                    }
                }
                else if (player1.GetHandSumValue() > player2.GetHandSumValue())
                {
                    renderer.RenderTable(player1, player2);
                    Custom.WriteFilled($"{player1.Name} is victorius with {player1.GetHandSumValue()}. Won: ${dealer.Pot}");
                    player1.CollectPot(dealer);
                    Custom.PressAnythingToContinue();
                }
                else if (player2.GetHandSumValue() > player1.GetHandSumValue())
                {
                    renderer.RenderTable(player1, player2);
                    Custom.WriteFilled($"{player2.Name} is victorius with {player2.GetHandSumValue()}. Won: ${dealer.Pot}", ConsoleColor.DarkYellow, true, player2.XPos);
                    player2.CollectPot(dealer);
                    Custom.PressAnythingToContinue(ConsoleColor.DarkYellow, true, player2.XPos, player2.CmdYPos + 2);
                }
                else if (player1.GetHandSumValue() == player2.GetHandSumValue())
                {
                    Custom.WriteFilled($"     Standoff with {player1.GetHandSumValue()}. Bets are returned     ", ConsoleColor.DarkYellow, true, 27);
                    ReturnBets(player1, player2);
                    Custom.PressAnythingToContinue(ConsoleColor.DarkYellow, true, 27, player1.CmdYPos + 2);

                }

            }

        }

        private void HandleInput(Player player)
        {
            Custom.WriteInColor($"--- {player.Name} (${player.Money})) Turn ---", ConsoleColor.Cyan, true, player.XPos, 0);
            Custom.WriteInColor("Commands:", ConsoleColor.Cyan, true, player.XPos, player.CmdYPos);
            Custom.WriteInColor("[h] Hit", ConsoleColor.Cyan, true, player.XPos, player.CmdYPos + 1);
            Custom.WriteInColor("[s] Stand", ConsoleColor.Cyan, true, player.XPos, player.CmdYPos + 2);

            switch (GetPlayerChoice(player))
            {
                case 'h':
                    dealer.DealCard(player);
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

        private bool HandleBets(Player player1, Player player2)
        {
            if (dealer.AttemptCollectBet(player1) && dealer.AttemptCollectBet(player2))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ReturnBets(Player player1, Player player2)
        {
            int halfOfPot;
            halfOfPot = dealer.Pot / 2;
            dealer.Pot = halfOfPot;
            player1.CollectPot(dealer);
            dealer.Pot = halfOfPot;
            player2.CollectPot(dealer);
        }

        //private void TransitionPot(Player player)
        //{
        //    dealer.MovePotToWinner();
        //    player.CollectPot(dealer);
        //}
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

            Custom.WriteInColor($"{player1.Name} (${player1.Money})", ConsoleColor.DarkGray, true, player1XPos, player1YPos);
            RenderHand(player1);

            Custom.WriteInColor($"{player2.Name} (${player2.Money})", ConsoleColor.DarkGray, true, player2XPos, player2YPos);
            RenderHand(player2);
        }
    }

    class Dealer
    {
        private Stack<Card> _deck52 = new Stack<Card>();

        public int Pot { get; set; }

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


        public void DealCard(Player player)
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

        public void CollectCards(Player player)
        {
            foreach (Card card in player.Hand)
            {
                _deck52.Push(card);
            }

            player.Hand.Clear();
        }

        public bool AttemptCollectBet(Player player)
        {
            if (player.AttemptBet())
            {
                Pot += BlackjackGame.BlackjackBet;
                return true;
            }
            else
            {
                return false;
            }
        }

        //public int MovePotToWinner()
        //{
        //    int potOnTheTable;
        //    potOnTheTable = Pot;
        //    Custom.WriteAtPosition(40, 12, $"Pot: ${Pot}");
        //    Custom.WriteAtPosition(40, 13, $"Pot on the table: ${potOnTheTable}");

        //    //Pot = 0;
        //    return potOnTheTable;
        //}

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

        public void UpdateBlackjackStatus()
        {
            if (GetHandSumValue() == BlackjackGame.BlackjackNum)
            {
                IsBlackjack = true;
                Custom.WriteFilled($"    {Name} just got blackjack!    ", ConsoleColor.DarkYellow, true, XPos, NotificationsYPos);
                Custom.PressAnythingToContinue(ConsoleColor.DarkGray, true, XPos, NotificationsYPos + 3);
            }
        }

        public void UpdateBustStatus()
        {
            if (GetHandSumValue() > BlackjackGame.BlackjackNum)
            {
                IsBust = true;
                Custom.WriteFilled($"          {Name} bust!           ", ConsoleColor.DarkRed, true, XPos, NotificationsYPos);
                Custom.PressAnythingToContinue(ConsoleColor.DarkGray, true, XPos, NotificationsYPos + 3);
            }
        }

        public void UpdateAliveStatus()
        {
            UpdateBustStatus();
            UpdateBlackjackStatus();

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

        public bool AttemptBet()
        {
            if (Money >= BlackjackGame.BlackjackBet)
            {
                Money -= BlackjackGame.BlackjackBet;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void CollectPot(Dealer dealer)
        {
            Money += dealer.Pot;
            dealer.Pot = 0;
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
