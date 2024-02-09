using System;
using System.Collections.Generic;

namespace CsRealLearning
{
    //money calculations

    internal class Program
    {
        public static Random rnd { get; } = new Random();

        static void Main()
        {
            ComputerClub daClub = new ComputerClub(rnd.Next(8, 12), rnd.Next(10, 30));
            daClub.Start();
        }
    }

    class ComputerClub
    {
        public static Random rnd = new Random();

        private int _electricityCost;
        private bool _isThereFreePcs;
        private int _freePcsCount;

        public bool IsOpen { get; private set; }

        public int WorkMinutes { get; private set; }

        public int Money { get; private set; }

        private readonly List<Computer> _computers = new List<Computer>();
        private readonly Queue<Client> _clients = new Queue<Client>();


        public ComputerClub(int computersAmount, int workMinutes)
        {
            Money = 0;
            IsOpen = true;
            _electricityCost = computersAmount * 2;
            WorkMinutes = workMinutes;

            for (int i = 0; i < computersAmount; i++)
            {
                _computers.Add(new Computer());
            }
        }

        public void Start()
        {
            Promote(rnd.Next(1, 4));
            Console.Clear();

            while (IsOpen)
            {
                if (_clients.Count > 0)
                {
                    foreach (Computer computer in _computers)
                    {
                        _isThereFreePcs = !computer.IsOccupied;
                        if (_isThereFreePcs)
                            _freePcsCount++;
                    }

                    ShowInfo();

                    Client nextClient = _clients.Dequeue();
                    Console.WriteLine("\nSomeone approaches you");
                    nextClient.AskForComputer();

                    if (_freePcsCount > 0)
                        Console.WriteLine("Select a computer for this dude:");
                    else
                        Console.WriteLine("You wished you could select a computer for this dude but there aren't any. Well, that's akward");

                    string userInputString;
                    Computer selectedComputer;
                    userInputString = Console.ReadLine();

                    if (int.TryParse(userInputString, out int userInput) && userInput > 0 && userInput <= _computers.Count)
                    {
                        selectedComputer = _computers[userInput - 1];

                        if (nextClient.CanPay(nextClient.RequiredMinutes * selectedComputer.DollarsPerMinute))
                        {
                            AcceptMoney(nextClient.RequiredMinutes * selectedComputer.DollarsPerMinute);
                            selectedComputer.Assign(nextClient.RequiredMinutes, userInput);

                        }
                    }
                    else if (int.TryParse(userInputString, out userInput))
                    {
                        Console.WriteLine($"\nI mean that's cool and all, but there is no computer {userInput}");
                        Console.WriteLine($"You think about it...");
                        Console.WriteLine($"You think about life...");
                        Console.WriteLine($"You think about what is happening to us...");
                        Console.WriteLine($"You think about where did we go wrong...");
                        Console.WriteLine($"The dude's left");
                    }
                    else
                    {
                        Console.WriteLine("\nAre you alright, administrator? Do you want me to call an ambulance?");
                        Console.WriteLine("The dude runs away");
                    }

                    ForwardMinute();
                }
                else
                {
                    ShowInfo();
                    Console.WriteLine("Seems like knowbody's here wants your computers");
                    Promote(rnd.Next(1, 7));
                    ForwardMinute();
                }
            }
        }

        private void ShowInfo()
        {
            int count = 0;

            Console.WriteLine($"Club DaClab");
            Console.WriteLine($"Open for {WorkMinutes} more minutes");

            if (_clients.Count == 1)
                Console.WriteLine($"In your waiting room just one single geek");
            else
                Console.WriteLine($"In your waiting room: {_clients.Count} geeks");

            if (Money == 0)
            {
                Console.WriteLine($"In your cash register: ${Money}\n");
            }
            else if (Money < 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"You own ${Money} to the government\n");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"In your cash register: ${Money}\n");
                Console.ResetColor();

            }

            Console.WriteLine();
            Console.WriteLine(new string('-', 56));
            Console.WriteLine();


            foreach (Computer computer in _computers)
            {
                Console.Write(++count + ". ");
                computer.ShowInfo();
            }

            Console.WriteLine();
            Console.WriteLine(new string('-', 56));
            Console.WriteLine();
        }

        private void Promote(int dudes)
        {
            for (int i = 0; i < dudes; i++)
            {
                _clients.Enqueue(new Client());
            }

            Console.WriteLine($"You promote your club. {dudes} dudes heard your promotion");
        }

        public void ForwardMinute()
        {
            Console.WriteLine();
            Console.WriteLine("Press Anyhing to Forward one Minute");
            Console.ReadKey();
            Console.Clear();

            foreach (Computer computer in _computers)
            {
                computer.TimerPlusMinute();
            }

            if (WorkMinutes > 0)
            {
                WorkMinutes--;
                Money -= _electricityCost;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"You paid ${_electricityCost} for the electricity\n");
                Console.ResetColor();
            }
            else
            {
                if (Money < 0)
                {
                    Console.Clear();
                    Console.WriteLine($"DaClub is closing. Everybody GET OUT. You own ${Money}. You decide to sold youself to slavery to an african pirates.");
                    Console.WriteLine("Press anything to continue");
                    Console.ReadKey();
                    Console.Clear();
                    Console.WriteLine("1 year later");
                    Console.ReadKey();
                    Console.Clear();
                    Console.WriteLine("They feed you, they let you hold their guns sometimes. They even allow you to look at the sky (unlike the club job). \nLife is not bad!");
                    Console.ReadKey();
                    Console.Clear();
                    Console.WriteLine("The End");
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine($"DaClub is closing. Everybody GET OUT. You made: ${Money}");
                    Console.ReadKey();
                    Console.Clear();
                    Console.WriteLine($"But what you wish is to be free with your African pirates buddies");
                    Console.ReadKey();
                    Console.Clear();
                    Console.WriteLine("The End");
                }
                IsOpen = false;
            }
        }

        public void AcceptMoney(int price)
        {
            Money += price;
        }
    }

    class Computer
    {
        public static Random rnd = new Random();

        public int DollarsPerMinute { get; private set; }

        public int MinutesRemaining { get; private set; }


        public bool IsOccupied
        {
            get { return (MinutesRemaining > 0); }
            private set { }
        }

        public Computer()
        {
            DollarsPerMinute = rnd.Next(1, 11);
            MinutesRemaining = 0;
            IsOccupied = false;
        }

        public void TimerPlusMinute()
        {
            int minute = 1;

            if (MinutesRemaining == 1)
            {
                MinutesRemaining -= minute;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Some random dude is left your club. The computer is now free");
                Console.ResetColor();
            }
            else if (MinutesRemaining > 1)
            {
                MinutesRemaining -= minute;
            }
        }


        public void Assign(int durationInMinutes, int selectedComputerNumber)
        {
            if (!IsOccupied)
            {
                MinutesRemaining = durationInMinutes;
                Console.WriteLine($"Computer {selectedComputerNumber} got occupied by this random dude for {MinutesRemaining} minutes");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error. Computer {selectedComputerNumber} is already occupied, administrator. If that's even your real name. Dude is left");
                Console.ResetColor();
            }
        }

        public void ShowInfo()
        {
            if (IsOccupied)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{MinutesRemaining} minutes remaining . Computer is occupied.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"${DollarsPerMinute} per minute. Computer is available");
                Console.ResetColor();
            }
        }
    }

    class Client
    {
        public static Random rnd = new Random();

        private int _money;

        private static int _latestId = 0;
        private int _id;

        private int _speach => _id;

        public int RequiredMinutes { get; private set; }

        public Client()
        {
            if (_latestId != 9)
            {
                _id = ++_latestId;
            }
            else
            {
                _latestId = 0;
                _id = ++_latestId;
            }
            _money = rnd.Next(35, 66);
            RequiredMinutes = rnd.Next(3, 16);
        }

        public bool CanPay(int price)
        {
            if (price < _money)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"The dude paid you ${price}. GG");
                Console.ResetColor();
                Pay(price);
                return true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"The dude is cheking his wallet... He is a loser that should get a job. You told him to get out");
                Console.ResetColor();
                return false;
            }
        }

        private void Pay(int price)
        {
            _money -= price;
        }

        public void AskForComputer()
        {
            switch (_speach)
            {
                case 1:
                    Console.WriteLine($"Random Dude: Yo, what's up man. Got any spare PC for {RequiredMinutes} mins?");
                    break;
                case 2:
                    Console.WriteLine($"Random Dude: I'm Bill, but you can call me random dude. Can I use your PC for {RequiredMinutes} minutes?");
                    break;
                case 3:
                    Console.WriteLine($"Random Dude: It's a robbery. I rob you of {RequiredMinutes} minutes of one of your PC. I will pay you of course.");
                    break;
                case 4:
                    Console.WriteLine($"Random Dude: Hey. Nice to meet you, Nia. I need to find something in the internet. I think {RequiredMinutes} minutes is what i need");
                    break;
                case 5:
                    Console.WriteLine($"Random Dude: Look, I aint' got no time for your pretty face [he said with new york accent]. I need it for how long I need it");
                    break;
                case 6:
                    Console.WriteLine($"Random Dude: Cool place you got here. I like the decorations. I would like to spend here {RequiredMinutes} minutes");
                    break;
                case 7:
                    Console.WriteLine($"Random Dude: Have you seen that video in the internet. It just {RequiredMinutes} minutes. Do you want to see it together?");
                    break;
                case 8:
                    Console.WriteLine($"Random Dude: I think I am lost. I don't know how I got here. Where am I?");
                    break;
                case 9:
                    Console.WriteLine($"Random Dude: Do you have a counter strike here? I think I got enough money for {RequiredMinutes} minutes");
                    break;
            }
        }
    }
}
