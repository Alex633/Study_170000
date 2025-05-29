//ДЗ: Работа со свойствами
//Создать класс игрока, у которого есть данные с его положением в X, Y и своим символом.
//Создать класс отрисовщик, с методом, который получает игрока и отрисовывает его. 
//Используйте автореализуемое свойство.

using System;

struct Vector2
{
    public int Y;
    public int X;

    public Vector2(int y, int x)
    {
        Y = y;
        X = x;
    }
}

public class Program
{
    static void Main()
    {
        int yPlayer = 16;
        int xPlayer = 56;
        Gamer gamer = new Gamer('G', new Vector2(yPlayer, xPlayer));

        Renderer renderer = new Renderer();

        Helper.WriteTitle("level 1");

        renderer.Draw(gamer);

        Helper.ClearAfterKeyPress();
    }
}

class Gamer
{
    public Gamer(char symbol, Vector2 point)
    {
        Symbol = symbol;
        Vector2 = point;
    }

    public Vector2 Vector2 { get; private set; }
    public char Symbol { get; private set; }
}

class Renderer
{
    public void Draw(Gamer gamer)
    {
        Helper.WriteAt(gamer.Symbol, gamer.Vector2.Y, gamer.Vector2.X);
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
