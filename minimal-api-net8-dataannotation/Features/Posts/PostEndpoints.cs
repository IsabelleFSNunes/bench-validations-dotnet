using minimal_api_net8_dataannotations.Features.Posts.DTOs;

namespace minimal_api_net8_dataannotations.Features.Posts;

public static class PostEndpoints
{
    public static void MapPostEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/posts").WithTags("Posts");

        group.MapGet("/", (PostService service) => Results.Ok(service.GetAll()))
             .WithName("GetAllPosts");

        // .NET 8: validation delegated to a reusable IEndpointFilter
        group.MapPost("/", (CreatePostRequest request, PostService service) =>
        {
            var created = service.Add(request);
            return Results.Created($"/api/posts/{created.Id}", created);
        })
        .AddEndpointFilter<ValidationEndpointFilter<CreatePostRequest>>();
    }
}
