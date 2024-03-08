using System;
using System.Collections.Generic;
using System.Xml.Linq;

//Создать хранилище книг.
//Каждая книга имеет название, автора и год выпуска (можно добавить еще параметры).
//В хранилище можно добавить книгу, убрать книгу, показать все книги и показать книги по указанному параметру (по названию, по автору, по году выпуска).
//Про указанный параметр, к примеру нужен конкретный автор, выбирается показ по авторам, запрашивается у пользователя автор и показываются все книги с этим автором. 

//todo: 
//      - handlecommand
//      - 
//      - 

namespace CsRealLearning
{
    internal class Program
    {
        static void Main()
        {
            Librarian librarian = new Librarian();

            librarian.Greet();
            librarian.ShowCommands();

        }

        class Librarian
        {
            Library library = new Library("Library");
            const string CommandAdd = "1";
            const string CommandRemove = "2";
            const string CommandSearch = "3";
            const string CommandExit = "0";

            public void Greet()
            {
                Console.WriteLine($"Librarian <with monotonomus voice>:\n" +
                    $"  Welcome to our library {library.Name}, traveler. What can I do for you today? ");
            }

            public void ShowCommands()
            {
                Console.WriteLine($"({CommandAdd}) Add a Book\n" +
                                    $"({CommandRemove}) Borrow a Book\n" +
                                    $"({CommandSearch}) Search a Book\n" +
                                    $"({CommandExit}) Leave this weardo");
            }

            public void HandleCommand()
            {
                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case CommandAdd:
                        break;
                    case CommandRemove:
                        break;
                    case CommandSearch:
                        break;
                    case CommandExit:
                        break;
                }
            }

        }

        class Library
        {
            private List<Book> books = new List<Book>();
            
            public string Name { get; private set; }

            public Library(string name)
            {
                Name = name;
            }

            public Library()
            {
                books.Add(new Book("Leviathan Wakes", "James Corey", 2011));
                books.Add(new Book("The Hitchhiker's Guide to the Galaxy", "Douglas Adams", 1979));
                books.Add(new Book("To Kill a Mockingbird", "Harper Lee", 1960));
                books.Add(new Book("1984", "George Orwell", 1949));
                books.Add(new Book("The Great Gatsby", "F. Scott Fitzgerald", 1925));
                books.Add(new Book("The Catcher in the Rye", "J.D. Salinger", 1951));
                books.Add(new Book("The Lord of the Rings", "J.R.R. Tolkien", 1954));
                books.Add(new Book("Pride and Prejudice", "Jane Austen", 1813));
                books.Add(new Book("The Chronicles of Narnia", "C.S. Lewis", 1950));
                books.Add(new Book("Brave New World", "Aldous Huxley", 1932));
                books.Add(new Book("Moby-Dick", "Herman Melville", 1851));
                books.Add(new Book("Frankenstein", "Mary Shelley", 1818));
                books.Add(new Book("Dracula", "Bram Stoker", 1897));
                books.Add(new Book("The Picture of Dorian Gray", "Oscar Wilde", 1890));
            }

            public void AddBook()
            {

            }

            public void RemoveBook()
            {

            }

            public void ShowAllBooks()
            {

            }

            public void SearchBook()
            {

            }
        }

        class Book
        {
            private readonly string _title;
            private readonly string _author;
            private readonly int _releaseYear;

            public Book(string title, string author, int releaseYear)
            {
                _title = title;
                _author = author;

                if (releaseYear < 0)
                {
                    throw new ArgumentException("Release year cannot be negative");
                }
                _releaseYear = releaseYear;
            }

            public void ShowInfo()
            {
                Console.WriteLine($"{_title} by {_author} ({_releaseYear})");
            }
        }
    }
}

class Custom
{
    public static void WriteInColor(string text, ConsoleColor color = ConsoleColor.DarkRed, bool customPos = false, int xPos = 0, int YPos = 0)
    {
        if (customPos)
            Console.SetCursorPosition(xPos, YPos);

        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    public static void PressAnythingToContinue(ConsoleColor color = ConsoleColor.DarkYellow, bool customPos = false, int xPos = 0, int YPos = 0, string text = "press anything to continue")
    {
        if (customPos)
            Console.SetCursorPosition(xPos, YPos);

        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
        Console.ReadKey();
        Console.Clear();
    }

    public static void WriteFilled(string text, ConsoleColor color = ConsoleColor.DarkYellow, bool customPos = false, int xPos = 0, int yPos = 0)
    {
        int borderLength = text.Length + 2;
        string filler = new string('═', borderLength);
        string topBorder = "╔" + filler + "╗";
        string line = $"║ {text} ║";
        string bottomBorder = "╚" + filler + "╝";

        Console.ForegroundColor = ConsoleColor.Black;
        Console.BackgroundColor = color;

        WriteAtPosition(xPos, yPos, topBorder);
        WriteAtPosition(xPos, yPos + 1, line);
        WriteAtPosition(xPos, yPos + 2, bottomBorder);

        Console.ResetColor();
    }

    public static void WriteAtPosition(int xPos, int yPos, string text)
    {
        Console.SetCursorPosition(xPos, yPos);
        Console.WriteLine(text);
    }
}
