using social_media_api_filter_dotnet10.Models;

namespace social_media_api_filter_dotnet10.Repository;

public class PostRepository : IPostRepository
{
    private readonly List<Post> _posts = [];
    private int _nextId = 1;

    public List<Post> GetAll() => _posts;

    public Post Add(Post post)
    {
        post.Id = _nextId++;
        _posts.Add(post);
        return post;
    }
}
