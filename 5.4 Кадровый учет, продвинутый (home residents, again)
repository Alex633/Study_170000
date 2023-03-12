using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

namespace CodingCoursesButThisTimeForFree
{
    internal class Program
    {
        static void Main()
        {
            Dictionary<string, string> residents = new Dictionary<string, string>();
            bool isOpen = true;
            const ConsoleKey CommandAdd = ConsoleKey.Enter;
            const ConsoleKey CommandRemove = ConsoleKey.Delete;
            const ConsoleKey CommandDisplay = ConsoleKey.Tab;
            const ConsoleKey CommandExit = ConsoleKey.Escape;

            //testing
            //MenuInput(mainOptions, out isOptionSelected, ref selectedOption, selectorCharXPos, ref selectorCharYPos, middleMenuStartX, middleMenuStartY);
            //Console.ReadKey();
            //

            ShowTitleScreen("Alexander's homework", "Home Residents: Again");
            AddExistingsResidents(residents);
            Console.CursorVisible = false;

            while (isOpen)
            {
                ColorizeText("(enter) Add Resident" +
                    "\n(delete) Kill Resident" +
                    "\n(tab) Look at Residents" +
                    "\n(esc) Leave Residents Alone", ConsoleColor.Gray);
                ConsoleKeyInfo inputKey = Console.ReadKey();

                switch (inputKey.Key)
                {
                    case CommandAdd:
                        AddFile(residents);
                        break;
                    case CommandRemove:
                        RemoveFile(residents);
                        break;
                    case CommandDisplay:
                        DisplayFiles(residents);
                        break;
                    case CommandExit:
                        isOpen = false;
                        break;
                }
                Console.Clear();
            }
            Console.Clear();
            WriteInFor(":(");
        }

        static void DisplayFiles(Dictionary<string, string> dictionary)
        {
            int count = 1;

            Console.Clear();

            foreach (string item in dictionary.Keys)
                Console.WriteLine($"{count++}. {item} - {dictionary[item]}");

            ColorizeText("press anything to continue", ConsoleColor.DarkGray);
            Console.ReadKey();
            Console.Clear();
        }

        static void AddFile(Dictionary<string, string> dictionary)
        {
            Console.Clear();
            Console.CursorVisible = true;
            Console.WriteLine("Name:");
            string key = Console.ReadLine();
            Console.Clear();
            WriteInFor("What kind of name is that?", 0, 0, 2000);
            Console.WriteLine("Profession:");
            string value = Console.ReadLine();
            dictionary.Add(key, value);
            Console.CursorVisible = false;
            Console.Clear();
            ColorizeText($"File {key} has been succesfully added", ConsoleColor.Green);
            ColorizeText("press anything to continue", ConsoleColor.DarkGray);
            Console.ReadKey();
            Console.Clear();
        }

        static void RemoveFile(Dictionary<string, string> dictionary)
        {
            Console.CursorVisible = true;
            Console.Clear();
            Console.WriteLine("Who do you want to kill:");
            string key = Console.ReadLine();
            Console.Clear();

            if (dictionary.ContainsKey(key))
            {
                dictionary.Remove(key);
                ColorizeText($"{key} has been succesfully killed", ConsoleColor.DarkRed);
            }
            else
            {
                ColorizeText($"No resident with name {key} has been found. Lucky him/her", ConsoleColor.Green);
            }
            Console.CursorVisible = false;
            ColorizeText("press anything to continue", ConsoleColor.DarkGray);
            Console.ReadKey();
            Console.Clear();
        }

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

        static void AddExistingsResidents(Dictionary<string, string> dictionary)
        {
            dictionary.Add("Nia-Yu", "Programmer");
            dictionary.Add("Alexander", "Programmer");
            dictionary.Add("Robert", "Dog");
            dictionary.Add("Mira", "Dog");
            dictionary.Add("Fiaktista", "Antidog");
        }

        static void ShowTitleScreen(string developer, string game, int startXDev = 47, int startXGame = 47)
        {
            int startY = 14;
            int timingDev = 1600;
            int timingGame = 2000;

            WriteInFor(developer, startXDev, startY, timingDev, false, ConsoleColor.White);
            WriteInFor(game, startXGame, startY, timingGame);
        }

        static void WriteInFor(string text, int xPosition = 47, int yPosition = 14, int duration = 1600, bool isSelected = false, ConsoleColor selectionColor = ConsoleColor.White)
        {
            Console.CursorVisible = false;
            ConsoleHelper.SetCurrentFont("Consolas", 28);
            ClearInput(xPosition, yPosition, text.Length);
            Console.SetCursorPosition(xPosition, yPosition);

            if (isSelected)
            {
                Console.BackgroundColor = selectionColor;
                Console.WriteLine(text);
            }
            else
            {
                Console.ForegroundColor = selectionColor;
                Console.WriteLine(text);
            }

            Console.BackgroundColor = default;
            Console.ForegroundColor = ConsoleColor.White;

            if (duration > 0)
            {
                Thread.Sleep(duration);
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
