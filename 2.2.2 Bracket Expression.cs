using System;

namespace _170000Courses
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string text;

            int depthTemp = 0;
            int depth = 0;

            int overallBrackets = 0;
            int insideBrackets = 0;
            bool isInside = false;
            bool justEntered = false;
            bool isCorrectExpression = false;
            bool haveOpening = false;

            Console.WriteLine("Enter your bracket expression. I'm waiting");
            text = Console.ReadLine();

            for (int i = 0; i < text.Length; i++)
            {
                switch (text[i])
                {
                    case '(':
                        if (isInside == false)
                        {
                            isInside = true;
                            justEntered = true;
                        }
                        overallBrackets++;
                        break;
                    case ')':
                        overallBrackets--;
                        break;
                }

                //counting inside Brackets
                if (isInside == true && justEntered != true) 
                {
                    switch (text[i])
                    {
                        case '(':
                            insideBrackets++;
                            haveOpening = true;
                            break;
                        case ')':
                            if (haveOpening == true)
                            {
                                insideBrackets--;
                                depthTemp++;
                            }
                            break;
                    }
                }

                //getting out completely and counting depth
                if (overallBrackets == 0) 
                {
                    isInside = false;
                    haveOpening = false;
                    if (depthTemp > depth)
                    {
                        depth = depthTemp;
                    }
                    depthTemp = 0;
                }

                //getting out expresion's inside and couting depth
                if (justEntered != true && insideBrackets == 0)
                {
                    haveOpening = false;

                    if (depthTemp > depth)
                    {
                        depth = depthTemp;
                    }
                    depthTemp = 0;
                }
                

                justEntered = false;

                //expression is incorrect - break out of cycle
                if (overallBrackets < 0)
                {
                    break;
                }
            }

            if (overallBrackets == 0)
            {
                isCorrectExpression = true;
            }

            if (isCorrectExpression == true)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(text + " bracket expression is correct");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"{depth + 1} bracket depth");
            }
            else if (isCorrectExpression == false)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(text + " bracket expression is incorrect");
                Console.ForegroundColor = ConsoleColor.White;
            }

            //testing
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\ninside brackets: " + insideBrackets + "\noverall brackets: " + overallBrackets);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
