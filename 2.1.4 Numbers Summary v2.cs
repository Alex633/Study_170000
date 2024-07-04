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
            int secondNumberOfDivisible = 5;
            int summoryForFirstSequence = 0;
            int summoryForSecondSequence = 0;

            Console.Write($"Numbers divisible by {firstNumberOfDivisibility} under {randomNumber}: ");

            for (int i = 1; i <= randomNumber; i++)
            {
                if (i % firstNumberOfDivisibility == 0)
                {
                    Console.Write(i + " ");
                    summoryForFirstSequence += i;
                }
            }

            Console.WriteLine($"\nFirst Sequence Summury: {summoryForFirstSequence}");
            Console.Write($"\n\nNumbers divisible by {secondNumberOfDivisible} under {randomNumber}: ");

            for (int i = 1; i <= randomNumber; i++)
            {
                if (i % secondNumberOfDivisible == 0)
                {
                    Console.Write(i + " ");
                    summoryForSecondSequence += i;
                }
            }

            Console.WriteLine($"\nSecond Sequence Summury: {summoryForSecondSequence}\n\n");
            int totalSummory = summoryForFirstSequence + summoryForSecondSequence;
            Console.WriteLine($"Total Summury: {totalSummory}");
        }
    }
}
