//ДЗ: База данных игроков
//Реализовать базу данных игроков и методы для работы с ней. 
//Должно быть консольное меню для взаимодействия пользователя с возможностями базы данных.
//Игрок должен состоять из уникального номера, ника, уровня и булевого значения, забанен ли игрок.
//Реализовать возможность добавления игрока, 
//бана игрока по уникальному номеру, разбана игрока по уникальному номеру и удаление игрока по уникальному номеру.

//Создавать полноценные системы баз данных не нужно, 
//задание выполняется инструментами, которые вы уже изучили в рамках курса. Надо сделать класс "База данных".

using System;
using System.Collections.Generic;
using System.Threading;

public class Program
{
    static void Main()
    {
        Database database = new Database();

        Menu menu = new Menu(database);

        menu.Run();
    }
}

class Menu
{
    private const int CommandAdd = 1;
    private const int CommandRemove = 2;
    private const int CommandBan = 3;
    private const int CommandUnban = 4;
    private const int CommandShow = 5;
    private const int CommandExit = 6;

    private Dictionary<int, string> _menuItems;
    private int _input;
    private bool _shouldRun;

    private Database _database;

    public Menu(Database database)
    {
        _database = database;
        _shouldRun = true;

        InitializeMenu();
    }

    public void Run()
    {
        while (_shouldRun)
        {
            OutputCommands();
            Console.WriteLine();

            _input = Helper.ReadInt("Input command: ");
            HandleInput();

            Helper.ClearAfterKeyPress();
        }
    }

    private void HandleInput()
    {
        Console.Clear();

        WriteCommandTitleIfExist();

        switch (_input)
        {
            default:
                Helper.WriteAt("Invalid command. Please try again.", foregroundColor: ConsoleColor.Red);
                return;

            case CommandAdd:
                _database.Add();
                break;

            case CommandRemove:
                _database.Remove();
                break;

            case CommandBan:
                _database.Ban();
                break;

            case CommandUnban:
                _database.Unban();
                break;

            case CommandShow:
                _database.OutputFull();
                break;

            case CommandExit:
                _shouldRun = false;
                break;
        }
    }

    private void OutputCommands()
    {
        Helper.WriteTitle("Gamers Database commands");

        foreach (var item in _menuItems)
            Helper.WriteAt($"{item.Key}) {item.Value}");
    }

    private void WriteCommandTitleIfExist()
    {
        if (_menuItems.ContainsKey(_input))
        {
            _menuItems.TryGetValue(_input, out string title);
            Helper.WriteTitle(title);
        }
    }

    private void InitializeMenu()
    {
        _menuItems = new Dictionary<int, string>();

        _menuItems.Add(CommandAdd, "Add");
        _menuItems.Add(CommandRemove, "Remove");
        _menuItems.Add(CommandBan, "Ban");
        _menuItems.Add(CommandUnban, "Unban");
        _menuItems.Add(CommandShow, "Show gamers");
        _menuItems.Add(CommandExit, "Exit");
    }
}

class Database
{
    private List<Gamer> _gamers;

    public Database()
    {
        _gamers = new List<Gamer>();
    }

    public void OutputFull()
    {
        foreach (Gamer gamer in _gamers)
            Helper.WriteAt(gamer.GetInfo(), foregroundColor: gamer._isBanned == false ? ConsoleColor.Green : ConsoleColor.Red);
    }

    public void Add()
    {
        string name = Helper.ReadString("Name: ");
        int level = Helper.ReadInt("Level: ");

        Gamer gamer = new Gamer(name, level);

        _gamers.Add(gamer);

        Helper.WriteAt($"{gamer.GetInfo()} added", foregroundColor: ConsoleColor.Green);
    }

    public void Remove()
    {
        int id = Helper.ReadInt("Id: ");

        if (TryFindGamer(id, out Gamer gamer))
        {
            Helper.WriteAt($"{gamer.GetInfo()} removed succesfully", foregroundColor: ConsoleColor.Green);
            _gamers.Remove(gamer);
        }
    }

    public void Ban()
    {
        int id = Helper.ReadInt("Id: ");

        if (TryFindGamer(id, out Gamer gamer))
        {
            gamer._isBanned = true;
            Helper.WriteAt($"{gamer.GetInfo()} banned succesfully", foregroundColor: ConsoleColor.Green);
        }
    }

    public void Unban()
    {
        int id = Helper.ReadInt("Id: ");

        if (TryFindGamer(id, out Gamer gamer))
        {
            Helper.WriteAt($"Unbanned succesfully", foregroundColor: ConsoleColor.Green);
            gamer._isBanned = false;
        }
    }

    private bool TryFindGamer(int idNumber, out Gamer foundGamer)
    {
        foundGamer = null;

        foreach (Gamer gamer in _gamers)
        {
            if (gamer.Id.Number == idNumber)
            {
                foundGamer = gamer;
                return true;
            }
        }

        Helper.WriteAt($"Gamer with id {idNumber} doesn't exist", foregroundColor: ConsoleColor.Red);

        return false;
    }
}

class Gamer
{
    public bool _isBanned;

    private string _name;
    private int _level;

    public Gamer(string name, int level)
    {
        Id = new Id();

        _name = name;
        _level = level;

        _isBanned = false;
    }

    public Id Id { get; private set; }

    public string GetInfo()
    {
        string bannedStatus = _isBanned ? "Banned" : "Not banned";

        return $"{Id.Number}. {_name} - level {_level} ({bannedStatus})";
    }
}

public class Id
{
    private static int _nextId = 0;

    public Id()
    {
        Number = Interlocked.Increment(ref _nextId);
    }

    public int Number { get; private set; }
}

class Helper
{
    public static string ReadString(string helpText, int length = 16)
    {
        WriteAt(helpText, foregroundColor: ConsoleColor.Cyan, isNewLine: true);

        int fieldStartY = Console.CursorTop;
        int fieldStartX = Console.CursorLeft;

        WriteAt(new string(' ', length), backgroundColor: ConsoleColor.DarkGray, isNewLine: false, xPosition: fieldStartX);

        Console.SetCursorPosition(fieldStartX + 1, fieldStartY);

        ConsoleColor color = Console.BackgroundColor;
        Console.BackgroundColor = ConsoleColor.DarkGray;

        string input = Console.ReadLine();

        Console.BackgroundColor = color;

        return input;
    }

    public static int ReadInt(string text, int fieldLength = 16)
    {
        int result;

        while (int.TryParse(ReadString(text, fieldLength), out result) == false) ;

        return result;
    }

    public static void WriteTitle(string title, bool isSecondary = false)
    {
        ConsoleColor backgroundColor = isSecondary ? ConsoleColor.DarkGray : ConsoleColor.Gray;

        WriteAt($" {title} ", foregroundColor: ConsoleColor.Black, backgroundColor: backgroundColor);

        if (isSecondary == false)
            Console.WriteLine();
    }

    public static void ClearAfterKeyPress()
    {
        Console.WriteLine();
        WriteAt($"Press any key", foregroundColor: ConsoleColor.Cyan);
        Console.ReadKey(true);
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
            Console.SetCursorPosition(xPosition ?? xStart, yPosition ?? yStart);

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
