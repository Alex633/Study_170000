//ДЗ: Работа со свойствами
//Создать класс игрока, у которого есть данные с его положением в X, Y и своим символом.
//Создать класс отрисовщик, с методом, который получает игрока и отрисовывает его. 
//Используйте автореализуемое свойство.

using System;

public class Program
{
    static void Main()
    {
        int yPlayer = 16;
        int xPlayer = 56;
        Gamer gamer = new Gamer('G', new Point(yPlayer, xPlayer));

        Renderer renderer = new Renderer();

        Helper.WriteTitle("level 1");
        renderer.Draw(gamer);

        Helper.ClearAfterKeyPress();
    }
}

class Gamer
{
    public Gamer(char symbol, Point point)
    {
        Symbol = symbol;
        Point = point;
    }

    public Point Point { get; private set; }
    public char Symbol { get; private set; }
}

class Point
{
    private int _y;
    private int _x;

    public Point(int y, int x)
    {
        _y = 0;
        _x = 0;

        Set(y, x);
    }

    public int Y
    {
        get => _y;
        private set => _y = Console.BufferHeight > value ? value : Console.BufferHeight - 1;
    }

    public int X
    {
        get => _x;
        private set => _x = Console.BufferWidth > value ? value : Console.BufferWidth - 1;
    }

    public void Set(int y, int x)
    {
        Y = y;
        X = x;
    }
}

class Renderer
{
    public void Draw(Gamer gamer)
    {
        Helper.WriteAt(gamer.Symbol, gamer.Point.Y, gamer.Point.X);
    }
}

class Helper
{
    public static void ClearAfterKeyPress()
    {
        Console.WriteLine();
        WriteAt($"Press any key", foregroundColor: ConsoleColor.Cyan);
        Console.ReadKey(true);
        Console.Clear();
    }

    public static void WriteTitle(string title, bool isSecondary = false)
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
}
