using System;

//Найдите минимальную степень двойки, превосходящую заданное число. 
//К примеру, для числа 4 будет 2 в степени 3, то есть 8. 4<8.
//Для числа 29 будет 2 в степени 5, то есть 32. 29<32.
//В консоль вывести число (лучше получить от Random), степень и само число 2 в найденной степени.
//Math.Pow не используйте, реализовать надо с помощью простых математических операций.

namespace millionDollarsCourses
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();
            int baseNumber = 2;
            int power = 2;
            int exponent = 1;
            int currentPowerValue = baseNumber;
            int maxNumber = 1024;

            #region generate random number
            int randomNumber = random.Next(maxNumber + 1);
            Console.WriteLine($"Number: {randomNumber}\n");
            #endregion 

            #region exponent calculation
            while (randomNumber > currentPowerValue)
            {
                currentPowerValue *= power;
                exponent++;
                Console.WriteLine($"{randomNumber} > {currentPowerValue} = {randomNumber > currentPowerValue}. Exponent: {exponent}");
            }
            #endregion 

            #region output the result
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"\nMinimum power of {baseNumber} that is greater than {randomNumber}: {baseNumber}^{exponent}\n");
            Console.ResetColor();
            #endregion
        }
    }
}
