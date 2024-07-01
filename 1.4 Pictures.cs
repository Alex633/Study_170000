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
            int rowLength = 3;
            int amountOfImages = 52;
            int amountOfRows;
            int imagesLeft;

            amountOfRows = amountOfImages / rowLength;
            imagesLeft = amountOfImages % rowLength;
            Console.WriteLine($"Amount of rows: {amountOfRows}\n" +
                $"Image(s) remaining: {imagesLeft}");
        }
    }
}
