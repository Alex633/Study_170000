using System;
using System.Collections.Generic;
using System.Diagnostics;

//todo:

//      do damage to enemy
//      make enemy hit back
//      bounce enemy forward
//      or if in attack _weaponState and pressing attack buttonSymbol again move sword to owner and then do regular attack again

//      connect with Player and enemy consoleText class
//      make console graphics into stage select

//      add victory royal title  in the os 2

//later      second substage - stage complete after death, when showing death message beginning (in OS)

public class Program
{
    public const char EmptySpace = ' ';
    public const char Enemy = '@';
    public const char Wall = '#';
    public const char Player = 'G';
    public const char Coin = '·';
    public const char BigCoin = '$';

    public const int verticalStep = 1;
    public const int horizontalStep = verticalStep * 2;

    static int xPlayerPosition = 27;
    static int yPlayerPosition = 16;

    static int Score = 0;
    static int Round = 0;

    static char[,] Map = GetMap();
    static char[,] EnemyMap = GetMap();

    static int MapHeight = Map.GetLength(0);
    static int MapWidth = Map.GetLength(1);

    static List<int> YEnemies = new List<int>();
    static List<int> XEnemies = new List<int>();
    static List<int> DirectionEnemy = new List<int>();

    public const int CoinValue = 1;
    public const int BigCoinValue = 5;

    private static void Main(string[] args)
    {
        Console.CursorVisible = false;

        DrawMap();
        DrawPlayer();

        Debug.WriteLine($"\nROUND {Round} - score reached: {IsMapCleared()}, dead: {IsDead()}");

        while (IsMapCleared() == false && IsDead() == false)
        {
            Debug.WriteLine($"\nROUND {Round} - score reached: {IsMapCleared()}, dead: {IsDead()}");
            Round++;
            AttemptSpawnEnemyAtRandomSpace();
            DrawScore();
            CompleteEnemiesTurn();
            MovePlayer();
            UpdateMap();
        }

        if (IsMapCleared())
            OutputVictoryScreen();
        else
            OutputDefeatScreen();
    }

    public static char[,] GetMap()
    {
        char[,] map = {
{   '#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#',    },
{   '#','#',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','#','#','#',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','#','#',    },
{   '#','#',' ','·',' ','#','#','#','#','#','#','#',' ','·',' ','#','#','#','#','#','#','#','#','#',' ','·',' ','#','#','#',' ','·',' ','#','#','#','#','#','#','#','#','#',' ','·',' ','#','#','#','#','#','#','#',' ','·',' ','#','#',    },
{   '#','#',' ','$',' ','#','#','#','#','#','#','#',' ','·',' ','#','#','#','#','#','#','#','#','#',' ',' ',' ','#','#','#',' ','·',' ','#','#','#','#','#','#','#','#','#',' ','·',' ','#','#','#','#','#','#','#',' ','$',' ','#','#',    },
{   '#','#',' ','·',' ','#','#','#','#','#','#','#',' ','·',' ','#','#','#','#','#','#','#','#','#',' ','·',' ','#','#','#',' ','·',' ','#','#','#','#','#','#','#','#','#',' ','·',' ','#','#','#','#','#','#','#',' ','·',' ','#','#',    },
{   '#','#',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','#','#',    },
{   '#','#',' ',' ',' ','#','#','#','#','#','#','#',' ','·',' ','#','#','#',' ','·',' ','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#',' ','·',' ','#','#','#',' ','·',' ','#','#','#','#','#','#','#',' ','·',' ','#','#',    },
{   '#','#',' ','·',' ','#','#','#','#','#','#','#',' ','·',' ','#','#','#',' ','·',' ','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#',' ','·',' ','#','#','#',' ',' ',' ','#','#','#','#','#','#','#',' ','·',' ','#','#',    },
{   '#','#',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','#','#','#',' ','·',' ','·',' ','·',' ','·',' ','#','#','#',' ','·',' ','·',' ','·',' ','·',' ','#','#','#',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','#','#',    },
{   '#','#','#','#','#','#','#','#','#','#','#','#',' ','·',' ','#','#','#','#','#','#','#','#','#',' ',' ',' ','#','#','#',' ',' ',' ','#','#','#','#','#','#','#','#','#',' ','·',' ','#','#','#','#','#','#','#','#','#','#','#','#',    },
{   '#','#','#','#','#','#','#','#','#','#','#','#',' ','·',' ','#','#','#','#','#','#','#','#','#',' ',' ',' ','#','#','#',' ',' ',' ','#','#','#','#','#','#','#','#','#',' ','·',' ','#','#','#','#','#','#','#','#','#','#','#','#',    },
{   '#','#','#','#','#','#','#','#','#','#','#','#',' ','·',' ','#','#','#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#','#','#',' ','·',' ','#','#','#','#','#','#','#','#','#','#','#','#',    },
{   '#','#','#','#','#','#','#','#','#','#','#','#',' ','·',' ','#','#','#',' ',' ',' ','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#',' ',' ',' ','#','#','#',' ','·',' ','#','#','#','#','#','#','#','#','#','#','#','#',    },
{   ' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','·',' ',' ',' ',' ',' ',' ',' ','#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#',' ',' ',' ',' ',' ',' ',' ','·',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',    },
{   '#','#','#','#','#','#','#','#','#','#','#','#',' ','·',' ','#','#','#',' ',' ',' ','#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#',' ',' ',' ','#','#','#',' ','·',' ','#','#','#','#','#','#','#','#','#','#','#','#',    },
{   '#','#','#','#','#','#','#','#','#','#','#','#',' ','·',' ','#','#','#',' ',' ',' ','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#',' ',' ',' ','#','#','#',' ','·',' ','#','#','#','#','#','#','#','#','#','#','#','#',    },
{   '#','#','#','#','#','#','#','#','#','#','#','#',' ','·',' ','#','#','#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#','#','#',' ','·',' ','#','#','#','#','#','#','#','#','#','#','#','#',    },
{   '#','#','#','#','#','#','#','#','#','#','#','#',' ','·',' ','#','#','#',' ',' ',' ','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#',' ',' ',' ','#','#','#',' ','·',' ','#','#','#','#','#','#','#','#','#','#','#','#',    },
{   '#','#',' ',' ',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','#','#','#',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','#','#',    },
{   '#','#',' ','·',' ','#','#','#','#','#','#','#',' ','·',' ','#','#','#','#','#','#','#','#','#',' ','·',' ','#','#','#',' ','·',' ','#','#','#','#','#','#','#','#','#',' ','·',' ','#','#','#','#','#','#','#',' ','·',' ','#','#',    },
{   '#','#',' ','·',' ','#','#','#','#','#','#','#',' ','·',' ','#','#','#','#','#','#','#','#','#',' ','·',' ','#','#','#',' ','·',' ','#','#','#','#','#','#','#','#','#',' ','·',' ','#','#','#','#','#','#','#',' ','·',' ','#','#',    },
{   '#','#',' ','$',' ','·',' ','·',' ','#','#','#',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','#','#','#',' ','·',' ','·',' ','$',' ','#','#',    },
{   '#','#','#','#','#','#',' ','·',' ','#','#','#',' ','·',' ','#','#','#',' ','·',' ','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#',' ','·',' ','#','#','#',' ','·',' ','#','#','#',' ','·',' ','#','#','#','#','#','#',    },
{   '#','#','#','#','#','#',' ','·',' ','#','#','#',' ','·',' ','#','#','#',' ','·',' ','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#',' ','·',' ','#','#','#',' ','·',' ','#','#','#',' ','·',' ','#','#','#','#','#','#',    },
{   '#','#',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','#','#','#',' ','·',' ','·',' ','·',' ','·',' ','#','#','#',' ','·',' ','·',' ','·',' ','·',' ','#','#','#',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','#','#',    },
{   '#','#',' ','·',' ','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#',' ','·',' ','#','#','#',' ','·',' ','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#',' ','·',' ','#','#',    },
{   '#','#',' ','·',' ','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#',' ','·',' ','#','#','#',' ',' ',' ','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#',' ','·',' ','#','#',    },
{   '#','#',' ',' ',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','#','#',    },
{   '#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#',    }
            };

        return map;
    }

    public static void DrawScore()
    {
        int y = 13;
        int x = 62;

        WriteAtPosition(Score, y, x, ConsoleColor.Cyan);
    }

    public static void DrawMap()
    {
        for (var y = 0; y < MapHeight; y++)
        {
            for (var x = 0; x < MapWidth; x++)
            {
                DrawCell(Map[y, x], y, x);
            }
        }
    }

    public static char[,] GetCoppiedAray(char[,] array)
    {
        char[,] tempObjects = new char[array.GetLength(0), array.GetLength(1)];

        for (int y = 0; y < MapHeight; ++y)
        {
            for (int x = 0; x < MapWidth; ++x)
            {
                tempObjects[y, x] = array[y, x];
            }
        }

        return tempObjects;
    }

    public static void DrawCell(char symbol, int y, int x)
    {
        switch (symbol)
        {
            case Wall:
                WriteAtPosition(Map[y, x], y, x, ConsoleColor.DarkBlue);
                break;

            case Coin:
                WriteAtPosition(Map[y, x], y, x, ConsoleColor.Yellow);
                break;

            case BigCoin:
                WriteAtPosition(Map[y, x], y, x, ConsoleColor.Yellow);
                break;
        }

    }

    public static void ClearMapPosition(char[,] map, int yPosition, int xPosition)
    {
        map[yPosition, xPosition] = EmptySpace;
        WriteAtPosition(Map[yPosition, xPosition], yPosition, xPosition);
    }

    public static void DrawPlayer()
    {
        WriteAtPosition(Player, yPlayerPosition, xPlayerPosition, ConsoleColor.Black, ConsoleColor.DarkYellow);
    }

    public static void DrawEnemy(int y, int x)
    {
        WriteAtPosition(Enemy, y, x, ConsoleColor.Black, ConsoleColor.DarkRed);

        EnemyMap[y, x] = Enemy;
    }

    public static void MovePlayer()
    {
        const ConsoleKey UpCommand1 = ConsoleKey.W;
        const ConsoleKey UpCommand2 = ConsoleKey.UpArrow;

        const ConsoleKey DownCommand1 = ConsoleKey.S;
        const ConsoleKey DownCommand2 = ConsoleKey.DownArrow;

        const ConsoleKey LeftCommand1 = ConsoleKey.A;
        const ConsoleKey LeftCommand2 = ConsoleKey.LeftArrow;

        const ConsoleKey RightCommand1 = ConsoleKey.D;
        const ConsoleKey RightCommand2 = ConsoleKey.RightArrow;

        ConsoleKeyInfo pressedKey = Console.ReadKey(true);

        switch (pressedKey.Key)
        {
            case UpCommand1:
            case UpCommand2:
                UpdatePlayerPosition(yTarget: yPlayerPosition - verticalStep, xTarget: xPlayerPosition);
                break;

            case DownCommand1:
            case DownCommand2:
                UpdatePlayerPosition(yTarget: yPlayerPosition + verticalStep, xTarget: xPlayerPosition);
                break;

            case LeftCommand1:
            case LeftCommand2:
                UpdatePlayerPosition(yTarget: yPlayerPosition, xTarget: xPlayerPosition - horizontalStep);
                break;

            case RightCommand1:
            case RightCommand2:
                UpdatePlayerPosition(yTarget: yPlayerPosition, xTarget: xPlayerPosition + horizontalStep);
                break;
        }
    }

    public static void UpdateMap()
    {
        bool isOnCoin = Map[yPlayerPosition, xPlayerPosition] == Coin;
        bool isOnBigCoin = Map[yPlayerPosition, xPlayerPosition] == BigCoin;

        if (isOnCoin == false && isOnBigCoin == false)
            return;

        if (isOnCoin)
            Score += CoinValue;

        if (isOnBigCoin)
            Score += BigCoinValue;

        Map[yPlayerPosition, xPlayerPosition] = EmptySpace;
    }

    public static void UpdatePlayerPosition(int yTarget, int xTarget)
    {
        if (IsValidMoveTarget(yTarget, xTarget) == false)
            return;

        ClearMapPosition(Map, yPlayerPosition, xPlayerPosition);

        yPlayerPosition = yTarget;
        xPlayerPosition = xTarget;

        DrawPlayer();
    }

    public static bool IsValidMoveTarget(int yTarget, int xTarget)
    {
        bool isWithinBounds = yTarget < MapHeight - 1 && yTarget > 0 && xTarget < MapWidth - 1 && xTarget > 0;

        if (isWithinBounds == false)
            return false;

        bool isPathFree = Map[yTarget, xTarget] != Wall;

        return isPathFree;
    }

    public static bool IsDead()
    {
        bool isSteppedOnEnemy = EnemyMap[yPlayerPosition, xPlayerPosition] == Enemy;

        Debug.WriteLineIf(isSteppedOnEnemy, $"Player stepped on the enemy");

        return isSteppedOnEnemy;
    }

    public static Random random = new Random();

    public static void AttemptSpawnEnemyAtRandomSpace()
    {
        if (Round % 4 != 0 || Round >= 40)
            return;

        bool isSpawned = false;

        var rng = new Random();

        while (isSpawned == false)
        {
            int y = rng.Next(0, MapHeight);
            int x = rng.Next(0, MapWidth);

            isSpawned = TrySpawnEnemy(y, x);
        }
    }

    public static bool TrySpawnEnemy(int y, int x)
    {
        if (EnemyMap[y, x] == Wall || x % 2 == 0 || EnemyMap[y, x] == Enemy)
            return false;

        EnemyMap[y, x] = Enemy;
        YEnemies.Add(y);
        XEnemies.Add(x);
        // give this new enemy a random starting direction:
        DirectionEnemy.Add(random.Next(1, 5));

        DrawEnemy(y, x);
        return true;
    }

    public static void CompleteEnemiesTurn()
    {
        for (int i = 0; i < YEnemies.Count; i++)
        {
            int y = YEnemies[i];
            int x = XEnemies[i];
            int direction = DirectionEnemy[i];

            MoveEnemy(y, x, ref direction, out int yNew, out int xNew);

            YEnemies[i] = yNew;
            XEnemies[i] = xNew;
            DirectionEnemy[i] = direction;
        }
    }

    public static void MoveEnemy(int y, int x, ref int direction, out int yNew, out int xNew)
    {
        const int up = 0, down = 1, left = 2, right = 3;
        int dy = 0, dx = 0;

        // Try to step in the *current* direction first
        switch (direction)
        {
            case up: dy = -verticalStep; dx = 0; break;
            case down: dy = verticalStep; dx = 0; break;
            case left: dy = 0; dx = -horizontalStep; break;
            case right: dy = 0; dx = horizontalStep; break;
        }

        // If blocked, pick a new direction
        if (!IsValidMoveTarget(y + dy, x + dx)
            || EnemyMap[y + dy, x + dx] == Enemy)
        {
            direction = random.Next(0, 4);
            // recompute deltas
            switch (direction)
            {
                case up: dy = -verticalStep; dx = 0; break;
                case down: dy = verticalStep; dx = 0; break;
                case left: dy = 0; dx = -horizontalStep; break;
                case right: dy = 0; dx = horizontalStep; break;
            }
        }

        // Now *move one step* in (potentially new) direction
        int yt = y + dy, xt = x + dx;

        // final safety check
        if (IsValidMoveTarget(yt, xt) && EnemyMap[yt, xt] != Enemy)
        {
            ClearMapPosition(EnemyMap, y, x);
            DrawEnemy(yt, xt);
            yNew = yt;
            xNew = xt;
        }
        else
        {
            // really stuck: stay in place
            yNew = y;
            xNew = x;
        }
    }


    public static bool IsMapCleared()
    {
        // y = row (0 to MapHeight-1), x = column (0 to MapWidth-1)
        for (int y = 0; y < MapHeight; y++)
        {
            for (int x = 0; x < MapWidth; x++)
            {
                char cell = Map[y, x];
                if (cell == Coin || cell == BigCoin)
                    return false;   // still a pellet somewhere
            }
        }

        return true;  // no pellets left
    }


    public static void WriteAtPosition(object element, int yPosition, int xPosition, ConsoleColor foregroundColor = ConsoleColor.Gray, ConsoleColor backgroundColor = ConsoleColor.Black)
    {
        Console.ForegroundColor = foregroundColor;
        Console.BackgroundColor = backgroundColor;

        Console.SetCursorPosition(xPosition, yPosition);

        Console.WriteLine(element);

        Console.ResetColor();
    }

    public static void OutputVictoryScreen()
    {
        int x = 50;
        int y = 16;

        Console.Clear();
        WriteAtPosition("You won, I guess", y, x, ConsoleColor.Yellow);
        Console.ReadKey();
    }

    public static void OutputDefeatScreen()
    {
        int x = 50;
        int y = 16;

        WriteAtPosition("Game over", y, x, ConsoleColor.DarkRed);
        Console.ReadKey();
        Console.Clear();
    }
}
