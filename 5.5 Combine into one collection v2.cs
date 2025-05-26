// if (isMentor)
//     isNextSnippetVisible = false; 
//  {
//Есть два массива строк. Надо их объединить в одну коллекцию, 
//исключив повторения, не используя Linq. Пример:
//{ "1", "2", "1"} + { "3", "2"} => { "1", "2", "3"}
//  }

using System;
using System.Collections.Generic;

public class Program
{
    static Random random = new Random();

    private static void Main()
    {
        Run();
    }

    static void Run()
    {
        bool isRun = true;

        int maxValue = 11;

        while (isRun)
        {
            string[] numbers1 =
            {
                 GetRandomNumberAsString(maxValue), GetRandomNumberAsString(maxValue), GetRandomNumberAsString(maxValue),
                 GetRandomNumberAsString(maxValue), GetRandomNumberAsString(maxValue), GetRandomNumberAsString(maxValue),
                 GetRandomNumberAsString(maxValue),
            };

            string[] numbers2 =
            {
                 GetRandomNumberAsString(maxValue), GetRandomNumberAsString(maxValue), GetRandomNumberAsString(maxValue),
                GetRandomNumberAsString(maxValue), GetRandomNumberAsString(maxValue),
            };

            List<string> mergedUniqueValues = MergeUniqueValues(numbers1, numbers2);

            WriteArray(numbers1, "1");
            WriteArray(numbers2, "2");
            WriteCollection(mergedUniqueValues, "merged");
            ClearAfterKeyPress();
        }
    }

    static List<string> MergeUniqueValues(string[] collection1, string[] collection2)
    {
        List<string> result = new List<string>();

        AddUniqueItems(collection1, result);
        AddUniqueItems(collection2, result);

        return result;
    }

    static void AddUniqueItems(string[] origin, List<string> result)
    {
        for (int i = 0; i < origin.Length; i++)
        {
            if (result.Contains(origin[i]) == false)
                result.Add(origin[i]);
        }
    }

    static string GetRandomNumberAsString(int max)
    {
        return random.Next(max).ToString();
    }

    #region Helper)
    static void WriteCollection(List<string> source, string title = null, bool shouldPrintEachOnNewLine = true)
    {
        if (title != null)
            WriteTitle(title, true);

        foreach (var item in source)
            WriteAt($"{item}", isNewLine: shouldPrintEachOnNewLine);

        Console.WriteLine();
    }

    static void WriteArray(string[] source, string title = null, bool shouldPrintEachOnNewLine = true)
    {
        if (title != null)
            WriteTitle(title, true);

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

    static void WriteTitle(string title, bool isSecondary = false)
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
