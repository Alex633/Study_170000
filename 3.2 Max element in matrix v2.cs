using System;

//Найти наибольший элемент матрицы A(10,10) и записать ноль в те ячейки, где он находятся. 
//Вывести наибольший элемент, исходную и полученную матрицу. 
//Массив под измененную версию не нужен.

namespace millionDollarsCourses
{
    internal class Program
    {
        static void Main()
        {
            Random random = new Random();
            int minRandomValue = 1;
            int maxRandomValue = 9;

            int rows = 3;
            int columns = 5;
            int[,] matrix = new int[rows, columns];

            int maxMatrixValue = int.MinValue;

            #region fill, display, find max
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] = random.Next(minRandomValue, maxRandomValue);
                    Console.Write(matrix[i, j] + " ");

                    if (matrix[i, j] > maxMatrixValue)
                        maxMatrixValue = matrix[i, j];
                }

                Console.WriteLine();
            }
            #endregion

            #region change max elements to zero
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (maxMatrixValue == matrix[i, j])
                        matrix[i, j] = 0;
                }
            }
            #endregion

            Console.WriteLine($"\nMax value in matrix: {maxMatrixValue}");

            Console.WriteLine($"\nChanged matrix: ");

            #region display matrix
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (0 == matrix[i, j])
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(matrix[i, j] + " ");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write(matrix[i, j] + " ");
                    }
                }

                Console.WriteLine();
            }
            #endregion
        }
    }
}
