using System;
using System.Collections.Generic;

//Реализовать базу данных игроков и методы для работы с ней.
//У игрока может быть уникальный номер, ник, уровень, флаг – забанен ли он(флаг - bool).
//Реализовать возможность добавления игрока, бана игрока по уникальный номеру,
//разбана игрока по уникальный номеру и удаление игрока.
//Создание самой БД не требуется, задание выполняется инструментами,
//которые вы уже изучили в рамках курса. Но нужен класс, который содержит игроков и её можно назвать "База данных".

namespace CsRealLearning
{
    internal class Program
    {
        public static Random rnd { get; } = new Random();

        static void Main()
        {
            Database dataBase = new Database();
            dataBase.Work();
            dataBase.ShowAllPlayers();
        }

        class Database
        {
            List<Player> players = new List<Player>();

            public void Work()
            {
                while (true)
                {
                    ShowCommands();
                    HandleCommand();

                    Console.ReadKey();
                    Console.Clear();
                }
            }
            enum UserAction
            {
                add_player = 1,
                remove_player_by_id,
                ban_player,
                show_all_players
            }

            private void ShowCommands()
            {
                Console.WriteLine("Select a command:");

                foreach (UserAction command in Enum.GetValues(typeof(UserAction)))
                {
                    Usability.WriteLineInStyle($"{(int)(command)}. {command}", ConsoleColor.Yellow);
                }
            }

            private void HandleCommand()
            {
                if (int.TryParse(Console.ReadLine(), out int userInput))
                {
                    switch (userInput)
                    {
                        case (int)UserAction.add_player:
                            AddPlayer();
                            break;
                        case (int)UserAction.remove_player_by_id:
                            RemovePlayer();
                            break;
                        case (int)UserAction.ban_player:
                            break;
                        case (int)UserAction.show_all_players:
                            Console.Clear();
                            ShowAllPlayers();
                            break;
                        default:
                            Usability.WriteLineInStyle("That's a wrong number. Try again, fool");
                            break;
                    }
                }
                else
                {
                    Usability.WriteLineInStyle("Not a number. I need a number");
                }
            }

            public void AddPlayer()
            {
                string input;

                Console.Clear();
                Console.WriteLine("Enter player name: ");
                input = Console.ReadLine();
                players.Add(new Player(input));
                Usability.WriteLineInStyle($"Player {input} succesfully added", ConsoleColor.Green);
            }

            public void RemovePlayer()
            {
                Player selectedPlayer = null;

                Console.Clear();

                if (players.Count == 0)
                {
                    Usability.WriteLineInStyle("The players list is empty", ConsoleColor.Blue);
                }
                else
                {
                    ShowAllPlayers();
                    Console.Write("\nEnter player ID you want to remove: ");

                    if (!int.TryParse(Console.ReadLine(), out int playerInput))
                    {
                        Usability.WriteLineInStyle($"Input is incorrect. Enter a number next time");

                    }
                    else
                    {
                        foreach (Player player in players)
                        {
                            if (player.Id == playerInput)
                            {
                                selectedPlayer = player;
                            }
                        }

                        if (selectedPlayer != null)
                        {
                            Usability.WriteLineInStyle($"Player with id {selectedPlayer.Id} (name: {selectedPlayer.Name}) succesfully removed", ConsoleColor.Green);
                            players.Remove(selectedPlayer);
                        }
                        else
                        {
                            Usability.WriteLineInStyle($"Player with id {playerInput} not found", ConsoleColor.Red);
                        }
                    }
                }
            }

            public void BanPlayer(int Id)
            {
            }

            public void ShowAllPlayers()
            {
                foreach (Player player in players)
                {
                    player.ShowInfo();
                }
            }
        }

        class Player
        {
            private static int _lastId;

            public int Id { get; private set; }
            public bool IsBanned { get { return false; } private set { } }
            public string Name { get; private set; }
            public int Level { get { return 1; } private set { } }

            public Player(string name)
            {
                Name = name;

                Id = _lastId++;
            }

            public void ShowInfo()
            {
                Console.Write($"Id: {Id}, name: {Name}, level: {Level}, ");

                switch (IsBanned)
                {
                    case true:
                        Console.WriteLine("status: banned");
                        break;
                    case false:
                        Console.WriteLine("status: not banned");
                        break;

                }
            }
        }

        class Usability
        {
            public static void WriteLineInStyle(string style, ConsoleColor color = ConsoleColor.Red)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(style);
                Console.ResetColor();
            }
        }
    }
}
