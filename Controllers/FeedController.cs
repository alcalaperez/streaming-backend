using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stream;

namespace RecYouBackend.Controllers
{
    /*
    * Controller for timeline actions
    */
    [Route("api/[controller]")]
    [ApiController]
    public class FeedController : ControllerBase
    {
        private readonly IStreamAPI _streamApi;

        // Dependency inyection
        public FeedController(IStreamAPI streamAPI)
        {
            _streamApi = streamAPI;
        }

        /* 
         * GET api/<FeedController>/5
         * Gets the timeline of the user calling this action
         * Authentication required
        */
        [HttpGet]
        [Authorize]
        public IEnumerable<Activity> Get()
        {
            IStreamFeed userTimeline = _streamApi.StreamClient.Feed("timeline", User.Identity.Name);
            return userTimeline.GetFlatActivities().Result.Results;
        }

    }
}
