//Будет 2 одномерных массива:
//1) Полные имена сотрудников (фамилия, имя, отчество);
//2) Должности.

//Описать функцию заполнения массивов досье, функцию форматированного вывода, функцию поиска по фамилии и функцию удаления досье.
//Функция добавления элемента расширяет уже имеющийся массив на 1 и дописывает туда новое значение. 

//Программа должна быть с меню, которое содержит пункты:  
//1) добавить досье
//2) вывести все досье (в одну строку через “-” фио и должность с порядковым номером в начале)  
//3) удалить досье  (Удаление должно быть конкретного элемента, указанного пользователем.
//Массивы уменьшаются на один элемент. Нужны дополнительные проверки, чтобы не возникало ошибок)
//4) поиск по фамилии (показ всех с данной фамилией)
//5) выход 

//Не используйте Array.Resize

using System;

namespace millionDollarsCourses
{
    internal class Program
    {
        static void Main()
        {
            const int CommandDisplayFiles = 1;
            const int CommandAddFile = 2;
            const int CommandDeleteFile = 3;
            const int CommandSearchFile = 4;
            const int CommandExit = 5;

            int commands = CommandExit;

            string[] names = new string[0];
            string[] positions = new string[0];

            bool isWorking = true;

            while (isWorking)
            {
                DisplayMenu(CommandDisplayFiles, CommandAddFile, CommandDeleteFile, CommandSearchFile, CommandExit);

                int userInput = GetNumber("Input command:", commands);

                #region HandleInput
                switch (userInput)
                {
                    case CommandDisplayFiles:
                        DisplayFiles(names, positions);
                        break;
                    case CommandAddFile:
                        AddFile(ref names, ref positions);
                        break;
                    case CommandDeleteFile:
                        RemoveFile(ref names, ref positions);
                        break;
                    case CommandSearchFile:
                        SearchFile(names, positions);
                        break;
                    case CommandExit:
                        isWorking = !Exit();
                        break;
                }
                #endregion

                PressAnyKeyToContinue();
                Console.Clear();
            }
        }

        public static void DisplayMenu(int command1, int command2, int command3, int command4, int command5)
        {
            string displayFilesDescription = "Show all Files";
            string addFileDescription = "Add File";
            string deleteFileDescription = "Delete File";
            string searchFileDescription = "Search File";
            string exitDescription = "Exit";

            Console.WriteLine($"{command1}) {displayFilesDescription}\n" +
                $"{command2}) {addFileDescription}\n" +
                $"{command3}) {deleteFileDescription}\n" +
                $"{command4}) {searchFileDescription}\n" +
                $"{command5}) {exitDescription}\n");
        }

        public static void DisplayFiles(string[] names, string[] positions)
        {
            Console.Clear();

            if (names.Length > 0)
            {
                for (int i = 0; i < names.Length; i++)
                    Console.WriteLine(GetFileInfo(names, positions, i));
            }
            else
            {
                WriteLine("There are no files");
            }
        }

        public static string GetFileInfo(string[] names, string[] positions, int fileIndex)
        {
            if (names.Length >= fileIndex)
            {
                return $"{fileIndex + 1}. {names[fileIndex]} - {positions[fileIndex]}";
            }
            else
            {
                return "File don't exist";
            }
        }

        public static void AddFile(ref string[] names, ref string[] positions)
        {
            Console.Clear();
            string name = GetUserInput("Input full name: ");
            string position = GetUserInput("Input working position: ");
            
            AddItemToArray(ref names, name);
            AddItemToArray(ref positions, position);
        
            WriteLine("File Added!", ConsoleColor.Yellow);
        }

        public static void RemoveFile(ref string[] names, ref string[] positions)
        {
            Console.Clear();

            if (names.Length > 0)
            {
                DisplayFiles(names, positions);
                int userInput = GetNumber("Input file number to remove:", names.Length);

                WriteLine($"File [{GetFileInfo(names, positions, userInput - 1)}] succescully removed", ConsoleColor.Yellow);

                RemoveItemFromArray(ref names, userInput - 1);
                RemoveItemFromArray(ref positions, userInput - 1);

            }
            else
            {
                WriteLine("There are no files");
            }
        }

        public static void SearchFile(string[] names, string[] positions)
        {
            Console.Clear();

            if (names.Length > 0)
            {
                Console.WriteLine("Input name: ");
                string userInput = Console.ReadLine();
                DisplaySearchResult(names, positions, userInput);
            }
            else
            {
                WriteLine("There are no files");
            }
        }

        public static void DisplaySearchResult(string[] names, string[] positions, string name)
        {
            bool isFileFound = false;

            Console.Clear();
            Console.WriteLine("Search result:");

            for (int i = 0; i < names.Length; i++)
            {
                if (names[i].ToLower() == name.ToLower())
                {
                    Console.WriteLine(GetFileInfo(names, positions, i));
                    isFileFound = true;
                }
            }

            if (isFileFound == false)
                WriteLine($"No files match name {name}");
        }

        public static bool Exit()
        {
            Console.WriteLine("\nExiting the Program");
            return true;
        }

        public static void WriteArray(string[] textArray)
        {
            foreach (string item in textArray)
                Console.Write(item + " ");

            Console.WriteLine();
        }

        public static void AddItemToArray(ref string[] textArray, string newItem)
        {
            ExtendArray(ref textArray);
            textArray[textArray.Length - 1] = newItem;
        }

        public static void RemoveItemFromArray(ref string[] textArray, int itemIndex)
        {
            bool isItemFound = false;

            string[] tempTextArray = new string[textArray.Length];

            for (int i = 0; i < textArray.Length - 1; i++)
            {
                if (i != itemIndex && isItemFound == false)
                {
                    tempTextArray[i] = textArray[i];
                }
                else
                {
                    isItemFound = true;
                }

                if (isItemFound)
                {
                    tempTextArray[i] = textArray[i + 1];
                }
            }

            ShrinkArray(ref tempTextArray);
            textArray = tempTextArray;
        }

        public static void ExtendArray(ref string[] textArray, int amount = 1)
        {
            string[] tempTextArray = new string[textArray.Length + amount];

            for (int i = 0; i < textArray.Length; i++)
                tempTextArray[i] = textArray[i];

            textArray = tempTextArray;
        }

        public static void ShrinkArray(ref string[] textArray, int amount = 1)
        {
            string[] tempTextArray = new string[textArray.Length - amount];

            for (int i = 0; i < tempTextArray.Length; i++)
                tempTextArray[i] = textArray[i];

            textArray = tempTextArray;
        }

        public static int GetNumber(string inputPrompt = "Input number", int maxValue = 100, int minValue = 1)
        {
            int userInput;

            Console.Write($"{inputPrompt} ({minValue} - {maxValue}): ");

            while (int.TryParse(Console.ReadLine(), out userInput) == false || userInput < minValue || userInput > maxValue)
                WriteLine($"Incorrect input. Please, input a number ({minValue} - {maxValue}): ");

            return userInput;
        }

        public static string GetUserInput(string inputPrompt)
        {
            Console.Clear();
            Console.Write(inputPrompt);
            return Console.ReadLine();
        }

        public static void WriteLine(string message, ConsoleColor textColor = ConsoleColor.DarkRed)
        {
            Console.ForegroundColor = textColor;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void PressAnyKeyToContinue(string message = "Press any key to continue")
        {
            WriteLine(message, ConsoleColor.Cyan);
            Console.ReadKey(true);
        }
    }
}
