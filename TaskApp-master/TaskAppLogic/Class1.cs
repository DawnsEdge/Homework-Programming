using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskAppLogic
{
    public enum Priority
    {
        Low,
        Medium,
        High
    }

    public interface IUser
    {
        string UserName { get; }
        string Password { get; }
    }

    public interface ITask
    {
        string Title { get; set; }
        string Description { get; set; }
        string AssignedTo { get; set; }
        DateTime Due { get; set; }
        Priority Priority { get; set; }
    }

    public interface IUserDatabase
    {
        IUser GetUser(string username, string password);
    }

    public interface ITaskDatabase
    {
        IEnumerable<ITask> GetTasks(string userName);
        ITask NewTask();
        void SaveTask(ITask task);
    }

    public class User : IUser
    {
        private string username_;
        private string password_;
        public string UserName { get { return username_; } }
        public string Password { get { return password_; } }

        public User(string name, string password)
        {
            username_ = name;
            password_ = password;
        }
    }

    public class Task : ITask
    {
        private string title_;
        private string description_;
        private string assignedto_;
        private DateTime due_;
        private Priority priority_;
        public string Title { get { return title_; } set { title_ = value; } }
        public string Description { get { return description_; } set { description_ = value; } }
        public string AssignedTo { get { return assignedto_; } set { assignedto_ = value; } }
        public DateTime Due { get { return due_; } set { due_ = value; } }
        public Priority Priority { get { return priority_; } set { priority_ = value; } }
    }

    public class MyUserDatabase : IUserDatabase
    {
        public IUser GetUser(string username, string password)
        {
            IUser user = new User("george", "monkey!");
            if (username == user.UserName || password == user.Password)
            {
                return user;
            } else
            {
                return null;
            }
        }
    }

    public class MyTaskDatabase : ITaskDatabase
    {
        List<ITask> TaskList = new List<ITask>();

        public IEnumerable<ITask> GetTasks(string userName)
        {
            return TaskList;
        }

        public ITask NewTask()
        {
            return new Task();
        }

        public void SaveTask(ITask task)
        {
            TaskList.Add(task);
        }
    }
}
