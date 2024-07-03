namespace millionDollarsCourses
{
    using System;

    //   Легенда: 
    //Вы заходите в поликлинику и видите огромную очередь из пациентов, вам нужно рассчитать время ожидания в очереди.
    //Формально: 
    //Пользователь вводит кол-во людей в очереди.  
    //Фиксированное время приема одного человека всегда равно 10 минутам.
    //Пример ввода: Введите кол-во пациентов: 14
    //Пример вывода: "Вы должны отстоять в очереди 2 часа и 20 минут."
    //Примечание:
    //- при расчетах надо использовать только переменные.
    //Если число не присваивается переменной, то в большинстве случаев это число магическое (исключение 0 и 1, но не во всех ситуациях).
    //- повторные расчеты так же стоит выносить в переменные

    internal class Program
    {
        static void Main()
        {
            const int MinutesInHour = 60;
            int appointmentDurationMinutes = 10;
            int noWaitingTime = 0;
            int defaultWaitingTimeInMinutes = 30;
            int longWaitingTimeInMinutes = 120;

            Console.WriteLine("Welcome to the doctor appointment\n" +
                "How many patients are currently in line?");
            int queueOfPatients = Convert.ToInt32(Console.ReadLine());
            int waitingTimeInMinutesTotal = appointmentDurationMinutes * queueOfPatients;
            int waitingTimeHours = waitingTimeInMinutesTotal / MinutesInHour;
            int waitingTimeMinutes = waitingTimeInMinutesTotal % MinutesInHour;

            if (waitingTimeInMinutesTotal < noWaitingTime)
                Console.WriteLine($"Number of people can't be negative, dummy. Or can it?");
            else if (waitingTimeInMinutesTotal == noWaitingTime)
                Console.WriteLine("Looks like you are in luck, step right in");
            else if (waitingTimeInMinutesTotal < defaultWaitingTimeInMinutes)
                Console.WriteLine($"It's only {waitingTimeHours}h:{waitingTimeMinutes}m. Not that bad, right?");
            else if (waitingTimeInMinutesTotal < longWaitingTimeInMinutes)
                Console.WriteLine($"Waiting time is {waitingTimeHours}h:{waitingTimeMinutes}m. We apologize for the inconvenience");
            else
                Console.WriteLine($"I hope you didn't have any plans for today. You will have to wait for " +
                    $"{waitingTimeHours}h:{waitingTimeMinutes}m");
        }
    }
}
