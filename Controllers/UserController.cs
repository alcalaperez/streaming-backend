using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecYouBackend.Model;
using RecYouBackend.Util;
using Stream;

namespace RecYouBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IStreamAPI _streamApi;
        private readonly IDatabaseConnection _database;

        public UserController(IStreamAPI streamAPI, IDatabaseConnection database)
        {
            _streamApi = streamAPI;
            _database = database;

        }

        // GET: api/<UserController>
        [HttpGet]
        public IEnumerable<Model.User> Get()
        {
            return _database.GetInstance.Query<Model.User>("SELECT username, pic_url FROM users");
        }

        // GET api/<UserController>/username/true/false
        [HttpGet("{userName}/{light}")]
        [Authorize]
        public async Task<Model.User> Get(string userName, bool light)
        {
            if (light)
            {               
                return _database.GetInstance.QuerySingleOrDefault<Model.User>("SELECT username, pic_url FROM users where username=@user", new { user = userName });
            } else
            {
                FullUser fullUser;
                fullUser = _database.GetInstance.QuerySingleOrDefault<FullUser>("SELECT username, pic_url FROM users where username=@user", new { user = userName });
                IStreamFeed userFeed = _streamApi.StreamClient.Feed("user", userName);
                IStreamFeed userTimeline = _streamApi.StreamClient.Feed("timeline", userName);
                fullUser.Posts = await userFeed.GetActivities();
                fullUser.Followers = await userFeed.Followers();
                fullUser.Following = await userTimeline.Following();

                // Remove the user itself from the list of followers/following
                fullUser.Followers = fullUser.Followers.Where(f => f.FeedId.Split(':')[1] != userName);
                fullUser.Following = fullUser.Following.Where(f => f.TargetId.Split(':')[1] != userName);


                return fullUser;
            }

        }

        // GET api/<UserController>/username/true/false
        [HttpGet("exists/{userName}")]
        [AllowAnonymous]
        public bool Get(string userName)
        {

            var u = _database.GetInstance.QuerySingleOrDefault<Model.User>("SELECT username, pic_url FROM users where username=@user", new { user = userName });
            if (u != null)
            {
                return true;
            }
            return false;

        }

        // POST api/<UserController>
        [HttpPost]
        public async Task<string> PostAsync([FromBody] UserDto userDto)
        {
            var userData = new Dictionary<string, object>()
            {
                {"profile_pic", userDto.ProfilePictureUrl}
            };

            try
            {
                // Add user to the Stream API
                await _streamApi.StreamClient.Users.Add(userDto.Username, userData);
                // Add user to the DB
                _database.GetInstance.Execute("INSERT INTO users (username, pass, pic_url) VALUES(@user, @pass, @pic)", new { user = userDto.Username, pass = userDto.Password, pic = userDto.ProfilePictureUrl });
                // Register user timeline to his profile (the user timeline will show his own posts)
                IStreamFeed userTimeline = _streamApi.StreamClient.Feed("timeline", userDto.Username);
                await userTimeline.FollowFeed("user", userDto.Username);
            }
            catch (Exception e)
            {
                return e.Message;
            }

            return JWT.GenerateToken(userDto.Username);

        }

    }
}
