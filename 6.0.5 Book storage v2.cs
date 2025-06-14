using System;
using System.Collections.Generic;

public class Program
{
    static void Main()
    {
        Library library = new Library();

        Func<int, int, int> math = Sum;
        int s = math(1, 5);
        int s1 = Sum(1, 5);
        Console.WriteLine(s);
        Helper.ClearAfterKeyPress();

        library.Run();

        Helper.ClearAfterKeyPress();
    }

    static int Sum(int x, int y)
    {
        return x + y;
    }
}

class Library
{
    private List<Book> _books;

    private CommandLineInterface _mainMenu;

    private CommandLineInterface _searchFiltered;
    private const int CommandSearchByTitle = 1;
    private const int CommandSearchByAuthor = 2;
    private const int CommandSearchByAnotation = 3;
    private const int CommandSearchByReleaseYear = 4;

    public Library()
    {
        InitializeCommands();

        _books = new List<Book>();
    }

    public void Run() =>
        _mainMenu.Run();

    private void AddBook()
    {
        string title = Helper.ReadString("Title");
        string author = Helper.ReadString("Author");
        int releastYear = ReadYear();
        string anotation = Helper.ReadString("Anotation");

        _books.Add(new Book(title, author, releastYear, anotation));

        Helper.WriteAt($"Book added", foregroundColor: ConsoleColor.DarkGreen);
    }

    private void RemoveBooks()
    {
        if (TryShowAllBooks() == false)
            return;

        int input = Helper.ReadInt("Input book number: ");
        int index = input - 1;
        bool isInvalidIndex = index >= _books.Count || index < 0;

        if (isInvalidIndex)
        {
            Helper.WriteAt($"Book at {input} doesnt exist");
        }
        else
        {
            _books.RemoveAt(index);
            Helper.WriteAt($"Book at {input} removed", isNewLine: false);
        }
    }

    private void SearchBooks()
    {
        if (CancelCommandIfNoBooks())
            return;

        _searchFiltered.Run();
    }

    private void SearchBooksFiltered(string filterTitle)
    {
        List<Book> matchedBooks = new List<Book>();

        string prompt = Helper.ReadString(filterTitle);

        switch (_searchFiltered.LastSelectedOption)
        {
            case CommandSearchByTitle:
                matchedBooks = _books.FindAll(book => book.Title.ToLower().Contains(prompt.ToLower()));
                break;

            case CommandSearchByAuthor:
                matchedBooks = _books.FindAll(book => book.Author.ToLower().Contains(prompt.ToLower()));
                break;

            case CommandSearchByAnotation:
                matchedBooks = _books.FindAll(book => book.Anotation.ToLower().Contains(prompt.ToLower()));
                break;

            case CommandSearchByReleaseYear:
                matchedBooks = GetBooksByYear(prompt);
                break;
        }

        ShowBooks(matchedBooks);
    }

    private List<Book> GetBooksByYear(string prompt)
    {
        if (int.TryParse(prompt, out int year))
        {
            return _books.FindAll(book => book.ReleaseYear.Equals(year));
        }
        else
        {
            Helper.WriteAt("Incorrect year format", foregroundColor: ConsoleColor.Red);
            return null;
        }
    }

    private bool TryShowAllBooks()
    {
        if (CancelCommandIfNoBooks())
            return false;

        for (int i = 1; i <= _books.Count; i++)
        {
            Console.Write(i + ". ");
            _books[i - 1].ShowInfo();
        }

        return true;
    }

    private void ShowBooks(List<Book> books)
    {
        if (CancelCommandIfNoBooks(books))
            return;

        foreach (Book book in books)
            book.ShowInfo();
    }

    private int ReadYear()
    {
        bool isCorrectYear = false;

        int currentYear = 2027;
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

    private bool CancelCommandIfNoBooks()
    {
        if (_books.Count == 0)
            Helper.WriteAt($"No books found", foregroundColor: ConsoleColor.DarkGray);

        return _books.Count == 0;
    }

    private bool CancelCommandIfNoBooks(List<Book> books)
    {
        bool isEmpty = books == null || books.Count == 0;

        if (isEmpty)
            Helper.WriteAt($"No books found", foregroundColor: ConsoleColor.DarkGray);

        return isEmpty;
    }

    private void InitializeCommands()
    {
        Dictionary<int, MenuItem> commandsMainMenu = new Dictionary<int, MenuItem>()
        {
            [1] = new MenuItem("Add book", AddBook),
            [2] = new MenuItem("Remove books", RemoveBooks),
            [3] = new MenuItem("Search books", SearchBooks),
            [4] = new MenuItem("Show all books", () => TryShowAllBooks()),
        };

        _mainMenu = new CommandLineInterface(commandsMainMenu, "Library commands");

        Dictionary<int, MenuItem> commandsSearchFiltered = new Dictionary<int, MenuItem>()
        {
            [CommandSearchByTitle] = new MenuItem("By Title",
             () => SearchBooksFiltered("Title")),

            [CommandSearchByAuthor] = new MenuItem("By Author",
             () => SearchBooksFiltered("Author")),

            [CommandSearchByAnotation] = new MenuItem("By Annotation",
             () => SearchBooksFiltered("Anotation")),

            [CommandSearchByReleaseYear] = new MenuItem("By Release Year",
             () => SearchBooksFiltered("Release Year")),
        };

        _searchFiltered = new CommandLineInterface(commandsSearchFiltered, "Search");
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

    public void ShowInfo()
    {
        Helper.WriteAt($"{Title} by {Author} ({ReleaseYear}) - {Anotation}");
    }
}

class CommandLineInterface
{
    private Dictionary<int, MenuItem> _items;

    private bool _shouldRun;

    private string _title;

    public CommandLineInterface(Dictionary<int, MenuItem> items, string title)
    {
        _shouldRun = true;
        _title = title;

        _items = items;
        _items.Add(_items.Count + 1, new MenuItem("Exit", Exit));
    }

    public int LastSelectedOption { get; private set; }

    public void Run()
    {
        Console.Clear();

        _shouldRun = true;

        while (_shouldRun)
        {
            OutputCommands();

            LastSelectedOption = Helper.ReadInt("Input command: ");
            HandleInput();
        }
    }

    private void HandleInput()
    {
        Console.Clear();

        if (_items.TryGetValue(LastSelectedOption, out MenuItem item))
            item.Execute();
        else
            Helper.WriteAt("Invalid command. Please try again.", foregroundColor: ConsoleColor.Red);

        if (_shouldRun)
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

        Helper.WriteAt($"Closing the {_title}");
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
