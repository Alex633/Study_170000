using System;

namespace millionDollarsCourses
{
    internal class Program
    {
        //На экране, в специальной зоне, выводятся картинки, по 3 в ряд(условно, ничего рисовать не надо). 
        //Всего у пользователя в альбоме 52 картинки.Код должен вывести, сколько полностью заполненных рядов можно будет вывести, и сколько картинок будет сверх меры.
        //В качестве решения ожидаются объявленные переменные с необходимыми значениями и, основываясь на значениях переменных, вывод необходимых данных. По задаче требуется выполнить простые математические действия.

        static void Main()
        {
            char image = '✧';
            int imagesPerRow = 3;
            int totalImageCount = 52;
            int rowsFilledWithImages = totalImageCount / imagesPerRow;
            int remainingImages = totalImageCount % imagesPerRow;

            Console.WriteLine($"Total rows in the album: {rowsFilledWithImages}\n" +
                $"Images left: {image} {remainingImages}");
        }
    };
}
