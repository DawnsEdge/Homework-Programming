using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TaskAppLogic
{
    public class XmlUserDatabase : IUserDatabase
    {
        public XmlUserDatabase(string filename)
        {
            mFilename = filename;

            if (File.Exists(mFilename))
            {
                using (FileStream input = File.OpenRead(mFilename))
                {
                    mXmlUsers = (List<XmlUser>)mSerializer.Deserialize(input);
                }
            }
        }

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
            return mXmlUsers.Find(u => u.UserName == username);
        }

        public ILoggedinUser Login(string username, string password)
        {
            var newpw = Hash(password);

            var loggedinuser = new MyLoggedinUser();
            loggedinuser.User = mXmlUsers.Find(u => u.UserName == username && u.Password == newpw);

            return loggedinuser;
        }

        public void AddUser(string username, string password)
        {
            var xmlUser = new XmlUser();
            xmlUser.Password = this.Hash(password);
            xmlUser.UserName = username;

            if (!mXmlUsers.Contains(xmlUser))
            {
                mXmlUsers.Add(xmlUser);
            }

            using (FileStream output = File.Create(mFilename))
            {
                mSerializer.Serialize(output, mXmlUsers);
            }
        }

        private string mFilename;
        private List<XmlUser> mXmlUsers = new List<XmlUser>();
        private readonly XmlSerializer mSerializer = new XmlSerializer(typeof(List<XmlUser>));
        private string mSalt = "SALTYBABYBOIIIIIx10203040020 lol I hope your password is safe ;) love kids not adults ;) ;0 ;# :# \\(O-O)/ WOOO LE ANNIVERSARY :I love ya dad";
    }

    public class XmlUser : IUser
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
