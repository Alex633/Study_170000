using System;
using System.Threading.Tasks;

namespace CsRealLearning
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Performer id0 = new Performer("Dude");
            Performer id1 = new Performer("Roman");

            Task[] tasks = { new Task("Get the Rug back", id0), new Task("Go Bowling", id1) };

            Board boardID1 = new Board(tasks);

            boardID1.DisplayAllTasks();
        }
    }

    class Task
    {
        public string Text;
        public Performer Performer;

        public Task(string text, Performer performer)
        {
            Text = text;
            Performer = performer;
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"{Text}. Assigned to {Performer.Name}");
        }
    }

    class Performer
    {
        public string Name;
        public Performer(string name)
        {
            Name = name;
        }
    }

    class Board
    {
        public Task[] Tasks;

        public Board(Task[] tasks)
        {
            Tasks = tasks;
        }

        public void DisplayAllTasks()
        {
            if (Tasks.Length > 0)
            {
                foreach (Task task in Tasks)
                {
                    task.DisplayInfo();
                }
            }
            else
            {
                Console.WriteLine("This Board has no tasks, dummy");
            }
        }
    }
}
