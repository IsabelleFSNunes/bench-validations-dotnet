using minimal_api_net10_dataannotation.Features.Posts.DTOs;

namespace minimal_api_net10_dataannotation.Features.Posts;

public static class PostEndpoints
{
    public static void MapPostEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/posts").WithTags("Posts");

        group.MapGet("/", (PostService service) => Results.Ok(service.GetAll()))
             .WithName("GetAllPosts");

        // .NET 10: built-in DataAnnotations validation — zero boilerplate
        group.MapPost("/", (CreatePostRequest request, PostService service) =>
        {
            var created = service.Add(request);
            return Results.Created($"/api/posts/{created.Id}", created);
        });
    }
}
