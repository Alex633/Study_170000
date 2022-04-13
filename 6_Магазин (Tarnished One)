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
            int goldenSeeds;
            int desiredFlask;
            int price = 3;

            Console.WriteLine("Tarnished, how many Golden Seeds do you have in your domain?");
            goldenSeeds = Convert.ToInt32(Console.ReadLine());

            if (goldenSeeds < 3)
            {
                Console.WriteLine("Foul Tarnished, Put these foolish ambitions to rest.\nYou don't have enough golden seeds for even one upgrade");
            }
            else
            { 
            Console.WriteLine("Do you want to upgrade your Esthus Flask? If so say number of desired upgrades?");
            desiredFlask = Convert.ToInt32(Console.ReadLine());
            goldenSeeds -=  price * desiredFlask;
            
                if (goldenSeeds < 0)
                {
                Console.WriteLine("Foul Tarnished, Put these foolish ambitions to rest.\nYou don't have enough golden seeds");
                }
                else 
                { 
                Console.WriteLine($"Tarnished, you have {goldenSeeds} Golden Seeds left and {desiredFlask+1} level of your Esthus Flask");
                }
            }
        }
    }
}
