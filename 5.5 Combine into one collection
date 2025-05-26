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
            string[] firstArray = { "0", "1", "2" };
            string[] secondArray = { "2", "3", "4", "5" };
            HashSet<string> set = new HashSet<string>();

            //testing
            //MenuInput(mainOptions, out isOptionSelected, ref selectedOption, selectorCharXPos, ref selectorCharYPos, middleMenuStartX, middleMenuStartY);
            //Console.ReadKey();
            //

            ShowTitleScreen("Alexander's game", "Combining Collection");
            FillCollectionFrom(set, firstArray);
            FillCollectionFrom(set, secondArray);
            ShowCollection(set);
        }

        static void FillCollectionFrom(HashSet<string> set, string[] array)
        {
            for (int index = 0; index < array.Length; index++)
                set.Add(array[index]);
        }

        static void ShowCollection(HashSet<string> set)
        {
            foreach (string item in set)
            {
                Console.Write(item + " ");
            }
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
            ConsoleHelper.SetCurrentFont("Unispace", 28);
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
