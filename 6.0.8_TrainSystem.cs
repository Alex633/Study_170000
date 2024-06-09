namespace millionDollarsCourses
{
    using System;
    using System.Collections.Generic;

    //У вас есть программа, которая помогает пользователю составить план поезда.
    //Есть 4 основных шага в создании плана:
    //-Создать направление - создает направление для поезда(к примеру Бийск - Барнаул)
    //-Продать билеты - вы получаете рандомное кол-во пассажиров, которые купили билеты на это направление
    //-Сформировать поезд - вы создаете поезд и добавляете ему столько вагонов(вагоны могут быть разные по вместительности), сколько хватит для перевозки всех пассажиров.
    //-Отправить поезд - вы отправляете поезд, после чего можете снова создать направление.
    //В верхней части программы должна выводиться полная информация о текущем рейсе или его отсутствии. 

    //todo: 
    //      make train naming after entering "creating a route"
    //      display hud info at the bottom
    //      - 

    internal class Program
    {
        static void Main()
        {
            TrainControlSystem trainControlSystem = new TrainControlSystem();
            trainControlSystem.Start();
        }

        class TrainControlSystem
        {
            private readonly List<Station> _stations = new List<Station>();
            private bool _isWorking = true;
            private Train _train;
            private Route _route;

            enum StationName
            {
                City17,
                BlackMesa,
                NovaProspekt,
                Citadel,
                Ravenholm,
                ApertureScience
            }

            public TrainControlSystem()
            {
                GetAvailableStations();
            }

            public void Start()
            {
                Console.WriteLine("Welcome to Combine Train Control System, soldier\n");

                while (_isWorking)
                {
                    DisplayCommands();
                    CreateTrain();
                    HandleInput();
                }

                Console.WriteLine("\nGlory to the Combine");
            }

            public void HandleInput()
            {
                string userInput;

                userInput = Console.ReadLine();

                switch (userInput)
                {
                    default:
                        DisplayError();
                        break;
                    case "1":
                        CreateRoute();
                        break;
                    case "2":
                        _isWorking = false;
                        break;
                }
            }

            public void DisplayCommands()
            {
                Console.WriteLine("Commands");
                Console.WriteLine($"1. Create a new route\n" +
                    $"2. Exit");
            }

            public void CreateRoute()
            {
                Console.Clear();
                _route = new Route();
                _route.SelectStationType("DEPARTURE", _stations);
                _route.SelectStationType("DESTINATION", _stations, true);
                Console.WriteLine($"Departure:{_route.DepartureStation.Name} Station\n" +
                    $"Destination: {_route.DestinationStation.Name} Station\n");
                Custom.PressAnythingToContinue();
            }

            public void CreateTrain()
            {
                Console.WriteLine("Train name:");
                _train = new Train(Console.ReadLine());
            }


            public void DisplayError()
            {
                Custom.WriteInColor("\nUnknown Command. Try again:", ConsoleColor.Red);
                Custom.PressAnythingToContinue();
            }

            public void GetPassengerTickets()
            {

            }

            public void GetAvailableStations()
            {
                foreach (StationName stationName in Enum.GetValues(typeof(StationName)))
                {
                    _stations.Add(new Station(stationName.ToString()));
                }
            }

            public void DisplayStations()
            {
                int count = 0;
                foreach (Station station in _stations)
                {
                    Console.Write(++count + " ");
                    station.ShowInfo();
                }
            }

            public void SellTickets()
            {

            }
        }

        class Route
        {
            public Station DepartureStation;
            public Station DestinationStation;

            public Route() { }

            public void SelectStationType(string stationType, List<Station> stations, bool isDestination = false)
            {
                bool isValidStation = false;
                int userInput;
                Station station;

                while (!isValidStation)
                {
                    Console.Clear();
                    string message = $"{stationType} station:\n";
                    userInput = Custom.GetUserNumberInRange(message, stations.Count);
                    station = stations[userInput];

                    if (isDestination)
                    {
                        if (station == DepartureStation)
                        {
                            Custom.WriteInColor("\nDestination and departure stations cannot be the same. Try again", ConsoleColor.Red);
                            Custom.PressAnythingToContinue();
                        }
                        else
                        {
                            isValidStation = true;
                        }
                    }
                    else
                    {
                        isValidStation = true;
                    }

                    Console.Clear();
                }
            }
        }

        class Train
        {
            private List<Wagon> _wagons = new List<Wagon>();

            public Train(string name)
            {
                Name = name;
            }

            public string Name { get; private set; }


            private void StartJourney()
            {
            }

            private void Construct()
            {
                int wagonCount = 1;

                Console.WriteLine($"Creating wagon number {wagonCount}. Number of seats::");
                _wagons.Add(new Wagon(wagonCount, 1));
            }

            private void ShowCurrentInfo()
            {
            }

            private void AddWagon()
            {

            }

            private void RemoveWagon()
            { }

        }

        class Wagon
        {
            private readonly int _num;
            private readonly int _maxSeats;
            private List<Passenger> _passengers = new List<Passenger>();

            public Wagon(int num, int seats)
            {
                _num = num;
                _maxSeats = seats;
            }
        }

        class Passenger
        {
            private readonly string _name;
            private readonly int _seat;
            private readonly int _wagon;

            public void Board()
            {

            }
        }

        class Station
        {
            public readonly string Name;

            public Station(string name)
            {
                Name = name;
            }

            public void ShowInfo()
            {
                Console.WriteLine($"station : {Name}");
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

        public static int GetUserNumberInRange(string startMessage = "The station number", int maxInput = 100)
        {
            int userInput = 0;
            bool isValidInput = false;

            Console.WriteLine(startMessage);

            while (!isValidInput)
            {
                if (int.TryParse(Console.ReadLine(), out userInput))
                {
                    if (userInput > 0 && userInput <= maxInput)
                        isValidInput = true;
                    else
                        Custom.WriteInColor($"\nPlease enter a number between 1 and {maxInput}:", ConsoleColor.Red);

                }
                else
                {
                    Custom.WriteInColor("\nInvalid input. Please enter a number:", ConsoleColor.Red);
                }
            }

            return userInput - 1;
        }
    }
}