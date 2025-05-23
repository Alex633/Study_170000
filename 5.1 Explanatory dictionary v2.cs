//Создать программу, которая принимает от пользователя слово и выводит его значение. 
//Если такого слова нет, то следует вывести соответствующее сообщение.

using System;
using System.Collections.Generic;

public class Program
{
    private static void Main()
    {
        Dictionary<string, int> words = new Dictionary<string, int>()
        {
            { "one", 1 },
            { "two", 2 },
            { "three", 3 }
        };

        Console.Write("Input key: ");
        OutputValueIfExists(Console.ReadLine(), words);
    }

    public static void OutputValueIfExists(string key, Dictionary<string, int> source)
    {
        if (source.TryGetValue(key, out int result))
            WriteLineAt($"{result}", foregroundColor: ConsoleColor.Green);
        else
            WriteLineAt($"Your search ({key}) did not match any keys", foregroundColor: ConsoleColor.Red);
    }

    #region Helper
    public static void WriteLineAt(object element, int? yPosition = null, int? xPosition = null,
        ConsoleColor foregroundColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black)
    {
        Console.ForegroundColor = foregroundColor;
        Console.BackgroundColor = backgroundColor;

        if (yPosition.HasValue || xPosition.HasValue)
            Console.SetCursorPosition(xPosition ?? 0, yPosition ?? 0);

        Console.WriteLine(element);

        Console.ResetColor();
    }
    #endregion
}
