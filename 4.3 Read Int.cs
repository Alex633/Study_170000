using System;

namespace CodingCoursesButThisTimeForFree
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string inputedStringNumber = null;
            int inputedNumber;

            InputAndConvertStringToInt(ref inputedStringNumber, out inputedNumber);
            Console.WriteLine("Your number:" + inputedNumber);
            Console.WriteLine("Your number alternatively converted, yeah:" + InputAndConvertStringToIntAlt());
            
        }

        static void InputAndConvertStringToInt(ref string inputedNumber, out int convertedNumber)
        {
            bool isConvertationSuccessful = false;
            convertedNumber = 0;

            while (isConvertationSuccessful != true)
            {
                Console.WriteLine("Enter your number:");
                inputedNumber = Console.ReadLine();

                isConvertationSuccessful = int.TryParse(inputedNumber, out convertedNumber);
                Console.Clear();

                if (isConvertationSuccessful == false)
                {
                    Console.WriteLine("Your number is " + inputedNumber +
                        "\nBut it's not a number\n" +
                        "Press something to continue, please?");
                    Console.ReadKey();
                }

                Console.Clear();
            }
        }

        static int InputAndConvertStringToIntAlt()
        {
            bool isConvertationSuccessful = false;
            string inputedNumber = null;
            int convertedNumber = 0;

            while (isConvertationSuccessful != true)
            {
                Console.WriteLine("Enter your number:");
                inputedNumber = Console.ReadLine();

                isConvertationSuccessful = int.TryParse(inputedNumber, out convertedNumber);
                Console.Clear();

                if (isConvertationSuccessful == false)
                {
                    Console.WriteLine("Your number is " + inputedNumber +
                        "\nBut it's not a number\n" +
                        "Press something to continue, please?");
                    Console.ReadKey();
                }
            }
            
            return convertedNumber;
        }
    }
}
