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
            int imageNumber = 52;
            int rowNumber;
            int imageLeft;

            rowNumber = imageNumber / rowLength;
            imageLeft = imageNumber % rowLength;
            Console.WriteLine($"Number of rows is {rowNumber}\n{imageLeft} image(s) remaining");
        }
    }
}
