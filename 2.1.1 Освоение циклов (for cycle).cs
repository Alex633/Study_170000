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
            int repeatCount;
            string message;

            Console.WriteLine("Your Message Please");
            message = Console.ReadLine();
            Console.WriteLine("Repeat how many times?");
            repeatCount = Convert.ToInt32(Console.ReadLine());
            for (int i = repeatCount; i > 0; i--)
            {
                Console.WriteLine(message);
            }
        }
    }
}
