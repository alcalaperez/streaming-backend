namespace RecYouBackend.Model
{
    /*
     * Data Transfer Object for posts
    */
    public class PostDto
    {
        public string PictureUrl { get; set; }
        public string AudioUrl { get; set; }
        public string Description { get; set; }
        public string ForeignId { get; set; }
    }
}
