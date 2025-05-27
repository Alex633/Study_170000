using System;

namespace CsRealLearning
{
    internal class Program
    {
        //Создать класс игрока, у которого есть поля с его положением в x,y.
        //Создать класс отрисовщик, с методом, который отрисует игрока.
        //Попрактиковаться в работе со свойствами.

        static void Main()
        {
            Renderer renderer = new Renderer();
            Player player = new Player(55, 10);

            renderer.Draw(player.X, player.Y);
        }

        class Renderer
        {
            public void Draw(int x, int y, char thingy = '@')
            {
                Console.CursorVisible = false;
                Console.SetCursorPosition(x, y);
                Console.Write(thingy);
                Console.ReadKey();
            }

        }

        class Player
        {
            public int X { get; private set; }
            public int Y { get; private set; }


            public Player(int x, int y)
            {
                X = x;
                Y = y;
            }
        }
    }
}
