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
            float bronze = 10;
            float silver = 5;
            float gold = 3;
            float bronzeToSilver = 2;
            float bronzeToGold = 4;
            float silverToBronze = 0.5f; //what does f mean
            float silverToGold = 2;
            float goldToBronze = 0.25f;
            float goldToSilver = 0.5f;

            string input = "";
            float currencyAmaunt;

            Console.WriteLine($"Exchange Rate:\n" +
                $"Bronze To Silver: {bronzeToSilver}\n" +
                $"Bronze To Gold: {bronzeToGold}\n" +
                $"Silver To Bronze: {silverToBronze}\n" +
                $"Silver To Gold: {silverToGold}\n" +
                $"Gold To Bronze: {goldToBronze}\n" +
                $"Gold To Silver: {goldToSilver}\n");

            Console.WriteLine($"You Balance:\n" +
                $"Bronze: {bronze}\n" +
                $"Silver: {silver}\n" +
                $"Gold: {gold}\n\n" +
                $"To convert\n" +
                $"Bronze to Silver press: 1\n" +
                $"Bronze to Gold press: 2\n" +
                $"Silver to Bronze press: 3\n" +
                $"Silver to Gold press: 4\n" +
                $"Gold to Bronze press: 5\n" +
                $"Gold to Silver press: 6\n" +
                $"To exit type exit");

            while (input != "exit")
            {
                input = Console.ReadLine();
                switch (input)
                {
                    default:
                        Console.WriteLine("What the Hell are you saying");
                        break;
                    case "exit":
                        Console.WriteLine("Thank you for working with Golden Airline. We hope to see you soon. Bye, Love");
                        break;

                    case "1":
                        Console.WriteLine("How many silver do you need?");
                        currencyAmaunt = Convert.ToSingle(Console.ReadLine());
                        if (bronze < currencyAmaunt * bronzeToSilver)
                        {
                            Console.WriteLine("You don't have enough money, dummy. Let's start again. Number?");
                        }
                        else 
                        {
                            bronze -= currencyAmaunt * bronzeToSilver;
                            silver += currencyAmaunt;
                            Console.WriteLine($"\n\n\nOPERATION SUCCESSFUL\nYou Balance:\n" +
                            $"Bronze: {bronze}\n" +
                            $"Silver: {silver}\n" +
                            $"Gold: {gold}\n\n" +
                            $"To convert\n" +
                            $"Bronze to Silver press 1\n" +
                            $"Bronze to Gold press 2\n" +
                            $"Silver to Bronze press 3\n" +
                            $"Silver to Gold press 4\n" +
                            $"Gold to Bronze press 5\n" +
                            $"Gold to Silver press 6\n" +
                            $"To exit type exit");
                        }
                        break;

                    case "2":
                        Console.WriteLine("How many gold do you need?");
                        currencyAmaunt = Convert.ToSingle(Console.ReadLine());
                        if (bronze < currencyAmaunt * bronzeToGold)
                        {
                            Console.WriteLine("You don't have enough money, dummy. Let's start again. Number?");
                        }
                        else
                        {
                            bronze -= currencyAmaunt * bronzeToGold;
                            gold += currencyAmaunt;
                            Console.WriteLine($"\n\n\nOPERATION SUCCESSFUL\nYou Balance:\n" +
                            $"Bronze: {bronze}\n" +
                            $"Silver: {silver}\n" +
                            $"Gold: {gold}\n\n" +
                            $"To convert\n" +
                            $"Bronze to Silver press 1\n" +
                            $"Bronze to Gold press 2\n" +
                            $"Silver to Bronze press 3\n" +
                            $"Silver to Gold press 4\n" +
                            $"Gold to Bronze press 5\n" +
                            $"Gold to Silver press 6\n" +
                            $"To exit type exit");
                        }
                        break;

                    case "3":
                        Console.WriteLine("How many bronze do you need?");
                        currencyAmaunt = Convert.ToSingle(Console.ReadLine());
                        if (silver < currencyAmaunt / silverToBronze)
                        {
                            Console.WriteLine("You don't have enough money, dummy. Let's start again. Number?");
                        }
                        else
                        {
                            silver -= currencyAmaunt * silverToBronze;
                            bronze += currencyAmaunt;
                            Console.WriteLine($"\n\n\nOPERATION SUCCESSFUL\nYou Balance:\n" +
                            $"Bronze: {bronze}\n" +
                            $"Silver: {silver}\n" +
                            $"Gold: {gold}\n\n" +
                            $"To convert\n" +
                            $"Bronze to Silver press 1\n" +
                            $"Bronze to Gold press 2\n" +
                            $"Silver to Bronze press 3\n" +
                            $"Silver to Gold press 4\n" +
                            $"Gold to Bronze press 5\n" +
                            $"Gold to Silver press 6\n" +
                            $"To exit type exit");
                        }
                        break;

                    case "4":
                        Console.WriteLine("How many gold do you need?");
                        currencyAmaunt = Convert.ToSingle(Console.ReadLine());
                        if (silver < currencyAmaunt * silverToGold)
                        {
                            Console.WriteLine("You don't have enough money, dummy. Let's start again. Number?");
                        }
                        else
                        {
                            silver -= currencyAmaunt * silverToGold;
                            gold += currencyAmaunt;
                            Console.WriteLine($"\n\n\nOPERATION SUCCESSFUL\nYou Balance:\n" +
                            $"Bronze: {bronze}\n" +
                            $"Silver: {silver}\n" +
                            $"Gold: {gold}\n\n" +
                            $"To convert\n" +
                            $"Bronze to Silver press 1\n" +
                            $"Bronze to Gold press 2\n" +
                            $"Silver to Bronze press 3\n" +
                            $"Silver to Gold press 4\n" +
                            $"Gold to Bronze press 5\n" +
                            $"Gold to Silver press 6\n" +
                            $"To exit type exit");
                        }
                        break;

                    case "5":
                        Console.WriteLine("How many bronze do you need?");
                        currencyAmaunt = Convert.ToSingle(Console.ReadLine());
                        if (gold < currencyAmaunt * goldToBronze)
                        {
                            Console.WriteLine("You don't have enough money, dummy. Let's start again. Number?");
                        }
                        else
                        {
                            gold -= currencyAmaunt / goldToBronze;
                            bronze += currencyAmaunt;
                            Console.WriteLine($"\n\n\nOPERATION SUCCESSFUL\nYou Balance:\n" +
                            $"Bronze: {bronze}\n" +
                            $"Silver: {silver}\n" +
                            $"Gold: {gold}\n\n" +
                            $"To convert\n" +
                            $"Bronze to Silver press 1\n" +
                            $"Bronze to Gold press 2\n" +
                            $"Silver to Bronze press 3\n" +
                            $"Silver to Gold press 4\n" +
                            $"Gold to Bronze press 5\n" +
                            $"Gold to Silver press 6\n" +
                            $"To exit type exit");
                        }
                        break;

                    case "6":
                        Console.WriteLine("How many silver do you need?");
                        currencyAmaunt = Convert.ToSingle(Console.ReadLine());
                        if (gold < currencyAmaunt * goldToSilver)
                        {
                            Console.WriteLine("You don't have enough money, dummy. Let's start again. Number?");
                        }
                        else
                        {
                            gold -= currencyAmaunt / goldToSilver;
                            silver += currencyAmaunt;
                            Console.WriteLine($"\n\n\nOPERATION SUCCESSFUL\nYou Balance:\n" +
                            $"Bronze: {bronze}\n" +
                            $"Silver: {silver}\n" +
                            $"Gold: {gold}\n\n" +
                            $"To convert\n" +
                            $"Bronze to Silver press 1\n" +
                            $"Bronze to Gold press 2\n" +
                            $"Silver to Bronze press 3\n" +
                            $"Silver to Gold press 4\n" +
                            $"Gold to Bronze press 5\n" +
                            $"Gold to Silver press 6\n" +
                            $"To exit type exit");
                        }
                        break;
                }
            }

        }
    }
}
