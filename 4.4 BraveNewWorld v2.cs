using System;

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
    private static void Main(string[] args)
    {
        int xPlayerPosition = 27;
        int yPlayerPosition = 16;

        int score = 0;

        char[,] map = LoadLevelMap();

        Console.CursorVisible = false;

        DrawMap(map);

        while (!HasNoCoinsLeft(map))
        {
            DrawPlayer(map, yPlayerPosition, xPlayerPosition);
            DrawScore(score);

            ConsoleKeyInfo inputKey = ReadInputKey();
            HandleInput(inputKey, map, ref score, ref yPlayerPosition, ref xPlayerPosition);
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

    public static void HandleInput(ConsoleKeyInfo pressedKey, char[,] map, ref int score, ref int yPlayerPosition, ref int xPlayerPosition)
    {
        int[] offset = TranslateKeyToOffset(pressedKey);

        int yTarget = yPlayerPosition + offset[0];
        int xTarget = xPlayerPosition + offset[1];

        if (IsValidMoveTarget(map, yTarget, xTarget))
            MovePlayer(map, ref score, yTarget, xTarget, ref yPlayerPosition, ref xPlayerPosition);
    }

    public static int[] TranslateKeyToOffset(ConsoleKeyInfo pressedKey)
    {
        const int verticalStep = 1;
        const int horizontalStep = verticalStep * 2;

        const ConsoleKey CommandUp1 = ConsoleKey.W;
        const ConsoleKey CommandUp2 = ConsoleKey.UpArrow;

        const ConsoleKey CommandDown1 = ConsoleKey.S;
        const ConsoleKey CommandDown2 = ConsoleKey.DownArrow;

        const ConsoleKey CommandLeft1 = ConsoleKey.A;
        const ConsoleKey CommandLeft2 = ConsoleKey.LeftArrow;

        const ConsoleKey CommandRight1 = ConsoleKey.D;
        const ConsoleKey CommandRight2 = ConsoleKey.RightArrow;

        int[] offset = { 0, 0 };

        switch (pressedKey.Key)
        {
            case CommandUp1:
            case CommandUp2:
                offset[0] -= verticalStep;
                break;

            case CommandDown1:
            case CommandDown2:
                offset[0] += verticalStep;
                break;

            case CommandLeft1:
            case CommandLeft2:
                offset[1] -= horizontalStep;
                break;

            case CommandRight1:
            case CommandRight2:
                offset[1] += horizontalStep;
                break;
        }

        return offset;
    }

    public static void MovePlayer(char[,] map, ref int score, int yTarget, int xTarget,
        ref int yPlayerPosition, ref int xPlayerPosition)
    {
        ClearMapCell(map, yPlayerPosition, xPlayerPosition);

        yPlayerPosition = yTarget;
        xPlayerPosition = xTarget;

        UpdateScore(map, ref score, yPlayerPosition, xPlayerPosition);

        DrawPlayer(map, yPlayerPosition, xPlayerPosition);
    }

    public static bool IsValidMoveTarget(char[,] map, int yTarget, int xTarget)
    {
        bool isWithinBounds = yTarget < map.GetLength(0) - 1
            && yTarget > 0
            && xTarget < map.GetLength(1) - 1
            && xTarget > 0;

        if (isWithinBounds == false)
            return false;

        const char Wall = '#';

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

    public static void UpdateScore(char[,] map, ref int score, int yPlayerPosition, int xPlayerPosition)
    {
        const char Coin = '·';
        const char BigCoin = '$';

        int CoinValue = 1;
        int BigCoinValue = 10;

        bool isOnCoin = map[yPlayerPosition, xPlayerPosition] == Coin;
        bool isOnBigCoin = map[yPlayerPosition, xPlayerPosition] == BigCoin;

        if (isOnCoin == false && isOnBigCoin == false)
            return;

        if (isOnCoin)
            score += CoinValue;

        if (isOnBigCoin)
            score += BigCoinValue;
    }

    public static bool HasNoCoinsLeft(char[,] map)
    {
        const int Coin = '·';
        const int BigCoin = '$';

        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                char cell = map[y, x];

                if (cell == Coin || cell == BigCoin)
                    return false;
            }
        }

        return true;
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
