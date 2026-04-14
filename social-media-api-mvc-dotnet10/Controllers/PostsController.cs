using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using social_media_api_fluentvalidation_dotnet10.DTOs;
using social_media_api_fluentvalidation_dotnet10.Models;
using social_media_api_fluentvalidation_dotnet10.Repository;
using social_media_api_fluentvalidation_dotnet10.Services;

namespace social_media_api_fluentvalidation_dotnet10.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController(PostServices postService, PostRepository _repository) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var posts = await postService.GetPostsAsync();
            return Ok(posts);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreatePostDto dto, IValidator<CreatePostDto> validator)
        {
            var result = validator.Validate(dto);

            if (!result.IsValid)
            {
                return BadRequest(result.Errors.Select(e => new {
                    field = e.PropertyName,
                    error = e.ErrorMessage
                }));
            }

            var post = new Post
            {
                Title = dto.Title,
                Body = dto.Body,
                UserId = dto.UserId,
                Views = dto.Views,
                Tags = dto.Tags
            };

            var created = _repository.Add(post);

            return CreatedAtAction(nameof(GetAll), new { id = created.Id }, created);
        }
    }
}
