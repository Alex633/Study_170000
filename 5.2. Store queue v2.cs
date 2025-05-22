using System;
using System.Collections.Generic;

//У вас есть множество целых чисел. Каждое целое число - это сумма покупки.
//Вам нужно обслуживать клиентов до тех пор, пока очередь не станет пуста. 
//После каждого обслуженного клиента деньги нужно добавлять на наш счёт и выводить его в консоль.  
//После обслуживания каждого клиента программа ожидает нажатия любой клавиши,
//после чего затирает консоль и по новой выводит всю информацию, только уже со следующим клиентом

public class Program
{
    private static void Main()
    {
        int balance = 0;

        Queue<int> payments = BuildPaymentsQueue();

        while (payments.Count > 0)
        {
            int nextPayment = payments.Dequeue();

            balance = DepositPayment(nextPayment, balance);

            DisplayBalance(balance);

            Console.WriteLine($"Press any key");
            Console.ReadKey(true);
        }
    }

    public static void DisplayBalance(int balance)
    {
        int yBalance = 0;
        int xBalance = 100;

        WriteLineAt($"${balance}", yBalance, xBalance);
    }

    public static int DepositPayment(int amount, int balance)
    {
        if (amount <= 0)
        {
            WriteLineAt("Payment can't be negative or null", foregroundColor: ConsoleColor.Red);
            amount = 0;
        }
        else
        {
            WriteLineAt($"${amount} succesfully deposited", foregroundColor: ConsoleColor.Green);
        }

        return balance + amount;
    }

    public static Random random = new Random();

    public static Queue<int> BuildPaymentsQueue()
    {
        Queue<int> result = new Queue<int>();

        int maxDeposit = 100;

        for (int i = 1; i < 10; i++)
            result.Enqueue(random.Next(1, maxDeposit + 1));

        return result;
    }

    #region Helper
    public static void WriteLineAt(object element, int? yPosition = null, int? xPosition = null,
        ConsoleColor foregroundColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black)
    {
        Console.ForegroundColor = foregroundColor;
        Console.BackgroundColor = backgroundColor;

        int yStart = Console.CursorTop;
        int xStart = Console.CursorLeft;

        bool isCustomPosition = yPosition.HasValue || xPosition.HasValue;

        if (isCustomPosition)
            Console.SetCursorPosition(xPosition ?? xStart, yPosition ?? yStart);

        Console.WriteLine(element);

        Console.ResetColor();

        if (isCustomPosition)
        {
            Console.CursorTop = yStart;
            Console.CursorLeft = xStart;
        }
    }
    #endregion
}
