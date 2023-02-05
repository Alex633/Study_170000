using System;
using System.Diagnostics;

namespace CodingCoursesButThisTimeForFree
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] array = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            WriteArray(array, "Starting Array:");
            ShuffleAllElements(ref array);
            WriteArray(array, "Shuffled Array:");
            ShuffleRandom(ref array);
            WriteArray(array, "Alternatevely Shaffled:");
        }

        static void ShuffleAllElements(ref int[] array)
        {
            int[] tempArray = new int[array.Length];
            Random random = new Random();

            for (int i = 0; i < tempArray.Length; i++)
            {
                int randomNumber = random.Next(0, array.Length);
                tempArray[i] = array[randomNumber];
                ShortenArray(ref array, randomNumber);
            }

            array = tempArray;
        }

        static void ShuffleRandom(ref int[] array)
        {
            Random random = new Random();
            int shuffleCount = random.Next(100);
            int tempElement = 0;
            Stopwatch sw = new Stopwatch();

            sw.Start();

            for (int i = 0; i < shuffleCount; i++)
            {
                int randomNumber1 = random.Next(0, array.Length);
                int randomNumber2 = random.Next(0, array.Length);
                tempElement = array[randomNumber1];
                array[randomNumber1] = array[randomNumber2];
                array[randomNumber2] = tempElement;
            }

            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
        }

        static void ShortenArray(ref int[] array, int elementIndexToDelete)
        {
            int position = 0;
            int[] tempArray = new int[array.Length - 1];

            for (int i = 0; i <= tempArray.Length; i++)
            {
                if (i != elementIndexToDelete)
                {
                    tempArray[position] = array[i];
                    position++;
                }
            }

            array = tempArray;
        }

        static void WriteArray(int[] array, string title = "")
        {
            Console.WriteLine(title);

            for (int i = 0; i < array.Length; i++)
            {
                Console.Write(array[i] + " ");
            }

            Console.WriteLine();
        }
    }
}
