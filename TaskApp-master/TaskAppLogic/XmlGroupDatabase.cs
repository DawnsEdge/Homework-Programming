using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TaskAppLogic
{
    public class XmlGroupDatabase : IGroupDatabase
    {
        public XmlGroupDatabase(string filename, IUserDatabase UserDb)
        {
            mUserDatabase = (XmlUserDatabase)UserDb;
            mFilename = filename;

            if (File.Exists(mFilename))
            {
                using (FileStream input = File.OpenRead(mFilename))
                {
                    Groups = (List<XmlGroup>)mSerializer.Deserialize(input);
                    foreach (XmlGroup xml in Groups)
                    {
                        foreach(var kvp in xml.MemberNames)
                        {
                            xml.Members.Add(mUserDatabase.GetUser(kvp.user), kvp.rank);
                        }
                    }
                }
            }
        }

        public string mFilename;

        public List<XmlGroup> Groups { get => mGroups; set => mGroups = value; }

        private List<XmlGroup> mGroups = new List<XmlGroup>();

        public XmlUserDatabase mUserDatabase;

        private readonly XmlSerializer mSerializer = new XmlSerializer(typeof(List<XmlGroup>));

        public void AddGroup(string groupname, IUser user)
        {
            var XmlGroup = new XmlGroup();

            if (Groups == null)
            {
                XmlGroup.Members.Add(user, Rank.Admin);
                XmlGroup.GroupName = groupname;
                Groups.Add(XmlGroup);
            }
            else
            {
                if (!Groups.Contains(XmlGroup))
                {
                    XmlGroup.Members.Add(user, Rank.Admin);
                    XmlGroup.GroupName = groupname;
                    Groups.Add(XmlGroup);
                }
                else
                {
                    XmlGroup = (XmlGroup)Groups.Find(g => g.GroupName == groupname);
                }
            }

            if (XmlGroup.Members != null)
            {
                foreach (var kvp in XmlGroup.Members)
                {
                    var newmember = new MemberDTO();
                    newmember.rank = kvp.Value;
                    newmember.user = kvp.Key.UserName;
                    XmlGroup.MemberNames.Add(newmember);
                }
            }

            using (FileStream output = File.Create(mFilename))
            {
                mSerializer.Serialize(output, Groups);
            }
        }

        public IGroup GetGroup(string groupname)
        {
            return Groups.Find(g => g.GroupName == groupname);
        }
    }

    public class XmlGroup : IGroup
    {
        public string GroupName { get; set; }

        [XmlIgnore] public Dictionary<IUser, Rank> Members { get => mMembers; set => mMembers = value; }
        [XmlElement("Member")]
        public List<MemberDTO> MemberNames { get => mMemberNames; set => mMemberNames = value; }

        private List<MemberDTO> mMemberNames = new List<MemberDTO>();
        private Dictionary<IUser, Rank> mMembers = new Dictionary<IUser, Rank>();

        public void AddMember(string username, Rank rank, List<IUser> players)
        {
            if (rank == Rank.Admin)
            {
                Members.Add(players.Find(p => p.UserName == username), Rank.Member);
            }
        }

        public IUser GetUser(string username)
        {
            return Members.Where(kvp => kvp.Key.UserName == username).FirstOrDefault().Key;
        }
    }
}
