using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stream;

namespace RecYouBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedController : ControllerBase
    {
        private readonly IStreamAPI _streamApi;

        public FeedController(IStreamAPI streamAPI)
        {
            _streamApi = streamAPI;
        }

        // GET api/<FeedController>/5
        [HttpGet("{username}")]
        [Authorize]
        public IEnumerable<Activity> Get(string username)
        {
            IStreamFeed userTimeline = _streamApi.StreamClient.Feed("timeline", username);
            return userTimeline.GetFlatActivities().Result.Results;
        }

    }
}
