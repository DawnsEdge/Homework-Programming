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
        public IUser GetUser(string username)
        {
            return mUsers.Find(u => u.UserName == username);
        }

        public ILoggedinUser Login(string username, string password)
        {
            string salt = "SALTYBABYBOIIIIIx10203040020 lol I hope your password is safe ;) love kids not adults ;) ;0 ;# :# \\(O-O)/ WOOO LE ANNIVERSARY :I love ya dad";
            HashAlgorithm hasher = MD5.Create();
            hasher.Initialize();

            var bytes = Encoding.UTF8.GetBytes(password + salt);
            bytes = hasher.ComputeHash(bytes);
            var hashedpass = Convert.ToBase64String(bytes);

            var user = new MyLoggedinUser();
            user.UserName = username;
            mUsers.Add(user);
            return user;
        }

        private readonly List<MyUser> mUsers = new List<MyUser>();
    }
    
    public class MyUser : IUser
    {
        public string UserName { get; internal set; }
    }

    public class MyLoggedinUser : MyUser, ILoggedinUser
    {
    }

}
