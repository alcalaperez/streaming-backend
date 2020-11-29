using System.Collections.Generic;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace RecYouBackend.Controllers
{
    /*
     * Controller for search related actions
    */
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IDatabaseConnection _database;

        // Dependency inyection
        public SearchController(IDatabaseConnection database)
        {
            _database = database;

        }

        /* 
         * GET api/<SearchController>
         * Search for the requestor info in the database using the JWT
         * Authentication required
        */
        [HttpGet]
        [Authorize]
        public IEnumerable<Model.User> Get()
        {
            using (_database.GetInstance)
            {
                return _database.GetInstance.Query<Model.User>("SELECT username, pic_url FROM users");
            }
        }

        /* 
         * GET api/<SearchController>/{searchname}
         * Search for an specific user in the database
         * Authentication required
        */
        [HttpGet("{searchname}")]
        [Authorize]
        public IEnumerable<Model.User> Get(string searchname)
        {
            return _database.GetInstance.Query<Model.User>("SELECT username, pic_url FROM users WHERE lower(username) LIKE @user", new { user = "%" + searchname.ToLower() + "%" });
        }

    }
}
