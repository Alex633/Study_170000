//ДЗ: Работа с классами
//Создать класс игрока, с полями, содержащими информацию об игроке и методом,
//который выводит информацию на экран.
//В классе обязательно должен быть конструктор

using System;

public class Program
{
    static void Main(string[] args)
    {
        Gamer gamer = new Gamer("777destroyer777");

        gamer.OutputInfo();

        ClearAfterKeyPress();
    }

    class Gamer
    {
        private string _name;

        private bool _isLit;

        private int _level;

        public Gamer(string name = "ProGamer", int level = 999999999)
        {
            _name = name;
            _level = level;

            _isLit = true;
        }

        public void OutputInfo()
        {
            string coolness = _isLit ? "of course" : "no";
            WriteTitle($"{_name}");
            WriteAt($"Lv.{_level}. Is he lit: {coolness}");
        }
    }

    #region Helper)
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
