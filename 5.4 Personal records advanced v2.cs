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
    const int CommandHire = 1;
    const int CommandFire = 2;
    const int CommandOutputEmployeesInfo = 3;
    const int CommandExit = 4;

    private static void Main()
    {
        Dictionary<string, List<string>> employees = new Dictionary<string, List<string>>();

        List<string> commandDescriptions = new List<string>()
        {
            "Hire employee",
            "Fire employee",
            "Show all job titles and employees",
            "Exit"
        };

        Run(commandDescriptions, employees);
    }

    static void Run(List<string> commandDescriptions, Dictionary<string, List<string>> employees)
    {
        bool isRunning = true;

        while (isRunning)
        {
            WriteTitle("Main", false);
            WriteCommands(commandDescriptions);

            int input = ReadInt("command");
            Console.Clear();

            isRunning = TryHandleInput(input, employees, commandDescriptions);

            if (isRunning)
                ClearAfterKeyPress();
        }

        Console.WriteLine("Bye");
        ClearAfterKeyPress();
    }

    static void ExecuteHireCommand(Dictionary<string, List<string>> employees, List<string> commandDescriptions)
    {
        WriteTitle(commandDescriptions[CommandHire - 1], false);

        string jobTitle = PromptForInput("job title");
        string name = PromptForInput("name");

        if (employees.TryGetValue(jobTitle, out List<string> names) == false)
        {
            employees.Add(jobTitle, new List<string>() { name });
            WriteAt($"\n{jobTitle} added as a new job title", foregroundColor: ConsoleColor.Green);
        }
        else
        {
            names.Add(name);
        }

        Console.WriteLine();

        WriteAt($"{name} hired as {jobTitle}", foregroundColor: ConsoleColor.Green);
    }

    static void ExecuteFireCommand(Dictionary<string, List<string>> employees, List<string> commandDescriptions)
    {
        WriteTitle(commandDescriptions[CommandFire - 1], false);

        if (TryPrintEmptyPlaceholder(employees, "No one to fire. Bummer"))
            return;

        string name = PromptForInput($"name");

        Console.WriteLine();

        bool isFired = RemoveEmployeeJobTitle(name, employees);
        RemoveUnusedJobTitles(employees);

        if (isFired == false)
            WriteAt($"({name}) not found", foregroundColor: ConsoleColor.Red);
    }

    static void ExecuteOutputEmployeesInfoCommand(Dictionary<string, List<string>> employees, List<string> commandDescriptions)
    {
        WriteTitle(commandDescriptions[CommandOutputEmployeesInfo - 1], false);

        if (TryPrintEmptyPlaceholder(employees))
            return;

        foreach (var jobTitleNamePair in employees)
        {
            string jobTitle = jobTitleNamePair.Key;
            List<string> names = jobTitleNamePair.Value;

            WriteAt($"{jobTitle}: ");
            WriteCollection(names);
        }
    }

    static bool RemoveEmployeeJobTitle(string name, Dictionary<string, List<string>> employees)
    {
        bool wasAnyEmployeeRemoved = false;

        foreach (KeyValuePair<string, List<string>> jobTitleNamePair in employees)
        {
            ExtractJobTitleNames(jobTitleNamePair, out string jobTitle, out List<string> names);

            if (TryRemoveAllWithSameName(name, jobTitle, names))
                wasAnyEmployeeRemoved = true;
        }

        return wasAnyEmployeeRemoved;
    }

    static bool TryRemoveAllWithSameName(string name, string jobTitle, List<string> names)
    {
        bool isAllRemoved = false;
        bool isSomeoneRemoved = false;

        while (isAllRemoved == false)
        {
            bool isRemoved = names.Remove(name);

            if (isRemoved == false)
            {
                isAllRemoved = true;
            }
            else
            {
                isSomeoneRemoved = true;
                WriteAt($"Name ({name}) fired from ({jobTitle})", foregroundColor: ConsoleColor.Green);
            }
        }

        return isSomeoneRemoved;
    }

    static void RemoveUnusedJobTitles(Dictionary<string, List<string>> employees)
    {
        List<string> jobTitlesToRemove = new List<string>();

        foreach (KeyValuePair<string, List<string>> jobTitleNamePair in employees)
        {
            ExtractJobTitleNames(jobTitleNamePair, out string jobTitle, out List<string> names);

            if (names.Count == 0)
            {
                jobTitlesToRemove.Add(jobTitle);
                WriteAt($"Job title ({jobTitle}) removed since no one left on that position",
                    foregroundColor: ConsoleColor.Green);
            }
        }

        for (int i = 0; i < jobTitlesToRemove.Count; i++)
        {
            employees.Remove(jobTitlesToRemove[i]);
        }
    }

    static void ExtractJobTitleNames(KeyValuePair<string, List<string>> jobTitleNamePair,
        out string jobTitle,
        out List<string> names)
    {
        jobTitle = jobTitleNamePair.Key;
        names = jobTitleNamePair.Value;
    }

    static bool TryHandleInput(int command, Dictionary<string, List<string>> employees, List<string>commandDescriptions)
    {
        switch (command)
        {
            case CommandHire:
                ExecuteHireCommand(employees, commandDescriptions);
                return true;

            case CommandFire:
                ExecuteFireCommand(employees, commandDescriptions);
                return true;

            case CommandOutputEmployeesInfo:
                ExecuteOutputEmployeesInfoCommand(employees, commandDescriptions);
                return true;

            case CommandExit:
                return false;

            default:
                WriteAt($"Command not found", foregroundColor: ConsoleColor.Red);
                return true;
        }
    }

    #region Helper
    static void WriteCommands(List<string> commandsDecriptions)
    {
        WriteTitle("Commands");

        if (TryPrintEmptyPlaceholder(commandsDecriptions))
            return;

        for (int i = 0; i < commandsDecriptions.Count; i++)
        {
            int commandNumber = i + 1;
            Console.WriteLine($"{commandNumber} - {commandsDecriptions[i]}");
        }
    }

    static void WriteCollection<T>(List<T> source, string title = null, bool shouldPrintEachOnNewLine = true)
    {
        if (title != null)
            WriteTitle(title, true);

        if (TryPrintEmptyPlaceholder(source))
            return;

        foreach (var item in source)
            WriteAt($"{item}", isNewLine: shouldPrintEachOnNewLine);

        Console.WriteLine();
    }

    static void ClearAfterKeyPress()
    {
        Console.WriteLine();
        WriteAt($"Press any key", foregroundColor: ConsoleColor.Cyan);
        Console.ReadKey(true);
        Console.Clear();
    }

    static int ReadInt(string fieldName)
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

    static string PromptForInput(string fieldName)
    {
        WriteAt($"Input a {fieldName}: ", foregroundColor: ConsoleColor.Cyan, isNewLine: false);
        return Console.ReadLine();
    }

    static bool TryPrintEmptyPlaceholder(Dictionary<string, List<string>> source, string description = "-")
    {
        if (source == null || source.Count == 0)
        {
            WriteAt(description, foregroundColor: ConsoleColor.DarkGray);
            return true;
        }

        return false;
    }

    static bool TryPrintEmptyPlaceholder<T>(List<T> source, string description = "-")
    {
        if (source == null || source.Count == 0)
        {
            WriteAt(description, foregroundColor: ConsoleColor.DarkGray);
            return true;
        }

        return false;
    }

    static void WriteTitle(string title, bool isSecondary = true)
    {
        ConsoleColor backgroundColor = isSecondary ? ConsoleColor.DarkGray : ConsoleColor.Gray;

        WriteAt($" {title} ", foregroundColor: ConsoleColor.Black, backgroundColor: backgroundColor);

        if (isSecondary == false)
            Console.WriteLine();
    }

    static void WriteAt(object element, int? yPosition = null, int? xPosition = null,
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
