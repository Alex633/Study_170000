using System;

namespace LearningLaptop
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[,] arrayMatrix = new int[3, 3];
            Random random = new Random();
            bool dewIt = true;
            string playerInput = " ";

            while (dewIt == true)
            {
                Console.WriteLine("Your Random Matrix:");
                for (int i = 0; i < arrayMatrix.GetLength(0); i++)
                {
                    for (int j = 0; j < arrayMatrix.GetLength(1); j++)
                    {
                        arrayMatrix[i, j] = random.Next(1, 10);
                        Console.Write(arrayMatrix[i, j] + " ");
                    }
                    Console.WriteLine();
                }

                int secondLineSum = 0;

                for (int i = 0; i < arrayMatrix.GetLength(0); i++)
                {
                    secondLineSum += arrayMatrix[1, i];
                }

                int firstRowMultiplication = arrayMatrix[0, 0];

                for (int i = 1; i < arrayMatrix.GetLength(1); i++)
                {
                    firstRowMultiplication *= arrayMatrix[i, 0];
                }

                Console.WriteLine($"\nSummary of the SECOND LINE is {secondLineSum}.\n" +
                    $"Multiplication of the FIRST ROW is {firstRowMultiplication}.\n\n" +
                    $"Do you wanna do it again with new matrix and stuff?");
                playerInput = Console.ReadLine();
                if (playerInput == "nope")
                {
                    dewIt = false;
                }
                Console.Clear();
            }
            Console.WriteLine($"Thank you for using our service.\n" +
                    $"Good bye. And have a worse day than you had yesterday :)\n");
        }
    }
}
