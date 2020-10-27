using System.Collections.Generic;

namespace RecYouBackend.Model
{
    public class User
    {
        public User(string userName, string profilePic)
        {
            Username = userName;
            Pic_url = profilePic;
        }

        public User()
        { }

        public string Username { get; set; }
        public string Pic_url { get; set; }

    }

    public class FullUser : User
    {
        public IEnumerable<Stream.Follower> Following { get; internal set; }
        public IEnumerable<Stream.Activity> Posts { get; internal set; }
        public IEnumerable<Stream.Follower> Followers { get; internal set; }
    }

}
