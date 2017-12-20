using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TaskAppLogic;

namespace TaskAppTests
{
    [TestFixture]
    public class MockTests
    {
        [Test]
        public void UserTest()
        {
            IUserDatabase database = new MyUserDatabase();

            IUser george = database.Login("george", "monkey!");
            Assert.IsNotNull(george);
            Assert.AreEqual("george", george.UserName);
        }

        [Test]
        public void TaskTest()
        {
            IUserDatabase userDb = new MyUserDatabase();
            IUser george = userDb.Login("george", "monkey!");

            ITaskDatabase database = new MockTaskDatabase();

            ITask task = database.NewTask();
            task.AssignedTo = george;
            task.Title = "George's task";
            database.SaveTask(task);

            var tasks = database.GetTasks(george);
            Assert.Greater(tasks.Count(), 0);
            Assert.AreEqual(george, tasks.First().AssignedTo);
        }
    }
}
