using System.Collections.Generic;
using Stream.Models;

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
        public IEnumerable<Follower> Following { get; internal set; }
        public IEnumerable<Activity> Posts { get; internal set; }
        public IEnumerable<Follower> Followers { get; internal set; }
    }

}
