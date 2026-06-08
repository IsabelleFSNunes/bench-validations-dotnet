using System.ComponentModel.DataAnnotations;

namespace social_media_api_filter_dotnet10.DTOs;

public class CreatePostDto
{
    [Required(ErrorMessage = "The title is required.")]
    [MaxLength(199, ErrorMessage = "The title is too long. It must not exceed 199 characters.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please provide content for the body.")]
    public string Body { get; set; } = string.Empty;

    [Range(1, 100, ErrorMessage = "UserId must be between 1 and 100.")]
    public int UserId { get; set; }

    [Range(0, 10000, ErrorMessage = "Views must be between 0 and 10,000.")]
    public int Views { get; set; }

    public List<string> Tags { get; set; } = [];
}
