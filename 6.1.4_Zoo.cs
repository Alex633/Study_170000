//Пользователь запускает приложение и перед ним находится меню, в котором он может выбрать, к какому вольеру подойти. 
//При приближении к вольеру, пользователю выводится информация о том, что это за вольер, сколько животных там обитает, их пол и какой звук издает животное.
//Вольеров в зоопарке может быть много, в решении нужно создать минимум 4 вольера.

using System;
using System.Collections.Generic;

public class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        ZooKeeper zooKeeper = new ZooKeeper();
        zooKeeper.Open();

        Helper.WaitForKeyPress();
    }
}

class ZooKeeper
{
    private readonly Zoo _zoo;

    private CommandLineInterface _interface;

    public ZooKeeper()
    {
        _zoo = new Zoo();
        PopulateZoo();
        InitializeInterface();
    }

    public void Open()
    => _interface.Run();

    private void PopulateZoo()
    {
        Animal elephant = new Animal("Moo?", "Male");
        Animal bat = new Animal("Clap", "Male");
        Animal octopus = new Animal("Water sound?", "Female");
        Animal lion = new Animal("Loud Zzz", "Male");
        Animal dolphin = new Animal("Eeeeee", "Female");
        Animal parrot = new Animal("Step", "Male");
        Animal cat = new Animal("Zzz", "Female");
        Animal dog = new Animal("Woof", "Male");

        Cage cage1 = new Cage("1");
        Cage cage2 = new Cage("2");
        Cage cage3 = new Cage("3");
        Cage cage4 = new Cage("4");

        cage1.PlaceAnimal(elephant);
        cage1.PlaceAnimal(bat);

        cage2.PlaceAnimal(octopus);
        cage2.PlaceAnimal(lion);

        cage3.PlaceAnimal(dolphin);
        cage3.PlaceAnimal(parrot);

        cage4.PlaceAnimal(cat);
        cage4.PlaceAnimal(dog);

        _zoo.BuildCage(cage1);
        _zoo.BuildCage(cage2);
        _zoo.BuildCage(cage3);
        _zoo.BuildCage(cage4);
    }

    private void InitializeInterface()
    {
        var commands = new Dictionary<int, Command>
        {
            [1] = new Command("Visit cage 1", () => _zoo.VisitCage(1)),
            [2] = new Command("Visit cage 2", () => _zoo.VisitCage(2)),
            [3] = new Command("Visit cage 3", () => _zoo.VisitCage(3)),
            [4] = new Command("Visit cage 4", () => _zoo.VisitCage(4))
        };


        _interface = new CommandLineInterface(commands, "Zoo");
    }
}

class Zoo
{
    private readonly List<Cage> _cages;

    public Zoo()
    {
        _cages = new List<Cage>();
    }

    public void VisitCage(int cageNumber)
    {
        Console.Clear();

        int cageIndex = cageNumber - 1;

        if (_cages.Count < cageIndex)
        {
            Helper.WriteAt($"There is no cage {cageNumber}", foregroundColor: ConsoleColor.Red);
            return;
        }

        _cages[cageIndex].OutputSounds();
    }

    public void BuildCage(Cage cage)
    {
        _cages.Add(cage);
    }
}

class Cage
{
    private readonly List<Animal> _animals = new List<Animal>();

    private readonly string _title;

    public Cage(string title)
    {
        _animals = new List<Animal>();
        _title = title;
    }

    public void OutputSounds()
    {
        if (_animals.Count <= 0)
            return;

        Helper.WriteTitle($"Cage {_title} | {_animals.Count} animals");

        foreach (var animal in _animals)
            animal.Speak();
    }

    public void PlaceAnimal(Animal animal)
    {
        _animals.Add(animal);
    }
}

class Animal
{
    private readonly string _speach;
    private readonly string _sex;

    public Animal(string speach, string sex = "Female")
    {
        _speach = speach;
        _sex = sex;
    }

    public void Speak()
    {
        int minSounds = 1;
        int maxSounds = 5;
        int speachCount = Helper.GetRandomInt(minSounds, maxSounds + 1);

        Console.WriteLine($"I am {_sex}. Nice to meet you");

        for (int i = 0; i < speachCount; i++)
            Console.Write($"{_speach} ");

        Console.WriteLine("\n");
    }
}

class CommandLineInterface
{
    private readonly Dictionary<int, Command> _items;

    private bool _shouldRun;
    private readonly bool _shouldRunOnce;

    private readonly string _title;

    public CommandLineInterface(Dictionary<int, Command> items, string title, bool shouldRunOnce = false)
    {
        _shouldRun = true;
        _shouldRunOnce = shouldRunOnce;
        _title = title;

        _items = items;
        _items.Add(_items.Count + 1, new Command("Exit", Exit));
    }

    public int LastSelectedOption { get; private set; }
    public bool IsExitSelected => LastSelectedOption == _items.Count;

    public void Run()
    {
        _shouldRun = true;

        while (_shouldRun)
        {
            OutputCommands();

            LastSelectedOption = Helper.ReadInt("Input command: ");
            HandleInput();

            if (_shouldRunOnce)
                _shouldRun = false;
        }
    }

    private void HandleInput()
    {
        if (_items.TryGetValue(LastSelectedOption, out Command item))
            item.Execute();
        else
            Helper.WriteAt("Invalid command. Please try again.", foregroundColor: ConsoleColor.Red);

        if (_shouldRun)
            Helper.WaitForKeyPress();
    }

    private void OutputCommands()
    {
        Helper.WriteTitle(_title);

        foreach (var item in _items)
            Helper.WriteAt($"{item.Key}) {item.Value.Description}");

        Console.WriteLine();
    }

    private void Exit()
    {
        _shouldRun = false;

        Helper.WriteAt($"Closing the {_title}");
    }
}

class Command
{
    private readonly Action _action;

    public Command(string description, Action action)
    {
        Description = description;
        _action = action;
    }

    public string Description { get; private set; }

    public void Execute()
    {
        Helper.WriteTitle(Description);
        _action();
    }
}

class Helper
{
    private static readonly Random s_random = new Random();

    public static string ReadString(string helpText, ConsoleColor primary = ConsoleColor.Cyan, ConsoleColor secondary = ConsoleColor.Black)
    {
        ConsoleColor backgroundColor = Console.BackgroundColor;

        WriteAt(helpText, foregroundColor: primary, isNewLine: true);

        int fieldStartY = Console.CursorTop;
        int fieldStartX = Console.CursorLeft;

        char emptiness = ' ';
        WriteAt(new string(emptiness, helpText.Length - 1), backgroundColor: primary, isNewLine: false, xPosition: fieldStartX);

        Console.SetCursorPosition(fieldStartX + 1, fieldStartY);

        Console.BackgroundColor = primary;
        Console.ForegroundColor = secondary;

        string input = Console.ReadLine();

        Console.BackgroundColor = backgroundColor;

        Console.WriteLine();

        return input;
    }

    public static int ReadInt(string text)
    {
        int result;

        while (int.TryParse(ReadString(text), out result) == false) ;

        return result;
    }

    public static int GetRandomInt(int min, int max)
    {
        return s_random.Next(min, max);
    }

    public static void WriteTitle(string title, bool isSecondary = false)
    {
        ConsoleColor backgroundColor = isSecondary ? ConsoleColor.DarkGray : ConsoleColor.Gray;

        WriteAt($" {title} ", foregroundColor: ConsoleColor.Black, backgroundColor: backgroundColor);

        if (isSecondary == false)
            Console.WriteLine();
    }

    public static void WaitForKeyPress(bool shouldClear = true)
    {
        WriteAt("Press any key", foregroundColor: ConsoleColor.Cyan);
        Console.ReadKey(true);

        if (shouldClear)
            Console.Clear();
    }

    public static void WriteAt(object element, int? yPosition = null, int? xPosition = null,
    ConsoleColor foregroundColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black,
    bool isNewLine = true)
    {
        Console.ForegroundColor = foregroundColor;
        Console.BackgroundColor = backgroundColor;

        int yStart = Console.CursorTop;
        int xStart = Console.CursorLeft;

        bool isCustomPosition = yPosition.HasValue || xPosition.HasValue;

        if (isCustomPosition)
        {
            int targetY = yPosition ?? yStart;
            int targetX = xPosition ?? xStart;

            targetY = Math.Max(0, targetY);
            targetX = Math.Max(0, targetX);

            int safeYPosition = Math.Min(targetY, Console.WindowHeight - 1);
            int safeXPosition = Math.Min(targetX, Console.WindowWidth - 1);

            Console.SetCursorPosition(safeXPosition, safeYPosition);
        }

        if (isNewLine)
            Console.WriteLine(element);
        else
            Console.Write(element);

        Console.ResetColor();

        if (isCustomPosition)
        {
            Console.CursorTop = yStart;
            Console.CursorLeft = xStart;
        }
    }
}
