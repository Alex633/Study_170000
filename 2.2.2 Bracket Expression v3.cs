//Дана строка из символов '(' и ')'.
//Определить, является ли она корректным скобочным выражением. Определить максимальную глубину вложенности скобок.
//Текущая глубина равняется разности открывающихся и закрывающихся скобок в момент подсчета каждого символа.
//К символу в строке можно обратиться по индексу
//Пример “(()(()))” - строка корректная и максимум глубины равняется 3.
//Пример некорректных строк: "(()", "())", ")(", "(()))(()"
//(((()(()())))) - 5
//(((()(())))) - 5
//(()((()))) - 4
//)()(())( - 2
//((()())) - 3
//((()()()()()())) - 3

using System;

namespace millionDollarsCourses
{
    internal class Program
    {
        static void Main()
        {
            string bracketExpression = null;

            char openBracketChar = '(';
            char closeBracketChar = ')';

            int currentDepth;
            int maxDepth;
            int balance;

            while (true)
            {
                maxDepth = 0;
                currentDepth = 0;
                balance = 0;

                Console.Write("Insert bracket expression: ");
                bracketExpression = Console.ReadLine();

                #region check bracket expression
                foreach (char bracket in bracketExpression)
                {
                    if (bracket == openBracketChar)
                    {
                        currentDepth++;
                        balance++;

                        if (currentDepth > maxDepth)
                            maxDepth = currentDepth;
                    }
                    else if (bracket == closeBracketChar)
                    {
                        currentDepth--;
                        balance--;

                        if (balance < 0)
                            break;
                    }
                    else
                    {
                        break;
                    }
                }
                #endregion

                #region output result
                if (balance == 0)
                    Console.WriteLine($"{bracketExpression} - This bracket is correct! Max depth: {maxDepth}.");
                else
                    Console.WriteLine($"{bracketExpression} - This bracket is incorrect!");
                #endregion
            }
        }
    }
}
