﻿using System;
using System.Collections.Generic;
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
        public void Get()
        {
            
        }

        // GET api/<UserController>/username/true/false
        [HttpGet("{userName}/{light}")]
        [Authorize]
        public async Task<Model.User> GetAsync(string userName, bool light)
        {
            if (light)
            {               
                return _database.GetInstance.QuerySingleOrDefault<Model.User>("SELECT username, pic_url FROM users where username=@user", new { user = userName });
            } else
            {
                FullUser fullUser;
                fullUser = _database.GetInstance.QuerySingleOrDefault<Model.FullUser>("SELECT username, pic_url FROM users where username=@user", new { user = userName });
                IStreamFeed userFeed = _streamApi.StreamClient.Feed("user", userName);
                fullUser.Posts = await userFeed.GetActivities();
                fullUser.Followers = await userFeed.Followers();
                fullUser.Following = await userFeed.Following();

                return fullUser;
            }

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
