using System;

namespace CodingCoursesButThisTimeForFree
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] array = new int[30];
            Random random = new Random();

            //вывод изначального массива
            Console.WriteLine("Array:");

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = random.Next(0, 10);
                Console.Write(array[i] + " ");
            }

            Console.WriteLine();

            //вывод окрашенного массива
            Console.WriteLine("\nColored array:");

            if (array[0] >= array[1])
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(array[0] + " ");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.Write(array[0] + " ");
            }

            for (int i = 1; i < array.Length - 1; i++)
            {
                if (array[i - 1] <= array[i] && array[i] >= array[i + 1])
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(array[i] + " ");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.Write(array[i] + " ");
                }
            }

            if (array[array.Length - 1] >= array[array.Length - 2])
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(array[array.Length - 1]);
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.Write(array[array.Length - 1]);
            }


            //максимальные элементы в списке
            Console.WriteLine("\n\nMax local elements:");

            if (array[0] >= array[1])
            {
                Console.Write(array[0] + " ");
            }

            for (int i = 1; i < array.Length - 1; i++)
            {
                if (array[i - 1] <= array[i] && array[i] >= array[i + 1])
                {
                    Console.Write(array[i] + " ");
                }
            }

            if (array[array.Length - 1] >= array[array.Length - 2])
            {
                Console.Write(array[array.Length - 1]);
            }

            Console.WriteLine("\n");
        }
    }
}
