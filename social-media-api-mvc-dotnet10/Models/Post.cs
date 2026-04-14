namespace social_media_api_fluentvalidation_dotnet10.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public int UserId { get; set; }
        public List<string> Tags { get; set; } = [];
        public int Views { get; set; }
    }
}
