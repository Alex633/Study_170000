//В массиве чисел найдите самый длинный подмассив из одинаковых чисел.
//Дано 30 чисел. Вывести в консоль сам массив, число, которое само больше раз повторяется подряд и количество повторений.
//Дополнительный массив не надо создавать.
//Пример 1: { 5, 5, 9, 9, 9, 5, 5}
//-число 9 повторяется 3 раза подряд.
//Пример 2: { 5, 5, 5, 3, 3, 3, 3}
//-число 3 повторяется 4 раза подряд.

using System;

namespace millionDollarsCourses
{
    internal class Program
    {
        static void Main()
        {
            Random random = new Random();
            int maxRandomValue = 5;
            
            int arrayLength = 10;
            int[] array = new int[arrayLength];
            int currentRepeatingElement;
            int mostRepeatableElement;
            int repeatCount = 0;
            int maxRepeat = 0;

            #region fill array
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = random.Next(maxRandomValue);
                Console.Write(array[i] + " ");
            }
            #endregion

            #region find most repeatable elements and count repetition
            currentRepeatingElement = array[0];
            mostRepeatableElement = currentRepeatingElement;

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == currentRepeatingElement)
                {
                    repeatCount++;

                    if (maxRepeat < repeatCount)
                    {
                        maxRepeat = repeatCount;
                        mostRepeatableElement = currentRepeatingElement;
                    }
                }
                else
                {
                    repeatCount = 1;
                    currentRepeatingElement = array[i];
                }
            }
            #endregion

            Console.WriteLine($"\nElement {mostRepeatableElement} repeats {maxRepeat} times and is most repeatable element");
        }
    }
}
