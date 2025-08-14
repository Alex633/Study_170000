//Пользователь запускает приложение и перед ним находится меню, в котором он может выбрать, к какому вольеру подойти. 
//При приближении к вольеру, пользователю выводится информация о том, что это за вольер, сколько животных там обитает, 
//их пол и какой звук издает животное.
//Вольеров в зоопарке может быть много, в решении нужно создать минимум 4 вольера.

using System;
using System.Collections.Generic;

enum Gender
{
    Male = 0,
    Female
}


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
        _zoo = new ZooFactory().GewNewZoo(GetCages());
        InitializeInterface();
    }

    public void Open()
    => _interface.Run();

    private List<Cage> GetCages()
    {
        CageFactory cageFactory = new CageFactory();
        AnimalFactory animalFactory = new AnimalFactory();

        List<Cage> cages = new List<Cage>();

        Animal elephant = animalFactory.GetNewAnimal("Moo?");
        Animal bat = animalFactory.GetNewAnimal("Clap");
        List<Animal> animalsCage1 = new List<Animal>()
        {
            elephant, bat
        };
        Cage cage1 = cageFactory.GetNewCage("1", animalsCage1);
        cages.Add(cage1);

        Animal octopus = animalFactory.GetNewAnimal("Water sound?");
        Animal lion = animalFactory.GetNewAnimal("Loud Zzz");
        List<Animal> animalsCage2 = new List<Animal>()
        {
            octopus, lion
        };
        Cage cage2 = cageFactory.GetNewCage("2", animalsCage2);
        cages.Add(cage2);

        Animal dolphin = animalFactory.GetNewAnimal("Eeeeee");
        Animal parrot = animalFactory.GetNewAnimal("Step");
        List<Animal> animalsCage3 = new List<Animal>()
        {
            dolphin, parrot
        };
        Cage cage3 = cageFactory.GetNewCage("3", animalsCage3);
        cages.Add(cage3);

        Animal cat = animalFactory.GetNewAnimal("Zzz");
        Animal dog = animalFactory.GetNewAnimal("Woof");
        List<Animal> animalsCage4 = new List<Animal>()
        {
            cat, dog
        };
        Cage cage4 = cageFactory.GetNewCage("4", animalsCage4);
        cages.Add(cage4);

        return cages;
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

class ZooFactory
{
    public Zoo GewNewZoo(List<Cage> cages)
    {
        return new Zoo(cages);
    }
}

class Zoo
{
    private readonly List<Cage> _cages;

    public Zoo(List<Cage> cages)
    {
        _cages = cages;
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
}

class CageFactory
{
    public Cage GetNewCage(string title, List<Animal> animals)
    {
        return new Cage(title, animals);
    }
}

class Cage
{
    private readonly List<Animal> _animals;

    private readonly string _title;

    public Cage(string title, List<Animal> animals)
    {
        _title = title;
        _animals = animals;
    }

    public void OutputSounds()
    {
        if (_animals.Count <= 0)
            return;

        Helper.WriteTitle($"Cage {_title} | {_animals.Count} animals");

        foreach (var animal in _animals)
            animal.Speak();
    }
}

class AnimalFactory
{
    public Animal GetNewAnimal(string speach)
    {
        return new Animal(speach);
    }
}

class Animal
{
    private readonly string _speach;
    private readonly Gender _gender;

    public Animal(string speach)
    {
        _speach = speach;

        int male = 0;
        int female = 1;
        _gender = (Gender)Helper.GetRandomInt(male, female + 1);
    }

    public void Speak()
    {
        int minSounds = 1;
        int maxSounds = 5;
        int speachCount = Helper.GetRandomInt(minSounds, maxSounds + 1);

        Console.WriteLine($"I am {_gender}. Nice to meet you");

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
