using System;

namespace CsRealLearning
{
    internal class Program
    {
        static void Main()
        {
            //Создать класс игрока, с полями, содержащими информацию об игроке и методом, который выводит информацию на экран.
            //В классе обязательно должен быть конструктор
            Player aleksandar = new Player(0, "NumberOneDrummer", 1000);
            aleksandar.TextNika("Good Night and please, by the universe, get well soon");
        }

        class Player
        {
            private int _id;
            private string _nickname;
            private int _level;

            public Player(int id, string nickname, int level)
            {
                _id = id;
                _nickname = nickname;
                _level = level;
            }

            public void TextNika(string message)
            {
                Console.WriteLine(message);
            }

            public void DisplayInfo()
            {
                Console.WriteLine($"ID: {_id}\n" +
                    $"Nickname: {_nickname}\n" +
                    $"Level: {_level}");
            }

            public void SayWhatsInYourMind()
            {
                Console.WriteLine("I miss you and wish you were here");
            }
        }
    }
}
