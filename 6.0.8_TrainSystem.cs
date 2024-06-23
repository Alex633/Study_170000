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
    //      check if route is correct then send (maybe get set is not correct?)
    //      check if everything else is correct then send (_soldiers.Count >= CountPassengers() && )
    //      reset values if correct all
    //      construct method and train with blueprints are weard
    //      create train hud (alternative train.showinfo) - same with everyhing else

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
            private Route _neededRoute = new Route();
            private Queue<Passenger> _soldiers = new Queue<Passenger>();
            private Dictionary<int, string> _commands;
            private const string _watchCamerasCommand = "Look at the cameras";
            private const string _createRouteCommand = "Create route";
            private const string _constructTrainCommand = "Construct train";
            private const string _transportSoldiersCommand = "Transport soldiers";
            private const string _exitCommand = "Exit";

            public bool IsCreatedRouteNeeded
            {
                get
                {
                    if (_route.IsFilled && _neededRoute.IsFilled && _route.DepartureStation == _neededRoute.DepartureStation && _route.DestinationStation == _neededRoute.DestinationStation)
                        return true;
                    else
                        return false;
                }
                private set { }
            }

            public TrainControlSystem()
            {
                InitializeCommands();
            }

            public void Start()
            {
                TextUtility.WriteLineInCustomColors(
                    ("Welcome to Combine Train Control System, ", ConsoleColor.White),
                    ("soldier\n", ConsoleColor.Red)
                    );

                while (_isWorking)
                {
                    ShowHud();
                    DisplayCommands();
                    HandleInput();
                }

                Exit();
            }

            private void InitializeCommands()
            {
                _commands = new Dictionary<int, string>
                {
                    { 1, _watchCamerasCommand },
                    { 2, _createRouteCommand },
                    { 3, _constructTrainCommand },
                    { 4, _transportSoldiersCommand },
                    { 5, _exitCommand }
                };
            }

            private void HandleInput()
            {
                int userInput;

                userInput = Utility.GetUserNumberInRange("\nAwaiting command: ", _commands.Count);

                if (_commands.TryGetValue(userInput, out string command))
                {
                    switch (command)
                    {
                        default:
                            DisplayError();
                            break;
                        case _watchCamerasCommand:
                            CheckCameras();
                            break;
                        case _createRouteCommand:
                            _route.EnterPoints();
                            break;
                        case _constructTrainCommand:
                            _train.Construct();
                            break;
                        case _transportSoldiersCommand:
                            Transport();
                            break;
                        case _exitCommand:
                            _isWorking = false;
                            break;
                    }
                }
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
                TextUtility.WriteInColor("\nUnknown Command. Try again:", ConsoleColor.Red);
                Utility.PressAnythingToContinue();
            }

            private void CheckCameras()
            {
                Console.Clear();
                Console.WriteLine("You are looking at the cameras...");
                Utility.PressAnythingToContinue(ConsoleColor.DarkYellow, false, 0, 0, "press anything to continue", false);
                _neededRoute.DetermineNeed();
                TextUtility.WriteLineInCustomColors(
                    ($"\n{CountPassengers()}", ConsoleColor.Blue),
                    (" combine soldiers waiting for the train to move from ", ConsoleColor.White),
                    ($"{_neededRoute.DepartureStation.Name}", ConsoleColor.Blue),
                    (" station to ", ConsoleColor.White),
                    ($"{_neededRoute.DestinationStation.Name}", ConsoleColor.Blue),
                    (" station", ConsoleColor.White));
                Utility.PressAnythingToContinue();
            }

            private int CountPassengers()
            {
                int soldiersAmmount;
                _soldiers.Clear();

                soldiersAmmount = Utility.GenerateRandomNumber(501);

                for (int i = 0; i < soldiersAmmount; i++)
                    _soldiers.Enqueue(new Passenger());

                return _soldiers.Count;
            }

            private void ShowHud()
            {
                ShowPassengersHud();
                _route.ShowInfo();
                _train.ShowInfo();
            }

            private void ShowPassengersHud()
            {
                int hudXPos = 0;
                int hudYPos = 27;

                if (_soldiers.Count > 0)
                    TextUtility.WriteInColor($"Passengers: {_soldiers.Count} soldiers waiting for [{_neededRoute.DepartureStation.Name} - {_neededRoute.DestinationStation.Name}] train", ConsoleColor.DarkGreen, true, hudXPos, hudYPos);
                else
                    TextUtility.WriteInColor($"Passengers: empty", ConsoleColor.DarkGray, true, hudXPos, hudYPos);
            }

            private void Transport()
            {
                if (IsCreatedRouteNeeded)
                {
                    Console.WriteLine("cool");
                }

                Utility.PressAnythingToContinue();
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

            public bool IsFilled { get; private set; }
            public Station DepartureStation { get; private set; }
            public Station DestinationStation { get; private set; }

            public Route()
            {
                GetAvailableStations();
                IsFilled = false;
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

            public void EnterPoints()
            {
                bool isCorrectDestination = false;

                Console.Clear();
                TextUtility.WriteLineInColor("Creating Route\n", ConsoleColor.DarkGray);
                ShowAllStations();
                DepartureStation = _stations[Utility.GetUserNumberInRange("\nSelect DEPARTURE station: ", _stations.Count) - 1];

                while (!isCorrectDestination)
                {
                    DestinationStation = _stations[Utility.GetUserNumberInRange("Select DESTINATION station: ", _stations.Count) - 1];

                    if (DestinationStation != DepartureStation)
                        isCorrectDestination = true;
                    else
                        TextUtility.WriteLineInColor("\nThe destination station and departure station cannot be the same. Choose a valid route, soldier");
                }

                TextUtility.WriteLineInColor($"\nRoute [{DepartureStation.Name} - {DestinationStation.Name}] created", ConsoleColor.Blue);
                IsFilled = true;
                Utility.PressAnythingToContinue();
            }

            public void DetermineNeed()
            {
                int departureStationNum;
                int destinationStationNum;

                departureStationNum = Utility.GenerateRandomNumber(_stations.Count);
                DepartureStation = _stations[departureStationNum];
                destinationStationNum = Utility.GenerateRandomNumber(_stations.Count);

                while (destinationStationNum == departureStationNum)
                    destinationStationNum = Utility.GenerateRandomNumber(_stations.Count);

                DestinationStation = _stations[destinationStationNum];
            }


            private void ShowAllStations()
            {
                int count = 1;

                Console.WriteLine("Stations: ");

                foreach (Station station in _stations)
                {
                    Console.Write($"{count}. {station.Name}\n");
                    count++;
                }
            }

            public void ShowInfo()
            {
                int hudXPos = 0;
                int hudYPos = 28;

                if (IsFilled)
                    TextUtility.WriteInColor($"Route: {DepartureStation.Name} - {DestinationStation.Name}", ConsoleColor.DarkGreen, true, hudXPos, hudYPos);
                else
                    TextUtility.WriteInColor($"Route: empty", ConsoleColor.DarkGray, true, hudXPos, hudYPos);
            }
        }

        class Train
        {
            private Stack<Wagon> _wagons = new Stack<Wagon>();
            private readonly Wagon _smallWagonBlueprint;
            private readonly Wagon _mediumWagonBlueprint;
            private readonly Wagon _largeWagonBlueprint;

            public bool IsConstructed { get; private set; }
            public int Seats { get; private set; }
            public int WagonsCount { get; private set; }

            public Train()
            {
                IsConstructed = false;
                WagonsCount = 0;
                _smallWagonBlueprint = new Wagon(Wagon.Capacity.Small);
                _mediumWagonBlueprint = new Wagon(Wagon.Capacity.Medium);
                _largeWagonBlueprint = new Wagon(Wagon.Capacity.Large);
            }

            private void Send()
            {
                IsConstructed = false;
                WagonsCount = 0;
                Seats = 0;
                _wagons.Clear();
            }

            public void Construct()
            {
                bool isBuilding = true;
                int blueprints = 3;

                Console.Clear();

                while (isBuilding)
                {
                    TextUtility.WriteLineInColor("Train construction\n", ConsoleColor.DarkGray);
                    ShowInfo();
                    Console.WriteLine();
                    DisplayAvailableWagonCapacities();
                    AddWagon(SelectWagonSize(Utility.GetUserNumberInRange($"Select wagon #{WagonsCount + 1} capacity: ", blueprints)));
                    ShowInfo();
                    isBuilding = Utility.GetBoolUserInput("Add more wagons?");
                }

                ShowInfo();
                IsConstructed = true;
                TextUtility.WriteLineInColor($"Train Construction Complete", ConsoleColor.Cyan);
                Utility.PressAnythingToContinue();
            }

            private void DisplayAvailableWagonCapacities()
            {
                Console.WriteLine($"Wagon capacities:\n" +
                    $"1. {_smallWagonBlueprint.CapacityTitle} ({_smallWagonBlueprint.Seats} seats)\n" +
                    $"2. {_mediumWagonBlueprint.CapacityTitle} ({_mediumWagonBlueprint.Seats} seats)\n" +
                    $"3. {_largeWagonBlueprint.CapacityTitle} ({_largeWagonBlueprint.Seats} seats)\n");
            }

            public void ShowInfo()
            {
                int hudXPos = 0;
                int hudYPos = 29;

                if (IsConstructed)
                    TextUtility.WriteInColor($"Train: {WagonsCount} wagon(s) | {Seats} seat(s)", ConsoleColor.DarkGreen, true, hudXPos, hudYPos);
                else
                    TextUtility.WriteInColor($"Train: empty", ConsoleColor.DarkGray, true, hudXPos, hudYPos);
            }

            private void AddWagon(Wagon wagonBlueprint)
            {
                _wagons.Push(wagonBlueprint);
                Seats += wagonBlueprint.Seats;
                WagonsCount++;
                TextUtility.WriteLineInColor($"{wagonBlueprint.CapacityTitle} wagon ({wagonBlueprint.Seats}) added\n", ConsoleColor.Cyan);
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
                        throw new ArgumentException("Invalid capacity");
                }
            }
        }

        class Wagon
        {
            public readonly int Seats;

            public string CapacityTitle { get; private set; }

            public enum Capacity
            {
                Small,
                Medium,
                Large
            }

            public Wagon(Capacity capacity)
            {
                switch (capacity)
                {
                    case Capacity.Small:
                        Seats = 10;
                        CapacityTitle = Capacity.Small.ToString();
                        break;
                    case Capacity.Medium:
                        Seats = 50;
                        CapacityTitle = Capacity.Medium.ToString();
                        break;
                    case Capacity.Large:
                        Seats = 100;
                        CapacityTitle = Capacity.Large.ToString();
                        break;
                    default:
                        throw new ArgumentException("Invalid capacity");
                }
            }
        }

        class Passenger
        {
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


    class Utility
    {
        public static void PressAnythingToContinue(ConsoleColor color = ConsoleColor.DarkYellow, bool isCustomPosition = false, int xPosition = 0, int yPosition = 0, string text = "press anything to continue", bool isConsoleClear = true)
        {
            if (isCustomPosition)
                Console.SetCursorPosition(xPosition, yPosition);

            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
            Console.ReadKey(true);

            if (isConsoleClear)
                Console.Clear();
        }
        public static int GenerateRandomNumber(int maxValue, int minValue = 1)
        {
            Random random = new Random();

            return random.Next(minValue, maxValue);
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
                        TextUtility.WriteInColor($"\nPlease enter a number between 1 and {maxInput}: ", ConsoleColor.Red);

                }
                else
                {
                    TextUtility.WriteInColor("\nInvalid input. Please enter a number instead: ", ConsoleColor.Red);
                }
            }

            return userInput;
        }

        public static bool GetBoolUserInput(string message, ConsoleColor color = ConsoleColor.DarkYellow)
        {
            bool isCorrectInput = false;
            ConsoleKeyInfo userInput;

            while (!isCorrectInput)
            {
                TextUtility.WriteLineInColor(message + " (y or n)", ConsoleColor.DarkYellow);
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
                        TextUtility.WriteLineInColor("Error. Incorrect input");
                        break;
                }
            }

            return false;
        }
    }

    class TextUtility
    {
        public static void WriteLineInCustomColors(params (string text, ConsoleColor color)[] coloredText)
        {
            int count = 0;

            foreach (var (text, color) in coloredText)
            {
                WriteInColor(text, color);
                count++;

                if (count == coloredText.Length)
                    Console.WriteLine();
            }

            Console.ResetColor();
        }

        public static void WriteLineInColor(string text, ConsoleColor color = ConsoleColor.DarkRed, bool isCustomPosition = false, int xPosition = 0, int yPosition = 0)
        {
            Console.ForegroundColor = color;

            if (isCustomPosition)
                WriteLineAtPosition(xPosition, yPosition, text);
            else
                Console.WriteLine(text);

            Console.ResetColor();
        }

        public static void WriteInColor(string text, ConsoleColor color = ConsoleColor.DarkRed, bool isCustomPosition = false, int xPosition = 0, int yPosition = 0)
        {
            Console.ForegroundColor = color;

            if (isCustomPosition)
                WriteAtPosition(xPosition, yPosition, text);
            else
                Console.Write(text);

            Console.ResetColor();
        }

        public static void WriteLineAtPosition(int xPosition, int yPosition, string text)
        {
            int currentXPostition = Console.CursorLeft;
            int currentYPostition = Console.CursorTop;

            Console.SetCursorPosition(xPosition, yPosition);
            Console.WriteLine(text);
            Console.SetCursorPosition(currentXPostition, currentYPostition);
        }

        public static void WriteAtPosition(int xPosition, int yPosition, string text)
        {
            int currentXPostition = Console.CursorLeft;
            int currentYPostition = Console.CursorTop;

            Console.SetCursorPosition(xPosition, yPosition);
            Console.Write(text);
            Console.SetCursorPosition(currentXPostition, currentYPostition);
        }
    }
}
