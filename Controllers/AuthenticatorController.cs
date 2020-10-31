using Dapper;
using Microsoft.AspNetCore.Mvc;
using RecYouBackend.Model;
using RecYouBackend.Util;

namespace RecYouBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticatorController : ControllerBase
    {
        private readonly IDatabaseConnection _database;

        public AuthenticatorController(IDatabaseConnection database)
        {
            _database = database;

        }

        // POST api/<AuthenticatorController>        
        [HttpPost]
        public string Post([FromBody] UserDto userDto)
        {
            var pic = _database.GetInstance.QuerySingleOrDefault<string>("SELECT pic_url FROM users WHERE username = @user AND pass = @pass", new { user = userDto.Username, pass = userDto.Password });
            if(pic != null)
            {
                return JWT.GenerateToken(userDto.Username);
            } else
            {
                HttpContext.Response.StatusCode = 401;
                return null;
            }
        }

        

    }
}
