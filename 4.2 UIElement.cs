using System;

namespace CodingCoursesButThisTimeForFree
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string userInput = "";
            int currentHPPercentage = 0;
            int currentHP = 0;
            bool isNumber = false;
            int maxHP = 50;
            int minHP = 0;
            int maxPercent = 100;

            while (!isNumber)
            {
                Console.Write("Enter HP percentage: ");
                userInput = Console.ReadLine();

                if (int.TryParse(userInput, out currentHPPercentage) != true)
                {
                    colorText("That's not a int number, fool\n", ConsoleColor.Red);
                    Console.WriteLine();
                }
                else
                {
                    isNumber = true;
                    currentHP = maxHP * currentHPPercentage / maxPercent;
                }

                if (currentHP > maxHP)
                    currentHP = maxHP;
                else if (currentHP < minHP)
                    currentHP = minHP;
            }

            displayHPBarAlt(currentHP, maxHP);
            displayHPBar(currentHP, maxHP);
            Console.WriteLine("");
        }

        static void displayHPBar(int currentHP, int maxHP)
        {
            Console.SetCursorPosition(0, 28);
            Console.Write("|");

            for (int i = 0; i < maxHP; i++)
                Console.Write("_");

            Console.Write("|");

            Console.SetCursorPosition(1, 28);

            for (int i = 0; i < currentHP; i++)
                colorText("#");
        }

        static void displayHPBarAlt(int currentHP, int maxHP)
        {
            Console.SetCursorPosition(0, 25);
            Console.BackgroundColor = ConsoleColor.Gray;

            for (int i = 0; i < maxHP; i++)
                Console.Write(" ");

            Console.SetCursorPosition(0, 25);
            Console.BackgroundColor = ConsoleColor.Red;

            for (int i = 0; i < currentHP; i++)
                colorText(" ");

            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(22, 26);
            Console.WriteLine($"{currentHP} / {maxHP}");
        }

        static void colorText(string text, ConsoleColor color = ConsoleColor.Red)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
