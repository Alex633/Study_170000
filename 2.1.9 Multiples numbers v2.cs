namespace millionDollarsCourses
{
    using System;

    //Дано N (10 ≤ N ≤ 25). 
    //Найти количество чисел от 50 до 150 (включая эти числа), которые кратны N.
    //Операции деления(/, %) не использовать.А умножение не требуется.Посмотрите на задачу “Последовательность”
    //Число N всего одно, его надо получить в нужном диапазоне. Хоть с помощью Random, хоть ввод пользователя.

    internal class Program
    {
        static void Main()
        {
            Random random = new Random();

            int maxNumberInRangeForNNumber = 25;
            int minNumberInRangeForNNumber = 10;
            int nNumber = random.Next(minNumberInRangeForNNumber, maxNumberInRangeForNNumber + 1);
            int maxNumberInRange = 150;
            int minNumberInRange = 50;
            int multiplesCount = 0;

            Console.WriteLine($"Multiples of {nNumber} in {minNumberInRange}-{maxNumberInRange} range:\n");

            for (int i = minNumberInRange; i <= maxNumberInRange; i++)
            {
                if (i % nNumber == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(i + " ");
                    Console.ResetColor();
                }
                else
                {
                    Console.Write(i + " ");
                }
            }

            Console.WriteLine("\n\n\n" + new string('-', 100) + "\n\n");

            #region i%nNumber == 0
            int firstMultiple = minNumberInRange;
            int decreasingValue = minNumberInRange;
            bool isFirstMultipleFound = false;

            for (int valueToCheck = minNumberInRange; valueToCheck <= maxNumberInRange; valueToCheck++)
            {
                decreasingValue = valueToCheck;

                while (decreasingValue >= 0 && isFirstMultipleFound == false)
                {
                    decreasingValue -= nNumber;

                    if (decreasingValue == 0)
                    {
                        firstMultiple = valueToCheck;
                        isFirstMultipleFound = true;
                    }
                }

                //if (isFirstMultipleFound)
                //    break;
            }
            #endregion

            for (int i = firstMultiple; i <= maxNumberInRange; i += nNumber)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(i + " ");
                Console.ResetColor();
            }

            Console.WriteLine("\n\n\n" + new string('-', 100) + "\n\n");

            for (int i = nNumber; i <= maxNumberInRange; i += nNumber)
            {
                if (i >= minNumberInRange)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(i + " ");
                    Console.ResetColor();
                    multiplesCount++;
                }
            }

            Console.WriteLine("\n\nMultiples amount : " + multiplesCount);
            Console.WriteLine("\n\n\n");
        }
    }
}
