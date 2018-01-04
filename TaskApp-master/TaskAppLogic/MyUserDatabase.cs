using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TaskAppLogic
{
    public class MyUserDatabase : IUserDatabase
    {
        public string Hash(string hashing)
        {
            HashAlgorithm hasher = MD5.Create();
            hasher.Initialize();

            var bytes = Encoding.UTF8.GetBytes(hashing + mSalt);
            bytes = hasher.ComputeHash(bytes);
            return Convert.ToBase64String(bytes);

        }

        public IUser GetUser(string username)
        {
            return mUsers.Find(u => u.UserName == username);
        }

        public ILoggedinUser Login(string username, string password)
        {
            var newpw = Hash(password);

            var user = new MyLoggedinUser();
            user.User = mUsers.Find(u => u.UserName == username);
            return user;
        }

        public void AddUser(string username, string password)
        {
            var newuser = new MyUser();
            newuser.Password = this.Hash(password);
            newuser.UserName = username;
            mUsers.Add(newuser);
        }

        private string mSalt = "SALTYBABYBOIIIIIx10203040020 lol I hope your password is safe ;) love kids not adults ;) ;0 ;# :# \\(O-O)/ WOOO LE ANNIVERSARY :I love ya dad";
        private readonly List<MyUser> mUsers = new List<MyUser>();
    }
    
    public class MyUser : IUser
    {
        public string UserName { get; internal set; }

        public string Password { get; internal set; }
    }

    public class MyLoggedinUser : ILoggedinUser
    {
        public IUser User { get; set; }
    }

}
