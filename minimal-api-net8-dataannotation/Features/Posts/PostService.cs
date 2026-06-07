using minimal_api_net8_dataannotations.Features.Posts.DTOs;
using minimal_api_net8_dataannotations.Models;

namespace minimal_api_net8_dataannotations.Features.Posts;

public class PostService
{
    private readonly List<Post> _posts = new();
    private int _nextId = 1;

    public List<PostResponse> GetAll() => _posts.Select(MapToResponse).ToList();

    public PostResponse Add(CreatePostRequest request)
    {
        var post = new Post
        {
            Id = _nextId++,
            Title = request.Title,
            Body = request.Body,
            UserId = request.UserId,
            Views = request.Views,
            Tags = request.Tags
        };
        _posts.Add(post);
        return MapToResponse(post);
    }

    private static PostResponse MapToResponse(Post p) =>
        new(p.Id, p.Title, p.Body, p.UserId, p.Views, p.Tags);
}
