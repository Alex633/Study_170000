using System;
using System.Collections.Generic;

// Implement a database of players and methods for working with it.
// Each player may have a unique number, nickname, level, and a flag indicating if they are banned (a boolean flag).
// Implement the ability to add a player, ban a player by their unique number,
// unban a player by their unique number, and delete a player.
// Creating the actual database is not required; the task is performed using tools
// that you have already learned within the course. However, a class is needed that contains players, and it can be called a "Database".

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
            private bool _yourFuneral = false;

            public void Work()
            {
                while (!_yourFuneral)
                {
                    ShowCommands();
                    HandleCommand();

                    Usability.PressAnything();
                }
            }
            enum UserAction
            {
                add_player = 1,
                remove_player_by_id,
                ban_player,
                unban_player,
                show_all_players,
                get_back_to_real_life
            }

            private void ShowCommands()
            {
                Console.WriteLine("Select a command:\n");

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
                            BanPlayer();
                            break;
                        case (int)UserAction.unban_player:
                            UnbanPlayer();
                            break;
                        case (int)UserAction.show_all_players:
                            ShowAllPlayers();
                            break;
                        case (int)UserAction.get_back_to_real_life:
                            ByeBye(ref _yourFuneral);
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

            public bool TryToSelectPlayerById(out Player selectedPlayer)
            {
                selectedPlayer = null;

                if (players.Count == 0)
                {
                    Usability.WriteLineInStyle("The players list is empty", ConsoleColor.Blue);
                    return false;
                }

                ShowAllPlayers();
                Console.Write("\nEnter player ID: ");

                if (!int.TryParse(Console.ReadLine(), out int playerInput))
                {
                    Usability.WriteLineInStyle($"Input is incorrect. Enter a number next time");
                    return false;
                }

                for (int i = 0; i < players.Count; i++)
                {
                    if (players[i].Id == playerInput)
                    {
                        selectedPlayer = players[i];
                        return true;
                    }

                    if (i == players.Count - 1)
                    {
                        Usability.WriteLineInStyle($"Player with ID: {playerInput} wasn't found");
                    }
                }

                return false;
            }

            public void RemovePlayer()
            {
                Console.Clear();

                if (TryToSelectPlayerById(out Player selectedPlayer))
                {
                    Usability.WriteLineInStyle($"Player with id {selectedPlayer.Id} " +
                        $"(name: {selectedPlayer.Name}) succesfully removed", ConsoleColor.Green);
                    players.Remove(selectedPlayer);
                }
            }

            public void BanPlayer()
            {
                Console.Clear();

                if (TryToSelectPlayerById(out Player playerToBan))
                {
                    Usability.WriteLineInStyle($"Player with id {playerToBan.Id} " +
                        $"(name: {playerToBan.Name}) succesfully BANNED", ConsoleColor.Green);

                    foreach (Player player in players)
                    {
                        if (playerToBan.Id == player.Id)
                        {
                            if (player.IsBanned == true)
                            {
                                Usability.WriteLineInStyle("Again, get that", ConsoleColor.Green);
                                player.IsBanned = true;
                            }
                            else
                            {
                                player.IsBanned = true;
                            }
                        }
                    }
                }
            }

            public void UnbanPlayer()
            {
                Console.Clear();

                if (TryToSelectPlayerById(out Player playerToUnban))
                {
                    foreach (Player player in players)
                    {
                        if (playerToUnban.Id == player.Id)
                        {
                            if (player.IsBanned == false)
                            {
                                Usability.WriteLineInStyle("He isn't even banned, man", ConsoleColor.White);
                            }
                            else
                            {
                                player.IsBanned = false;
                                Usability.WriteLineInStyle($"Player with id {playerToUnban.Id} " +
                                    $"(name: {playerToUnban.Name}) succesfully unbanned", ConsoleColor.Green);
                            }
                        }
                    }
                }
            }

            public void ShowAllPlayers()
            {
                Console.Clear();

                if (players.Count != 0)
                {
                    foreach (Player player in players)
                    {
                        player.ShowInfo();
                    }
                }
                else
                {
                    Usability.WriteLineInStyle("Player list is empty", ConsoleColor.Blue);
                }
            }

            public void ByeBye(ref bool yourFuneral)
            {
                Usability.WriteLineInStyle($"Are you sure? Life is cruel and full of suffering", ConsoleColor.White);
                string playerInput = Console.ReadLine();

                if (playerInput.ToLower() == "yes")
                {
                    Usability.WriteLineInStyle($"Are you like sure sure? It's not going to be pretty", ConsoleColor.Red);
                    playerInput = Console.ReadLine();

                    if (playerInput.ToLower() == "yes")
                    {
                        Usability.WriteLineInStyle("okay, it's your funeral, bye-bye", ConsoleColor.DarkRed);
                        yourFuneral = true;
                        return;
                    }
                }

                Usability.WriteLineInStyle($"That was a correct desision. Good job", ConsoleColor.Green);
            }
        }

        class Player
        {
            private static int _lastId;
            public bool IsBanned;

            public int Id { get; private set; }
            public string Name { get; private set; }
            public int Level { get { return 1; } private set { } }

            public Player(string name)
            {
                Name = name;

                IsBanned = false;
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

            public static void PressAnything(string text = "\npress anything to continue")
            {
                Usability.WriteLineInStyle(text, ConsoleColor.DarkYellow);
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}
