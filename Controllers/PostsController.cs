using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecYouBackend.Model;
using Stream;


namespace RecYouBackend.Controllers
{
    /*
     * Controller for posts actions
    */
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IStreamAPI _streamApi;

        // Dependency inyection
        public PostsController(IStreamAPI streamAPI)
        {
            _streamApi = streamAPI;
        }


        /* 
         * POST api/<PostsController>
         * Creates a new post for the user
         * Authentication required
        */
        [HttpPost]
        [Authorize]
        public void Post([FromBody] PostDto pdto)
        {
            IStreamFeed userTimeline = _streamApi.StreamClient.Feed("user", User.Identity.Name);
            var activityData = new Activity(User.Identity.Name, "posts", pdto.AudioUrl + "," + pdto.PictureUrl)
            {
                ForeignId = pdto.AudioUrl,
                Target = pdto.Description,
            };


            userTimeline.AddActivity(activityData);
        }

        /* 
         * DELETE api/<PostsController>/5
         * Deletes a post from the user using the ForeignId
         * Authentication required
        */
        [HttpDelete]
        [Authorize]
        public async void Delete([FromBody] PostDto pdto)
        {
            IStreamFeed userTimeline = _streamApi.StreamClient.Feed("user", User.Identity.Name);
            await userTimeline.RemoveActivity(pdto.ForeignId);
        }
    }
}
