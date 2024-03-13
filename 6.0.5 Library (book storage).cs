using System;
using System.Collections.Generic;

//Создать хранилище книг.
//Каждая книга имеет название, автора и год выпуска (можно добавить еще параметры).
//В хранилище можно добавить книгу, убрать книгу, показать все книги и показать книги по указанному параметру (по названию, по автору, по году выпуска).
//Про указанный параметр, к примеру нужен конкретный автор, выбирается показ по авторам, запрашивается у пользователя автор и показываются все книги с этим автором. 

//todo: 
//      - searches
//      - 
//      - 

namespace CsRealLearning
{
    internal class Program
    {
        static void Main()
        {
            Librarian librarian = new Librarian();


            librarian.Work();

        }

        class Librarian
        {
            Library library = new Library("Library™");
            const string CommandAdd = "1";
            const string CommandRemove = "2";
            const string CommandSearch = "3";
            const string CommandShowAllBooks = "4";
            const string CommandSayGoodbye = "`";

            public void Greet()
            {
                Console.WriteLine($"Librarian <with monotonomus voice>:\n" +
                    $"Welcome to our library {library.Name}, traveler. What can I do for you today? ");
            }

            public void ShowCommands()
            {
                Console.WriteLine($"\n({CommandAdd}) Add a Book\n" +
                                    $"({CommandRemove}) Borrow a Book\n" +
                                    $"({CommandSearch}) Search a Book\n" +
                                    $"({CommandShowAllBooks}) See all Books\n" +
                                    $"({CommandSayGoodbye}) Leave this weardo");
            }

            public void HandleCommand()
            {
                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case CommandAdd:
                        library.AddBook();
                        break;
                    case CommandRemove:
                        library.RemoveBook();
                        break;
                    case CommandSearch:
                        library.SearchBook();
                        break;
                    case CommandShowAllBooks:
                        library.ShowAllBooks(true);
                        break;
                    case CommandSayGoodbye:
                        library.SayGoodbye();
                        break;
                }
            }

            public void Work()
            {
                Greet();

                while (library.IsOpen)
                {
                    ShowCommands();
                    HandleCommand();
                }
            }
        }

        class Library
        {
            private List<Book> _books = new List<Book>();

            public string Name { get; private set; }
            public bool IsOpen { get; private set; }


            public Library(string name)
            {
                Name = name;
                IsOpen = true;
                _books.Add(new Book("Leviathan Wakes", "James Corey", 2011));
                _books.Add(new Book("The Hitchhiker's Guide to the Galaxy", "Douglas Adams", 1979));
                _books.Add(new Book("To Kill a Mockingbird", "Harper Lee", 1960));
                _books.Add(new Book("1984", "George Orwell", 1949));
                _books.Add(new Book("The Great Gatsby", "F. Scott Fitzgerald", 1925));
                _books.Add(new Book("The Catcher in the Rye", "J.D. Salinger", 1951));
                _books.Add(new Book("The Lord of the Rings", "J.R.R. Tolkien", 1954));
                _books.Add(new Book("Pride and Prejudice", "Jane Austen", 1813));
                _books.Add(new Book("The Chronicles of Narnia", "C.S. Lewis", 1950));
                _books.Add(new Book("Brave New World", "Aldous Huxley", 1932));
                _books.Add(new Book("Moby-Dick", "Herman Melville", 1851));
                _books.Add(new Book("Frankenstein", "Mary Shelley", 1818));
                _books.Add(new Book("Dracula", "Bram Stoker", 1897));
                _books.Add(new Book("The Picture of Dorian Gray", "Oscar Wilde", 1890));
            }

            public void AddBook()
            {
                Console.WriteLine("\nLibrarian:\nWhat is this book title?");
                string title = Console.ReadLine();
                Console.WriteLine("\nLibrarian:\nWho wrote it?");
                string author = Console.ReadLine();
                Console.WriteLine("\nLibrarian:\nWhen was this book published");

                int releaseYear = 0;
                bool succesfulInput = false;
                int warnings = 0;

                while (!succesfulInput)
                {
                    if (Int32.TryParse(Console.ReadLine(), out releaseYear) && releaseYear > 0)
                    {
                        succesfulInput = true;
                    }
                    else
                    {
                        if (warnings == 0)
                            Console.WriteLine($"\nAhm, I'dont think so. You have {++warnings} warning, mister. I'll ask gain. When was this book published?");
                        else
                            Console.WriteLine($"\n{++warnings} warnings now. When was this book published? I won't let you go");
                    }
                }

                _books.Add(new Book(title, author, releaseYear));
                Console.WriteLine($"\nLibrarian:\n" +
                    $"I'll take {title} by {author} ({releaseYear}) to my library. Thank you for using {Name}. Eshneshto?");
                Custom.PressAnythingToContinue();
            }

            public void RemoveBook()
            {
                Console.WriteLine("\nWhich one?");
                ShowAllBooks(false);

                if (Int32.TryParse(Console.ReadLine(), out int userInput))
                {
                    Console.WriteLine($"\nHere, it's yours. You better bring it back <she puts the book on the table>");
                    _books[userInput - 1].ShowInfo();
                    _books.RemoveAt(userInput - 1);
                }
                else
                {
                    Console.WriteLine($"\nI don't know what's wrong with you mister. Did your wife left you or something? But there is no book {userInput}");
                }

                Console.WriteLine($"\nLibrarian:\n" +
                                     $"Thank you for using {Name}. Eshneshto?");
                Custom.PressAnythingToContinue();
            }

            public void ShowAllBooks(bool StopUser)
            {
                int count = 0;
                Console.WriteLine();

                foreach (Book book in _books)
                {
                    Console.Write(++count + ". ");
                    book.ShowInfo();
                }

                if (StopUser)
                    Custom.PressAnythingToContinue();
            }

            public void SearchBook()
            {
                string userInput;
                bool isFound = false;

                Console.WriteLine($"\nWhat do you want to find?");
                userInput = Console.ReadLine();

                Console.WriteLine($"\nBooks, that contain {userInput}:");

                foreach (Book book in _books)
                {
                    if (book.Title == userInput || book.Author == userInput ||
                        (Int32.TryParse(userInput, out int userInputNumber) && userInputNumber == book.ReleaseYear))
                    {
                        book.ShowInfo();
                        isFound = true;
                    }
                }

                if (!isFound)
                    Console.WriteLine("Nothing found");

                Custom.PressAnythingToContinue();

            }

            public void SayGoodbye()
            {
                Console.WriteLine($"\nLibrarian:\n" +
                    $"I wish you enjoyed your visit to {Name}. We hope to see you again very soon. Tomorrow. Bye");
                IsOpen = false;
            }
        }

        class Book
        {
            public string Title { get; private set; }
            public string Author { get; private set; }
            public int ReleaseYear { get; private set; }

            public Book(string title, string author, int releaseYear)
            {
                Title = title;
                Author = author;

                if (releaseYear < 0)
                {
                    throw new ArgumentException("Release year cannot be negative");
                }
                ReleaseYear = releaseYear;
            }

            public void ShowInfo()
            {
                Console.WriteLine($"{Title} by {Author} ({ReleaseYear})");
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
