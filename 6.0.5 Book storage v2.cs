using System;
using System.Collections.Generic;

public class Program
{
    static void Main()
    {
        Library library = new Library();
        LibraryControl libraryControl = new LibraryControl(library);

        libraryControl.Run();

        Helper.ClearAfterKeyPress();
    }
}

class Library
{
    private List<Book> _books;

    public Library()
    {
        _books = new List<Book>();
    }

    public void AddBook()
    {
        string title = Helper.ReadString("Title");
        string author = Helper.ReadString("Author");
        int releastYear = ReadYear();
        string anotation = Helper.ReadString("Anotation");

        _books.Add(new Book(title, author, releastYear, anotation));

        Helper.WriteAt($"Book added", foregroundColor: ConsoleColor.DarkGreen);
    }

    public void RemoveBooks()
    {
        if (CancelCommandIfNoBooks())
            return;

        List<Book> result = ReadBooks();

        if (result.Count == 0)
            return;

        foreach (Book book in result)
        {
            Helper.WriteAt($"Removing ", isNewLine: false);
            book.ShowInfo();
            _books.Remove(book);
        }
    }

    public void FindBooks()
    {
        if (CancelCommandIfNoBooks())
            return;

        List<Book> result = ReadBooks();

        foreach (Book book in result)
            book.ShowInfo(true);
    }

    public void ShowAllBooks()
    {
        if (CancelCommandIfNoBooks())
            return;

        foreach (Book book in _books)
            book.ShowInfo(true);
    }

    private int ReadYear()
    {
        bool isCorrectYear = false;

        int currentYear = 2026;
        int year = 0;

        while (isCorrectYear == false)
        {
            year = Helper.ReadInt("Release Year");

            if (year < 0)
                Helper.WriteAt("Year can't be negative");
            else if (year > currentYear)
                Helper.WriteAt("Books from the future are forbidden", foregroundColor: ConsoleColor.DarkRed);
            else
                isCorrectYear = true;
        }

        return year;
    }

    private List<Book> ReadBooks()
    {
        string input = Helper.ReadString("Keywords:");

        List<Book> matchedBooks = new List<Book>();

        foreach (Book book in _books)
        {
            if (book.Title == input)
                matchedBooks.Add(book);
            else if (book.Author == input)
                matchedBooks.Add(book);
            else if (book.ReleaseYear == Convert.ToInt32(input))
                matchedBooks.Add(book);
            else if (book.Anotation == input)
                matchedBooks.Add(book);
        }

        if (matchedBooks.Count == 0)
            Helper.WriteAt($"Books with inputed keywords doesn't exist", foregroundColor: ConsoleColor.Red);

        return matchedBooks;
    }

    private bool CancelCommandIfNoBooks()
    {
        if (_books.Count == 0)
            Helper.WriteAt($"-", foregroundColor: ConsoleColor.DarkGray);

        return _books.Count == 0;
    }
}

class Book
{
    public Book(string title, string author, int releaseYear, string anotation)
    {
        Title = title;
        Author = author;
        ReleaseYear = releaseYear;
        Anotation = anotation;
    }

    public string Title { get; private set; }
    public string Author { get; private set; }
    public int ReleaseYear { get; private set; }
    public string Anotation { get; private set; }

    public void ShowInfo(bool shouldShowAnotation = false)
    {
        Helper.WriteTitle($"{Title} by {Author} ({ReleaseYear})", true);

        if (shouldShowAnotation)
            Helper.WriteAt(Anotation + "\n");
    }
}

class LibraryControl
{
    private Dictionary<int, MenuItem> _items;

    private int _input;
    private bool _shouldRun;

    private Library _library;

    public LibraryControl(Library library)
    {
        _library = library;
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
        Helper.WriteTitle("Library commands");

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
        const int CommandSearch = 3;
        const int CommandShowAll = 4;
        const int CommandExit = 5;

        _items = new Dictionary<int, MenuItem>()
        {
            [CommandAdd] = new MenuItem("Add book", _library.AddBook),
            [CommandRemove] = new MenuItem("Remove books", _library.RemoveBooks),
            [CommandSearch] = new MenuItem("Search books", _library.FindBooks),
            [CommandShowAll] = new MenuItem("Show all books", () => _library.ShowAllBooks()),
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
    private static readonly Random _random = new Random();

    public static int GetRandomInt(int minValue, int maxValue)
    {
        return _random.Next(minValue, maxValue);
    }

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
