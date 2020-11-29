using System.Collections.Generic;

namespace RecYouBackend.Model
{
    /*
     * Model for the users
     * Light version
    */
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

    /*
     * Model for the users with full info
     * Extends the normal user and adds followers, following and posts
    */
    public class FullUser : User
    {
        public IEnumerable<Stream.Follower> Following { get; internal set; }
        public IEnumerable<Stream.Activity> Posts { get; internal set; }
        public IEnumerable<Stream.Follower> Followers { get; internal set; }
    }

}
