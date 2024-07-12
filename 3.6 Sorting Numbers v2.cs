//Дан массив чисел (минимум 10 чисел). Надо вывести в консоль числа отсортированы, от меньшего до большего.
//Нельзя использовать Array.Sort. Используйте пузырьковую сортировку.

using System;

namespace millionDollarsCourses
{
    internal class Program
    {
        static void Main()
        {
            Random random = new Random();
            int maxRandomValue = 10;

            int arrayLength = 10;
            int[] numbers = new int[arrayLength];

            #region fill and display array
            for (int i = 0; i < numbers.Length; i++)
            {
                numbers[i] = random.Next(maxRandomValue);
                Console.Write(numbers[i] + " ");
            }
            #endregion

            Console.WriteLine();

            #region sort array
            for (int i = 0; i < numbers.Length - 1; i++)
            {
                for (int j = 0; j < numbers.Length - i - 1; j++)
                {
                    if (numbers[j] > numbers[j + 1])
                    {
                        int temp = numbers[j];
                        numbers[j] = numbers[j + 1];
                        numbers[j + 1] = temp;
                    }
                }
            }
            #endregion

            foreach (int i in numbers)
                Console.Write($"{i} ");
        }
    }
}
