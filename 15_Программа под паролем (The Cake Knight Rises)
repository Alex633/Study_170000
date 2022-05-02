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
            string correctPassword = "rachel";
            string password = "";
            string secretMSG = "i am the darkness";
            int attempts = 3;

            Console.WriteLine("Where is the detonator?");
            while (attempts > 0 & password != correctPassword)
            {
                password = Console.ReadLine();
                if (password == correctPassword)
                {
                    Console.WriteLine(secretMSG);
                }
                else
                {
                    attempts--;
                    Console.WriteLine($"{attempts} attempts left");
                }
            }

            if (attempts == 0 & password != correctPassword)
            {
                Console.WriteLine("explosion is happening right now");
            }
        }
    }
}
