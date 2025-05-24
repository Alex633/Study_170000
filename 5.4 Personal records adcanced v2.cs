// if (isMentor)
//     isNextSnippetVisible = false; 
//  {
//ДЗ: Кадровый учет продвинутый
//Перерабатываем задание “Кадровый учет”.
//У нас может быть множество должностей, без повторений. На одной должности может быть несколько сотрудников (их полное имя).
//Вам надо реализовать:
//1.Добавление сотрудника(при отсутствии должности, она добавляется)
//2.Удаление сотрудника. (при отсутствии у должности каких либо сотрудников, должность также удаляется)
//3.Показ полной информации(показ всех должностей и сотрудников по этой должности)
//Для решения задачи понадобится использовать две разные коллекции.
//  }

using System;
using System.Collections.Generic;

public class Program
{
    public static int Id = 1;

    private static void Main()
    {
        const int CommandHire = 1;
        const int CommandFire = 2;
        const int CommandOutputEmployeesInfo = 3;
        const int CommandExit = 4;

        Dictionary<string, string> employees = new Dictionary<string, string>();
        List<string> jobTitles = new List<string>();

        List<string> commandDescriptions = new List<string>()
        {
            "hire employee",
            "fire employee",
            "show all job titles and employees",
            "exit"
        };

        Dictionary<int, Func<bool>> commands = new Dictionary<int, Func<bool>>
        {
            [CommandHire] = () =>
            {
                ExecuteHireCommand(employees, jobTitles);
                return true;
            },

            [CommandFire] = () =>
            {
                ExecuteFireCommand(employees, jobTitles);
                return true;
            },

            [CommandOutputEmployeesInfo] = () =>
            {
                ExecuteOutputEmployeesInfoCommand(jobTitles, employees);
                return true;
            },

            [CommandExit] = () => false,
        };

        Run(commandDescriptions, commands);
    }

    public static void Run(List<string> commandDescriptions, Dictionary<int, Func<bool>> commands)
    {
        bool isRunning = true;

        while (isRunning)
        {
            WriteTitle("Main", false);
            WriteCommands(commandDescriptions);

            int input = ReadInt("command");
            isRunning = HandleInput(input, commands);

            if (isRunning)
                ClearAfterKeyPress();
        }

        Console.WriteLine("Bye");
        ClearAfterKeyPress();
    }

    public static void ExecuteHireCommand(Dictionary<string, string> employees, List<string> jobTitles)
    {
        WriteTitle("Hire Employee", false);

        string name = PromptForInput("name");
        string jobTitle = PromptForInput("job title");

        AddNewEmployee(name, jobTitle, employees);
        AddNewJobTitleIfNew(jobTitle, jobTitles);

        Console.WriteLine();

        WriteAt($"{name} - {jobTitle} hired as an employee", foregroundColor: ConsoleColor.Green);
    }

    public static void ExecuteFireCommand(Dictionary<string, string> employees, List<string> jobTitles)
    {
        WriteTitle("Fire Employee", false);

        if (employees == null || employees.Count == 0)
        {
            WriteAt("No one to fire. Bummer", foregroundColor: ConsoleColor.Red);
            return;
        }

        string name = PromptForInput($"name");
        employees.TryGetValue(name, out string jobTitle);

        Console.WriteLine();

        bool isFired = employees.Remove(name);

        if (isFired)
        {
            RemoveJobTitleIfUnused(jobTitle, employees, jobTitles);
            WriteAt($"{name} succesfuly fired", foregroundColor: ConsoleColor.Green);
        }
        else
        {
            WriteAt($"({name}) not found", foregroundColor: ConsoleColor.Red);
        }
    }

    public static void ExecuteOutputEmployeesInfoCommand(List<string> titles, Dictionary<string, string> employees)
    {
        WriteTitle("Displaying full info", false);

        WriteCollection(titles, "Job Title");
        Console.WriteLine();
        WriteCollection(employees, "Employees");
    }

    public static void AddNewEmployee(string name, string jobTitle, 
        Dictionary<string, string> employees)
    {
        bool isDuplicateName = employees.ContainsKey(name);

        if (isDuplicateName)
        {
            WriteAt($"Another {name} already working for you. What a coincidence!");
            name += " " + ++Id;
        }

        employees.Add(name, jobTitle);
    }

    public static void AddNewJobTitleIfNew(string jobTitle, List<string> jobTitles)
    {
        if (jobTitles.Contains(jobTitle))
            return;

        jobTitles.Add(jobTitle);

        WriteAt($"{jobTitle} added as a new job title", foregroundColor: ConsoleColor.Green);
    }

    public static void RemoveJobTitleIfUnused(string jobTitle,
        Dictionary<string, string> employees, List<string> jobTitles)
    {
        if (IsJobTitleInUse(jobTitle, employees))
            return;

        jobTitles.Remove(jobTitle);

        WriteAt($"{jobTitle} job title removed since no one is working under that title", foregroundColor: ConsoleColor.Green);
    }

    public static bool IsJobTitleInUse(string jobTitle, Dictionary<string, string> employees)
    {
        foreach (string jobTitleInUse in employees.Values)
        {
            if (jobTitleInUse == jobTitle)
                return true;
        }

        return false;
    }

    #region Helper
    public static void WriteCommands(List<string> commandsDecriptions)
    {
        WriteTitle("Commands");

        if (PrintEmptyPlaceholder(commandsDecriptions))
            return;

        for (int i = 0; i < commandsDecriptions.Count; i++)
        {
            int commandNumber = i + 1;
            Console.WriteLine($"{commandNumber} - {commandsDecriptions[i]}");
        }
    }

    public static bool HandleInput(int input, Dictionary<int, Func<bool>> commands)
    {
        if (commands.TryGetValue(input, out Func<bool> handler))
        {
            Console.Clear();
            return handler();
        }
        else
        {
            WriteAt($"Command ({input}) not found", foregroundColor: ConsoleColor.Red);
            return true;
        }
    }

    public static void WriteCollection<T>(Dictionary<T, T> source, string title)
    {
        WriteTitle(title, true);

        if (PrintEmptyPlaceholder(source))
            return;

        foreach (KeyValuePair<T, T> item in source)
            Console.WriteLine($"{item.Key} - {item.Value}");
    }

    public static void WriteCollection<T>(List<T> source, string title)
    {
        WriteTitle(title, true);

        if (PrintEmptyPlaceholder(source))
            return;

        foreach (var item in source)
            Console.WriteLine($"{item}");
    }

    public static void ClearAfterKeyPress()
    {
        Console.WriteLine();
        WriteAt($"Press any key", foregroundColor: ConsoleColor.Cyan);
        Console.ReadKey(true);
        Console.Clear();
    }

    public static int ReadInt(string fieldName)
    {
        int number = 0;
        bool isNumberInputed = false;

        while (isNumberInputed == false)
        {
            string input = PromptForInput(fieldName);

            isNumberInputed = int.TryParse(input, out number);

            if (isNumberInputed == false)
                WriteAt($"\n[{input}] is not a number, dummy", foregroundColor: ConsoleColor.Red);
        }

        Console.WriteLine();

        return number;
    }

    public static string PromptForInput(string fieldName)
    {
        WriteAt($"Input a {fieldName}: ", foregroundColor: ConsoleColor.Cyan, isNewLine: false);
        return Console.ReadLine();
    }

    public static bool PrintEmptyPlaceholder<T>(ICollection<T> collection)
    {
        if (collection == null || collection.Count == 0)
        {
            WriteAt("-", foregroundColor: ConsoleColor.DarkGray);
            return true;
        }

        return false;
    }

    public static void WriteTitle(string title, bool isSecondary = true)
    {
        ConsoleColor backgroundColor = isSecondary ? ConsoleColor.DarkGray : ConsoleColor.Gray;

        WriteAt($" {title} ", foregroundColor: ConsoleColor.Black, backgroundColor: backgroundColor);

        if (isSecondary == false)
            Console.WriteLine();
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
    #endregion
}
