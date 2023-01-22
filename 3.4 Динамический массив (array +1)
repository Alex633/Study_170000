using System;

namespace CodingCoursesButThisTimeForFree
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] array = new int[0];
            int[] tempArray = new int[array.Length + 1];
            string userInput = "0";
            int userNumber = 0;
            int summary = 0;

            while (userInput != "exit")
            {
                //your array
                Console.SetCursorPosition(0, 27);
                Console.WriteLine("Current array:");

                if (array.Length > 0)
                {
                    foreach (int element in array)
                    {
                        Console.Write(element + " ");
                    }
                }
                else
                {
                    Console.WriteLine("No entered numbers");
                }

                Console.SetCursorPosition(0, 0);

                //enter command
                Console.WriteLine("Enter your number or command:" +
                    "\n  sum - to write the sum of all numbers" +
                    "\n  exit - to close the program");

                //user input
                userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "sum":
                        summary = 0;
                        foreach (int element in array)
                        {
                            summary += element;
                        }

                        Console.WriteLine("sum of all your elements:" + summary);
                        break;

                    case "exit":
                        Console.WriteLine("Exiting the Program");
                        break;

                    default:
                        if (array.Length > 0)
                        {
                            for (int i = 0; i < array.Length; i++)
                            {
                                tempArray[i] = array[i];
                            }
                        }
                        userNumber = Convert.ToInt32(userInput);
                        tempArray[tempArray.Length - 1] = userNumber;
                        array = tempArray;
                        Console.WriteLine($"Your number ({userNumber}) has been added");
                        tempArray = new int[array.Length + 1];
                        break;
                }

                Console.WriteLine("Press anything to continue");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}
