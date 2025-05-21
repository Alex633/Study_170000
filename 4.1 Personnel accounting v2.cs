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

                int userInput = ReadInt("Input command:", commands);

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

                WriteLine("Press any key", ConsoleColor.Cyan);
                Console.ReadKey(true);

                Console.Clear();
            }
        }

        public static void DisplayMenu(int displayFilesNumber, int addFileNumber, 
            int deleteFileNumber, int searchFileNumber, int exitNumber)
        {
            string displayFilesDescription = "Show all Files";
            string addFileDescription = "Add File";
            string deleteFileDescription = "Delete File";
            string searchFileDescription = "Search File";
            string exitDescription = "Exit";

            Console.WriteLine($"{displayFilesNumber}) {displayFilesDescription}\n" +
                $"{addFileNumber}) {addFileDescription}\n" +
                $"{deleteFileNumber}) {deleteFileDescription}\n" +
                $"{searchFileNumber}) {searchFileDescription}\n" +
                $"{exitNumber}) {exitDescription}\n");
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
                WriteLine("File folder is empty");
            }
        }

        public static void AddFile(ref string[] names, ref string[] positions)
        {
            Console.Clear();
            string name = ReadUserInput("Input full name: ");
            string position = ReadUserInput("Input working position: ");

            names = AppendToArray(names, name);
            positions = AppendToArray(positions, position);

            WriteLine("File Added!", ConsoleColor.Yellow);
        }

        public static void RemoveFile(ref string[] names, ref string[] positions)
        {
            Console.Clear();

            if (names.Length == 0)
            {
                WriteLine("File folder is empty");
                return;
            }

            DisplayFiles(names, positions);
            int userInput = ReadInt("Input file number to remove:", names.Length);

            int userIndex = userInput - 1;

            WriteLine($"File [{GetFileInfo(names, positions, userIndex)}] succescully removed", ConsoleColor.Yellow);

            names = RemoveItemAt(names, userIndex);
            positions = RemoveItemAt(positions, userIndex);
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

        public static bool Exit()
        {
            Console.WriteLine("\nExiting the Program");
            return true;
        }

        public static string GetFileInfo(string[] names, string[] positions, int fileIndex)
        {
            if (names.Length >= fileIndex)
                return $"{fileIndex + 1}. {names[fileIndex]} - {positions[fileIndex]}";
            else
                return "File don't exist";
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

        public static string[] RemoveItemAt(string[] textArray, int itemIndex)
        {
            string[] tempTextArray = new string[textArray.Length - 1];

            for (int i = 0; i < itemIndex; i++)
                tempTextArray[i] = textArray[i];

            for (int i = itemIndex + 1; i < textArray.Length; i++)
                tempTextArray[i - 1] = textArray[i];

            return tempTextArray;
        }

        public static string[] AppendToArray(string[] textArray, string item)
        {
            string[] tempTextArray = new string[textArray.Length + 1];

            for (int i = 0; i < textArray.Length; i++)
                tempTextArray[i] = textArray[i];

            textArray = tempTextArray;

            textArray[textArray.Length - 1] = item;

            return textArray;
        }

        public static int ReadInt(string inputPrompt = "Input number", int maxValue = 100, int minValue = 1)
        {
            int userInput;

            Console.Write($"{inputPrompt} ({minValue} - {maxValue}): ");

            while (int.TryParse(Console.ReadLine(), out userInput) == false || userInput < minValue || userInput > maxValue)
                WriteLine($"Incorrect input. Please, input a number ({minValue} - {maxValue}): ");

            return userInput;
        }

        public static string ReadUserInput(string inputPrompt)
        {
            Console.Write(inputPrompt);
            return Console.ReadLine();
        }

        public static void WriteLine(string message, ConsoleColor textColor = ConsoleColor.Red)
        {
            Console.ForegroundColor = textColor;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
