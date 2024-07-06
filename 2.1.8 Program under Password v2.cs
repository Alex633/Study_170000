namespace millionDollarsCourses
{
    using System;

    //Создайте переменную типа string, в которой хранится пароль для доступа к тайному сообщению.
    //Пользователь вводит пароль, далее происходит проверка пароля на правильность, и если пароль неверный,
    //то попросите его ввести пароль ещё раз. Если пароль подошёл, выведите секретное сообщение.  
    //Если пользователь неверно ввел пароль 3 раза, программа завершается.

    internal class Program
    {
        static void Main()
        {
            string password = "1235";
            string secretMessage = "very secret message";

            int tryCount = 3;
            string userInput = null;

            while (userInput != secretMessage && tryCount > 0)
            {
                Console.Write("Password: ");
                userInput = Console.ReadLine();

                if (password != userInput)
                {
                    tryCount--;

                    if (tryCount == 0)
                        Console.WriteLine($"Wrong Password. Self destructing...");
                    else
                        Console.WriteLine($"Wrong Password. Self destruction in {tryCount} tries");
                }
                else
                {
                    Console.WriteLine(secretMessage);
                    tryCount = 0;
                }
            }
        }
    }
}
