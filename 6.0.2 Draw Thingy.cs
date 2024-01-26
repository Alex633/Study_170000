using System;

namespace CsRealLearning
{
    internal class Program
    {
        static void Main()
        {
            Renderer renderer = new Renderer();
            Player player = new Player(55,10);

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
