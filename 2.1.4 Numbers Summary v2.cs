namespace millionDollarsCourses
{
    using System;

    //С помощью Random получить число number, которое не больше 100. 
    //Найти сумму всех положительных чисел меньше number(включая число), которые кратные 3 или 5. 
    //(К примеру, это числа 3, 5, 6, 9, 10, 12, 15 и т.д.)

    internal class Program
    {
        static void Main()
        {
            Random random = new Random();
            int minRandomNumber = 1;
            int maxRandomNumber = 100;
            int randomNumber = random.Next(minRandomNumber, maxRandomNumber + 1);
            int firstNumberOfDivisibility = 3;
            int secondNumberOfDivisibility = 5;
            int numbersSummory = 0;

            Console.WriteLine($"Numbers divisible by {firstNumberOfDivisibility} or {secondNumberOfDivisibility} under {randomNumber}: ");

            for (int i = 1; i <= randomNumber; i++)
            {
                if (i % firstNumberOfDivisibility == 0 || i % secondNumberOfDivisibility == 0)
                {
                    Console.Write(i + " ");
                    numbersSummory += i;
                }
            }

            Console.WriteLine($"\n\nSummury: {numbersSummory}\n");
        }
    }
}
