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
            int summary = 0;

            int check3 = 3;
            int check5 = 5;

            int rand100;
            int ranMin = 0;
            int ranMax = 101;

            Random random = new Random();
            rand100 = random.Next(ranMin, ranMax);

            Console.WriteLine($"Random Number: {rand100}.\n\nNumbers devided by three and five:");

            for (int currentNumber = 0; currentNumber <= rand100; currentNumber++)
            {
                if (currentNumber % check3 == 0 || currentNumber % check5 == 0)
                {
                    Console.WriteLine(currentNumber);
                    summary += currentNumber;
                }
            }

            Console.WriteLine($"\nSummary: {summary}.");
        }
    }
}
