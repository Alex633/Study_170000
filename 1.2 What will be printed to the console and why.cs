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
            int a = 10;
            int b = 38;
            int c = (31 - 5 * a) / b;
            Console.WriteLine(c); //Ответ на выражение 0.5. Но так как int это целочисленный тип, число округлится в ближайшую сторону. Ответ 0
        }
    }
}
