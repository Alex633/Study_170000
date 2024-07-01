using System;

namespace millionDollarsCourses
{
    //Вы задаете вопросы пользователю, по типу "как вас зовут", "какой ваш знак зодиака" и т.д., и пользователь отвечает на вопросы.
    //После чего, по данным, которые он ввел, формируете небольшой текст о пользователе.
    //Пример текста о пользователе
    //"Вас зовут Алексей, вам 21, вы водолей и работаете на заводе."

    internal class Program
    {
        static void Main()
        {
            string inputName;
            int inputDate;
            string inputLoversName;
            string inputLoversJob;
            int currentYear = DateTime.Now.Year;
            int age;

            Console.WriteLine("Пройдите небольшой тест:");
            Console.WriteLine("Как вас зовут?");
            inputName = Console.ReadLine();
            Console.WriteLine("Какой год вашего рождения?");
            inputDate = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Как зовут вашу вторую половинку?");
            inputLoversName = Console.ReadLine();
            Console.WriteLine("Кем он(а) работает?");
            inputLoversJob = Console.ReadLine();
            age = currentYear - inputDate;

            Console.WriteLine($"Вас зовут {inputName}. Вам {age - 1}-{age} лет. {inputLoversName} очень вас любит. И даже быть {inputLoversJob} ей/ему не помеха.");
        }
    }
}
