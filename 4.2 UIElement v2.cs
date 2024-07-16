//Разработайте функцию, которая рисует некий бар (Healthbar, Manabar) в определённой позиции.
//Функция принимает некий закрашенный процент, длину бара и при необходимости дополнительные параметры.  
//При 40% бар выглядит так:  [####______] 
//Реализуйте показ данных здоровья и маны.

using System;

namespace millionDollarsCourses
{
    internal class Program
    {
        static void Main()
        {
            int hpPointsMax = 32;
            int mpPointMax = 12;
            int hpPointsCurrentPercentage = 80;
            int mpPointCurrentPercentage = 33;
            int hpBarXPosition = 80;
            int hpBarYPosition = 2;
            int mpBarXPosition = 80;
            int mpBarYPosition = 4;

            ConsoleColor hpBarColor = ConsoleColor.DarkRed;
            ConsoleColor mpBarColor = ConsoleColor.DarkBlue;

            DisplayBar(hpPointsMax, hpPointsCurrentPercentage, hpBarColor, hpBarXPosition, hpBarYPosition);
            DisplayBar(mpPointMax, mpPointCurrentPercentage, mpBarColor, mpBarXPosition, mpBarYPosition);
        }

        public static void DisplayBar(int length = 10, float currentPercentage = 25, ConsoleColor currentValueColor = ConsoleColor.DarkRed, int xPosition = 0, int yPosition = 0)
        {
            int percentageCalculator = 100;
            float currenValue = length * (currentPercentage / percentageCalculator);
            int currentXPosition = xPosition;
            int currentYPosition = yPosition;

            Console.SetCursorPosition(xPosition, yPosition);
            DrawBarSection((int)currenValue, currentValueColor);
            DrawBarSection(length - (int)currenValue, ConsoleColor.DarkGray);

            Console.ResetColor();
            Console.SetCursorPosition(currentXPosition, currentYPosition);
            Console.WriteLine();
        }

        private static void DrawBarSection(int length, ConsoleColor color)
        {
            char cell = ' ';

            Console.BackgroundColor = color;

            for (int i = 0; i < length; i++)
                Console.Write(cell);
        }
    }
}
