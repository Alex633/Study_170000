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
    //      create method inside train control system display hud (at the bottom of the console Soldiers: 5 waiting A-B, 29 waiting B-C; Route(s): A-B; Trains: Sokol(25)

    internal class Program
    {
        static void Main()
        {
            TrainControlSystem trainControlSystem = new TrainControlSystem();
            trainControlSystem.Start();
        }

        class TrainControlSystem
        {
            private bool _isWorking = true;
            private Train _train = new Train("Joe");
            private Route _route = new Route();
            private Dictionary<int, string> _commands;

            public TrainControlSystem()
            {
                InitializeCommands();
            }

            public void Start()
            {
                Console.WriteLine("Welcome to Combine Train Control System, soldier\n");

                while (_isWorking)
                {
                    DisplayCommands();
                    HandleInput();
                    //CreateTrain();
                }

                Exit();
            }

            private void HandleInput()
            {
                int userInput;

                userInput = Misc.GetUserNumberInRange("\nAwaiting command: ", 2);

                switch (userInput)
                {
                    default:
                        DisplayError();
                        break;
                    case 1:
                        //look at the cameras - 33 soldiers waiting for train to move from black mesa to city 17
                        break;
                    case 2:
                        _route.Create();
                        break;
                    case 3:
                        //create a train
                        break;
                    case 4:
                        //Transport soldiers
                        break;
                    case 5:
                        _isWorking = false;
                        break;
                }
            }

            private void InitializeCommands()
            {
                _commands = new Dictionary<int, string>
                {
                    { 1, "Look at the cameras" },
                    { 2, "Create route" },
                    { 3, "Create train" },
                    { 4, "Transport soldiers" },
                    { 5, "Exit" }
                };
            }

            private void DisplayCommands()
            {
                Console.WriteLine("Commands:");

                foreach (var command in _commands)
                {
                    Console.WriteLine($"{command.Key}. {command.Value}");
                }
            }

            private void CreateTrain()
            {
                Console.WriteLine("Train name:");
                _train = new Train(Console.ReadLine());
            }


            private void DisplayError()
            {
                Text.WriteInColor("\nUnknown Command. Try again:", ConsoleColor.Red);
                Misc.PressAnythingToContinue();
            }

            private void GetPassengerTickets()
            {

            }

            private void SellTickets()
            {

            }
            private void Exit()
            {
                Console.Clear();
                Console.WriteLine("\nGlory to the Combine");
            }
        }

        class Route
        {
            private readonly List<Station> _stations = new List<Station>();

            public Station DepartureStation { get; private set; }
            public Station DestinationStation { get; private set; }

            public Route()
            {
                GetAvailableStations();
            }

            enum StationName
            {
                City17,
                BlackMesa,
                NovaProspekt,
                Citadel,
                Ravenholm,
                ApertureScience
            }

            private void GetAvailableStations()
            {
                foreach (StationName stationName in Enum.GetValues(typeof(StationName)))
                {
                    _stations.Add(new Station(stationName.ToString()));
                }
            }

            public void Create()
            {
                bool isCorrectDestination = false;

                Console.Clear();
                Text.WriteLineInColor("Creating Route\n", ConsoleColor.DarkGray);
                DisplayAllStations();
                DepartureStation = _stations[Misc.GetUserNumberInRange("\nSelect DEPARTURE station: ", _stations.Count)];

                while (!isCorrectDestination)
                {
                    DestinationStation = _stations[Misc.GetUserNumberInRange("Select DESTINATION station: ", _stations.Count)];

                    if (DestinationStation != DepartureStation)
                        isCorrectDestination = true;
                    else
                        Text.WriteLineInColor("\nThe destination station and departure station cannot be the same. Choose a valid route, soldier");
                }

                Text.WriteLineInColor($"\nRoute [{DepartureStation.Name} - {DestinationStation.Name}] created", ConsoleColor.Blue);
                Misc.PressAnythingToContinue();
            }

            private void DisplayAllStations()
            {
                int count = 1;

                Console.WriteLine("Stations: ");

                foreach (Station station in _stations)
                {
                    Console.Write($"{count}. {station.Name}\n");
                    count++;
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
            public string Name { get; private set; }

            public Station(string name)
            {
                Name = name;
            }
        }
    }


    class Misc
    {
        public static void PressAnythingToContinue(ConsoleColor color = ConsoleColor.DarkYellow, bool isCustomPos = false, int xPos = 0, int YPos = 0, string text = "press anything to continue", bool isConsoleClear = true)
        {
            if (isCustomPos)
                Console.SetCursorPosition(xPos, YPos);

            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
            Console.ReadKey(true);

            if (isConsoleClear)
                Console.Clear();
        }

        public static int GetUserNumberInRange(string startMessage = "Select Number: ", int maxInput = 100)
        {
            int userInput = 0;
            bool isValidInput = false;

            Console.Write(startMessage);

            while (!isValidInput)
            {
                if (int.TryParse(Console.ReadLine(), out userInput))
                {
                    if (userInput > 0 && userInput <= maxInput)
                        isValidInput = true;
                    else
                        Text.WriteInColor($"\nPlease enter a number between 1 and {maxInput}: ", ConsoleColor.Red);

                }
                else
                {
                    Text.WriteInColor("\nInvalid input. Please enter a number instead: ", ConsoleColor.Red);
                }
            }

            return userInput;
        }
    }

    class Text
    {
        public static void WriteLineInColor(string text, ConsoleColor color = ConsoleColor.DarkRed, bool isCustomPos = false, int xPos = 0, int YPos = 0)
        {
            Console.ForegroundColor = color;

            if (isCustomPos)
                WriteLineAtPosition(xPos, YPos, text);
            else
                Console.WriteLine(text);

            Console.ResetColor();
        }

        public static void WriteInColor(string text, ConsoleColor color = ConsoleColor.DarkRed, bool isCustomPos = false, int xPos = 0, int YPos = 0)
        {
            Console.ForegroundColor = color;

            if (isCustomPos)
                WriteLineAtPosition(xPos, YPos, text);
            else
                Console.Write(text);

            Console.ResetColor();
        }

        public static void WriteLineAtPosition(int xPos, int yPos, string text)
        {
            Console.SetCursorPosition(xPos, yPos);
            Console.WriteLine(text);
        }

        public static void WriteAtPosition(int xPos, int yPos, string text)
        {
            Console.SetCursorPosition(xPos, yPos);
            Console.Write(text);
        }
    }
}
