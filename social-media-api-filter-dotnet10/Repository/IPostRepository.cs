using social_media_api_filter_dotnet10.Models;

namespace social_media_api_filter_dotnet10.Repository;

public interface IPostRepository
{
    List<Post> GetAll();
    Post Add(Post post);
}
