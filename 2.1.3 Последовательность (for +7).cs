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
            int initialNumber = 5;
            int finaleNumber = 97;
            int step = 7;

            while (initialNumber < finaleNumber)
            {
                Console.Write(initialNumber);
                initialNumber += 7;
            }
            
            Console.WriteLine();            
            initialNumber = 5;
            
            for (int i = initialNumber; i < finaleNumber; i += step)
            {
                Console.Write($" {i}");
            }

            Console.WriteLine();
        }
    }
}
