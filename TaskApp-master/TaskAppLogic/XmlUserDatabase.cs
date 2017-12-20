using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskAppLogic
{
    class XmlUserDatabase : IUserDatabase
    {
        public IUser GetUser(string username)
        {
            throw new NotImplementedException();
        }

        public IUser Login(string username, string password)
        {
            XmlUser huser = new XmlUser();
            huser.UserName = username;
            return huser;
        }

        ILoggedinUser IUserDatabase.Login(string username, string password)
        {
            throw new NotImplementedException();
        }
    }

    class XmlUser : IUser
    {
        public string UserName { get => username; set { username = value; } }

        private string username;
    }
}
