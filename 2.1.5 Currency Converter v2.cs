namespace millionDollarsCourses
{
    using System;

    //    Написать конвертер валют(3 валюты).  
    //У пользователя есть баланс в каждой из представленных валют.
    //Он может попросить сконвертировать часть баланса с одной валюты в другую.
    //Тогда у него с баланса одной валюты снимется X и зачислится на баланс другой Y.
    //Курс конвертации должен быть просто прописан в программе.
    //По имени переменной курса конвертации должно быть понятно, из какой валюты в какую валюту конвертируется.
    //В консольном меню будет 6 команд конвертации, на каждый обмен.И для каждого обмена своя переменная с коэффициентом обмена.
    //Валюта не может быть отрицательной и это стоит учитывать.
    //Программа должна завершиться тогда, когда это решит пользователь.

    internal class Program
    {
        static void Main()
        {
            const int CommandBronzeToSilver = 1;
            const int CommandBronzeToGold = 2;
            const int CommandSilverToBronze = 3;
            const int CommandSilverToGold = 4;
            const int CommandGoldToBronze = 5;
            const int CommandGoldToSilver = 6;
            const int CommandExit = 7;


            int userInput = 0;
            bool isSuccefulConvertion = false;
            bool isGivenAmountOfBronzeFraction = false;
            float amountOfGivenCoins = 0;
            float amountOfTakenCoins;
            Random random = new Random();
            float bronze = random.Next(1000);
            float silver = random.Next(100); ;
            float gold = random.Next(10);
            float bronzeToSilverConvertRate = 0.01f;
            float bronzeToGoldConvertRate = 0.0001f;
            float silverToBronzeConvertRate = 100f;
            float silverToGoldConvertRate = 0.01f;
            float goldToBronzeConvertRate = 10000f;
            float goldToSilverConvertRate = 100f;

            while (userInput != CommandExit)
            {
                Console.WriteLine("Balance:\n" +
                    $"{bronze} Bronze\n" +
                    $"{silver} Silver\n" +
                    $"{gold} Gold\n");

                Console.WriteLine("Coins exchange options:\n" +
                $"{CommandBronzeToSilver}. Bronze to Silver\n" +
                $"{CommandBronzeToGold}. Bronze to Gold\n" +
                $"{CommandSilverToBronze}. Silver to Bronze\n" +
                $"{CommandSilverToGold}. Silver to Gold\n" +
                $"{CommandGoldToBronze}. Gold to Bronze\n" +
                $"{CommandGoldToSilver}. Gold to Silver\n" +
                $"{CommandExit}. Exit\n");
                Console.Write("Input command: ");
                userInput = Convert.ToInt32(Console.ReadLine());

                switch (userInput)
                {
                    case CommandBronzeToSilver:
                        Console.Write("Amount of coins given: ");
                        amountOfGivenCoins = Convert.ToSingle(Console.ReadLine());
                        isGivenAmountOfBronzeFraction = amountOfGivenCoins > 0 && amountOfGivenCoins < 1;

                        if (amountOfGivenCoins <= bronze && amountOfGivenCoins >= 1)
                        {
                            bronze -= amountOfGivenCoins;
                            amountOfTakenCoins = amountOfGivenCoins * bronzeToSilverConvertRate;
                            silver += amountOfTakenCoins;
                            isSuccefulConvertion = true;
                            Console.WriteLine($"-{amountOfGivenCoins} bronze | +{amountOfTakenCoins} silver " +
                                $"(Exchange Rate: {bronzeToSilverConvertRate})");
                        }

                        break;

                    case CommandBronzeToGold:
                        Console.Write("Amount of coins given: ");
                        amountOfGivenCoins = Convert.ToSingle(Console.ReadLine());
                        isGivenAmountOfBronzeFraction = amountOfGivenCoins > 0 && amountOfGivenCoins < 1;

                        if (amountOfGivenCoins <= bronze && amountOfGivenCoins >= 1)
                        {
                            bronze -= amountOfGivenCoins;
                            amountOfTakenCoins = amountOfGivenCoins * bronzeToGoldConvertRate;
                            gold += amountOfTakenCoins;
                            isSuccefulConvertion = true;
                            Console.WriteLine($"-{amountOfGivenCoins} bronze | +{amountOfTakenCoins} gold " +
                                $"(Exchange Rate: {bronzeToGoldConvertRate})");
                        }

                        break;

                    case CommandSilverToBronze:
                        Console.Write("Amount of coins given: ");
                        amountOfGivenCoins = Convert.ToSingle(Console.ReadLine());

                        if (amountOfGivenCoins <= silver && amountOfGivenCoins > 0)
                        {
                            silver -= amountOfGivenCoins;
                            amountOfTakenCoins = amountOfGivenCoins * silverToBronzeConvertRate;
                            bronze += amountOfTakenCoins;
                            isSuccefulConvertion = true;
                            Console.WriteLine($"-{amountOfGivenCoins} silver | +{amountOfTakenCoins} bronze " +
                                $"(Exchange Rate: {silverToBronzeConvertRate})");
                        }

                        break;

                    case CommandSilverToGold:
                        Console.Write("Amount of coins given: ");
                        amountOfGivenCoins = Convert.ToSingle(Console.ReadLine());

                        if (amountOfGivenCoins <= silver && amountOfGivenCoins > 0)
                        {
                            silver -= amountOfGivenCoins;
                            amountOfTakenCoins = amountOfGivenCoins * silverToGoldConvertRate;
                            gold += amountOfTakenCoins;
                            isSuccefulConvertion = true;
                            Console.WriteLine($"-{amountOfGivenCoins} silver | +{amountOfTakenCoins} gold " +
                                $"(Exchange Rate: {silverToGoldConvertRate})");
                        }

                        break;

                    case CommandGoldToBronze:
                        Console.Write("Amount of coins given: ");
                        amountOfGivenCoins = Convert.ToSingle(Console.ReadLine());

                        if (amountOfGivenCoins <= gold && amountOfGivenCoins > 0)
                        {
                            gold -= amountOfGivenCoins;
                            amountOfTakenCoins = amountOfGivenCoins * goldToBronzeConvertRate;
                            bronze += amountOfTakenCoins;
                            isSuccefulConvertion = true;
                            Console.WriteLine($"-{amountOfGivenCoins} gold | +{amountOfTakenCoins} bronze " +
                                $"(Exchange Rate: {goldToBronzeConvertRate})");
                        }

                        break;

                    case CommandGoldToSilver:
                        Console.Write("Amount of coins given: ");
                        amountOfGivenCoins = Convert.ToSingle(Console.ReadLine());

                        if (amountOfGivenCoins <= gold && amountOfGivenCoins > 0)
                        {
                            gold -= amountOfGivenCoins;
                            amountOfTakenCoins = amountOfGivenCoins * goldToSilverConvertRate;
                            silver += amountOfTakenCoins;
                            isSuccefulConvertion = true;
                            Console.WriteLine($"-{amountOfGivenCoins} gold | +{amountOfTakenCoins} silver " +
                                $"(Exchange Rate: {goldToSilverConvertRate})");
                        }

                        break;

                    case CommandExit:
                        Console.WriteLine("Exiting the program.");
                        break;

                    default:
                        Console.WriteLine("Invalid command");
                        break;
                }

                if (userInput != CommandExit)
                {
                    if (isSuccefulConvertion)
                    {
                        Console.WriteLine($"\nConvertion Succesful");
                        isSuccefulConvertion = false;
                    }
                    else
                    {
                        if (isGivenAmountOfBronzeFraction)
                            Console.WriteLine("\nConvertion failed. Bronze coin can't be devided and must be one or greater.");
                        else if (amountOfGivenCoins <= 0)
                            Console.WriteLine("\nConvertion failed. Given amount must be greater than zero.");
                        else
                            Console.WriteLine("\nConvertion failed. Not enough coins");
                    }

                    Console.WriteLine("Press anything to Continue");
                    Console.ReadKey(true);
                    Console.Clear();
                }
            }
        }
    }
}
