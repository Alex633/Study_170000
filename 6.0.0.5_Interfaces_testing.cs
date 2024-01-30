using System;

namespace CsRealLearning
{
    internal class Program
    {
        static void Main()
        {
        }

        interface IMovable
        {
            void Move();
            void GetSpeed();
            void StopMoving();
        }

        interface ISpeak
        {
            void Say(string words);
            void Whisper(string words);
            void Sing(string words);
        }

        class Human : IMovable, ISpeak
        {
            public void Move()
            {
                Console.WriteLine("Made a step");
            }
            public void GetSpeed()
            {
                Console.WriteLine("Display Speed");
            }
            public void StopMoving()
            {
                Console.WriteLine("Stopped moving");
            }

            public void Say(string words)
            {
                Console.WriteLine(words);
            }
            public void Whisper(string words)
            {
                Console.WriteLine(words);
            }
            public void Sing(string words)
            {
                Console.WriteLine(words);
            }
        }

        class Chield : Human
        {
            public new void Move()
            {
                Console.WriteLine("Crawl");
            }
            public new void StopMoving()
            {
                Console.WriteLine("Stopped crawling");
            }
        }

        class Car : IMovable
        {
            public void Move()
            {
                Console.WriteLine("Moved Forward");
            }
            public void GetSpeed()
            {
                Console.WriteLine("Display Speed");
            }
            public void StopMoving()
            {
                Console.WriteLine("Stopped moving");
            }
        }
    }
}
