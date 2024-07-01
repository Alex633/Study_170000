using System;

namespace _170000Courses
{
    internal class Program
    {
        //Легенда: 
        //Вы приходите в магазин и хотите купить за своё золото кристаллы. В вашем кошельке есть какое-то количество золота, продавец спрашивает у вас, сколько кристаллов вы хотите купить?
        //После сделки у вас остаётся какое-то количество золота в кошельке и появляется какое-то количество кристаллов.
        //Формально: 
        //При старте программы пользователь вводит начальное количество золота.
        //Потом ему предлагается купить какое-то количество кристаллов по цене N(задать в программе самому).
        //Пользователь вводит число и его золото конвертируется в кристаллы. Остаток золота и кристаллов выводится на экран. 
        //Проверять на то, что у игрока достаточно денег не нужно. 

        static void Main()
        {
            int goldenSeeds;
            int desiredFlask;
            int price = 3;

            Console.WriteLine("Tarnished, how many Golden Seeds do you have in your domain? (3 required for just one upgrade)");
            goldenSeeds = Convert.ToInt32(Console.ReadLine());

            if (goldenSeeds < 3)
            {
                Console.WriteLine("Foul Tarnished, put these foolish ambitions to rest.\nYou don't have enough golden seeds for even one upgrade");
            }
            else
            {
                Console.WriteLine("Do you want to upgrade your Esthus Flask? If so say number of desired upgrades? (3 required for just one upgrade)");
                desiredFlask = Convert.ToInt32(Console.ReadLine());
                goldenSeeds -= price * desiredFlask;

                if (goldenSeeds < 0)
                {
                    Console.WriteLine($"Foul Tarnished, Put these foolish ambitions to rest.\nYou don't have enough golden seeds for {desiredFlask} upgrades");
                }
                else
                {
                    Console.WriteLine($"Tarnished, you have {goldenSeeds} Golden Seeds left and now your Esthus Flask is level {desiredFlask + 1}");
                }
            }
        }
    }
}
