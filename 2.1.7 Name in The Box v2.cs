namespace millionDollarsCourses
{
    using System;

    //    Вывести имя в прямоугольник из символа, который введет сам пользователь.
    //От пользователя получаете символ и имя и по этим данным выводите имя в прямоугольнике.
    //Длина всех выводимых строк в прямоугольнике одинаковая, а узнать длину всегда можно у второй строки. Длину строки можно всегда узнать через свойство Length
    //string someString = “Hello”;
    //    Console.WriteLine(someString.Length);  //5 
    //То есть при вводе символа % и имени Alexey получиться, что в каждой строке 8 символов(в консоли длина символа одинаковая)
    //%%%%%%%%
    //%Alexey% 
    //%%%%%%%%

    internal class Program
    {
        static void Main()
        {
            char outlineSymbol;
            string name;

            Console.Write("Input name: ");
            name = Console.ReadLine();

            Console.Write("Input symbol: ");
            outlineSymbol = Convert.ToChar(Console.ReadLine());

            int nameLength = name.Length;
            int outlineLength = nameLength + 2;

            Console.WriteLine();

            for (int i = 0; i < outlineLength; i++)
                Console.Write(outlineSymbol);

            Console.Write("\n" + outlineSymbol + name + outlineSymbol + "\n");
            Console.WriteLine(new string(outlineSymbol, outlineLength));
            Console.WriteLine();
        }
    }
}
