using System;
using System.Collections.Generic;

interface IDamageable
{
    void TakeDamage(int damage);
}


public class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;


        Helper.ClearAfterKeyPress();
    }
}

class Arena
{
    private Fighter _firstFighter;
    private Fighter _secondFighter;

    private List<Fighter> _fighterClasses;

    private CommandLineInterface _commandInterface;

    public Arena()
    {
        IntializeFighters();
        InitializeCommands();
    }

    public void Run()
    {

        OutputFighterClasses();
        SelectFighters();

    }

    private void SelectFighters()
    {
        Helper.WriteTitle($"Выбор воинов");

        int input = Helper.ReadInt("Выберите первого воина: ");
        _firstFighter = new Fighter();
        _firstFighter = _fighterClasses[input];

        input = Helper.ReadInt("Выберите второго воина: ");
        _secondFighter = _fighterClasses[input];
    }

    private void SayHi()
    {
        Console.WriteLine("Привет!");
    }

    private void Fight()
    {
        Helper.WriteTitle($"Бой");

        while (_firstFighter.IsAlive && _secondFighter.IsAlive)
        {

        }
    }

    private void ShowFightResult()
    {
        Helper.WriteTitle($"Победитель");

        if (_firstFighter.IsAlive == false && _secondFighter.IsAlive == false)
            Helper.WriteAt("Дружба", foregroundColor: ConsoleColor.DarkRed);
        else if (_firstFighter.IsAlive == false)
            Helper.WriteAt(_firstFighter.Name);
        else if (_firstFighter.IsAlive == false)
            Helper.WriteAt(_firstFighter.Name);
    }

    private void OutputFighterClasses()
    {
        foreach (Fighter fighter in _fighterClasses)
            fighter.ShowInfo();
    }

    private void IntializeFighters()
    {
        _fighterClasses = new List<Fighter>();

    }



    private void InitializeCommands()
    {
        Dictionary<int, Command> commands = new Dictionary<int, Command>()
        {
            [1] = new Command("Сказать привет еще раз", SayHi),
            [2] = new Command("Начать бой", Fight)
        };

        _commandInterface = new CommandLineInterface(commands, "Арена");

    }
}

abstract class Fighter : IDamageable
{
    public void ShowInfo()
    {

    }

    public string Name { get; private set; }
    public int Health { get; private set; }
    public int Damage { get; private set; }
    public int Armor { get; private set; }

    public bool IsAlive => Health > 0;

    public void Attack(IDamageable fighter)
    {
        Helper.WriteAt($"({Health}) {Name} атакует на {Damage} урона");

        fighter.TakeDamage(Damage);
    }

    public void TakeDamage(int damage)
    {
        int minHealth = 0;

        int actualDamage = Math.Max(minHealth, damage - Armor);

        Health = Math.Max(minHealth, Health - actualDamage);

        if (actualDamage > 0)
            Helper.WriteAt($"({Health}) {Name} получает {actualDamage} урона");
        else
            Helper.WriteAt($"({Health}) {Name} поглащает весь урон");
    }
}

class CommandLineInterface
{
    private Dictionary<int, Command> _items;

    private bool _shouldRun;
    private bool _shouldRunOnce;

    private string _title;

    public CommandLineInterface(Dictionary<int, Command> items, string title, bool shouldRunOnce = false)
    {
        _shouldRun = true;
        _shouldRunOnce = shouldRunOnce;
        _title = title;

        _items = items;
        _items.Add(_items.Count + 1, new Command("Выход", Exit));
    }

    public int LastSelectedOption { get; private set; }
    public bool IsExitSelected => LastSelectedOption == _items.Count;

    public void Run()
    {
        _shouldRun = true;

        while (_shouldRun)
        {
            OutputCommands();

            LastSelectedOption = Helper.ReadInt("Введите команду: ");
            HandleInput();

            if (_shouldRunOnce)
                _shouldRun = false;
        }
    }

    private void HandleInput()
    {
        Console.Clear();

        if (_items.TryGetValue(LastSelectedOption, out Command item))
            item.Execute();
        else
            Helper.WriteAt("Неверная команда. Пожалуйста, попробуйте снова.", foregroundColor: ConsoleColor.Red);

        if (_shouldRun)
            Helper.ClearAfterKeyPress();
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

        Helper.WriteAt($"Закрытие {_title}");
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

    public static int ReadIntInRange(string prompt, int max, int min = 1)
    {
        int result = 0;

        bool isNumberInRangeInputed = false;

        while (isNumberInRangeInputed == false)
        {
            result = ReadInt(prompt);

            if (result > max || result < min)
                WriteAt($"Неверный ввод ({result}). Введите число от {min} до {max}");
            else
                isNumberInRangeInputed = true;
        }

        return result;
    }

    public static int ReadInt(string prompt)
    {
        int result = 0;

        bool isNumberInputed = false;

        while (isNumberInputed == false)
        {
            isNumberInputed = int.TryParse(ReadString(prompt), out result);

            if (isNumberInputed == false)
                WriteAt($"Неверный ввод ({result}). Введите число");
        }

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
        WriteAt("Нажмите любую клавишу", foregroundColor: ConsoleColor.Cyan);
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
