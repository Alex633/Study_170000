using System;

public class Program
{
    private static void Main()
    {
        int xPlayerPosition = 27;
        int yPlayerPosition = 16;

        int score = 0;

        char[,] map = LoadLevelMap();

        Console.CursorVisible = false;

        DrawMap(map);

        while (HasCoins(map))
        {
            DrawPlayer(map, yPlayerPosition, xPlayerPosition);
            DrawScore(score);

            ConsoleKeyInfo inputKey = ReadInputKey();
            TryPerformPlayerMoveFromInput(inputKey, map, ref score, ref yPlayerPosition, ref xPlayerPosition);
        }

        DrawVictoryScreen();
    }


    #region Rendering
    public static void DrawMap(char[,] map)
    {
        for (int y = 0; y < map.GetLength(0); y++)
            for (int x = 0; x < map.GetLength(1); x++)
                DrawCell(map[y, x], y, x);
    }

    public static void DrawCell(char symbol, int y, int x)
    {
        const char Wall = '#';
        const char Coin = '·';
        const char BigCoin = '$';

        switch (symbol)
        {
            case Wall:
                WriteAtPosition(symbol, y, x, ConsoleColor.DarkBlue);
                break;

            case Coin:
            case BigCoin:
                WriteAtPosition(symbol, y, x, ConsoleColor.Yellow);
                break;
        }
    }

    public static void DrawPlayer(char[,] map, int yPosition, int xPosition)
    {
        const char Player = 'G';

        map[yPosition, xPosition] = Player;
        WriteAtPosition(Player, yPosition, xPosition, ConsoleColor.Black, ConsoleColor.DarkYellow);
    }

    public static void ClearMapCell(char[,] map, int y, int x)
    {
        const char Empty = ' ';

        map[y, x] = Empty;
        WriteAtPosition(Empty, y, x);
    }

    #region UI
    public static void DrawScore(int score)
    {
        int y = 13;
        int x = 62;

        WriteAtPosition(score, y, x, ConsoleColor.Cyan);
    }

    public static void DrawVictoryScreen()
    {
        int x = 50;
        int y = 16;

        Console.Clear();
        WriteAtPosition("You won, I guess", y, x, ConsoleColor.Yellow);
        Console.ReadKey();
    }
    #endregion
    #endregion

    #region Movement
    public static ConsoleKeyInfo ReadInputKey()
    {
        ConsoleKeyInfo pressedKey = Console.ReadKey(true);

        int yLastPressedKey = 12;
        int xLastPressedKey = 61;
        WriteAtPosition($"[{pressedKey.KeyChar}]", yLastPressedKey, xLastPressedKey, ConsoleColor.Cyan);

        return pressedKey;
    }

    public static void TryPerformPlayerMoveFromInput(ConsoleKeyInfo pressedKey, char[,] map, ref int score, ref int yPlayerPosition, ref int xPlayerPosition)
    {
        int[] offset = GetDirection(pressedKey);

        int yTarget = yPlayerPosition + offset[0];
        int xTarget = xPlayerPosition + offset[1];

        if (IsValidMoveTarget(map, yTarget, xTarget))
            PerformPlayerMove(map, ref score, yTarget, xTarget, ref yPlayerPosition, ref xPlayerPosition);
    }

    public static int[] GetDirection(ConsoleKeyInfo pressedKey)
    {
        int doubleDistance = 2;
        int verticalStep = 1;
        int horizontalStep = verticalStep * doubleDistance;

        const ConsoleKey CommandUpButton1 = ConsoleKey.W;
        const ConsoleKey CommandUpButton2 = ConsoleKey.UpArrow;

        const ConsoleKey CommandDownButton1 = ConsoleKey.S;
        const ConsoleKey CommandDownButton2 = ConsoleKey.DownArrow;

        const ConsoleKey CommandLeftButton1 = ConsoleKey.A;
        const ConsoleKey CommandLeftButton2 = ConsoleKey.LeftArrow;

        const ConsoleKey CommandRightButton1 = ConsoleKey.D;
        const ConsoleKey CommandRightButton2 = ConsoleKey.RightArrow;

        int[] offset = { 0, 0 };

        switch (pressedKey.Key)
        {
            case CommandUpButton1:
            case CommandUpButton2:
                offset[0] -= verticalStep;
                break;

            case CommandDownButton1:
            case CommandDownButton2:
                offset[0] += verticalStep;
                break;

            case CommandLeftButton1:
            case CommandLeftButton2:
                offset[1] -= horizontalStep;
                break;

            case CommandRightButton1:
            case CommandRightButton2:
                offset[1] += horizontalStep;
                break;
        }

        return offset;
    }

    public static void PerformPlayerMove(char[,] map, ref int score, int yTarget, int xTarget,
        ref int yPlayerPosition, ref int xPlayerPosition)
    {
        ClearMapCell(map, yPlayerPosition, xPlayerPosition);

        Move(yTarget, xTarget, ref yPlayerPosition, ref xPlayerPosition);

        score = CalculateScore(map, score, yPlayerPosition, xPlayerPosition);

        DrawPlayer(map, yPlayerPosition, xPlayerPosition);
    }

    public static void Move(int yTarget, int xTarget,
        ref int yPosition, ref int xPosition)
    {
        yPosition = yTarget;
        xPosition = xTarget;
    }

    public static bool IsValidMoveTarget(char[,] map, int yTarget, int xTarget)
    {
        const char Wall = '#';

        bool isWithinBounds = yTarget < map.GetLength(0) - 1
            && yTarget > 0
            && xTarget < map.GetLength(1) - 1
            && xTarget > 0;

        if (isWithinBounds == false)
            return false;

        return map[yTarget, xTarget] != Wall;
    }
    #endregion

    #region Map
    public static char[,] LoadLevelMap()
    {
        char[,] map = {
{   '#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#',    },
{   '#','#',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','#','#','#',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','#','#',    },
{   '#','#',' ','·',' ','#','#','#','#','#','#','#',' ','·',' ','#','#','#','#','#','#','#','#','#',' ','·',' ','#','#','#',' ','·',' ','#','#','#','#','#','#','#','#','#',' ','·',' ','#','#','#','#','#','#','#',' ','·',' ','#','#',    },
{   '#','#',' ','$',' ','#','#','#','#','#','#','#',' ','·',' ','#','#','#','#','#','#','#','#','#',' ','·',' ','#','#','#',' ','·',' ','#','#','#','#','#','#','#','#','#',' ','·',' ','#','#','#','#','#','#','#',' ','$',' ','#','#',    },
{   '#','#',' ','·',' ','#','#','#','#','#','#','#',' ','·',' ','#','#','#','#','#','#','#','#','#',' ','·',' ','#','#','#',' ','·',' ','#','#','#','#','#','#','#','#','#',' ','·',' ','#','#','#','#','#','#','#',' ','·',' ','#','#',    },
{   '#','#',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','#','#',    },
{   '#','#',' ','·',' ','#','#','#','#','#','#','#',' ','·',' ','#','#','#',' ','·',' ','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#',' ','·',' ','#','#','#',' ','·',' ','#','#','#','#','#','#','#',' ','·',' ','#','#',    },
{   '#','#',' ','·',' ','#','#','#','#','#','#','#',' ','·',' ','#','#','#',' ','·',' ','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#',' ','·',' ','#','#','#',' ','·',' ','#','#','#','#','#','#','#',' ','·',' ','#','#',    },
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
{   '#','#',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','#','#','#',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','#','#',    },
{   '#','#',' ','·',' ','#','#','#','#','#','#','#',' ','·',' ','#','#','#','#','#','#','#','#','#',' ','·',' ','#','#','#',' ','·',' ','#','#','#','#','#','#','#','#','#',' ','·',' ','#','#','#','#','#','#','#',' ','·',' ','#','#',    },
{   '#','#',' ','·',' ','#','#','#','#','#','#','#',' ','·',' ','#','#','#','#','#','#','#','#','#',' ','·',' ','#','#','#',' ','·',' ','#','#','#','#','#','#','#','#','#',' ','·',' ','#','#','#','#','#','#','#',' ','·',' ','#','#',    },
{   '#','#',' ','$',' ','·',' ','·',' ','#','#','#',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','#','#','#',' ','·',' ','·',' ','$',' ','#','#',    },
{   '#','#','#','#','#','#',' ','·',' ','#','#','#',' ','·',' ','#','#','#',' ','·',' ','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#',' ','·',' ','#','#','#',' ','·',' ','#','#','#',' ','·',' ','#','#','#','#','#','#',    },
{   '#','#','#','#','#','#',' ','·',' ','#','#','#',' ','·',' ','#','#','#',' ','·',' ','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#',' ','·',' ','#','#','#',' ','·',' ','#','#','#',' ','·',' ','#','#','#','#','#','#',    },
{   '#','#',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','#','#','#',' ','·',' ','·',' ','·',' ','·',' ','#','#','#',' ','·',' ','·',' ','·',' ','·',' ','#','#','#',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','#','#',    },
{   '#','#',' ','·',' ','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#',' ','·',' ','#','#','#',' ','·',' ','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#',' ','·',' ','#','#',    },
{   '#','#',' ','·',' ','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#',' ','·',' ','#','#','#',' ','·',' ','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#',' ','·',' ','#','#',    },
{   '#','#',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','·',' ','#','#',    },
{   '#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#',    }
            };

        return map;
    }

    public static int CalculateScore(char[,] map, int score, int yPlayerPosition, int xPlayerPosition)
    {
        const char Coin = '·';
        const char BigCoin = '$';

        const int CoinValue = 1;
        const int BigCoinValue = 10;

        char cell = map[yPlayerPosition, xPlayerPosition];

        bool isCoin = cell == Coin;
        bool isBigCoin = cell == BigCoin;

        if (isCoin)
            score += CoinValue;

        if (isBigCoin)
            score += BigCoinValue;

        return score;
    }

    public static bool HasCoins(char[,] map)
    {
        const int Coin = '·';
        const int BigCoin = '$';

        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                char cell = map[y, x];

                if (cell == Coin || cell == BigCoin)
                    return true;
            }
        }

        return false;
    }
    #endregion

    #region Helper
    public static void WriteAtPosition(object element, int yPosition, int xPosition,
        ConsoleColor foregroundColor = ConsoleColor.Gray, ConsoleColor backgroundColor = ConsoleColor.Black)
    {
        Console.ForegroundColor = foregroundColor;
        Console.BackgroundColor = backgroundColor;

        Console.SetCursorPosition(xPosition, yPosition);

        Console.WriteLine(element);

        Console.ResetColor();
    }
    #endregion
}
