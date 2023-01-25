using System;

namespace CodingCoursesButThisTimeForFree
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] array = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int movingLength = 0;
            int tempElement = 0;

            Console.WriteLine("Your array:");

            for (int i = 0; i < array.Length; i++)
            {
                Console.Write(array[i] + " ");
            }

            Console.WriteLine("\n\nOn how many steps (left) would you like to move it?\n(Zero or below zero is not allowed)");
            movingLength = Convert.ToInt32(Console.ReadLine());

            for (int i = 0; i < movingLength; i++)
            {
                for (int j = 0; j < array.Length - 1; j++)
                {
                    tempElement = array[j];
                    array[j] = array[j + 1];
                    array[j + 1] = tempElement;
                }
            }

            Console.WriteLine("\nYour moved (and really touched) array:");

            for (int i = 0; i < array.Length; i++)
            {
                Console.Write(array[i] + " ");
            }

            Console.WriteLine("\n");
        }
    }
}
