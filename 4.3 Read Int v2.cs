//Написать функцию, которая запрашивает число у пользователя (с помощью метода Console.ReadLine() )
//и пытается сконвертировать его в тип int (с помощью int.TryParse()) 
//Если конвертация не удалась у пользователя запрашивается число повторно до тех пор,
//пока не будет введено верно. После ввода, который удалось преобразовать в число, число возвращается.   
//Полученное число из функции надо в Main вывести в консоль.

//P.S. Задача решается с помощью циклов 
//P.S. Также в TryParse используется модификатор параметра out 

using System;

namespace millionDollarsCourses
{
    internal class Program
    {
        static void Main()
        {
            int userInput;

            userInput = GetNumber();
            Console.WriteLine($"{userInput}, really?");
        }

        public static int GetNumber(string message = "Input number", int maxValue = 100, int minValue = 1)
        {
            int userInput;

            Console.Write($"{message} ({minValue} - {maxValue}): ");

            while (int.TryParse(Console.ReadLine(), out userInput) == false || userInput < minValue || userInput > maxValue)
                OutputError($"Incorrect input. Please, input a number ({minValue} - {maxValue})");

            return userInput;
        }

        public static void WriteLine(string message, ConsoleColor textColor = ConsoleColor.Blue)
        {
            Console.ForegroundColor = textColor;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void OutputError(string message = "Error")
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
