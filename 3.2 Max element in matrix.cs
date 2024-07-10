using System;

namespace LearningLaptop
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[,] arrayA = new int[5, 5];
            Random random = new Random();
            int maxValueElement = int.MinValue;

            for (int i = 0; i < arrayA.GetLength(0); i++)
            {
                for (int j = 0; j < arrayA.GetLength(1); j++)
                {
                    arrayA[i, j] = random.Next(0, 10);
                    Console.Write(arrayA[i, j]);
                }
                Console.WriteLine();
            }

            foreach (int elementValue in arrayA)
            {
                if (maxValueElement < elementValue)
                {
                    maxValueElement = elementValue;
                }
            }

            Console.WriteLine("\nMax Value Element:\n" + maxValueElement + "\n\nNew array:");

            for (int i = 0; i < arrayA.GetLength(0); i++)
            {
                for (int j = 0; j < arrayA.GetLength(1); j++)
                {
                    if (maxValueElement == arrayA[i,j])
                    {
                        arrayA[i, j] = 0;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(arrayA[i, j]);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.Write(arrayA[i, j]);
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
