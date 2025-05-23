using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CodingCoursesButThisTimeForFree
{
    internal class program
    {
        static void Main(string[] args)
        {
            bool isOpen = true;
            bool isAddingNumber = true;

            string userInput = null;
            int inputedNumber = 0;
            List<int> numbersList = new List<int>();
            int listSum = 0;
            const string enterNumber = "1";
            const string sum = "2";
            const string exit = "3";

            //testing
            //MenuInput(mainOptions, out isOptionSelected, ref selectedOption, selectorCharXPos, ref selectorCharYPos, middleMenuStartX, middleMenuStartY);
            //Console.ReadKey();
            //

            WriteIn("Alexander's homework", 47, 14, true, 1600, false, ConsoleColor.White);

            WriteIn("Dynamic array (collection actually): Advance", 35, 14, true, 2000);

            while (isOpen)
            {
                SumList(listSum, numbersList);
                Console.Write("\n Ника, привет!" +
                    "\n (1) Ввести число" +
                    "\n (2) Показать сумму чисел" +
                    "\n (3) Выйти\n" +
                    "\n Жду команды: "); ;
                userInput = Console.ReadLine();

                switch (userInput)
                {
                    case enterNumber:
                        isAddingNumber = true;
                        while (isAddingNumber)
                        {
                            EnterTextGUI(out userInput, "Твоё число:");

                            if (!int.TryParse(userInput, out inputedNumber))
                            {
                                Notify("Это не число, глупышка", "Ну ладно", 1, 1, ConsoleColor.Red);
                                Console.WriteLine("\n Попробуем еще раз? (y или n)");
                                userInput = Console.ReadLine();

                                if (userInput != "y")
                                {
                                    isAddingNumber = false;
                                }
                                else
                                {
                                    Console.Clear();
                                }
                            }
                            else
                            {
                                numbersList.Add(inputedNumber);
                                isAddingNumber = false;
                                WriteIn("Число успешно добавлено. Молодец!", 1, 1, true, 2000, false, ConsoleColor.Green);
                            }
                        }

                        break;
                    case sum:
                        Console.Clear();
                        Console.WriteLine("\n Но зачем? Все же было внизу. Ладно, я покажу еще раз\n");
                        SumList(listSum, numbersList, false);
                        Console.WriteLine("\n Теперь довольна?\n");
                        Console.ReadLine();
                        Console.Clear();
                        WriteIn(" Как скажешь, подруга", 2, 2, true, 2000);
                        break;
                    case exit:
                        isOpen = false;
                        Console.Clear();
                        WriteIn(":(", 4, 2, true, 2000);
                        break;
                }
            }
        }

        static void SumList(int sum, List<int> list, bool isAtBottom = true)
        {
            if (isAtBottom)
                Console.SetCursorPosition(0, 27);

            foreach (int item in list)
            {
                sum += item;
                ColorizeText($" {item}", ConsoleColor.DarkGray, false);
            }

            Console.WriteLine();
            ColorizeText($" Сумма чисел: ", ConsoleColor.DarkGray, false);
            ColorizeText($" {sum}g", ConsoleColor.Green, true);

            if (isAtBottom)
                Console.SetCursorPosition(0, 0);
        }

        static void DrawHoverChar(int menuStartX, int menuStartY, bool isVisible = true, ConsoleColor color = ConsoleColor.DarkGreen)
        {
            char hoverChar = '♦';
            if (menuStartX > 1)
            {
                Console.SetCursorPosition(menuStartX - 2, menuStartY);

                if (isVisible == true)
                {
                    ColorizeText(hoverChar, color);
                }
                else
                {
                    Console.Write(" ");
                }
            }
        } //menu CHAR

        static void DrawMenu(List<string> options, int selectedOption, int menuStartX = 55, int menuStartY = 14)
        {
            for (int i = 0; i < options.Count; i++)
            {
                if (selectedOption == i)
                {
                    WriteIn($" {options[i]} ", menuStartX, menuStartY + i, false, 0, true);
                    DrawHoverChar(menuStartX, menuStartY + i, true);
                }
                else
                {
                    WriteIn($" {options[i]} ", menuStartX, menuStartY + i, false, 0, false);
                    DrawHoverChar(menuStartX, menuStartY + i, false);
                }
            }
        }  //menu DRAW

        static void SelectOption(List<string> options, out int selectedOption, int menuStartX = 55, int menuStartY = 14)
        {
            ConsoleKeyInfo pressedButton = new ConsoleKeyInfo();
            bool isOptionSelected = false;
            Console.CursorVisible = false;
            selectedOption = 0;

            while (!isOptionSelected)
            {
                DrawMenu(options, selectedOption, menuStartX, menuStartY);
                pressedButton = Console.ReadKey();

                switch (pressedButton.Key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W:
                        if (options.Count > 0)
                        {
                            if (selectedOption != 0)
                            {
                                selectedOption--;
                            }
                            else
                            {
                                selectedOption = options.Count - 1;
                            }
                        }
                        break;

                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S:
                        if (options.Count > 0)
                        {
                            if (selectedOption != options.Count - 1)
                            {
                                selectedOption++;
                            }
                            else
                            {
                                selectedOption = 0;
                            }
                        }
                        break;

                    case ConsoleKey.Enter:
                    case ConsoleKey.Spacebar:
                        isOptionSelected = true;
                        Console.Clear();
                        break;
                }
            }

            Console.CursorVisible = true;
        } //menu DRAW AND INPUT

        static void Notify(string title, string buttonText, int menuStartX = 2, int menuStartY = 1, ConsoleColor color = ConsoleColor.Gray, bool displayTitle = true)
        {
            ConsoleKeyInfo pressedButton = new ConsoleKeyInfo();
            bool isOptionSelected = false;
            Console.CursorVisible = false;

            if (displayTitle)
            {
                Console.SetCursorPosition(menuStartX, menuStartY);
                ColorizeText($"{title}", color);
            }

            WriteIn($" {buttonText} ", menuStartX + 1, menuStartY + 2, false, 0, true, ConsoleColor.Blue);

            while (!isOptionSelected)
            {
                pressedButton = Console.ReadKey();

                switch (pressedButton.Key)
                {
                    case ConsoleKey.Enter:
                    case ConsoleKey.Spacebar:
                        isOptionSelected = true;
                        break;
                }
            }

            Console.Clear();
        }

        static void EnterTextGUI(out string userInput, string title, string buttonText = "Confirm", int menuStartX = 0, int menuStartY = 2)
        {
            userInput = "";

            Console.Clear();
            Console.WriteLine(title);
            WriteIn($" {buttonText} ", menuStartX, menuStartY + 1, false, 0, true, ConsoleColor.Blue); //confirm button
            WriteIn($"              ", menuStartX, menuStartY - 1, false, 0, true, ConsoleColor.DarkGray); //text field
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.SetCursorPosition(menuStartX, menuStartY - 1);
            userInput = Console.ReadLine();
            Console.BackgroundColor = default;
            Console.Clear();
        }

        static void WriteIn(string text, int xPosition = 47, int yPosition = 14, bool isTimed = false, long duration = 1600, bool isSelected = false, ConsoleColor selectionColor = ConsoleColor.White)
        {
            Stopwatch timer = new Stopwatch();
            long currentTime = 0;

            Console.CursorVisible = false;
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
                Console.ForegroundColor = selectionColor;
                Console.WriteLine(text);
                Console.ForegroundColor = ConsoleColor.Black;
            }


            if (isTimed)
            {
                while (currentTime < duration)
                {
                    timer.Start();
                    currentTime = timer.ElapsedMilliseconds;
                }

                Console.SetCursorPosition(xPosition, yPosition);
                Console.WriteLine("                                             ");
                timer.Stop();
                ClearInput(xPosition, yPosition, text.Length);
            }

            Console.CursorVisible = true;
            Console.SetCursorPosition(0, 0);
        } //write TEXT

        static void ClearInput(int xPos = 0, int yPos = 0, int length = 0)
        {
            Console.SetCursorPosition(xPos, yPos);

            for (int i = 0; i < length; i++)
                Console.Write(" ");
        } //clear

        static void ColorizeText(char character, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.ForegroundColor = color;
            Console.Write(character);
            Console.ForegroundColor = ConsoleColor.White;
        } //colorize char

        static void ColorizeText(string text, ConsoleColor color = ConsoleColor.Gray, bool isWriteLine = true)
        {
            if (isWriteLine)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(text);
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ForegroundColor = color;
                Console.Write(text);
                Console.ForegroundColor = ConsoleColor.White;
            }
        } //colorize text

        static void ColorizeBackground(char character, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.BackgroundColor = color;
            Console.ForegroundColor = color;
            Console.Write(character);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        } //colored tile
        static void ColorizeBackground(string text, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.BackgroundColor = color;
            //Console.ForegroundColor = color;
            Console.Write(text);
            Console.BackgroundColor = ConsoleColor.Black;
            //Console.ForegroundColor = ConsoleColor.White;
        } //colored BG

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
        } //font and font size
    }
}
