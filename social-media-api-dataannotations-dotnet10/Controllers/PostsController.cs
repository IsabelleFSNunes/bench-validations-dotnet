using Microsoft.AspNetCore.Mvc;
using social_media_api_dataannotations_dotnet10.DTOs;
using social_media_api_dataannotations_dotnet10.Models;
using social_media_api_dataannotations_dotnet10.Repository;

namespace social_media_api_dataannotations_dotnet10.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController(PostRepository _repository) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll() => Ok(_repository.GetAll());

    [HttpPost]
    public IActionResult Create([FromBody] CreatePostDto dto)
    {

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
