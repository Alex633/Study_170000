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

            int rows = 4;
            int columns = 4;
            int[,] matrix = new int[rows, columns];

            int firstColumn = 0;
            int secondRow = 1;
            int secondRowSummory = -100;
            int firstColumnMultiplication = 0;

            bool isSecondSummoruNumberFound = false;
            bool isFirstColumnNumberFound = false;

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

            Console.WriteLine("\nsum: ");
            #region count summory and multiplication
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (i == secondRow)
                    {
                        if (isSecondSummoruNumberFound == false)
                        {
                            secondRowSummory = matrix[i, j];
                            isSecondSummoruNumberFound = true;
                        }
                        else
                        {
                            secondRowSummory += matrix[i, j];
                        }
                    }

                    if (j == firstColumn)
                    {
                        if (isFirstColumnNumberFound == false)
                        {
                            firstColumnMultiplication = matrix[i, j];
                            isFirstColumnNumberFound = true;
                        }
                        else
                        {
                            firstColumnMultiplication *= matrix[i, j];
                        }
                    }
                }
            }
            #endregion

            Console.WriteLine($"\n\nSecond row summory: {secondRowSummory}\n" +
                $"First column multiplication: {firstColumnMultiplication}");
        }
    }
}
