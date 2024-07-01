using System;

namespace millionDollarsCourses
{
    internal class Program
    {
        //Легенда: 
        //Вы Foul Tarnished и вы приходите в магазин и хотите купить за свои golden seeds - Esthus Flask upgrades.
        //В вашем кошельке есть какое-то количество golden seeds, Margit the Fell Omen  спрашивает у вас, сколько Esthus Flask upgrades вы хотите получить?
        //После сделки у вас остаётся какое-то количество golden seeds и появляется какое-то количество Esthus Flask upgrades.
        //Формально: 
        //При старте программы пользователь вводит начальное количество золота.
        //Потом ему предлагается купить какое-то количество кристаллов по цене N(задать в программе самому).
        //Пользователь вводит число и его золото конвертируется в кристаллы. Остаток золота и кристаллов выводится на экран. 
        //Проверять на то, что у игрока достаточно денег не нужно. 

        static void Main()
        {
            int availableGoldenSeeds;
            int esthusFlaskLevel = 1;
            int esthusFlaskPricePerLevel = 2;
            int desiredEsthusFlaskUpgrades;
            int totalPriceForDesiredUpgrade;
            bool isEnoughGoldenSeeds;

            Console.WriteLine("Greetings, Foul Tarnished, in search of the Esthus Flask upgrades. Emboldened by the flame of ambition.");
            Console.WriteLine($"How many Golden Seeds do you have in your domain ({esthusFlaskPricePerLevel} golden seeds required for one Esthus Flask Upgrade)?");
            availableGoldenSeeds = Convert.ToInt32(Console.ReadLine());

            if (availableGoldenSeeds < esthusFlaskPricePerLevel)
            {
                Console.WriteLine("Foul Tarnished, not enough golden seeds for even one upgrade, so put these foolish ambitions to rest or just restart the program");
            }
            else
            {
                Console.WriteLine($"\nInventory: {availableGoldenSeeds} golden seeds | Price of one Esthus Flask upgrade: {esthusFlaskPricePerLevel} golden seeds \nFor how many times do you want to upgrade your Esthus Flask, Tarnished? ");
                desiredEsthusFlaskUpgrades = Convert.ToInt32(Console.ReadLine());
                totalPriceForDesiredUpgrade = desiredEsthusFlaskUpgrades * esthusFlaskPricePerLevel;
                isEnoughGoldenSeeds = availableGoldenSeeds >= totalPriceForDesiredUpgrade;

                if (isEnoughGoldenSeeds)
                {
                    availableGoldenSeeds -= totalPriceForDesiredUpgrade;
                    esthusFlaskLevel += desiredEsthusFlaskUpgrades;
                    Console.WriteLine($"\nTarnished, you have {availableGoldenSeeds} golden seeds left and your Esthus Flask is level {esthusFlaskLevel} now. \nWell, thou art of passing skill. Warrior blood must truly run in thy veins, Tarnished.");
                }
                else
                {
                    Console.WriteLine($"\nNo, Foul Tarnished, you don't have enough golden seeds for {desiredEsthusFlaskUpgrades} esthus flask upgrades. \nYou see, you have just {availableGoldenSeeds} golden seeds. So no. " +
                        $"You eshus level is still {esthusFlaskLevel}. Go farm");
                }
            }
        }
    }
}
