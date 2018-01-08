using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskAppLogic
{
    public enum Rank
    {
        Member,
        Admin
    }

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

    public interface ILoggedinUser
    {
        IUser User { get; }
    }

    public interface ITask
    {
        string Title { get; set; }
        string Description { get; set; }
        IUser AssignedTo { get; set; }
        DateTime Due { get; set; }
        Priority Priority { get; set; }
    }

    public interface IUserDatabase
    {
        ILoggedinUser Login(string username, string password);
        IUser GetUser(string username);
        void AddUser(string username, string password);
    }

    public interface ITaskDatabase
    {
        IEnumerable<ITask> GetTasks(IUser user);
        ITask NewTask();
        void SaveTask(ITask task);
    }

    public interface IGroup
    {
        IUser GetUser(string username);
        void AddMember(string username, Rank rank, List<IUser> player);
        string GroupName { get; set; }
        Dictionary<IUser, Rank> Members { get; }
    }

    public interface IGroupDatabase
    {
        void AddGroup(string groupname, IUser user);
        IGroup GetGroup(string groupname);
        List<XmlGroup> Groups { get; set; }
    }
}
