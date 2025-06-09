using System;
using System.Collections.Generic;

public class Program
{
    static void Main()
    {
        Library library = new Library();

        library.Run();

        Helper.ClearAfterKeyPress();
    }
}

class Library
{
    private List<Book> _books;

    private UserCommandInterface _mainMenu;

    public Library()
    {
        InitializeCommands();

        _books = new List<Book>();
    }

    public void Run() =>
        _mainMenu.Run();

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

        List<Book> result = ReadBooksByTitle();

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

    private List<Book> ReadBooks(bool searchCondition, string title)
    {
        string input = Helper.ReadString(title);

        List<Book> matchedBooks = new List<Book>();

        foreach (Book book in _books)
        {
            if (searchCondition)
                matchedBooks.Add(book);
        }

        if (matchedBooks.Count == 0)
            Helper.WriteAt($"Books with inputed {title} doesn't exist", foregroundColor: ConsoleColor.Red);

        return matchedBooks;
    }

    private void ApplySearchType()
    {
        
    }

    private bool CancelCommandIfNoBooks()
    {
        if (_books.Count == 0)
            Helper.WriteAt($"-", foregroundColor: ConsoleColor.DarkGray);

        return _books.Count == 0;
    }

    private void InitializeCommands()
    {
        Dictionary<int, MenuItem> commandsMainMenu = new Dictionary<int, MenuItem>()
        {
            [1] = new MenuItem("Add book", AddBook),
            [2] = new MenuItem("Remove books", RemoveBooks),
            [3] = new MenuItem("Search books", FindBooks),
            [4] = new MenuItem("Show all books", () => ShowAllBooks()),
        };

        _mainMenu = new UserCommandInterface(commandsMainMenu, "Library commands");

        Dictionary<int, MenuItem> commandsSearch = new Dictionary<int, MenuItem>()
        {
            [1] = new MenuItem("Title", AddBook),
            [2] = new MenuItem("Author", RemoveBooks),
            [3] = new MenuItem("Anotation", FindBooks),
            [4] = new MenuItem("Release year", () => ShowAllBooks()),
        };

        _mainMenu = new UserCommandInterface(commandsSearch, "Search by");
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

class UserCommandInterface
{
    private Dictionary<int, MenuItem> _items;

    private int _input;
    private bool _shouldRun;

    private string _title;

    public UserCommandInterface(Dictionary<int, MenuItem> items, string title)
    {
        _shouldRun = true;
        _title = title;

        _items = items;
        _items.Add(_items.Count + 1, new MenuItem("Exit", Exit));

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
        Helper.WriteTitle(_title);

        foreach (var item in _items)
            Helper.WriteAt($"{item.Key}) {item.Value.Description}");

        Console.WriteLine();
    }

    private void Exit()
    {
        _shouldRun = false;
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
    public int Key { get; private set; }

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
