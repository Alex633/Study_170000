using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _170000Courses
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool doHeOrShe;
            string answer;

            int inputNumber;
            int multiplier = 2;
            int numberAbove = 2;
            int finalDegree = 1;

            Console.WriteLine("Enter your favorite number:");
            inputNumber = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Do you want to know minimalnuyu stepen dvoiku of your favorite number?");
            answer = Console.ReadLine();

            if (answer == "yes")
            {
                doHeOrShe = true;
            }
            else
            {
                doHeOrShe = false;
            }

            if (doHeOrShe == false)
            {
                Console.WriteLine("okay...");
            }

            else
            {
                Console.WriteLine();
                while (numberAbove <= inputNumber)
                {
                    Console.WriteLine($"Number {numberAbove} <= {inputNumber}");
                    numberAbove *= multiplier;
                    finalDegree++;
                    doHeOrShe = false;
                }

                if (doHeOrShe == false)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Number {numberAbove} > {inputNumber}. Number Found!");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.WriteLine($"Your favorite number bellow two");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"Congratiolation?");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine($"\nYour number was: {inputNumber}\n" +
                $"Degree was: {finalDegree}\n" +
                $"Minimal number that is degree of two above your number is: {numberAbove}");
            }
        }
    }
}
