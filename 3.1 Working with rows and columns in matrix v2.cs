using System;

//Дан двумерный массив.
//Вычислить сумму второй строки и произведение первого столбца. Вывести исходную матрицу и результаты вычислений. 

namespace millionDollarsCourses
{
    internal class Program
    {
        static void Main()
        {
            Random random = new Random();
            int minRandomValue = 1;
            int maxRandomValue = 9;

            int rows = 2;
            int columns = 3;
            int[,] matrix = new int[rows, columns];
            int firstColumn = 0;
            int secondRow = 1;
            int secondRowSummory = 0;
            int firstColumnMultiplication = 1;

            #region fill and display matrix
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] = random.Next(minRandomValue, maxRandomValue + 1);
                    Console.Write(matrix[i, j] + " ");
                }

                Console.WriteLine();
            }
            #endregion

            #region count summory
            for (int i = 0; i < matrix.GetLength(1); i++)
                secondRowSummory += matrix[secondRow, i];
            #endregion

            #region count multiplication
            for (int i = 0; i < matrix.GetLength(0); i++)
                firstColumnMultiplication *= matrix[i, firstColumn];
            #endregion

            Console.WriteLine($"\nSecond row summory: {secondRowSummory}\n" +
                            $"First column multiplication: {firstColumnMultiplication}");
        }
    }
}
