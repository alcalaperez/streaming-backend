using Dapper;
using Microsoft.AspNetCore.Mvc;
using RecYouBackend.Model;
using RecYouBackend.Util;

namespace RecYouBackend.Controllers
{
    /*
    * Controller for auth actions
    */
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticatorController : ControllerBase
    {
        private readonly IDatabaseConnection _database;

        // Dependency inyection
        public AuthenticatorController(IDatabaseConnection database)
        {
            _database = database;

        }

        /* 
        * POST api/<AuthenticatorController>
        * Login for the user
        * Returns 401 if the credentials are incorrect
        * Returns a JWT if the credentials are valid
        * No authentication required
        */
        [HttpPost]
        public string Post([FromBody] UserDto userDto)
        {
            PasswordHasher ph = new PasswordHasher();            
            UserDto user = _database.GetInstance.QuerySingleOrDefault<UserDto>("SELECT username, pass FROM users WHERE username = @user", new { user = userDto.Username });
            if(user == null)
            {
                HttpContext.Response.StatusCode = 401;
                return null;

            }

            (bool Verified, bool NeedsUpgrade) checkResult = ph.Check(user.Pass, userDto.Password);
            if (checkResult.Verified)
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
