using System;
using System.Collections.Generic;

namespace CsRealLearning
{
    //money not working (not adding when people using pc)
    //timer on computer not working (when in use time is not going away)
    //learn how to cycle dialogue
    internal class Program
    {
        static void Main()
        {
            ComputerClub daClub = new ComputerClub(5, 30);
            daClub.Start();
        }
    }

    class ComputerClub
    {
        public static Random rnd = new Random();

        private int _electricityCost;

        public bool IsOpen { get; private set; }

        public int WorkMinutes { get; private set; }

        public int Money { get; private set; }

        private List<Computer> _computers = new List<Computer>();
        private Queue<Client> _clients = new Queue<Client>();


        public ComputerClub(int computersAmount, int workMinutes)
        {
            Money = 0;
            IsOpen = true;
            _electricityCost = computersAmount;
            WorkMinutes = workMinutes;

            for (int i = 0; i < computersAmount; i++)
            {
                _computers.Add(new Computer());
            }
        }

        public void Start()
        {
            while (IsOpen)
            {
                if (_clients.Count > 0)
                {
                    ShowInfo();

                    Client nextClient = _clients.Dequeue();
                    Console.WriteLine("\nSomeone approaches you");
                    nextClient.AskForComputer();
                    Console.WriteLine("Select a computer for this dude:");

                    string userInputString;
                    Computer selectedComputer;
                    userInputString = Console.ReadLine();

                    if (int.TryParse(userInputString, out int userInput) && userInput > 0 && userInput <= _computers.Count)
                    {
                        selectedComputer = _computers[userInput - 1];
                        selectedComputer.Assign(nextClient.RequiredMinutes, userInput);
                    }
                    else if (int.TryParse(userInputString, out userInput))
                    {
                        Console.WriteLine($"\nI mean that's cool and all, but there is no computer {userInput}");
                        Console.WriteLine($"You think about it.");
                        Console.WriteLine($"You think about life.");
                        Console.WriteLine($"You think about what is happening to us.");
                        Console.WriteLine($"You think about where did we go wrong.");
                        Console.WriteLine($"The dude's left");




                    }
                    else
                    {
                        Console.WriteLine("\nAre you alright, administrator? Do you want me to call an ambulance?");
                        Console.WriteLine($"The Same Random Dude: ... I better be on my way...");
                        Console.WriteLine("He runs away");


                    }


                    ForwardMinute();
                }
                else
                {
                    ShowInfo();
                    Console.WriteLine("Seems like knowbody's here wants your computers");
                    Promote(rnd.Next(1, 6));

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

        private void Promote(int people)
        {
            for (int i = 0; i < people; i++)
            {
                _clients.Enqueue(new Client());
            }

            Console.WriteLine($"You promote your club. {people} dudes heard your promotion");
        }

        public void ForwardMinute()
        {
            Console.WriteLine();
            Console.WriteLine("Press Anyhing to Forward one Minute");
            Console.ReadKey();
            Console.Clear();

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
                    Console.WriteLine($"DaClub is closing. Everybody GET OUT. You own ${Money}. You decided to sold youself to slavery to an african pirates.");
                    Console.WriteLine("Press anything to Continue");
                    Console.ReadKey();
                    Console.Clear();
                    Console.WriteLine("1 year later");
                    Console.ReadKey();
                    Console.Clear();
                    Console.WriteLine("They feed you, they allow you to look at the sky (unlike the club job). They even let you hold their gun sometimes.\nLife is not bad!");
                    Console.ReadKey();
                    Console.Clear();
                    Console.WriteLine("The End");
                }
                else
                {
                    Console.WriteLine($"DaClub is closing. Everybody GET OUT. You made: ${Money}. Congratilations. Could have been better");
                }
                IsOpen = false;
            }
        }
    }

    class Computer
    {
        public static Random rnd = new Random();

        public int DollarPerMinute { get; private set; }

        public int MinutesRemaining { get; private set; }


        public bool IsOccupied
        {
            get { return (MinutesRemaining > 0); }
            private set { }
        }

        public Computer()
        {
            DollarPerMinute = rnd.Next(1, 10);
            MinutesRemaining = 0;
            IsOccupied = false;
        }

        public void ForwardMinute()
        {
            int minute = 1;

            if (MinutesRemaining > 0)
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
                Console.WriteLine($"Computer is occupied. Time Remaining: {MinutesRemaining}");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Computer is available! Price per minute: ${DollarPerMinute}");
                Console.ResetColor();
            }
        }

        public void AssignArchive()
        {
            if (!IsOccupied)
            {
                IsOccupied = true;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Computer occupied by this random dude for {MinutesRemaining}");
                ForwardMinute();
                Console.ResetColor();

            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error. Computer is already occupied, administrator. If that's even your real name. Dude is left");
                Console.ResetColor();
            }
        }
    }

    class Client
    {
        public static Random rnd = new Random();

        private int _money;
        private int _speach;

        public int RequiredMinutes { get; private set; }

        public Client()
        {
            _speach = rnd.Next(1, 10);
            _money = rnd.Next(20, 201);
            RequiredMinutes = rnd.Next(5, 20);
        }

        public void TryToPay(int price)
        {
            if (price > _money)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"The guy paid you {price}. GG");
                Console.ResetColor();
                Pay(price);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"The guy is cheking his wallet... He is a loser that should get a job. You told him to get out");
                Console.ResetColor();
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
                    Console.WriteLine($"Random Dude: Hey. Nice to meet you, Nia. I need to find something in the internet. I think {RequiredMinutes} minutes is what i need?");
                    break;
                case 5:
                    Console.WriteLine($"Random Dude: Look, I aint' got no time for your pretty face [he said with new york accent]. I need it for how long I need it");
                    break;
                case 6:
                    Console.WriteLine($"Random Dude: Cool place you got here. I like the decorations. I would like to spend here {RequiredMinutes} minutes");
                    break;
                case 7:
                    Console.WriteLine($"Random Dude: Have you seen that video in the internet. It just {RequiredMinutes} minutes. Do you want to see it together");
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
