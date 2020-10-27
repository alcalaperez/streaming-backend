using System.Collections.Generic;
using Dapper;
using Microsoft.AspNetCore.Mvc;


namespace RecYouBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IStreamAPI _streamApi;
        private readonly IDatabaseConnection _database;

        public SearchController(IStreamAPI streamAPI, IDatabaseConnection database)
        {
            _streamApi = streamAPI;
            _database = database;

        }

        // GET: api/<SearchController>
        [HttpGet]
        public IEnumerable<Model.User> Get()
        {
            using (_database.GetInstance)
            {
                return _database.GetInstance.Query<Model.User>("SELECT username, pic_url FROM users");
            }
        }

        // GET api/<SearchController>/al
        [HttpGet("{searchname}")]
        public IEnumerable<Model.User> Get(string searchname)
        {
            return _database.GetInstance.Query<Model.User>("SELECT username, pic_url FROM users WHERE username LIKE @user", new { user = "%" + searchname + "%" });
        }

    }
}
