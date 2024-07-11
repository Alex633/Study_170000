using System;

namespace CodingCoursesButThisTimeForFree
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] array = { 3, 9, 7, 5, 9, 9, 9, 2, 4, 2 };
            int arrayElementTemp = 0;
            int sortingCount = 0;
            bool isSorted = false;
            Random random = new Random();

            Console.WriteLine("Starting array:");
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = random.Next(0, 10);
                Console.Write(array[i] + " ");
            }

            while (isSorted == false)
            {
                for (int i = 0; i < array.Length - 1; i++)
                {
                    if (array[i] > array[i + 1])
                    {
                        arrayElementTemp = array[i];
                        array[i] = array[i + 1];
                        array[i + 1] = arrayElementTemp;
                        sortingCount++;
                    }

                    if (array.Length - 2 == i && sortingCount > 0)
                    {
                        sortingCount = 0;
                    }
                    else if (array.Length - 2 == i && sortingCount == 0)
                    {
                        isSorted = true;
                    }
                }
            }

            Console.WriteLine("\n\nSorted array:");

            for (int i = 0; i < array.Length; i++)
            {
                Console.Write(array[i] + " ");
            }

            Console.WriteLine();
        }
    }
}
