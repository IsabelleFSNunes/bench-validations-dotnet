using System.ComponentModel.DataAnnotations;

namespace minimal_api_net8_dataannotations.Features.Posts.DTOs;

public record CreatePostRequest
{
    [Required(ErrorMessage = "The title is required.")]
    [MaxLength(199, ErrorMessage = "The title is too long. It must not exceed 199 characters.")]
    public string Title { get; init; } = string.Empty;

    [Required(ErrorMessage = "Please provide content for the body.")]
    public string Body { get; init; } = string.Empty;

    [Range(1, 100, ErrorMessage = "UserId must be between 1 and 100.")]
    public int UserId { get; init; }

    [Range(0, 10000, ErrorMessage = "Views must be between 0 and 10,000.")]
    public int Views { get; init; }

    public List<string> Tags { get; init; } = new();
}
