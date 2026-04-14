namespace social_media_api_mvc_dotnet10.DTOs
{
    public class CreatePostDto
    {
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public int UserId { get; set; }   // range: 1 a 100
        public int Views { get; set; }    // range: 0 a 10000
        public List<string> Tags { get; set; } = [];
    }
}
