//Пользователь вводит числа, и программа их запоминает. 
//Как только пользователь введёт команду sum, программа выведет сумму всех веденных чисел. 
//Выход из программы должен происходить только в том случае, если пользователь введет команду exit.
//Если введено не sum и не exit, значит это число и его надо добавить в массив.
//В начале цикла надо выводить в консоль все числа, которые содержатся в массиве, а значит их ввел пользователь ранее. 
//Программа должна работать на основе расширения массива. 
//Внимание, нельзя использовать List<T> и Array.Resize 

using System;

namespace millionDollarsCourses
{
    internal class Program
    {
        static void Main()
        {
            const string CommandArraySum = "sum";
            const string CommandExit = "exit";

            int[] array = new int[0];
            int[] tempArray;
            int arraySummory;

            string userInput = null;

            while (userInput != CommandExit.ToLower())
            {
                #region output current array
                Console.Write("Array: ");

                if (array.Length > 0)
                {
                    foreach (int i in array)
                        Console.Write(i + " ");
                }
                else
                {
                    Console.Write("empty");
                }
                #endregion

                #region input command
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("input command or number: ");
                userInput = Console.ReadLine();
                Console.ResetColor();
                #endregion

                #region handle command
                switch (userInput)
                {
                    case CommandExit:
                        Console.WriteLine("Closing the program...");
                        break;

                    case CommandArraySum:
                        arraySummory = 0;

                        for (int i = 0; i < array.Length; i++)
                            arraySummory += array[i];

                        Console.WriteLine($"summory of array: {arraySummory}");
                        break;

                    default:
                        tempArray = new int[array.Length + 1];

                        for (int i = 0; i < array.Length; i++)
                            tempArray[i] = array[i];

                        array = tempArray;

                        array[array.Length - 1] = Convert.ToInt32(userInput);
                        Console.WriteLine($"Your number {userInput} has been added");
                        break;
                }
                #endregion

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}
