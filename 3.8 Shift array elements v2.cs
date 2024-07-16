//  Дан массив чисел. Нужно его сдвинуть циклически на указанное пользователем значение позиций влево, не используя других массивов. 
//  Пример для сдвига один раз: { 1, 2, 3, 4} => { 2, 3, 4, 1}

using System;

namespace millionDollarsCourses
{
    internal class Program
    {
        static void Main()
        {
            int[] numbers = { 1, 2, 3, 4, 5 };
            int tempNumber;

            Console.Write($"Shift value: ");
            int shiftSteps = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine($"Before: ");

            foreach (int number in numbers)
                Console.Write($"{number} ");

            Console.WriteLine();

            for (int i = 0; i < shiftSteps; i++)
            {
                for (int j = 0; j < numbers.Length - 1; j++)
                {
                    tempNumber = numbers[j];
                    numbers[j] = numbers[j + 1];
                    numbers[j + 1] = tempNumber;
                }
            }

            Console.WriteLine($"After: ");

            foreach (int number in numbers)
                Console.Write($"{number} ");

            Console.WriteLine();
        }
    }
}
