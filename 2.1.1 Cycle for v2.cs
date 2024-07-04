namespace millionDollarsCourses
{
    using System;

    //При помощи циклов вы можете повторять один и тот же код множество раз.
    //Напишите простейшую программу, которая выводит указанное (установленное) пользователем сообщение заданное количество раз.
    //Количество повторов также должен ввести пользователь.

    internal class Program
    {
        static void Main()
        {
            Console.Write("Output message: ");
            string message = Console.ReadLine();
            Console.Write("Repetition amount: ");
            int repeatCount = Convert.ToInt32(Console.ReadLine());

            for (int i = 1; i <= repeatCount; i++)
                Console.WriteLine($"{i}. {message}");
        }
    }
}
