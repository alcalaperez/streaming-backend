using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecYouBackend.Model;
using Stream;


namespace RecYouBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelationsController : ControllerBase
    {
        private readonly IStreamAPI _streamApi;

        public RelationsController(IStreamAPI streamAPI)
        {
            _streamApi = streamAPI;
        }

        // POST api/<RelationsController>
        [HttpPost]
        [Authorize]
        public async Task PostAsync([FromBody] RelationDto rdto)
        {
            IStreamFeed userTimeline = _streamApi.StreamClient.Feed("timeline", User.Identity.Name);
            await userTimeline.FollowFeed("user", rdto.UserToFollowUnfollow);
        }

        // DELETE api/<RelationsController>
        [HttpDelete]
        [Authorize]
        public async Task DeleteAsync([FromBody] RelationDto rdto)
        {
            IStreamFeed userTimeline = _streamApi.StreamClient.Feed("timeline", User.Identity.Name);
            await userTimeline.UnfollowFeed("user", rdto.UserToFollowUnfollow);
        }
    }
}
