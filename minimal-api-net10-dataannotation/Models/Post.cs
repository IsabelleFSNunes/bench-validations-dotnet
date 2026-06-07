namespace minimal_api_net10_dataannotation.Models;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public int UserId { get; set; }
    public int Views { get; set; }
    public List<string> Tags { get; set; } = new();
}
