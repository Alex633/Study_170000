using System;

namespace _170000Courses
{
    internal class Program
    {
        //Даны две переменные.Поменять местами значения двух переменных.Вывести на экран значения переменных до перестановки и после.
        //Два примера.
        //1. Есть две переменные имя и фамилия, они сразу инициализированные, но данные не верные, перепутанные.Вот эти данные и надо поменять местами через код.
        //2. Есть две чашки, в одном кофе, во втором чай.Вам надо поменять местами содержимое чашек

        static void Main()
        {
            string name = "Johnson";
            string surname = "Boris";
            string temp;
            int coffee = 3;
            int tea = -17;

            Console.WriteLine($"You thought your name was {name} {surname}");
            temp = name;
            name = surname;
            surname = temp;
            Console.WriteLine($"But actually it's {name} {surname}\n\n");
            Console.WriteLine($"At first you had:" +
                $"\nCoffee : {coffee}\n" +
                $"Tea : {tea}\n");
            tea = coffee + tea; 
            coffee = tea - coffee;
            tea -= coffee;
            Console.WriteLine($"But now you have:" +
                $"\nCoffee : {coffee}\n" +
                $"Tea : {tea}\n\n");
            Console.WriteLine($"Now you are in coffee debt. Was it worth it?");
        }
    }
}
