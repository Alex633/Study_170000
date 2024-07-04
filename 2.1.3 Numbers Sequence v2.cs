namespace millionDollarsCourses
{
    using System;

//Надо написать программу(используя циклы, обязательно пояснить выбор вашего цикла),
//чтобы она выводила следующую последовательность 5 12 19 26 33 40 47 54 61 68 75 82 89 96 103  
//Нужны переменные для обозначения чисел в условии цикла.
//Считать количество итераций не надо. Даже если максимальное число будет равно 789, в коде измениться только максимальное число

    internal class Program
    {
        static void Main()
        {
            int incrementValue = 7;
            int firstNumber = 5;
            int lastNumber = 103;

            for (int currentNumber = firstNumber; currentNumber <= lastNumber; currentNumber += incrementValue)
                Console.Write(currentNumber + " ");

            Console.WriteLine();
        }
    }
}
