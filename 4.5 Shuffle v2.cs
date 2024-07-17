//Реализуйте функцию Shuffle, которая перемешивает элементы массива в случайном порядке. 

using System;

namespace millionDollarsCourses
{
    internal class Program
    {
        static void Main()
        {
            int[] numbers = { 1, 2, 3, 4, 5 };

            Console.WriteLine("Before shuffle: ");
            WriteArray(numbers);
            Shuffle(numbers);
            Console.WriteLine();
            Console.WriteLine("After shuffle: ");
            WriteArray(numbers);
            Console.WriteLine();
        }

        public static void Shuffle(int[] numbers)
        {
            Random random = new Random();

            for (int i = 0; i < numbers.Length; i++)
            {
                int randomIndex = random.Next(numbers.Length);

                SwapValues(ref numbers[i], ref numbers[randomIndex]);
            }
        }

        public static void WriteArray(int[] numbers)
        {
            foreach (int number in numbers)
                Console.Write(number + " ");

            Console.WriteLine();
        }

        public static void SwapValues(ref int firstValue, ref int secondValue)
        {
            int tempValue = firstValue;

            firstValue = secondValue;
            secondValue = tempValue;
        }
    }
}
