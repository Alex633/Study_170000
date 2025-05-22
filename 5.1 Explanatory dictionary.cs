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
            //bool isTimed = true; //timed 
            //long currentTime = 0;
            //long duration = 100;
            //Stopwatch timer = new Stopwatch();
            int middleMenuStartX = 55;
            int middleMenuStartY = 14;
            int selectedOption = 0;
            string userInput = null;
            bool isOpen = true;

            Dictionary<string, string> residents = new Dictionary<string, string>();
            residents.Add("Nia", "is Alexander's favorite person");
            residents.Add("Alexander", "doing his programmer's tasks one per week");
            residents.Add("Robert", "just wants to sleep");
            residents.Add("Mira", "loves her pink ball");
            residents.Add("Masya", "still haven't caugth a fantastic beast in a baseboard");
            residents.Add("Joseph Chan", "loves his trolley");
            residents.Add("Ника", "самый любимый человек в жизни Александра");
            residents.Add("Александр", "делает по одной программе в неделю (если повезет)");
            residents.Add("Роберт", "просто хочет спать");
            residents.Add("Мира", "любит свой розовый мячик");
            residents.Add("Мася", "так и не поймала фантастическую тварь в плинтусе");

            List<string> mainOptions = new List<string>();
            mainOptions.AddRange(new string[] { "New Search", "Exit" });
            List<string> onlyOption = new List<string>();
            onlyOption.AddRange(new string[] { "Continue" });
            List<string> languageSelect = new List<string>();
            languageSelect.AddRange(new string[] { "English", "Русский" });

            List<string> mainOptionsRU = new List<string>();
            mainOptionsRU.AddRange(new string[] { "Новый поиск", "Выход" });
            List<string> onlyOptionRU = new List<string>();
            onlyOptionRU.AddRange(new string[] { "Продолжить" });

            //testing
            //MenuInput(mainOptions, out isOptionSelected, ref selectedOption, selectorCharXPos, ref selectorCharYPos, middleMenuStartX, middleMenuStartY);
            //Console.ReadKey();
            //

            SelectOption(languageSelect, ref selectedOption, 3, 1);

            switch (selectedOption)
            {
                case 0: //english
                    while (isOpen)
                    {
                        SelectOption(mainOptions, ref selectedOption, 3, 1);

                        switch (selectedOption)
                        {
                            case 0: //search
                                EnterTextGUI(out userInput, "Enter Resident:");
                                ShowResidentInformation(residents, userInput);
                                SelectOption(onlyOption, ref selectedOption, 2, 2);
                                Console.Clear();
                                break;
                            case 1: //exit
                                isOpen = false;
                                break;
                        }
                    }
                    break;

                case 1: //russian
                    while (isOpen)
                    {
                        SelectOption(mainOptionsRU, ref selectedOption, 3, 1);

                        switch (selectedOption)
                        {
                            case 0: //search
                                EnterTextGUI(out userInput, "Введите обитателей текущей квартиры:", "Подтвердить");
                                ShowResidentInformation(residents, userInput, true);
                                SelectOption(onlyOptionRU, ref selectedOption, 5, 3);
                                Console.Clear();
                                break;
                            case 1: //exit
                                isOpen = false;
                                break;
                        }
                    }
                    break;
            }

            WriteIn(":(", 2, 2, true);
        }

        static void ShowResidentInformation(Dictionary<string, string> dictionary, string userInput, bool isRu = false)
        {
            if (dictionary.ContainsKey(userInput))
            {
                Console.Clear();
                Console.WriteLine($"{userInput} {dictionary[userInput]}");
            }
            else
            {
                Console.Clear();
                if (!isRu)
                    Console.WriteLine($"Nobody called {userInput} lives here");
                else
                    Console.WriteLine($"В этой квартире нет никого по имени {userInput}");
            }
        }

        static void DrawHoverChar(int menuStartX, int menuStartY, bool isVisible = true, ConsoleColor color = ConsoleColor.DarkGreen)
        {
            char hoverChar = '♦';
            if (menuStartX > 1)
            {
                Console.SetCursorPosition(menuStartX - 2, menuStartY);

                if (isVisible == true)
                {
                    ColorizeChar(hoverChar, color);
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
                }
            }
        }  //menu DRAW

        static void SelectOption(List<string> options, ref int selectedOption, int menuStartX = 55, int menuStartY = 14)
        {
            ConsoleKeyInfo pressedButton = new ConsoleKeyInfo();
            pressedButton = new ConsoleKeyInfo();
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
                        break;
                }

                if (options.Count > 1)
                    Console.Clear();
            }

            Console.CursorVisible = true;
        } //menu DRAW AND INPUT

        static void EnterTextGUI(out string userInput, string title, string buttonText = "Confirm", int menuStartX = 0, int menuStartY = 2)
        {
            userInput = "";

            Console.WriteLine(title);
            WriteIn($" {buttonText} ", menuStartX, menuStartY + 1, false, 0, true); //confirm button
            WriteIn($"              ", menuStartX, menuStartY - 1, false, 0, true, ConsoleColor.DarkGray); //text field
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.SetCursorPosition(menuStartX, menuStartY - 1);
            userInput = Console.ReadLine();
            Console.BackgroundColor = default;

        }

        static void WriteIn(string text, int xPosition = 47, int yPosition = 14, bool isTimed = false, long duration = 1600, bool isSelected = false, ConsoleColor selectionColor = ConsoleColor.DarkGreen)
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
        } //write TEXT

        static void WriteInLog(string text, int xPos = 1, int yPos = 13)
        {
            ClearInput(xPos, yPos);
            Console.SetCursorPosition(xPos, yPos);
            Console.Write(text);
        } //write LOG

        static void ClearInput(int xPos = 0, int yPos = 0, int length = 0)
        {
            Console.SetCursorPosition(xPos, yPos);

            for (int i = 0; i < length; i++)
                Console.Write(" ");
        } //clear

        static void ColorizeChar(char character, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(character);
            Console.ForegroundColor = ConsoleColor.White;
        } //colored text

        static void ColorozeBackground(char character, ConsoleColor color)
        {
            Console.BackgroundColor = color;
            Console.ForegroundColor = color;
            Console.Write(character);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
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

