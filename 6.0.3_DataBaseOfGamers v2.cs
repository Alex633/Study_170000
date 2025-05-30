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

public class Program
{
    static void Main()
    {
        Database database = new Database();

        DatabaseView menu = new DatabaseView(database);

        menu.Run();
    }
}

class Database
{
    private List<Gamer> _gamers;

    public Database()
    {
        _gamers = new List<Gamer>();
    }

    public bool TryOutputFull()
    {
        if (_gamers.Count == 0)
        {
            Helper.WriteAt("Empty", foregroundColor: ConsoleColor.DarkGray);
            return false;
        }

        foreach (Gamer gamer in _gamers)
        {
            ConsoleColor consoleColor = gamer.IsBanned == false ? ConsoleColor.Green : ConsoleColor.Red;
            Helper.WriteAt(gamer.GetInfo(), foregroundColor: consoleColor);
        }

        Console.WriteLine();

        return true;
    }

    public void AddGamer()
    {
        string name = Helper.ReadString("Name: ");
        int level = Helper.ReadInt("Level: ");

        Gamer gamer = new Gamer(name, level);

        _gamers.Add(gamer);

        Helper.WriteAt($"{gamer.GetInfo()} added", foregroundColor: ConsoleColor.Green);
    }

    public void RemoveGamer()
    {
        if (TryOutputFull() == false || TryReadGuid(out Guid guid) == false)
            return;

        if (TryFindGamer(guid, out Gamer gamer))
        {
            _gamers.Remove(gamer);
            Helper.WriteAt($"{gamer.GetInfo()} removed", foregroundColor: ConsoleColor.Green);
        }
    }

    public void BanGamer()
    {
        if (TryOutputFull() == false || TryReadGuid(out Guid guid) == false)
            return;

        if (TryFindGamer(guid, out Gamer gamer))
        {
            gamer.SetBanStatus(true);
            Helper.WriteAt($"{gamer.GetInfo()} banned", foregroundColor: ConsoleColor.Red);
        }
    }

    public void UnbanGamer()
    {
        if (TryOutputFull() == false || TryReadGuid(out Guid guid) == false)
            return;

        if (TryFindGamer(guid, out Gamer gamer))
        {
            gamer.SetBanStatus(false);
            Helper.WriteAt($"{gamer.GetInfo()} unbanned", foregroundColor: ConsoleColor.Green);
        }
    }

    private bool TryFindGamer(Guid guid, out Gamer result)
    {
        result = null;

        foreach (Gamer gamer in _gamers)
        {
            if (gamer.Id == guid)
            {
                result = gamer;
                return true;
            }
        }

        Helper.WriteAt($"Gamer with id {guid} doesn't exist", foregroundColor: ConsoleColor.Red);

        return false;
    }

    private bool TryReadGuid(out Guid result)
    {
        var input = Helper.ReadString("Id: ");

        if (Guid.TryParse(input, out result))
            return true;

        Helper.WriteAt("Invalid Id format", foregroundColor: ConsoleColor.Red);
        return false;
    }

}

class Gamer
{
    private string _name;
    private int _level;

    public Gamer(string name, int level)
    {
        Id = Guid.NewGuid();

        _name = name;
        _level = level;

        IsBanned = false;
    }

    public bool IsBanned { get; private set; }

    public Guid Id { get; private set; }

    public string GetInfo()
    {
        string bannedStatus = IsBanned ? "Banned" : "Not banned";

        return $"{Id}. {_name} - level {_level} ({bannedStatus})";
    }

    public void SetBanStatus(bool isBanned)
    {
        IsBanned = isBanned;
    }
}

class DatabaseView
{
    private Dictionary<int, MenuItem> _items;

    private int _input;
    private bool _shouldRun;

    private Database _database;

    public DatabaseView(Database database)
    {
        _database = database;
        _shouldRun = true;

        InitilizeItems();
    }

    public void Run()
    {
        while (_shouldRun)
        {
            OutputCommands();

            _input = Helper.ReadInt("Input command: ");
            HandleInput();
        }
    }

    private void HandleInput()
    {
        Console.Clear();

        if (_items.TryGetValue(_input, out MenuItem item))
            item.Execute();
        else
            Helper.WriteAt("Invalid command. Please try again.", foregroundColor: ConsoleColor.Red);

        Helper.ClearAfterKeyPress();
    }


    private void OutputCommands()
    {
        Helper.WriteTitle("Gamers Database commands");

        foreach (var item in _items)
            Helper.WriteAt($"{item.Key}) {item.Value.Description}");

        Console.WriteLine();
    }

    private void Exit()
    {
        _shouldRun = false;
    }

    private void InitilizeItems()
    {
        const int CommandAdd = 1;
        const int CommandRemove = 2;
        const int CommandBan = 3;
        const int CommandUnban = 4;
        const int CommandFull = 5;
        const int CommandExit = 6;

        _items = new Dictionary<int, MenuItem>()
        {
            [CommandAdd] = new MenuItem("Add gamer", _database.AddGamer),
            [CommandRemove] = new MenuItem("Remove gamer", _database.RemoveGamer),
            [CommandBan] = new MenuItem("Ban gamer", _database.BanGamer),
            [CommandUnban] = new MenuItem("Unban gamer", _database.UnbanGamer),
            [CommandFull] = new MenuItem("Output full database", () => _database.TryOutputFull()),
            [CommandExit] = new MenuItem("Exit", Exit),
        };
    }
}

class MenuItem
{
    private readonly Action _action;

    public MenuItem(string description, Action action)
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
    public static string ReadString(string helpText, ConsoleColor primary = ConsoleColor.Cyan, ConsoleColor secondary = ConsoleColor.Black)
    {
        ConsoleColor backgroundColor = Console.BackgroundColor;

        WriteAt(helpText, foregroundColor: primary, isNewLine: true);

        int fieldStartY = Console.CursorTop;
        int fieldStartX = Console.CursorLeft;

        WriteAt(new string(' ', helpText.Length - 1), backgroundColor: primary, isNewLine: false, xPosition: fieldStartX);

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
