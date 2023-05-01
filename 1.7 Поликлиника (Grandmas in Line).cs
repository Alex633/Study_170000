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
            int people;
            int waitingTime = 10;

            Console.WriteLine("How Many Old Ladies do you see before you, o Mighty Hero?");
            people = Convert.ToInt32(Console.ReadLine());
            waitingTime = waitingTime * people;

            if (people == 0) 
            {
            Console.WriteLine($"{people}? Lucky Man. Go inside");
            }
            else if (people <= 2)
            {
            Console.WriteLine($"{people}? It's not too bed. You will have to wait for {waitingTime} minutes");
            }
            else if (people >= 3 & people <= 5)
            {
            Console.WriteLine($"{people}? Uff. You will have to wait for a while. For about {waitingTime} minutes");
            }
            else if (people > 5)
            {
            Console.WriteLine($"{people}? Ok. Maybe you should just turn around. Waiting time is {waitingTime} minutes, my man...");
            }
        }
    }
}
