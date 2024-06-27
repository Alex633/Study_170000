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

    internal class Program
    {
        static void Main()
        {
            TrainControlSystem trainControlSystem = new TrainControlSystem();
            trainControlSystem.Start();
        }

    }

    class TrainControlSystem
    {
        private Train _train = new Train();
        private Route _route = new Route();
        private Route _neededRoute = new Route();
        private int _soldiersWaiting = 0;
        private Dictionary<int, string> _commands;
        private readonly TrainControlHud TrainControlSystemHud = new TrainControlHud();
        private bool _isWorking = true;
        private const string _watchCamerasCommand = "Look at the cameras";
        private const string _createRouteCommand = "Create route";
        private const string _constructTrainCommand = "Construct train";
        private const string _transportSoldiersCommand = "Transport soldiers";
        private const string _exitCommand = "Exit";

        private bool CamerasChecked { get { return _soldiersWaiting > 0; } }

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
                TrainControlSystemHud.DisplayFull(_soldiersWaiting, _neededRoute, _route, _train);
                DisplayCommands();
                HandleInput();
            }
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
                    case _watchCamerasCommand:
                        CheckCameras();
                        break;
                    case _createRouteCommand:
                        _route.EnterPoints(_soldiersWaiting, _neededRoute, _route, _train);
                        break;
                    case _constructTrainCommand:
                        _train.Construct(_soldiersWaiting, _neededRoute, _route, _train);
                        break;
                    case _transportSoldiersCommand:
                        TryToTransport();
                        break;
                    case _exitCommand:
                        Exit();
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

        private void CheckCameras()
        {
            Console.Clear();
            _soldiersWaiting = CountPassengers();
            Utility.PressAnythingToContinue(ConsoleColor.DarkYellow, false, 0, 0, "press anything to continue", false);
            _neededRoute.DetermineNeed();
            TextUtility.WriteLineInCustomColors(
                ("\nIt's ", ConsoleColor.White),
                ($"{_soldiersWaiting}", ConsoleColor.Blue),
                (" combine soldiers waiting for the train to move from ", ConsoleColor.White),
                ($"{_neededRoute.DepartureStation.Name}", ConsoleColor.Blue),
                (" station to ", ConsoleColor.White),
                ($"{_neededRoute.DestinationStation.Name}", ConsoleColor.Blue),
                (" station", ConsoleColor.White));
            Utility.PressAnythingToContinue();
        }

        private int CountPassengers()
        {
            Console.WriteLine("The soldiers are waiting. You counting the soldiers. 1... 2...");
            return Utility.GenerateRandomNumber(501);
        }

        private void TryToTransport()
        {
            if (_route.IsFilled && CamerasChecked && _train.IsConstructed == false)
            {
                FireSoldierFromExistence();
            }
            else
            {
                if (_route.IsFilled || CamerasChecked || _train.IsConstructed == false)
                    DisplayErrors();
                else if (_route.IsMatching(_neededRoute) || _train.IsBigEnough(_soldiersWaiting) == false)
                    DisplayErrors();
                else
                    Transport();
            }

            Utility.PressAnythingToContinue();
        }

        private void DisplayErrors()
        {
            Console.WriteLine();

            if (_route.IsFilled == false)
            {
                TextUtility.WriteLineInColor("You didn't create any route");
            }

            if (_train.IsConstructed == false)
            {
                TextUtility.WriteLineInColor("We don't see any created trains");
            }

            if (CamerasChecked == false)
            {
                TextUtility.WriteLineInColor("You don't even know which route to choose and how many soldiers are waiting. What are you doing?");
            }
            else
            {
                if (_route.IsFilled)
                {
                    if (_route.IsMatching(_neededRoute) == false)
                        TextUtility.WriteLineInColor("The route you created is incorrect");
                }

                if (_train.IsConstructed)
                {
                    if (_train.IsBigEnough(_soldiersWaiting) == false)
                        TextUtility.WriteLineInColor("Your train doesn't have enough seats");
                }
            }

            TextUtility.WriteLineInColor("Do better\n");
        }

        private void Transport()
        {
            Console.Clear();
            Console.WriteLine($"The train is leaving {_route.DepartureStation.Name} station");
            _train.Send();
            _soldiersWaiting = 0;
            _route = new Route();
            _neededRoute = new Route();
            Utility.PressAnythingToContinue();
            TextUtility.WriteLineInColor($"Now return to your job, soldier", ConsoleColor.DarkRed);
            Utility.PressAnythingToContinue();
            TextUtility.WriteLineInColor($"Your comrades are waiting", ConsoleColor.DarkRed);
        }

        private void FireSoldierFromExistence()
        {
            TextUtility.WriteLineInColor("\nYou don't have any information, train and route");
            Utility.PressAnythingToContinue();
            TextUtility.WriteLineInColor("You are not fit for this job");
            Utility.PressAnythingToContinue();
            TextUtility.WriteLineInColor("We are going to be waiting for you in our office");
            Exit();
        }

        private void Exit()
        {
            _isWorking = false;
            Console.Clear();
            Console.WriteLine("Glory to the Combine");
        }
    }

    class TrainControlHud
    {
        public void DisplayFull(int passengers, Route neededRoute, Route route, Train train)
        {
            DisplayPassengersData(passengers, neededRoute);
            route.DisplayInfo();
            train.DisplayInfo();
        }

        private void DisplayPassengersData(int passengers, Route neededRoute)
        {
            int hudXPosition = 0;
            int hudYPosition = 27;

            if (passengers > 0)
                TextUtility.WriteInColor($"Passengers: {passengers} soldiers waiting for [{neededRoute.DepartureStation.Name} - {neededRoute.DestinationStation.Name}] train", ConsoleColor.DarkGreen, true, hudXPosition, hudYPosition);
            else
                TextUtility.WriteInColor($"Passengers: unknown", ConsoleColor.DarkGray, true, hudXPosition, hudYPosition);
        }
    }

    class Route
    {
        private readonly TrainControlHud _controlHud = new TrainControlHud();
        private readonly List<Station> _stations = new List<Station>();

        public Station DepartureStation { get; private set; }
        public Station DestinationStation { get; private set; }
        public bool IsFilled
        {
            get
            {
                return DepartureStation != null && DestinationStation != null;
            }
            private set
            {
            }
        }

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
                _stations.Add(new Station(stationName.ToString()));
        }

        public void EnterPoints(int passengers, Route neededRoute, Route route, Train train)
        {
            bool isCorrectDestination = false;

            Console.Clear();
            _controlHud.DisplayFull(passengers, neededRoute, route, train);
            TextUtility.WriteLineInColor("Creating Route\n", ConsoleColor.DarkGray);
            DisplayAllStations();
            DepartureStation = _stations[Utility.GetUserNumberInRange("\nSelect DEPARTURE station: ", _stations.Count) - 1];

            while (isCorrectDestination == false)
            {
                DestinationStation = _stations[Utility.GetUserNumberInRange("Select DESTINATION station: ", _stations.Count) - 1];

                if (DestinationStation != DepartureStation)
                    isCorrectDestination = true;
                else
                    TextUtility.WriteLineInColor("\nThe destination station and departure station cannot be the same. Choose a valid route, soldier");
            }

            TextUtility.WriteLineInColor($"\nRoute [{DepartureStation.Name} - {DestinationStation.Name}] created", ConsoleColor.Blue);
            _controlHud.DisplayFull(passengers, neededRoute, route, train);
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

        public bool IsMatching(Route route)
        {
            bool isStationsMatch = DepartureStation.Name == route.DepartureStation.Name && DestinationStation.Name == route.DestinationStation.Name;
            bool isStationsFilled = IsFilled && route.IsFilled;
            return isStationsMatch && isStationsFilled;
        }

        public void DisplayInfo()
        {
            int hudXPosition = 0;
            int hudYPosition = 28;

            if (IsFilled)
                TextUtility.WriteInColor($"Route: {DepartureStation.Name} - {DestinationStation.Name}", ConsoleColor.DarkGreen, true, hudXPosition, hudYPosition);
            else
                TextUtility.WriteInColor($"Route: empty", ConsoleColor.DarkGray, true, hudXPosition, hudYPosition);
        }
    }

    class Train
    {
        private readonly TrainControlHud _controlHud = new TrainControlHud();
        private Stack<Wagon> _wagons = new Stack<Wagon>();
        private readonly List<Wagon> _wagonsBlueprints = new List<Wagon>();

        public int Seats { get; private set; }
        public int WagonsCount { get; private set; }
        public bool IsConstructed { get { return Seats > 0; } private set { } }

        public Train()
        {
            WagonsCount = 0;
            _wagonsBlueprints.Add(new Wagon(Wagon.Capacity.Small));
            _wagonsBlueprints.Add(new Wagon(Wagon.Capacity.Medium));
            _wagonsBlueprints.Add(new Wagon(Wagon.Capacity.Large));
        }

        public bool IsBigEnough(int passengers)
        {
            return Seats >= passengers;
        }

        public void Send()
        {
            WagonsCount = 0;
            Seats = 0;
            _wagons.Clear();
        }

        public void Construct(int passengers, Route neededRoute, Route route, Train train)
        {
            bool isBuilding = true;
            int userInput;

            Console.Clear();

            while (isBuilding)
            {
                _controlHud.DisplayFull(passengers, neededRoute, route, train);
                TextUtility.WriteLineInColor("Train construction", ConsoleColor.DarkGray);
                Console.WriteLine();
                DisplayCommands();
                userInput = Utility.GetUserNumberInRange($"\nSelect wagon #{WagonsCount + 1} capacity: ", _wagonsBlueprints.Count + 1);
                HandleInput(userInput, ref isBuilding);
                _controlHud.DisplayFull(passengers, neededRoute, route, train);

                if (isBuilding == false)
                {
                    _controlHud.DisplayFull(passengers, neededRoute, route, train);
                    TextUtility.WriteLineInColor($"\nTrain Construction Complete", ConsoleColor.Cyan);
                }

                Utility.PressAnythingToContinue();
            }
        }

        private void DisplayCommands()
        {
            int count = 1;

            Console.WriteLine($"Commands:");

            foreach (Wagon blueprint in _wagonsBlueprints)
            {
                Console.WriteLine($"{count}. Add {blueprint.CapacityTitle} wagon ({blueprint.Seats} seats)");
                count++;
            }

            Console.WriteLine($"{count}. Back");
        }

        public void DisplayInfo()
        {
            int hudXPosition = 0;
            int hudYPosistion = 29;

            if (WagonsCount > 0)
                TextUtility.WriteInColor($"Train: {WagonsCount} wagon(s) | {Seats} seat(s)", ConsoleColor.DarkGreen, true, hudXPosition, hudYPosistion);
            else
                TextUtility.WriteInColor($"Train: empty", ConsoleColor.DarkGray, true, hudXPosition, hudYPosistion);
        }

        private void AddWagon(Wagon wagonBlueprint)
        {
            _wagons.Push(wagonBlueprint);
            Seats += wagonBlueprint.Seats;
            WagonsCount++;
            TextUtility.WriteLineInColor($"\n{wagonBlueprint.CapacityTitle} wagon ({wagonBlueprint.Seats}) added\n", ConsoleColor.Cyan);
        }

        private void HandleInput(int userInput, ref bool isWorking)
        {
            switch (userInput)
            {
                case 1:
                    AddWagon(_wagonsBlueprints[0]);
                    break;
                case 2:
                    AddWagon(_wagonsBlueprints[1]);
                    break;
                case 3:
                    AddWagon(_wagonsBlueprints[2]);
                    break;
                case 4:
                    isWorking = false;
                    break;
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

    class Station
    {
        public string Name { get; private set; }

        public Station(string name)
        {
            Name = name;
        }
    }

    class Utility
    {
        private static Random _random = new Random();

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
            return _random.Next(minValue, maxValue);
        }

        public static int GetUserNumberInRange(string startMessage = "Select Number: ", int maxInput = 100)
        {
            int userInput = 0;
            bool isValidInput = false;

            Console.Write(startMessage);

            while (isValidInput == false)
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
