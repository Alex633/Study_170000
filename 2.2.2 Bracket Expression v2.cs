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

            int countOpenBracket;
            int countCloseBracket;

            int depthCurrentExpression;
            int maxDepth;

            bool isCorrectBracket;
            bool isIncorrectExpressionForSure;

            bool isInsideExpression;
            bool isInsideInnerExpression;

            int counOpenInnerBracket;
            int countCloseInnerBracker;

            int depthtInnerCurrentExpression;
            int maxDepthOfInnerExpression;

            while (true)
            {
                countOpenBracket = 0;
                countCloseBracket = 0;

                depthCurrentExpression = 0;
                maxDepth = 0;

                isCorrectBracket = false;

                isIncorrectExpressionForSure = false;
                isInsideExpression = false;
                isInsideInnerExpression = false;

                counOpenInnerBracket = 0;
                countCloseInnerBracker = 0;

                depthtInnerCurrentExpression = 0;
                maxDepthOfInnerExpression = 0;

                Console.Write("\nInput bracket expression: ");
                bracketExpression = Console.ReadLine();

                #region check bracket
                for (int i = 0; i < bracketExpression.Length; i++)
                {
                    #region count brackets
                    if (bracketExpression[i] == openBracketChar)
                    {
                        countOpenBracket++;

                        if (isInsideExpression)
                        {
                            isInsideInnerExpression = true;
                        }

                        if (isInsideInnerExpression)
                        {
                            countOpenBracket--;
                            counOpenInnerBracket++;
                        }

                        if (countOpenBracket > countCloseBracket)
                        {
                            isInsideExpression = true;
                        }
                        else
                        {
                            isInsideExpression = false;
                        }
                    }
                    else if (bracketExpression[i] == closeBracketChar)
                    {
                        countCloseBracket++;

                        if (countCloseBracket > countOpenBracket)
                        {
                            countCloseBracket = 0;
                            isIncorrectExpressionForSure = true;
                        }

                        if (isInsideInnerExpression)
                        {
                            countCloseInnerBracker++;
                            countCloseBracket--;
                        }
                    }
                    else
                    {
                        isIncorrectExpressionForSure = true;
                    }
                    #endregion

                    #region check depth and correctness
                    if (isInsideExpression == true && countOpenBracket > 0 && countCloseBracket > 0)
                    {
                        depthCurrentExpression++;

                        countCloseBracket--;
                        countOpenBracket--;
                    }

                    if (isInsideExpression == true && counOpenInnerBracket > 0 && countCloseInnerBracker > 0)
                    {
                        depthtInnerCurrentExpression++;

                        counOpenInnerBracket--;
                        countCloseInnerBracker--;
                    }

                    if (i != bracketExpression.Length - 1)
                    {
                        if (bracketExpression[i + 1] == openBracketChar && counOpenInnerBracket > 0)
                        {
                            isInsideInnerExpression = false;
                        }
                    }

                    if (counOpenInnerBracket == 0 && countCloseInnerBracker == 0)
                    {
                        isInsideInnerExpression = false;
                    }

                    if (isInsideInnerExpression == false)
                    {
                        if (maxDepthOfInnerExpression < depthtInnerCurrentExpression)
                            maxDepthOfInnerExpression = depthtInnerCurrentExpression;

                        depthtInnerCurrentExpression = 0;
                    }

                    if (countCloseBracket == 0 && countOpenBracket == 0)
                    {
                        isInsideExpression = false;
                    }

                    if (isInsideExpression == false)
                    {
                        depthCurrentExpression += maxDepthOfInnerExpression;

                        if (depthCurrentExpression > maxDepth)
                        {
                            maxDepth = depthCurrentExpression;
                        }

                        depthCurrentExpression = 0;
                        maxDepthOfInnerExpression = 0;
                    }
                    #endregion
                }

                depthCurrentExpression += maxDepthOfInnerExpression;

                if (depthCurrentExpression > maxDepth)
                {
                    maxDepth = depthCurrentExpression;
                }

                if (countOpenBracket == 0 && countCloseBracket == 0 && isIncorrectExpressionForSure != true)
                {
                    isCorrectBracket = true;
                }
                #endregion

                #region output result
                if (isCorrectBracket)
                    Console.WriteLine($"{bracketExpression} - This bracket is .... correct! Max depth: {maxDepth}.");
                else
                    Console.WriteLine($"{bracketExpression} - This bracket is .... incorrect! Max depth: --{maxDepth}--.");
                #endregion
            }
        }
    }
}
