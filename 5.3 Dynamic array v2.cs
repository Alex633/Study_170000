//В массивах вы выполняли задание "Динамический массив"
//Используя всё изученное, напишите улучшенную версию динамического массива(не обязательно брать своё старое решение)
//Задание нужно, чтобы вы освоились с List и прощупали его преимущество. 
//Проверка на ввод числа обязательна.
//Пользователь вводит числа, и программа их запоминает. 
//Как только пользователь введёт команду sum, программа выведет сумму всех введенных чисел. 
//Выход из программы должен происходить только в том случае, если пользователь введет команду exit.

using System;
using System.Collections.Generic;

public class Program
{
    private static void Main()
    {
        List<int> numbers = new List<int>();

        bool isRunning = true;

        while (isRunning)
        {
            OutputNumbers(numbers);
            DisplayMenu();

            int input = ReadInt("Input a command: ");
            isRunning = HandleInput(numbers, input);

            ClearAfterKeyPress();
        }

        Console.WriteLine("Bye");
    }

    public static void OutputNumbers(List<int> numbers)
    {
        Console.Write("Inputed numbers: ");

        if (numbers?.Count == 0)
            Console.Write($"-");

        foreach (int number in numbers)
            Console.Write($"{number} ");

        Console.WriteLine("\n");
    }

    public static void DisplayMenu()
    {
        string addNumberDescription = "Add number";
        string sumDescription = "Numbers sum";
        string exitDescription = "Exit";

        List<string> commands = new List<string>()
        {
            addNumberDescription,
            sumDescription,
            exitDescription,
        };

        for (int i = 0; i < commands.Count; i++)
        {
            int commandNumber = i + 1;
            Console.WriteLine($"{commandNumber} - {commands[i]}");
        }
    }

    public static bool HandleInput(List<int> numbers, int input)
    {
        const int CommandAddNumber = 1;
        const int CommandSum = 2;
        const int CommandExit = 3;

        switch (input)
        {
            default:
                WriteLineAt($"Command ({input}) not found", foregroundColor: ConsoleColor.Red);
                break;

            case CommandAddNumber:
                AddNumber(numbers);
                break;

            case CommandSum:
                OutputSum(numbers);
                break;

            case CommandExit:
                return false;
        }

        return true;
    }

    public static void AddNumber(List<int> numbers)
    {
        int number = ReadInt($"Input a number: ");

        numbers.Add(number);

        WriteLineAt($"{number} added", foregroundColor: ConsoleColor.Green);
    }

    public static void OutputSum(List<int> numbers)
    {
        if (numbers?.Count == 0)
        {
            WriteLineAt($"No numbers to sum", foregroundColor: ConsoleColor.Red);
            return;
        }

        int sum = 0;

        for (int i = 0; i < numbers.Count; i++)
            sum += numbers[i];

        Console.WriteLine($"Sum: {sum}");
    }

    public static void ClearAfterKeyPress()
    {
        Console.WriteLine($"Press any key");
        Console.ReadKey(true);
        Console.Clear();
    }

    public static int ReadInt(string description)
    {
        Console.Write($"{description}");

        int number;

        while (int.TryParse(Console.ReadLine(), out number) == false)
            Console.Write($"It's not a number, dummy.\n\nTry again: ");

        return number;
    }

    #region Helper
    public static void WriteLineAt(object element, int? yPosition = null, int? xPosition = null,
        ConsoleColor foregroundColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black)
    {
        Console.ForegroundColor = foregroundColor;
        Console.BackgroundColor = backgroundColor;

        int yStart = Console.CursorTop;
        int xStart = Console.CursorLeft;

        bool isCustomPosition = yPosition.HasValue || xPosition.HasValue;

        if (isCustomPosition)
            Console.SetCursorPosition(xPosition ?? xStart, yPosition ?? yStart);

        Console.WriteLine(element);

        Console.ResetColor();

        if (isCustomPosition)
        {
            Console.CursorTop = yStart;
            Console.CursorLeft = xStart;
        }
    }
    #endregion
}
