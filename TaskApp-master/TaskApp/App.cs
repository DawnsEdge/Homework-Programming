using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskAppLogic;

namespace TaskApp
{
    class App
    {
        public void Run()
        {
            for (int attempts = 0; attempts < 3; ++attempts)
            {
                mLoggedInUser = SignIn();
                if (mLoggedInUser != null) break;
                Console.WriteLine("Error: login attempt failed.");
            }

            // If we couldn't log in after a few tries, exit the program.
            if (mLoggedInUser == null) return;

            // Create a temporary task.
            var task = mTaskDb.NewTask();
            task.AssignedTo = mLoggedInUser;
            task.Title = "Do stuff";
            task.Due = DateTime.Now + TimeSpan.FromDays(1);
            task.Priority = Priority.High;
            mTaskDb.SaveTask(task);

            MainMenu();
        }

        private IUser SignIn()
        {
            Console.Write("Enter your user name: ");
            string username = Console.ReadLine();
            Console.Write("Enter your password: ");
            string password = ConsoleHelpers.ReadPasswordLine();

            return mUserDb.Login(username, password);
        }

        private void MainMenu()
        {
            while (true)
            {
                Console.Write(@"
Main menu:
    (L)ist my tasks
    (Q)uit

What'll it be? ");

                switch (char.ToLower(Console.ReadKey().KeyChar))
                {
                    case 'l':
                        ListTasks(mLoggedInUser);
                        break;
                    case 'q':
                        Console.WriteLine($"\nSee ya later, {mLoggedInUser.UserName}!");
                        return;
                    case 'a':
                        Console.WriteLine("\nType the name of the Task you want to add.");
                        Console.Write("> ");
                        string Name = Console.ReadLine();
                        Console.WriteLine("\nType the Description.");
                        Console.Write("> ");
                        string Desc = Console.ReadLine();
                        var task = mTaskDb.NewTask();
                        task.AssignedTo = mLoggedInUser;
                        task.Title = Name;
                        task.Description = Desc;
                        task.Due = DateTime.Now + TimeSpan.FromDays(5);
                        task.Priority = Priority.High;
                        mTaskDb.SaveTask(task);
                        break;
                    case 'f':
                        Console.WriteLine("\nType the task you want to complete");
                        Console.Write("> ");
                        string TaskName = Console.ReadLine();
                        var ttask = GetTask(mLoggedInUser, TaskName);
                        if(ttask == null)
                        {
                            break;
                        }
                        ttask.Description = "Done";
                        break;
                    default:
                        Console.WriteLine("\nI don't know that command.");
                        break;
                }
            }
        }

        private ITask GetTask(IUser user, string taskName)
        {
            var task = mTaskDb.GetTasks(user).Where(t => t.Title == taskName).FirstOrDefault();
            return task;
        }

        private void ListTasks(IUser user)
        {
            Console.WriteLine($"\nOk, here are the tasks for {user.UserName}:");
            foreach (var task in mTaskDb.GetTasks(user))
            {
                Console.WriteLine($"\t{task.Title}, due {task.Due}, priority {task.Priority}");
            }
            Console.WriteLine();
        }

        private ITaskDatabase mTaskDb = new XmlTaskDatabase(new MyUserDatabase(), "tasks.xml");
        private IUserDatabase mUserDb = new MyUserDatabase();
        private IUser mLoggedInUser = null;
    }
}
