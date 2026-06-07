using minimal_api_net7_dataannotations.Features.Posts;
using minimal_api_net7_dataannotations.Features.Posts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniValidation;

namespace minimal_api_net7_dataannotations
{
    public static class PostEndpoints
    {
        public static void MapPostEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/posts").WithTags("Posts");

            group.MapGet("/", (PostService service) => Results.Ok(service.GetAll()))
                 .WithName("GetAllPosts");

            group.MapPost("/", (CreatePostRequest request, PostService service) =>
            {
                // * to include the ranges and data annotations requirements
                if (!MiniValidator.TryValidate(request, out var errors))
                    return Results.ValidationProblem(errors);
                // ***

                var created = service.Add(request);
                return Results.Created($"/api/posts/{created.Id}", created);
            });
        }
    }
}
