namespace millionDollarsCourses
{
    using System;

    //Написать программу, которая будет выполняться до тех пор, пока не будет введено слово exit.
    //Помните, цикл работает, пока выполняется условие.А противоположное отвечает за выход.
    //Это надо, чтобы любой разработчик взглянув на ваш код, понял четкие границы вашего цикла.

    internal class Program
    {
        static void Main()
        {
            const string ExitCommand = "exit";
            
            string userInput = null;

            while (userInput != ExitCommand)
            {
                Console.WriteLine("Input command: ");
                userInput = Console.ReadLine().ToLower();
            }
        }
    }
}
