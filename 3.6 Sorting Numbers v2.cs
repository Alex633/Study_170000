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

            bool isAraySorted = false;
            int swapsCount;

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
            while (isAraySorted == false)
            {
                swapsCount = 0; 

                for (int i = 0; i < numbers.Length - 1; i++)
                {
                    if (numbers[i] > numbers[i + 1])
                    {
                        int tempNumber = numbers[i];
                        numbers[i] = numbers[i + 1];
                        numbers[i + 1] = tempNumber;
                        swapsCount++;
                    }
                }

                if (swapsCount == 0)
                    isAraySorted = true;
            }
            #endregion

            foreach (int i in numbers)
                Console.Write($"{i} ");
        }
    }
}
