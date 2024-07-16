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
            bool isInvalidInput = true;
            int userInput = 0;

            while (isInvalidInput)
            {
                Console.Write($"{message} ({minValue} - {maxValue}): ");

                if (int.TryParse(Console.ReadLine(), out userInput))
                {
                    if (userInput >= minValue && userInput < maxValue + 1)
                        return userInput;
                    else
                        WriteLine($"Incorrect input. Please enter a number ({minValue} - {maxValue + 1})");
                }
                else
                {
                    WriteLine($"Incorrect input. Please enter a number ({minValue} - {maxValue + 1})");
                }
            }

            return userInput;
        }

        public static void WriteLine(string message = "Incorrect input", ConsoleColor textColor = ConsoleColor.DarkRed)
        {
            Console.ForegroundColor = textColor;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
