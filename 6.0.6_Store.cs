using System;
using System.Collections.Generic;

//Существует продавец, он имеет у себя список товаров, и при нужде, может вам его показать, 
//также продавец может продать вам товар. После продажи товар переходит к вам, и вы можете также посмотреть свои вещи.
//Возможные классы – игрок, продавец, товар.

//todo: 
//      -
//      -
//      - 

namespace CsRealLearning
{
    internal class Program
    {
        static void Main()
        {
            Game game = new Game();
            game.Start();
        }

        class Game
        {
            private Merchant _reMerchant = new Merchant();
            private Player _leon = new Player();
            private bool _isShopping = true;

            enum PlayerInput
            {
                Buy = '1',
                OpenInventory = '2',
                Close = '3',
            }

            public void Start()
            {
                _reMerchant.OpenStore();

                while (_isShopping)
                {
                    ShowCommands();
                    HandleInput(out _isShopping);
                    Console.Clear();
                }

                _reMerchant.CloseStore();
            }

            public void HandleInput(out bool isBuying)
            {
                isBuying = true;
                char playerInput = Console.ReadKey(true).KeyChar;

                switch (playerInput)
                {
                    case (char)PlayerInput.Buy:
                        CompleteTransaction();
                        break;
                    case (char)PlayerInput.OpenInventory:
                        _leon.OpenInventory();
                        break;
                    case (char)PlayerInput.Close:
                        Close();
                        break;
                    default:
                        break;
                }
            }

            public void ShowCommands()
            {
                Console.WriteLine("Got a selection of good things on sale, stranger");
                Console.WriteLine($"{(char)PlayerInput.Buy}. {PlayerInput.Buy}");
                Console.WriteLine($"{(char)PlayerInput.OpenInventory}. {PlayerInput.OpenInventory}");
                Console.WriteLine($"{(char)PlayerInput.Close}. {PlayerInput.Close}");
            }

            public void Close()
            {
                _isShopping = false;
            }

            public void CompleteTransaction()
            {
                if (_reMerchant.AtemptToSell(out Item soldItem))
                    _leon.ReceiveItem(ref soldItem);
            }
        }

        class Merchant
        {
            private readonly List<Item> _items;

            public Merchant()
            {
                _items = new List<Item>();
                FillStore();
            }

            public void ShowGoods()
            {
                int count = 0;

                Console.Clear();
                Console.WriteLine("What are you buying?");

                foreach (Item item in _items)
                {
                    Console.Write("\n" + ++count + ".");
                    item.ShowInfo();

                }
            }

            public bool AtemptToSell(out Item soldItem)
            {
                ShowGoods();

                if (Int32.TryParse(Console.ReadLine(), out int playerInput) && _items.Count >= playerInput && playerInput > 0)
                {
                    soldItem = _items[playerInput - 1];
                    _items.RemoveAt(playerInput - 1);


                    Console.Clear();
                    Console.WriteLine($"Hehehe, Thank you (you bought {soldItem.Name})");
                    Custom.PressAnythingToContinue();
                    return true;
                }
                else
                {
                    soldItem = null;
                    Console.Clear();
                    Console.WriteLine("Sorry, stranger. I don't have it");
                    Custom.PressAnythingToContinue();
                    return false;
                }
            }

            public void FillStore()
            {
                _items.AddRange(new Item[]
                {
                    new Item("Red9", 14000, "A powerful 9mm handgun"),
                    new Item("Shotgun", 20000, "A 12-gauge pump-action shotgun. Don't leave home without it"),
                    new Item("First Aid Spray", 1000, "A healing spray that will restore you to full health"),
                    new Item("Green Herb", 500, "Restores some health"),
                    new Item("Hand Grenade", 2000, "A handy explosive that will detonate several seconds after throwing it")

            });
            }

            public void OpenStore()
            {
                Console.WriteLine("Hello, stranger");
            }

            public void CloseStore()
            {
                Console.WriteLine("Come back any time");
            }
        }

        class Item
        {
            public string Name { get; private set; }
            public int Price { get; private set; }
            public string Description { get; private set; }

            public Item(string name, int price, string description)
            {
                Name = name;
                Price = price;
                Description = description;
            }
            public void ShowInfo()
            {
                Console.WriteLine($"\n{Name}\n{Description}.\nPrice: {Price} ptas.");
            }
        }

        class Player
        {
            private readonly List<Item> _items;

            public Player()
            {
                _items = new List<Item>();
            }

            public void OpenInventory()
            {
                Console.Clear();
                Console.WriteLine("INVENTORY");

                if (_items.Count > 0)
                {
                    foreach (Item item in _items)
                    {
                        item.ShowInfo();
                    }
                }
                else
                {
                    Console.WriteLine("such empty");
                }

                Custom.PressAnythingToContinue();
            }

            public void ReceiveItem(ref Item soldItem)
            {
                _items.Add(soldItem);
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
}
