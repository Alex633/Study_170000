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
            int randomBeggining = 1;
            int randomEnd = 28;
            int number;
            int sequenceBeggining = 100;
            int sequenceEnd = 999;
            int correctNumberCounter = 0;

            Random random = new Random();
            number = random.Next(randomBeggining, randomEnd);                                   

            Console.WriteLine(number + "\n");

            for (int i = number; i <= sequenceEnd; i += number)
            {
                if (i >= sequenceBeggining)
                {
                    Console.WriteLine("Correct:" + i);
                    correctNumberCounter++;

                }
                else
                {
                    Console.WriteLine("Wrong:" + i);
                }
            }
            
            Console.WriteLine("\n" + correctNumberCounter);
        }
    }
}
