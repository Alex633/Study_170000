//Дан одномерный массив целых чисел из 30 элементов.
//Найдите все локальные максимумы и вывести их. (Элемент является локальным максимумом, если он больше своих соседей)
//Крайний элемент является локальным максимумом, если он больше своего соседа.
//Программа должна работать с массивом любого размера.
//Массив всех локальных максимумов не нужен.

using System;

namespace millionDollarsCourses
{
    internal class Program
    {
        static void Main()
        {
            Random random = new Random();
            int minRandomValue = 1;
            int maxRandomValue = 100;

            int arrayLength = 30;
            int[] array = new int[arrayLength];
            int lastIndex = array.Length - 1;

            for (int i = 0; i < array.Length; i++)
                array[i] = random.Next(minRandomValue, maxRandomValue);

            if (array[0] > array[1])
                Console.ForegroundColor = ConsoleColor.Green;

            Console.Write(array[0] + " ");
            Console.ResetColor();

            for (int i = 1; i < lastIndex; i++)
            {
                if (array[i] > array[i - 1] && array[i] > array[i + 1])
                    Console.ForegroundColor = ConsoleColor.Green;

                Console.Write(array[i] + " ");
                Console.ResetColor();
            }

            if (array[lastIndex] > array[lastIndex - 1])
                Console.ForegroundColor = ConsoleColor.Green;

            Console.Write(array[lastIndex] + " ");
            Console.ResetColor();
            Console.WriteLine();
        }
    }
}
