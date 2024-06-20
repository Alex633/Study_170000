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
    //      create train methods
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
            private List<Wagon> _wagons = new List<Wagon>();

            public int Number { get; private set; }

            public Train()
            {
                Number += 1;
            }

            private void StartJourney()
            {
            }

            public void Construct()
            {
                int wagonCount = 1;

                Text.WriteLineInColor("Train construction", ConsoleColor.DarkGray);

                Console.WriteLine($"Creating wagon number {wagonCount}. Number of seats::");
                _wagons.Add(new Wagon(wagonCount, Wagon.Сapacities.Small));
                _wagons[0].DisplayInfo();
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
            private int _smallSizeSeats = 10;
            private int _mediumSizeSeats = 50;
            private int _largeSizeSeats = 100;

            public string CapacityName {  get; private set; }
            public int MaxSeats { get; private set; }
            public int Number { get; private set; }
            public enum Сapacities
            {
                Small,
                Medium,
                Large
            }

            public Wagon(int number, Сapacities size)
            {
                Number = number;
                MaxSeats = GetMaxSeats(size);
                CapacityName = GetCapacityType(size);
            }
            private int GetMaxSeats(Сapacities size)
            {
                switch (size)
                {
                    case Сapacities.Small:
                        return _smallSizeSeats;
                    case Сapacities.Medium:
                        return _mediumSizeSeats;
                    case Сapacities.Large:
                        return _largeSizeSeats;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(size), $"Unsupported wagon size: {size}");
                }
            }

            private string GetCapacityType(Сapacities size)
            {
                switch (size)
                {
                    case Сapacities.Small:
                        return Сapacities.Small.ToString();
                    case Сapacities.Medium:
                        return Сapacities.Medium.ToString();
                    case Сapacities.Large:
                        return Сapacities.Large.ToString();
                    default:
                        throw new ArgumentOutOfRangeException(nameof(size), $"Unsupported wagon size: {size}");
                }
            }

            public void DisplayInfo()
            {
                Text.WriteLineInCustomColors($"Wagon ", ConsoleColor.White, $"#{Number}", ConsoleColor.Blue, $". Size: ", ConsoleColor.White, $"{this}{MaxSeats}", ConsoleColor.Blue);
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
