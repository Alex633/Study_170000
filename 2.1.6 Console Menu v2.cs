namespace millionDollarsCourses
{
    using System;

    //    При помощи всего, что вы изучили, создать приложение, которое может обрабатывать команды.
    //    Т.е.вы создаете меню, ожидаете ввода нужной команды, после чего выполняете действие, которое присвоено этой команде.
    //    Программа не должна завершаться после ввода, пользователь сам должен выйти из программы при помощи команды.
    //    Меню должно содержать следующие команды:
    //- пара команд на вывод разного текста
    //- команда показать случайное число
    //- команда очистить консоль
    //    - команда выхода
    //    Если решение строится на switch, то принято работать с константами (в остальных случаях объявляются переменные).
    //    Подробнее вы можете изучить в статье Использование констант

    internal class Program
    {
        static void Main()
        {
            const string CommandHelp = "help";
            const string CommandExit = "exit";
            const string CommandOutputText = "output text";
            const string CommandSaySomethingNice = "say something nice about me";
            const string CommandDisplayRandomNumber = "display number";
            const string CommandClearConsole = "clear console";

            Random random = new Random();
            string userInput = null;
            int randomNumber;

            Console.WriteLine("\nhi there, hello! type anything you want me to do (type help to scream for help)");

            while (userInput != CommandExit)
            {
                Console.Write("\nto do: ");
                userInput = Console.ReadLine().ToLower();

                switch (userInput)
                {
                    case CommandHelp:
                        Console.WriteLine($"\ntype {CommandHelp} to get help\n" +
                            $"type {CommandOutputText} to get me to write something\n" +
                            $"type {CommandSaySomethingNice} to ask me to say something nice about you\n" +
                            $"type {CommandDisplayRandomNumber} to see the coolest random number\n" +
                            $"type {CommandClearConsole} to forget everything\n" +
                            $"type {CommandExit} to leave me (please don't)\n");
                        break;
                    case CommandExit:
                        Console.WriteLine($"\nwait, don't lea");
                        userInput = CommandExit;
                        break;
                    case CommandOutputText:
                        Console.WriteLine($"\ni don't know what text do you want me to write here");
                        break;
                    case CommandSaySomethingNice:
                        Console.WriteLine($"\ni mean i don't know you. you might be a terrible human being");
                        break;
                    case CommandDisplayRandomNumber:
                        randomNumber = random.Next();
                        Console.WriteLine($"\n{randomNumber} is the coolest random number. pretty cool, right? right?...");
                        break;
                    case CommandClearConsole:
                        Console.Clear();
                        Console.WriteLine("\neverything is forgotten");
                        Console.WriteLine("press anything to continue");
                        Console.ReadKey(true);
                        Console.Clear();
                        Console.WriteLine("\nhey, do i know you? want me to do anything?");
                        break;
                    default:
                        Console.WriteLine("\nanything except that! type help to see what i can do");
                        break;
                }
            }
        }
    }
}
