using social_media_api_fluentvalidation_dotnet10.Models;

namespace social_media_api_fluentvalidation_dotnet10.Services
{
    public class PostServices(HttpClient httpClient)
    {
        public async Task<List<Post>> GetPostsAsync()
        {
            var response = await httpClient.GetFromJsonAsync<PostsResponse>("posts?limit=10");
            return response?.Posts ?? [];
        }
    }

    public record PostsResponse(List<Post> Posts, int Total, int Skip, int Limit);
}