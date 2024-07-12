//Дана строка с текстом, используя метод строки String.Split() получить массив слов,
//которые разделены пробелом в тексте и вывести массив, каждое слово с новой строки.
//Ссылка на документацию: Документация о String.Split()

using System;

namespace millionDollarsCourses
{
    internal class Program
    {
        static void Main()
        {
            string text = "String with text - You win some. You lose some.";

            string[] substrings = text.Split(' ');

            foreach (string substring in substrings)
                Console.WriteLine(substring);
        }
    }
}
