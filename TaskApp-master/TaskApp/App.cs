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
        public App()
        {
            mTaskDb = new XmlTaskDatabase(mUserDb, "tasks.xml");
        }

        public void Run()
        {
            while(true)
            {
                Console.WriteLine("\n(L)ogin or Create a (N)ew profile? ");
                char lorn = char.ToLower(Console.ReadKey().KeyChar);
                if(lorn == 'l')
                {
                    mLoggedInUser = this.LogIn();
                    break;
                }
                if(lorn == 'n')
                {
                    mLoggedInUser = this.Signup();
                    break;
                }
            }

            // If we couldn't log in after a few tries, exit the program.
            if (mLoggedInUser == null) return;

            
            // Create a temporary task.
            /*
            var task = mTaskDb.NewTask();
            task.AssignedTo = mLoggedInUser;
            task.Title = "Do stuff";
            task.Due = DateTime.Now + TimeSpan.FromDays(1);
            task.Priority = Priority.High;
            mTaskDb.SaveTask(task);
            */
            MainMenu();
        }

        private IUser LogIn() 
        {
            for (int attempts = 0; attempts < 3; ++attempts)
            {
                var LoggedInUser = SignIn();
                if (LoggedInUser != null) return LoggedInUser;
                Console.WriteLine("\nError: login attempt failed.");
            }
            return null;
        }

        private IUser Signup()
        {
            while(true)
            {
                Console.WriteLine("\nChoose a username: ");
                string name = Console.ReadLine();
                if(mUserDb.GetUser(name) != null)
                {
                    Console.WriteLine("Username Taken!");
                    continue;
                }
                Console.WriteLine("Type your Password: ");
                string password = ConsoleHelpers.ReadPasswordLine();
                Console.WriteLine("Type your Password again: ");
                string password2 = ConsoleHelpers.ReadPasswordLine();
                if(password != password2)
                {
                    Console.WriteLine("The passwords aren't the same!");
                    continue;
                }
                mUserDb.AddUser(name, password);
                return mUserDb.GetUser(name);
            }
        }

        private IUser SignIn()
        {
            Console.Write("Enter your user name: ");
            string username = Console.ReadLine();
            Console.Write("Enter your password: ");
            string password = ConsoleHelpers.ReadPasswordLine();

            return (IUser)mUserDb.Login(username, password).User;
        }

        private void MainMenu()
        {
            while (true)
            {
                Console.Write(@"
Main menu:
    (L)ist my tasks
    (Q)uit
    (A)dd a task
    (F)inish a task

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
                        ttask.AssignedTo = null;
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
                Console.WriteLine($"\t{task.Title}, due {task.Due}, priority {task.Priority}, description {task.Description}");
            }
            Console.WriteLine();
        }

        private IUserDatabase mUserDb = new XmlUserDatabase("users.xml");
        private ITaskDatabase mTaskDb;
        private IUser mLoggedInUser = null;
    }
}
