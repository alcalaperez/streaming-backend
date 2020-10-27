using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecYouBackend.Model;
using Stream;


namespace RecYouBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IStreamAPI _streamApi;

        public PostsController(IStreamAPI streamAPI)
        {
            _streamApi = streamAPI;
        }


        // POST api/<PostsController>
        [HttpPost]
        [Authorize]
        public void Post([FromBody] PostDto pdto)
        {
            IStreamFeed userTimeline = _streamApi.StreamClient.Feed("user", pdto.UserName);
            var activityData = new Activity(pdto.UserName, "posts", pdto.AudioUrl + "," + pdto.PictureUrl)
            {
                ForeignId = pdto.AudioUrl,
                Target = pdto.Description,
            };


            userTimeline.AddActivity(activityData);
        }

        // DELETE api/<PostsController>/5
        [HttpDelete]
        [Authorize]
        public void Delete([FromBody] PostDto pdto)
        {
            IStreamFeed userTimeline = _streamApi.StreamClient.Feed("user", pdto.UserName);
            userTimeline.RemoveActivity(pdto.AudioUrl, true);
        }
    }
}
