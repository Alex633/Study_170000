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
            string name;
            int age;
            
            Console.WriteLine("Just tell me your name");
            name = Console.ReadLine();
            Console.WriteLine($"{name}? I mean... Are you sure? Whatever.\nYour age now");
            age = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine($"{name} that is {age} years old. Well thats embarassing.\nI'm gonna leave now");
        }
    }
}
