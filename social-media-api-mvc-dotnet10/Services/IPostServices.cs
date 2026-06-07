using social_media_api_fluentvalidation_dotnet10.Models;

namespace social_media_api_fluentvalidation_dotnet10.Services;

public interface IPostServices
{
    Task<List<Post>> GetPostsAsync();
}
