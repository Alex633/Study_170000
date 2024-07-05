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
        private const int EvaluatePassengersAmountAndRouteCommand = 1;
        private const int CreateRouteCommand = 2;
        private const int ConstructTrainCommand = 3;
        private const int TransportPassengersCommand = 4;
        private const int ExitCommand = 5;
        
        private readonly TrainControlHud _trainControlHud = new TrainControlHud();
        private Train _train = new Train();
        private Route _route = new Route();
        private Route _neededRoute = new Route();
        private int _passengersWaiting = 0;
        private bool _isWorking = true;

        private bool IsPassengersAmountAndRouteEvaluated { get { return _passengersWaiting > 0; } }

        public void Start()
        {
            Utility.WriteLineInCustomColors(
                ("Welcome to Combine Train Control System, ", ConsoleColor.White),
                ("soldier\n", ConsoleColor.Red)
                );

            while (_isWorking)
            {
                _trainControlHud.DisplayFull(_passengersWaiting, _neededRoute, _route, _train);
                DisplayCommands();
                HandleInput();
            }
        }

        private void HandleInput()
        {
            int userInput;
            int CommandsCount = 5;

            userInput = Utility.GetUserNumberInRange("\nAwaiting command: ", CommandsCount);

            switch (userInput)
            {
                case EvaluatePassengersAmountAndRouteCommand:
                    EvaluatePassengersAmountAndRoute();
                    break;
                case CreateRouteCommand:
                    _route.EnterPoints(_passengersWaiting, _neededRoute, _train);
                    break;
                case ConstructTrainCommand:
                    _train.Construct(_passengersWaiting, _neededRoute, _route);
                    break;
                case TransportPassengersCommand:
                    TryToTransportPassengers();
                    break;
                case ExitCommand:
                    Exit();
                    break;
            }
        }

        private void DisplayCommands()
        {
            Console.WriteLine("Commands:");
            Console.WriteLine($"{EvaluatePassengersAmountAndRouteCommand}. Look at the cameras");
            Console.WriteLine($"{CreateRouteCommand}. Create route");
            Console.WriteLine($"{ConstructTrainCommand}. Construct train");
            Console.WriteLine($"{TransportPassengersCommand}. Transport soldiers");
            Console.WriteLine($"{ExitCommand}. Exit");
        }

        private void EvaluatePassengersAmountAndRoute()
        {
            Console.Clear();
            _passengersWaiting = CountPassengers();
            Utility.PressAnythingToContinue(ConsoleColor.DarkYellow, false, 0, 0, "press anything to continue", false);
            _neededRoute.DetermineNeed();
            Utility.WriteLineInCustomColors(
                ("\nIt's ", ConsoleColor.White),
                ($"{_passengersWaiting}", ConsoleColor.Blue),
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

        private void TryToTransportPassengers()
        {
            if (_route.IsFilled == false && IsPassengersAmountAndRouteEvaluated == false && _train.IsConstructed == false)
            {
                FireSoldierFromExistence();
            }
            else
            {
                if (_route.IsFilled == false || IsPassengersAmountAndRouteEvaluated == false || _train.IsConstructed == false)
                    DisplayErrors();
                else if (_route.IsMatching(_neededRoute) == false || _train.IsBigEnough(_passengersWaiting) == false)
                    DisplayErrors();
                else
                    TransportPassengers();
            }

            Utility.PressAnythingToContinue();
        }

        private void DisplayErrors()
        {
            Console.WriteLine();

            if (_route.IsFilled == false)
            {
                Utility.WriteLineInColor("You didn't create any route");
            }

            if (_train.IsConstructed == false)
            {
                Utility.WriteLineInColor("We don't see any created trains");
            }

            if (IsPassengersAmountAndRouteEvaluated == false)
            {
                Utility.WriteLineInColor("You don't even know which route to choose and how many soldiers are waiting. What are you doing?");
            }
            else
            {
                if (_route.IsFilled && _route.IsMatching(_neededRoute) == false)
                    Utility.WriteLineInColor("The route you created is incorrect");

                if (_train.IsConstructed && _train.IsBigEnough(_passengersWaiting) == false)
                    Utility.WriteLineInColor("Your train doesn't have enough seats");
            }

            Utility.WriteLineInColor("Do better\n");
        }

        private void TransportPassengers()
        {
            Console.Clear();
            Console.WriteLine($"The train is leaving {_route.DepartureStation.Name} station");
            _train.Send();
            _passengersWaiting = 0;
            _route = new Route();
            _neededRoute = new Route();
            Utility.PressAnythingToContinue();
            Utility.WriteLineInColor($"Now return to your job, soldier", ConsoleColor.DarkRed);
            Utility.PressAnythingToContinue();
            Utility.WriteLineInColor($"Your comrades are waiting", ConsoleColor.DarkRed);
        }

        private void FireSoldierFromExistence()
        {
            Utility.WriteLineInColor("\nYou don't have any information on train, route and soldiers");
            Utility.PressAnythingToContinue();
            Utility.WriteLineInColor("You are not fit for this job");
            Utility.PressAnythingToContinue();
            Utility.WriteLineInColor("We are going to be waiting for you in our office");
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
                Utility.WriteInColor($"Passengers: {passengers} soldiers waiting for [{neededRoute.DepartureStation.Name} - {neededRoute.DestinationStation.Name}] train", ConsoleColor.DarkGreen, true, hudXPosition, hudYPosition);
            else
                Utility.WriteInColor($"Passengers: unknown", ConsoleColor.DarkGray, true, hudXPosition, hudYPosition);
        }
    }

    class Route
    {
        private readonly TrainControlHud _controlHud = new TrainControlHud();
        private readonly List<Station> _stations = new List<Station>();

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

        public Station DepartureStation { get; private set; }
        public Station DestinationStation { get; private set; }
        public bool IsFilled
        {
            get
            {
                if (this != null)
                    return DepartureStation != null && DestinationStation != null;
                else return false;
            }
            private set
            {
            }
        }

        public void EnterPoints(int passengers, Route neededRoute, Train train)
        {
            bool isCorrectDestination = false;

            Console.Clear();
            _controlHud.DisplayFull(passengers, neededRoute, this, train);
            Utility.WriteLineInColor("Creating Route\n", ConsoleColor.DarkGray);
            DisplayAllStations();
            DepartureStation = _stations[Utility.GetUserNumberInRange("\nSelect DEPARTURE station: ", _stations.Count) - 1];

            while (isCorrectDestination == false)
            {
                DestinationStation = _stations[Utility.GetUserNumberInRange("Select DESTINATION station: ", _stations.Count) - 1];

                if (DestinationStation != DepartureStation)
                    isCorrectDestination = true;
                else
                    Utility.WriteLineInColor("\nThe destination station and departure station cannot be the same. Choose a valid route, soldier");
            }

            Utility.WriteLineInColor($"\nRoute [{DepartureStation.Name} - {DestinationStation.Name}] created", ConsoleColor.Blue);
            _controlHud.DisplayFull(passengers, neededRoute, this, train);
            Utility.PressAnythingToContinue();
        }

        public void DetermineNeed()
        {
            int departureStationNumber;
            int destinationStationNumber;

            departureStationNumber = Utility.GenerateRandomNumber(_stations.Count);
            DepartureStation = _stations[departureStationNumber];
            destinationStationNumber = Utility.GenerateRandomNumber(_stations.Count);

            while (destinationStationNumber == departureStationNumber)
                destinationStationNumber = Utility.GenerateRandomNumber(_stations.Count);

            DestinationStation = _stations[destinationStationNumber];
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
                Utility.WriteInColor($"Route: {DepartureStation.Name} - {DestinationStation.Name}", ConsoleColor.DarkGreen, true, hudXPosition, hudYPosition);
            else
                Utility.WriteInColor($"Route: empty", ConsoleColor.DarkGray, true, hudXPosition, hudYPosition);
        }

        private void GetAvailableStations()
        {
            foreach (StationName stationName in Enum.GetValues(typeof(StationName)))
                _stations.Add(new Station(stationName.ToString()));
        }
    }

    class Train
    {
        private const int AddSmallWagon = 1;
        private const int AddMediumWagon = 2;
        private const int AddLargeWagon = 3;
        private const int Back = 4;
        private const int Commands = 4;
        
        private readonly TrainControlHud _controlHud = new TrainControlHud();
        private Stack<Wagon> _wagons = new Stack<Wagon>();

        private const int SmallWagonCapacity = 10;
        private const int MediumWagonCapacity = 50;
        private const int LargeWagonCapacity = 100;

        public Train()
        {
            WagonsCount = 0;
        }

        public enum WagonCapacity
        {
            Small,
            Medium,
            Large
        }

        public int Seats { get; private set; }
        public int WagonsCount { get; private set; }
        public bool IsConstructed
        {
            get
            {
                if (this != null)
                    return Seats > 0;
                else return false;
            }
            private set { }
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

        public void Construct(int passengers, Route neededRoute, Route route)
        {
            bool isBuilding = true;
            int userInput;

            Console.Clear();

            while (isBuilding)
            {
                _controlHud.DisplayFull(passengers, neededRoute, route, this);
                Utility.WriteLineInColor("Train construction", ConsoleColor.DarkGray);
                Console.WriteLine();
                DisplayCommands();
                userInput = Utility.GetUserNumberInRange($"\nSelect wagon #{WagonsCount + 1} capacity: ", Commands);
                isBuilding = HandleInput(userInput);
                _controlHud.DisplayFull(passengers, neededRoute, route, this);

                if (isBuilding == false)
                {
                    _controlHud.DisplayFull(passengers, neededRoute, route, this);

                    if (IsConstructed)
                        Utility.WriteLineInColor($"\nTrain Construction Complete", ConsoleColor.Cyan);
                    else
                        Utility.WriteLineInColor($"\nTrain Construction Canceled", ConsoleColor.DarkRed);
                }

                Utility.PressAnythingToContinue();
            }
        }

        public void DisplayInfo()
        {
            int hudXPosition = 0;
            int hudYPosistion = 29;

            if (WagonsCount > 0)
                Utility.WriteInColor($"Train: {WagonsCount} wagon(s) | {Seats} seat(s)", ConsoleColor.DarkGreen, true, hudXPosition, hudYPosistion);
            else
                Utility.WriteInColor($"Train: empty", ConsoleColor.DarkGray, true, hudXPosition, hudYPosistion);
        }

        private void DisplayCommands()
        {
            Console.WriteLine($"Commands:");
            Console.WriteLine($"{AddSmallWagon}. Add {GetWagonCapacityInfo(GetWagon(WagonCapacity.Small))}");
            Console.WriteLine($"{AddMediumWagon}. Add {GetWagonCapacityInfo(GetWagon(WagonCapacity.Medium))}");
            Console.WriteLine($"{AddLargeWagon}. Add {GetWagonCapacityInfo(GetWagon(WagonCapacity.Large))}");
            Console.WriteLine($"{Back}. Back");
        }

        private void AddWagon(Wagon wagon)
        {
            _wagons.Push(wagon);
            Seats += wagon.Seats;
            WagonsCount++;
            Utility.WriteLineInColor($"\n{GetWagonCapacityInfo(wagon)} wagon added\n", ConsoleColor.Cyan);
        }

        private Wagon GetWagon(WagonCapacity wagonCapacity)
        {
            switch (wagonCapacity)
            {
                case WagonCapacity.Small:
                    return new Wagon(SmallWagonCapacity);
                case WagonCapacity.Medium:
                    return new Wagon(MediumWagonCapacity);
                case WagonCapacity.Large:
                    return new Wagon(LargeWagonCapacity);
                default:
                    throw new ArgumentException("Invalid wagon capacity");
            }
        }

        private string GetWagonCapacityInfo(Wagon wagon)
        {
            return $"{GetWagonCapacityTitle(wagon)} wagon ({wagon.Seats})";
        }

        private string GetWagonCapacityTitle(Wagon wagon)
        {
            switch (wagon.Seats)
            {
                case SmallWagonCapacity:
                    return WagonCapacity.Small.ToString();
                case MediumWagonCapacity:
                    return WagonCapacity.Medium.ToString();
                case LargeWagonCapacity:
                    return WagonCapacity.Large.ToString();
                default:
                    throw new ArgumentException("Invalid wagon capacity");
            }
        }

        private bool HandleInput(int userInput)
        {
            switch (userInput)
            {
                case AddSmallWagon:
                    AddWagon(GetWagon(WagonCapacity.Small));
                    return true;
                case AddMediumWagon:
                    AddWagon(GetWagon(WagonCapacity.Medium));
                    return true;
                case AddLargeWagon:
                    AddWagon(GetWagon(WagonCapacity.Large));
                    return true;
                case Back:
                    return false;
                default:
                    return true;
            }
        }
    }

    class Wagon
    {
        public readonly int Seats;

        public Wagon(int seats)
        {
            Seats = seats;
        }
    }

    class Station
    {
        public Station(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }

    class Utility
    {
        private static Random s_random = new Random();

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
            return s_random.Next(minValue, maxValue);
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
                        Utility.WriteInColor($"\nPlease enter a number between 1 and {maxInput}: ", ConsoleColor.Red);
                }
                else
                {
                    Utility.WriteInColor("\nInvalid input. Please enter a number instead: ", ConsoleColor.Red);
                }
            }

            return userInput;
        }

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
