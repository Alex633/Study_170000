using System;
using System.Collections.Generic;
using static CsRealLearning.Card;

//Есть колода с картами. Игрок достает карты, пока не решит, что ему хватит карт
//(может быть как выбор пользователя, так и количество сколько карт надо взять).
//После выводиться вся информация о вытянутых картах.
//Возможные классы: Карта, Колода, Игрок.

namespace CsRealLearning
{
    internal class Program
    {
        static void Main()
        {
            Deck deck = new Deck();
            deck.ShowInfo();
        }
    }

    class Deck
    {
        List<Card> cards = new List<Card>();


        public Deck()
        {
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Value value in Enum.GetValues(typeof(Value)))
                {
                    cards.Add(new Card(suit, value));
                }
            }
        }

        public void ShowInfo()
        {
            foreach (Card card in cards)
            {
                card.ShowInfo();
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

        public void ShowInfo()
        {
            switch (CardSuit)
            {
                case Suit.Hearts:
                    NewConsole.WriteLine($" ---------", ConsoleColor.DarkRed);
                    NewConsole.WriteLine($"[ ♥ {CardValue, - 5} ]", ConsoleColor.DarkRed);
                    NewConsole.WriteLine($" ---------", ConsoleColor.DarkRed);

                    break;
                case Suit.Diamonds:
                    NewConsole.WriteLine($" ---------", ConsoleColor.DarkRed);
                    NewConsole.WriteLine($"[ ♦ {CardValue,-5} ]", ConsoleColor.DarkRed);
                    NewConsole.WriteLine($" ---------", ConsoleColor.DarkRed);

                    break;
                case Suit.Clubs:
                    NewConsole.WriteLine($" ---------", ConsoleColor.DarkGray);
                    NewConsole.WriteLine($"[ ♣ {CardValue,-5} ]", ConsoleColor.DarkGray);
                    NewConsole.WriteLine($" ---------", ConsoleColor.DarkGray);
                    break;
                case Suit.Spades:
                    NewConsole.WriteLine($" ---------", ConsoleColor.DarkGray);
                    NewConsole.WriteLine($"[ ♠ {CardValue,-5} ]", ConsoleColor.DarkGray);
                    NewConsole.WriteLine($" ---------", ConsoleColor.DarkGray);
                    break;
            }
        }
    }





    class Player
    {

    }
}

class NewConsole
{
    public static void WriteLine(string style, ConsoleColor color = ConsoleColor.Red)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(style);
        Console.ResetColor();
    }

    public static void PressAnything(string text = "\npress anything to continue")
    {
        NewConsole.WriteLine(text, ConsoleColor.DarkYellow);
        Console.ReadKey();
        Console.Clear();
    }
}
