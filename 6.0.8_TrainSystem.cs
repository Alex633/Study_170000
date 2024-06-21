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
    //      create train hud (alternative train.showinfo) - same with everyhing else
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
            private Train _train = new Train();
            private Route _route = new Route();
            private Queue<Passenger> _soldiers = new Queue<Passenger>();
            private Wagon[] _smallWagons;
            private Wagon[] _mediumWagons;
            private Wagon[] _largeWagons;

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

                userInput = Misc.GetUserNumberInRange("\nAwaiting command: ", _commands.Count);

                switch (userInput)
                {
                    default:
                        DisplayError();
                        break;
                    case 1:
                        CheckCameras();
                        break;
                    case 2:
                        _route.DeterminePoints();
                        break;
                    case 3:
                        _train.Construct();
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
                    { 3, "Construct train" },
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

            private void DisplayError()
            {
                Text.WriteInColor("\nUnknown Command. Try again:", ConsoleColor.Red);
                Misc.PressAnythingToContinue();
            }

            private void CheckCameras()
            {
                Route neededRoute = new Route();

                Console.Clear();
                Console.WriteLine("You are looking at the cameras...");
                Misc.PressAnythingToContinue(ConsoleColor.DarkYellow, false, 0, 0, "press anything to continue", false);
                neededRoute.DetermineRequest();
                Text.WriteLineInCustomColors($"\n{CountPassengers()}", ConsoleColor.Blue, " combine soldiers waiting for the train to move from ", ConsoleColor.White, $"{neededRoute.DepartureStation.Name}", ConsoleColor.Blue, " station to ", ConsoleColor.White, $"{neededRoute.DestinationStation.Name}", ConsoleColor.Blue, " station", ConsoleColor.White);
                Misc.PressAnythingToContinue();
            }

            private int CountPassengers()
            {
                int soldiersAmmount;
                Random random = new Random();

                soldiersAmmount = random.Next(501);

                for (int i = 0; i < soldiersAmmount; i++)
                    _soldiers.Enqueue(new Passenger());

                return _soldiers.Count;
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
            Random random = new Random();

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

            public void DeterminePoints()
            {
                bool isCorrectDestination = false;

                Console.Clear();
                Text.WriteLineInColor("Creating Route\n", ConsoleColor.DarkGray);
                DisplayAllStations();
                DepartureStation = _stations[Misc.GetUserNumberInRange("\nSelect DEPARTURE station: ", _stations.Count) - 1];

                while (!isCorrectDestination)
                {
                    DestinationStation = _stations[Misc.GetUserNumberInRange("Select DESTINATION station: ", _stations.Count) - 1];

                    if (DestinationStation != DepartureStation)
                        isCorrectDestination = true;
                    else
                        Text.WriteLineInColor("\nThe destination station and departure station cannot be the same. Choose a valid route, soldier");
                }

                Text.WriteLineInColor($"\nRoute [{DepartureStation.Name} - {DestinationStation.Name}] created", ConsoleColor.Blue);
                Misc.PressAnythingToContinue();
            }

            public void DetermineRequest()
            {
                int departureStationNum;
                int destinationStationNum;

                departureStationNum = random.Next(_stations.Count);
                DepartureStation = _stations[departureStationNum];
                destinationStationNum = random.Next(_stations.Count);

                while (destinationStationNum == departureStationNum)
                    destinationStationNum = random.Next(_stations.Count);

                DestinationStation = _stations[destinationStationNum];
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
            private Stack<Wagon> _wagons = new Stack<Wagon>();
            private Wagon _smallWagonBlueprint = new Wagon(10);
            private Wagon _mediumWagonBlueprint = new Wagon(50);
            private Wagon _largeWagonBlueprint = new Wagon(100);

            public int Seats { get; private set; }
            public int WagonCount { get; private set; }

            public Train()
            {
                WagonCount = 0;
            }

            private void StartJourney()
            {
            }

            public void Construct()
            {
                bool isConstracting = true;

                Console.Clear();

                while (isConstracting)
                {
                    Text.WriteLineInColor("Train construction\n", ConsoleColor.DarkGray);
                    ShowInfo();
                    Console.WriteLine();
                    DisplayAvailableWagonSizes();
                    AddWagon(SelectWagonSize(Misc.GetUserNumberInRange($"Select wagon #{WagonCount + 1} size: ", 3)));

                    isConstracting = Misc.GetBoolUserInput("Add more wagons?");
                }

                Text.WriteLineInColor($"Train Construction Complete", ConsoleColor.Cyan);
                Misc.PressAnythingToContinue();
            }

            private void DisplayAvailableWagonSizes()
            {
                Console.WriteLine($"Wagon sizes:\n" +
                    $"1. {_smallWagonBlueprint.SizeName} ({_smallWagonBlueprint.Seats} seats)\n" +
                    $"2. {_mediumWagonBlueprint.SizeName} ({_mediumWagonBlueprint.Seats} seats)\n" +
                    $"3. {_largeWagonBlueprint.SizeName} ({_largeWagonBlueprint.Seats} seats)\n");

            }

            private void ShowInfo()
            {
                Text.WriteLineInCustomColors("Train info:\n" +
                    $"The train has {WagonCount} wagon(s) | ", ConsoleColor.White, $"{Seats} seats", ConsoleColor.Blue);
            }

            private void AddWagon(Wagon wagonBlueprint)
            {
                _wagons.Push(wagonBlueprint);
                Seats += wagonBlueprint.Seats;
                WagonCount++;
                Text.WriteLineInColor($"\n{wagonBlueprint.SizeName} wagon ({wagonBlueprint.Seats}) added\n", ConsoleColor.Cyan);
            }

            private Wagon SelectWagonSize(int userInput)
            {
                switch (userInput)
                {
                    case 1:
                        return _smallWagonBlueprint;
                    case 2:
                        return _mediumWagonBlueprint;
                    case 3:
                        return _largeWagonBlueprint;
                    default:
                        return null;
                }
            }
        }

        class Wagon
        {
            public int Seats { get; private set; }
            public string SizeName { get; private set; }
            private enum Capacity
            {
                Small,
                Medium,
                Large
            }

            public Wagon(int maxSeats)
            {
                Seats = maxSeats;
                SizeName = DetermineSize();
                if (Seats <= 0)
                    Seats = 1;
            }

            public string DetermineSize()
            {
                int minSmall = 1;
                int maxSmall = 10;
                int minMedium = 11;
                int maxMedium = 50;

                if (Seats <= maxSmall && Seats >= minSmall)
                    return Capacity.Small.ToString();
                else if (Seats <= maxMedium && Seats >= minMedium)
                    return Capacity.Medium.ToString();
                else
                    return Capacity.Large.ToString();
            }

            public void DisplayInfo()
            {
                Text.WriteLineInCustomColors($"Wagon ", ConsoleColor.White, $"#", ConsoleColor.Blue, $". Size: ", ConsoleColor.White, $"{DetermineSize()}({Seats})", ConsoleColor.Blue);
            }
        }

        class Passenger
        {
            //private readonly string _name;
            //private readonly int _seat;
            //private readonly int _wagon;

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

        public static bool GetBoolUserInput(string message)
        {
            bool isCorrectInput = false;
            ConsoleKeyInfo userInput;

            while (!isCorrectInput)
            {
                Console.WriteLine(message + " (y or n)");
                userInput = Console.ReadKey(true);

                switch (userInput.KeyChar)
                {
                    case 'y':
                    case 'Y':
                        Console.Clear();
                        return true;
                    case 'n':
                    case 'N':
                        Console.Clear();
                        return false;
                    default:
                        Text.WriteLineInColor("Error. Incorrect input");
                        break;
                }
            }

            return false;
        }
    }

    class Text
    {
        public static void WriteLineInCustomColors(string text1, ConsoleColor color1 = ConsoleColor.White, string text2 = "", ConsoleColor color2 = ConsoleColor.White, string text3 = "", ConsoleColor color3 = ConsoleColor.White, string text4 = "", ConsoleColor color4 = ConsoleColor.White, string text5 = "", ConsoleColor color5 = ConsoleColor.White, string text6 = "", ConsoleColor color6 = ConsoleColor.White, bool isCustomPos = false, int xPos = 0, int YPos = 0)
        {

            if (isCustomPos)
            {
                Console.ForegroundColor = color1;
                WriteAtPosition(xPos, YPos, text1);
                Console.ForegroundColor = color2;
                Console.Write(text2);
                Console.ForegroundColor = color3;
                Console.Write(text3);
                Console.ForegroundColor = color4;
                Console.Write(text4);
                Console.ForegroundColor = color5;
                Console.Write(text5);
                Console.ForegroundColor = color6;
                Console.WriteLine(text6);
            }
            else
            {
                Console.ForegroundColor = color1;
                Console.Write(text1);
                Console.ForegroundColor = color2;
                Console.Write(text2);
                Console.ForegroundColor = color3;
                Console.Write(text3);
                Console.ForegroundColor = color4;
                Console.Write(text4);
                Console.ForegroundColor = color5;
                Console.Write(text5);
                Console.ForegroundColor = color6;
                Console.WriteLine(text6);
            }

            Console.ResetColor();
        }

        public static void WriteLineInColor(string text, ConsoleColor color = ConsoleColor.Red, bool isCustomPos = false, int xPos = 0, int YPos = 0)
        {
            Console.ForegroundColor = color;

            if (isCustomPos)
                WriteLineAtPosition(xPos, YPos, text);
            else
                Console.WriteLine(text);

            Console.ResetColor();
        }

        public static void WriteInColor(string text, ConsoleColor color = ConsoleColor.Red, bool isCustomPos = false, int xPos = 0, int YPos = 0)
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
