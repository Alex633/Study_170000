using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
//timed log
// second player


namespace CodingCoursesButThisTimeForFree
{
    internal class program
    {
        static void Main(string[] args)
        {
            int pacmanXPos = 57;
            int pacmanYPos = 16;

            int pacmanXPosM = 53;
            int pacmanYPosM = 14;
            char[,] currentLevel = new char[0, 0];
            //char[,] levelOuterMap = ReadOuterMap("level1.txt");
            int menuOption = 0;
            ConsoleKeyInfo pressedKey = new ConsoleKeyInfo();
            const char wall = '#'; //map objects
            const char point = '.';
            const char playSpace = 'x';
            const char nothing = '-';
            const char circle = 'o';
            const char teleport = '@'; //
            int score = 0;
            int finalScore = 0;
            bool inGame = false;

            bool isTimed = true;
            long currentTime = 0;
            long duration = 100;
            Stopwatch timer = new Stopwatch();

            Console.CursorVisible = false;

            Task.Run(() =>
            {
                while (true)
                {
                    while (inGame == true)
                    {
                        pressedKey = Console.ReadKey();
                    }
                }
            });

            OffsetWrite("Alexander's Game");
            OffsetWrite("The Pac-Man 2", 50, 14, true, 2000);   

            while (menuOption == 0)
            {
                DrawMenu(ref pacmanYPosM);
                MenuSelector(ref menuOption, ref pacmanXPosM, ref pacmanYPosM, pressedKey);
                Console.Clear();
            }

            if (menuOption == 1)
            {
                OffsetWrite("Level 1", 56);
                currentLevel = LevelMap(1);
                finalScore = GetLevelFinalScore(currentLevel, point, circle);
                Score(ref currentLevel, pacmanXPos, pacmanYPos, point, circle, playSpace, ref score);
                inGame = true;

                while (score != finalScore)
                {
                    DrawMap(currentLevel, wall, point, playSpace, nothing, circle, teleport);
                    GraphicallyImpressivePacman(ref pacmanXPos, ref pacmanYPos);
                    PressedButtonHUD(pressedKey);
                    HandleInput(ref pacmanXPos, ref pacmanYPos, currentLevel, wall, point, playSpace, circle, teleport, pressedKey);
                    Score(ref currentLevel, pacmanXPos, pacmanYPos, point, circle, playSpace, ref score);

                    //Thread.Sleep(100);

                    ClearInput();
                }

                inGame = false;
                Console.Clear();
                OffsetWrite("Good Job", 55, 14);
                OffsetWrite("You did it", 55, 14);
                OffsetWrite("Now go", 55, 14);
            }
            else if (menuOption == 2)
            {
                OffsetWrite("There is no options. Go away");
            }
            else if (menuOption == 3)
            {
                OffsetWrite(":(", 60);
            }
        }
        private static char[,] ReadOuterMap(string path)
        {
            string[] outerFile = File.ReadAllLines("level1.txt");

            char[,] map = new char[GetMaxLineLength(outerFile), outerFile.Length];

            for (int x = 0; x < map.GetLength(0); x++)
                for (int y = 0; y < map.GetLength(1); y++)
                    map[x, y] = outerFile[y][x];

            return map;
        } //map and character drawing

        static char[,] LevelMap(int level)
        {
            char[,] level1 = {
{ '@', '@', '#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#', '@', '@' },
{ '@', '@', '#','#','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','#','#','#','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','#','#', '@', '@' },
{ '@', '@', '#','#','-','.','-','#','#','#','#','#','#','#','-','.','-','#','#','#','#','#','#','#','#','#','-','.','-','#','#','#','-','.','-','#','#','#','#','#','#','#','#','#','-','.','-','#','#','#','#','#','#','#','-','.','-','#','#', '@', '@' },
{ '@', '@', '#','#','-','o','-','#','#','#','#','#','#','#','-','.','-','#','#','#','#','#','#','#','#','#','-','.','-','#','#','#','-','.','-','#','#','#','#','#','#','#','#','#','-','.','-','#','#','#','#','#','#','#','-','o','-','#','#', '@', '@' },
{ '@', '@', '#','#','-','.','-','#','#','#','#','#','#','#','-','.','-','#','#','#','#','#','#','#','#','#','-','.','-','#','#','#','-','.','-','#','#','#','#','#','#','#','#','#','-','.','-','#','#','#','#','#','#','#','-','.','-','#','#', '@', '@' },
{ '@', '@', '#','#','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','#','#', '@', '@' },
{ '@', '@', '#','#','-','.','-','#','#','#','#','#','#','#','-','.','-','#','#','#','-','.','-','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','-','.','-','#','#','#','-','.','-','#','#','#','#','#','#','#','-','.','-','#','#', '@', '@' },
{ '@', '@', '#','#','-','.','-','#','#','#','#','#','#','#','-','.','-','#','#','#','-','.','-','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','-','.','-','#','#','#','-','.','-','#','#','#','#','#','#','#','-','.','-','#','#', '@', '@' },
{ '@', '@', '#','#','-','.','-','.','-','.','-','.','-','.','-','.','-','#','#','#','-','.','-','.','-','.','-','.','-','#','#','#','-','.','-','.','-','.','-','.','-','#','#','#','-','.','-','.','-','.','-','.','-','.','-','.','-','#','#', '@', '@' },
{ '@', '@', '#','#','#','#','#','#','#','#','#','#','#','#','-','.','-','#','#','#','#','#','#','#','#','#','-','x','-','#','#','#','-','x','-','#','#','#','#','#','#','#','#','#','-','.','-','#','#','#','#','#','#','#','#','#','#','#','#', '@', '@' },
{ '@', '@', '#','#','#','#','#','#','#','#','#','#','#','#','-','.','-','#','#','#','#','#','#','#','#','#','-','x','-','#','#','#','-','x','-','#','#','#','#','#','#','#','#','#','-','.','-','#','#','#','#','#','#','#','#','#','#','#','#', '@', '@' },
{ '@', '@', '#','#','#','#','#','#','#','#','#','#','#','#','-','.','-','#','#','#','-','x','-','x','-','x','-','x','-','x','-','x','-','x','-','x','-','x','-','x','-','#','#','#','-','.','-','#','#','#','#','#','#','#','#','#','#','#','#', '@', '@' },
{ '@', '@', '#','#','#','#','#','#','#','#','#','#','#','#','-','.','-','#','#','#','-','x','-','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','-','x','-','#','#','#','-','.','-','#','#','#','#','#','#','#','#','#','#','#','#', '@', '@' },
{ '@', '@', '-','-','-','-','-','-','-','-','-','-','-','-','-','.','-','-','-','-','-','x','-','#','x','x','x','x','x','x','x','x','x','x','x','x','x','#','-','x','-','-','-','-','-','.','-','-','-','-','-','-','-','-','-','-','-','-','-', '@', '@' },
{ '@', '@', '#','#','#','#','#','#','#','#','#','#','#','#','-','.','-','#','#','#','-','x','-','#','x','x','x','x','x','x','x','x','x','x','x','x','x','#','-','x','-','#','#','#','-','.','-','#','#','#','#','#','#','#','#','#','#','#','#', '@', '@' },
{ '@', '@', '#','#','#','#','#','#','#','#','#','#','#','#','-','.','-','#','#','#','-','x','-','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','-','x','-','#','#','#','-','.','-','#','#','#','#','#','#','#','#','#','#','#','#', '@', '@' },
{ '@', '@', '#','#','#','#','#','#','#','#','#','#','#','#','-','.','-','#','#','#','-','x','-','x','-','x','-','x','-','x','-','x','-','x','-','x','-','x','-','x','-','#','#','#','-','.','-','#','#','#','#','#','#','#','#','#','#','#','#', '@', '@' },
{ '@', '@', '#','#','#','#','#','#','#','#','#','#','#','#','-','.','-','#','#','#','-','x','-','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','-','x','-','#','#','#','-','.','-','#','#','#','#','#','#','#','#','#','#','#','#', '@', '@' },
{ '@', '@', '#','#','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','#','#','#','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','#','#', '@', '@' },
{ '@', '@', '#','#','-','.','-','#','#','#','#','#','#','#','-','.','-','#','#','#','#','#','#','#','#','#','-','.','-','#','#','#','-','.','-','#','#','#','#','#','#','#','#','#','-','.','-','#','#','#','#','#','#','#','-','.','-','#','#', '@', '@' },
{ '@', '@', '#','#','-','.','-','#','#','#','#','#','#','#','-','.','-','#','#','#','#','#','#','#','#','#','-','.','-','#','#','#','-','.','-','#','#','#','#','#','#','#','#','#','-','.','-','#','#','#','#','#','#','#','-','.','-','#','#', '@', '@' },
{ '@', '@', '#','#','-','o','-','.','-','.','-','#','#','#','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','#','#','#','-','.','-','.','-','o','-','#','#', '@', '@' },
{ '@', '@', '#','#','#','#','#','#','-','.','-','#','#','#','-','.','-','#','#','#','-','.','-','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','-','.','-','#','#','#','-','.','-','#','#','#','-','.','-','#','#','#','#','#','#', '@', '@' },
{ '@', '@', '#','#','#','#','#','#','-','.','-','#','#','#','-','.','-','#','#','#','-','.','-','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','-','.','-','#','#','#','-','.','-','#','#','#','-','.','-','#','#','#','#','#','#', '@', '@' },
{ '@', '@', '#','#','-','.','-','.','-','.','-','.','-','.','-','.','-','#','#','#','-','.','-','.','-','.','-','.','-','#','#','#','-','.','-','.','-','.','-','.','-','#','#','#','-','.','-','.','-','.','-','.','-','.','-','.','-','#','#', '@', '@' },
{ '@', '@', '#','#','-','.','-','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','-','.','-','#','#','#','-','.','-','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','-','.','-','#','#', '@', '@' },
{ '@', '@', '#','#','-','.','-','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','-','.','-','#','#','#','-','.','-','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','-','.','-','#','#', '@', '@' },
{ '@', '@', '#','#','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','.','-','#','#', '@', '@' },
{ '@', '@', '#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#', '@', '@' }
            };

            if (level == 1)
            {
                return level1;
            }
            else
            {
                return level1;
            }
        }

        static void DrawMap(char[,] currentLevel, char wall, char dot, char playSpace, char nothing, char circle, char teleport)
        {
            int startingMapXPos = 28;
            int startingMapYPos = 0;

            for (int x = 0; x < currentLevel.GetLength(0); x++)
            {
                Console.SetCursorPosition(startingMapXPos, startingMapYPos++);
                for (int y = 0; y < currentLevel.GetLength(1); y++)
                {
                    DrawSingleChar(currentLevel[x, y], wall, dot, playSpace, nothing, circle, teleport);
                }
                Console.WriteLine();
            }

            startingMapYPos = 0;
        }

        static void DrawSingleChar(char character, char wall, char dot, char playSpace, char nothing, char circle, char teleport)
        {
            if (character == wall)
            {
                CharToBackgroundColor(wall, ConsoleColor.DarkBlue);
            }
            else if (character == dot)
            {
                CharToColor(dot, ConsoleColor.DarkYellow);
            }
            else if (character == playSpace)
            {
                CharToBackgroundColor(playSpace, ConsoleColor.Black);
            }
            else if (character == nothing)
            {
                CharToBackgroundColor(nothing, ConsoleColor.Black);
            }
            else if (character == circle)
            {
                CharToColor(circle, ConsoleColor.Yellow);
            }
            else if (character == teleport)
            {
                CharToBackgroundColor(teleport, ConsoleColor.Black);
            }
        }

        static void CharToColor(char character, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(character);
            Console.ForegroundColor = ConsoleColor.White;
        }

        static void CharToBackgroundColor(char character, ConsoleColor color)
        {
            Console.BackgroundColor = color;
            Console.ForegroundColor = color;
            Console.Write(character);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static int GetMaxLineLength(string[] lines)
        {
            int maxLength = lines[0].Length;

            foreach (var currentLine in lines)
                if (currentLine.Length > maxLength)
                    maxLength = currentLine.Length;

            return maxLength;
        }

        static void GraphicallyImpressivePacman(ref int pacmanXPos, ref int pacmanYPos, bool isVisible = true, ConsoleColor color = ConsoleColor.DarkYellow)
        {
            char pacmanEar = '^';
            char pacman = 'o';
            Console.SetCursorPosition(pacmanXPos, pacmanYPos);

            if (isVisible == true)
            {
                Console.BackgroundColor = color;
                CharToColor(pacman, ConsoleColor.Black);
                CharToColor(pacmanEar, ConsoleColor.Black);
                Console.SetCursorPosition(pacmanXPos - 1, pacmanYPos);
                CharToColor(pacmanEar, ConsoleColor.Black);
            }

            Console.BackgroundColor = ConsoleColor.Black;
        } //

        static void Score(ref char[,] currentLevel, int pacmanXPos, int pacmanYPos, char point, char circle, char playSpace, ref int score)
        {
            if (currentLevel[pacmanYPos, pacmanXPos - 28] == point)
            {
                score += 10;
                currentLevel[pacmanYPos, pacmanXPos - 28] = playSpace;
            }

            if (currentLevel[pacmanYPos, pacmanXPos - 28] == circle)
            {
                score += 50;
                currentLevel[pacmanYPos, pacmanXPos - 28] = playSpace;
            }

            Console.SetCursorPosition(10, 6);
            Console.Write("Score: ");
            Console.SetCursorPosition(10, 7);
            Console.Write(score);
        }

        static void DrawMenu(ref int pacmanYPosM)
        {
            if (pacmanYPosM == 14)
            {
                OffsetWrite(" New_Game ", 55, 14, false, 0, true);
            }
            else
            {
                OffsetWrite("new_game", 55, 14, false, 0, false);
            }

            if (pacmanYPosM == 15)
            {
                OffsetWrite(" options ", 55, 15, false, 0, true);
            }
            else
            {
                OffsetWrite("options", 55, 15, false, 0, false);
            }

            if (pacmanYPosM == 16)
            {
                OffsetWrite(" exit_Game ", 55, 16, false, 0, true);
            }
            else
            {
                OffsetWrite("exit_game", 55, 16, false, 0, false);
            }
        } //menu stuff

        static void MenuSelector(ref int menuOption, ref int pacmanXPosM, ref int pacmanYPosM, ConsoleKeyInfo pressedKey)
        {
            ConsoleKeyInfo pressedButton = new ConsoleKeyInfo();
            pressedButton = new ConsoleKeyInfo();

            GraphicallyImpressivePacman(ref pacmanXPosM, ref pacmanYPosM, false);
            pressedButton = Console.ReadKey();
            switch (pressedButton.Key)
            {
                case ConsoleKey.UpArrow:
                case ConsoleKey.W:
                    if (pacmanYPosM != 14)
                        pacmanYPosM--;
                    break;

                case ConsoleKey.DownArrow:
                case ConsoleKey.S:
                    if (pacmanYPosM != 16)
                        pacmanYPosM++;
                    break;

                case ConsoleKey.Enter:
                case ConsoleKey.Spacebar:
                    switch (pacmanYPosM)
                    {
                        case 14:
                            menuOption = 1;
                            break;
                        case 15:
                            menuOption = 2;
                            break;
                        case 16:
                            menuOption = 3;
                            break;
                    }
                    break;
            }
        } //

        static void HandleInput(ref int pacmanXPos, ref int pacmanYPos, char[,] currentLevel, char wall, char point, char playSpace, char circle, char teleport, ConsoleKeyInfo pressedKey)
        {
            switch (pressedKey.Key)
            {
                case ConsoleKey.UpArrow:
                case ConsoleKey.W:
                    MoveIfYouCan(currentLevel, ref pacmanYPos, ref pacmanXPos, wall, teleport, -1);
                    break;

                case ConsoleKey.DownArrow:
                case ConsoleKey.S:
                    MoveIfYouCan(currentLevel, ref pacmanYPos, ref pacmanXPos, wall, teleport, 1);
                    break;

                case ConsoleKey.LeftArrow:
                case ConsoleKey.A:
                    MoveIfYouCan(currentLevel, ref pacmanYPos, ref pacmanXPos, wall, teleport, 0, -2);
                    break;

                case ConsoleKey.RightArrow:
                case ConsoleKey.D:
                    MoveIfYouCan(currentLevel, ref pacmanYPos, ref pacmanXPos, wall, teleport, 0, 2);
                    break;
            }
        } //movement

        static void PressedButtonHUD(ConsoleKeyInfo pressedKey)
        {
            int upX = 12;
            int upY = 25;
            int downX = 12;
            int downY = 27;
            int leftX = 7;
            int leftY = 27;
            int rightX = 17;
            int rightY = 27;

            OffsetWrite(" ↑ ", upX, upY, false, 0, true);
            OffsetWrite(" ↓ ", downX, downY, false, 0, true);
            OffsetWrite(" ← ", leftX, leftY, false, 0, true);
            OffsetWrite(" → ", rightX, rightY, false, 0, true);
            OffsetWrite("                    ", 4, 16, false, 0, false);

            switch (pressedKey.Key)
            {
                case ConsoleKey.UpArrow:
                case ConsoleKey.W:
                    OffsetWrite(" ↑ ", upX, upY, false, 0, true, ConsoleColor.DarkBlue);
                    break;

                case ConsoleKey.DownArrow:
                case ConsoleKey.S:
                    OffsetWrite(" ↓ ", downX, downY, false, 0, true, ConsoleColor.DarkBlue);
                    break;

                case ConsoleKey.LeftArrow:
                case ConsoleKey.A:
                    OffsetWrite(" ← ", leftX, leftY, false, 0, true, ConsoleColor.DarkBlue);
                    break;

                case ConsoleKey.RightArrow:
                case ConsoleKey.D:
                    OffsetWrite(" → ", rightX, rightY, false, 0, true, ConsoleColor.DarkBlue);
                    break;

                default:
                    if (pressedKey.Key != ConsoleKey.Enter)
                        OffsetWrite(" Unkoown Button (" + pressedKey.KeyChar + ") ", 4, 16, false, 0, true, ConsoleColor.DarkBlue);
                    else
                        OffsetWrite(" Unkoown Button (enter) ", 4, 16, false, 0, true, ConsoleColor.DarkBlue);
                    break;
            }
        }

        static void MoveIfYouCan(char[,] currentLevel, ref int pacmanYPos, ref int pacmanXPos, char wall, char teleport, int moveForY = 0, int moveForX = 0)
        {
            const int leftTeleportX = 29;
            const int rightTeleportX = 87;
            const int pacmanXStartPos = 57;
            const int pacmanYStartPos = 16;

            if (currentLevel[pacmanYPos + moveForY, pacmanXPos - 28 + moveForX] == wall)
            {
                WriteInLog("<knoking sound>", 4);
            }
            else
            {
                pacmanYPos += moveForY;
                pacmanXPos += moveForX;
            }
            if (currentLevel[pacmanYPos, pacmanXPos - 28] == teleport)
            {
                switch (pacmanXPos)
                {
                    case leftTeleportX:
                        WriteInLog("jumped to left teleport", 4);
                        pacmanXPos = rightTeleportX - 2;
                        break;

                    case rightTeleportX:
                        WriteInLog("jumped to right teleport", 4);
                        pacmanXPos = leftTeleportX + 2;
                        break;

                    default:
                        WriteInLog("default teleport");
                        pacmanXPos = pacmanXStartPos;
                        pacmanYPos = pacmanYStartPos;
                        break;
                }
            }
        } //

        static void OffsetWrite(string text, int xPosition = 47, int yPosition = 14, bool isTimed = true, long duration = 1600, bool isSelected = false, ConsoleColor selectionColor = ConsoleColor.Blue)
        {
            Stopwatch timer = new Stopwatch();
            long currentTime = 0;

            ClearInput(xPosition, yPosition, text.Length);
            Console.SetCursorPosition(xPosition, yPosition);
            ConsoleHelper.SetCurrentFont("Consolas", 28);

            if (isSelected)
            {
                Console.BackgroundColor = selectionColor;
                Console.WriteLine(text);
                Console.BackgroundColor = ConsoleColor.Black;
            }
            else
            {
                Console.WriteLine(text);
            }

            Console.SetCursorPosition(0, 0);

            if (isTimed)
            {
                while (currentTime < duration)
                {
                    timer.Start();
                    currentTime = timer.ElapsedMilliseconds;
                }

                timer.Stop();
                ClearInput(xPosition, yPosition, text.Length);
            }
        } //misc

        static void WriteInLog(string text, int xPos = 1, int yPos = 13)
        {
            ClearInput(xPos, yPos);
            Console.SetCursorPosition(xPos, yPos);
            Console.Write(text);
        }

        static void ClearInput(int xPos = 0, int yPos = 0, int length = 0)
        {
            Console.SetCursorPosition(xPos, yPos);

            for (int i = 0; i < length; i++)
                Console.Write(" ");
        }

        public static class ConsoleHelper
        {
            private const int FixedWidthTrueType = 54;
            private const int StandardOutputHandle = -11;

            [DllImport("kernel32.dll", SetLastError = true)]
            internal static extern IntPtr GetStdHandle(int nStdHandle);

            [return: MarshalAs(UnmanagedType.Bool)]
            [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
            internal static extern bool SetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool MaximumWindow, ref FontInfo ConsoleCurrentFontEx);

            [return: MarshalAs(UnmanagedType.Bool)]
            [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
            internal static extern bool GetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool MaximumWindow, ref FontInfo ConsoleCurrentFontEx);


            private static readonly IntPtr ConsoleOutputHandle = GetStdHandle(StandardOutputHandle);

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            public struct FontInfo
            {
                internal int cbSize;
                internal int FontIndex;
                internal short FontWidth;
                public short FontSize;
                public int FontFamily;
                public int FontWeight;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
                //[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.wc, SizeConst = 32)]
                public string FontName;
            }

            public static FontInfo[] SetCurrentFont(string font, short fontSize = 0)
            {
                //Console.WriteLine("Set Current Font: " + font);

                FontInfo before = new FontInfo
                {
                    cbSize = Marshal.SizeOf<FontInfo>()
                };

                if (GetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref before))
                {

                    FontInfo set = new FontInfo
                    {
                        cbSize = Marshal.SizeOf<FontInfo>(),
                        FontIndex = 0,
                        FontFamily = FixedWidthTrueType,
                        FontName = font,
                        FontWeight = 400,
                        FontSize = fontSize > 0 ? fontSize : before.FontSize
                    };

                    // Get some settings from current font.
                    if (!SetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref set))
                    {
                        var ex = Marshal.GetLastWin32Error();
                        Console.WriteLine("Set error " + ex);
                        throw new System.ComponentModel.Win32Exception(ex);
                    }

                    FontInfo after = new FontInfo
                    {
                        cbSize = Marshal.SizeOf<FontInfo>()
                    };
                    GetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref after);

                    return new[] { before, set, after };
                }
                else
                {
                    var er = Marshal.GetLastWin32Error();
                    Console.WriteLine("Get error " + er);
                    throw new System.ComponentModel.Win32Exception(er);
                }
            }
        }

        static int GetLevelFinalScore(char[,] currentLevel, int point, int circle)
        {
            int finalScore = 0;

            for (int x = 0; x < currentLevel.GetLength(0); x++)
                for (int y = 0; y < currentLevel.GetLength(1); y++)
                {
                    if (currentLevel[x, y] == point)
                        finalScore += 10;
                    else if (currentLevel[x, y] == circle)
                        finalScore += 50;
                }

            return finalScore;
        }
    }
}
