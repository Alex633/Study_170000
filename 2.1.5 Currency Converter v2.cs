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

            bool isSuccefulConvertion = false;
            bool isWrongCommandInput = false;
            bool isGivenAmountOfBronzeFraction = false;

            Random random = new Random();

            int userInput = 0;
            int maxInitialBronzeCoins = 1000;
            int maxInitialSilverCoins = 100;
            int maxInitialGoldCoins = 10;
            int minAmountOfBronze = 1;

            float coinsCount = 0;
            float bronzeCoinsBalance = random.Next(maxInitialBronzeCoins);
            float silverCoinsBalanace = random.Next(maxInitialSilverCoins); ;
            float goldCoinsBalance = random.Next(maxInitialGoldCoins);
            float bronzeToSilverConvertRate = 0.01f;
            float bronzeToGoldConvertRate = 0.0001f;
            float silverToBronzeConvertRate = 100f;
            float silverToGoldConvertRate = 0.01f;
            float goldToBronzeConvertRate = 10000f;
            float goldToSilverConvertRate = 100f;

            while (userInput != CommandExit)
            {
                Console.WriteLine("Balance:\n" +
                    $"{bronzeCoinsBalance} Bronze\n" +
                    $"{silverCoinsBalanace} Silver\n" +
                    $"{goldCoinsBalance} Gold\n");

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
                        coinsCount = Convert.ToSingle(Console.ReadLine());

                        isGivenAmountOfBronzeFraction = coinsCount > 0 && coinsCount < minAmountOfBronze;

                        if (coinsCount <= bronzeCoinsBalance && coinsCount >= minAmountOfBronze)
                        {
                            bronzeCoinsBalance -= coinsCount;
                            coinsCount *= bronzeToSilverConvertRate;
                            silverCoinsBalanace += coinsCount;
                            isSuccefulConvertion = true;
                            Console.WriteLine($"+{coinsCount} silver " +
                                $"(Exchange Rate: {bronzeToSilverConvertRate})");
                        }

                        break;

                    case CommandBronzeToGold:
                        Console.Write("Amount of coins given: ");
                        coinsCount = Convert.ToSingle(Console.ReadLine());

                        isGivenAmountOfBronzeFraction = coinsCount > 0 && coinsCount < minAmountOfBronze;

                        if (coinsCount <= bronzeCoinsBalance && coinsCount >= minAmountOfBronze)
                        {
                            bronzeCoinsBalance -= coinsCount;
                            coinsCount *= bronzeToGoldConvertRate;
                            goldCoinsBalance += coinsCount;
                            isSuccefulConvertion = true;
                            Console.WriteLine($"+{coinsCount} gold " +
                                $"(Exchange Rate: {bronzeToGoldConvertRate})");
                        }

                        break;

                    case CommandSilverToBronze:
                        Console.Write("Amount of coins given: ");
                        coinsCount = Convert.ToSingle(Console.ReadLine());

                        if (coinsCount <= silverCoinsBalanace && coinsCount > 0)
                        {
                            silverCoinsBalanace -= coinsCount;
                            coinsCount *= silverToBronzeConvertRate;
                            bronzeCoinsBalance += coinsCount;
                            isSuccefulConvertion = true;
                            Console.WriteLine($"+{coinsCount} bronze " +
                                $"(Exchange Rate: {silverToBronzeConvertRate})");
                        }

                        break;

                    case CommandSilverToGold:
                        Console.Write("Amount of coins given: ");
                        coinsCount = Convert.ToSingle(Console.ReadLine());

                        if (coinsCount <= silverCoinsBalanace && coinsCount > 0)
                        {
                            silverCoinsBalanace -= coinsCount;
                            coinsCount *= silverToGoldConvertRate;
                            goldCoinsBalance += coinsCount;
                            isSuccefulConvertion = true;
                            Console.WriteLine($"+{coinsCount} gold " +
                                $"(Exchange Rate: {silverToGoldConvertRate})");
                        }

                        break;

                    case CommandGoldToBronze:
                        Console.Write("Amount of coins given: ");
                        coinsCount = Convert.ToSingle(Console.ReadLine());

                        if (coinsCount <= goldCoinsBalance && coinsCount > 0)
                        {
                            goldCoinsBalance -= coinsCount;
                            coinsCount *= goldToBronzeConvertRate;
                            bronzeCoinsBalance += coinsCount;
                            isSuccefulConvertion = true;
                            Console.WriteLine($"+{coinsCount} bronze " +
                                $"(Exchange Rate: {goldToBronzeConvertRate})");
                        }

                        break;

                    case CommandGoldToSilver:
                        Console.Write("Amount of coins given: ");
                        coinsCount = Convert.ToSingle(Console.ReadLine());

                        if (coinsCount <= goldCoinsBalance && coinsCount > 0)
                        {
                            goldCoinsBalance -= coinsCount;
                            coinsCount *= goldToSilverConvertRate;
                            silverCoinsBalanace += coinsCount;
                            isSuccefulConvertion = true;
                            Console.WriteLine($"+{coinsCount} silver " +
                                $"(Exchange Rate: {goldToSilverConvertRate})");
                        }

                        break;

                    case CommandExit:
                        Console.WriteLine("Exiting the program.");
                        break;

                    default:
                        Console.WriteLine("\nInvalid command");
                        isWrongCommandInput = true;
                        break;
                }

                if (userInput != CommandExit && isWrongCommandInput != true)
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
                        else if (coinsCount <= 0)
                            Console.WriteLine("\nConvertion failed. Given amount must be greater than zero.");
                        else
                            Console.WriteLine("\nConvertion failed. Not enough coins");
                    }
                }

                Console.WriteLine("Press anything to Continue");
                Console.ReadKey(true);
                Console.Clear();
            }
        }
    }
}
