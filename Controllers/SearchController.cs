using System.Collections.Generic;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace RecYouBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IDatabaseConnection _database;

        public SearchController(IDatabaseConnection database)
        {
            _database = database;

        }

        // GET: api/<SearchController>
        [HttpGet]
        [Authorize]
        public IEnumerable<Model.User> Get()
        {
            using (_database.GetInstance)
            {
                return _database.GetInstance.Query<Model.User>("SELECT username, pic_url FROM users");
            }
        }

        // GET api/<SearchController>/al
        [HttpGet("{searchname}")]
        [Authorize]
        public IEnumerable<Model.User> Get(string searchname)
        {
            return _database.GetInstance.Query<Model.User>("SELECT username, pic_url FROM users WHERE username LIKE @user", new { user = "%" + searchname + "%" });
        }

    }
}
