using System.ComponentModel.DataAnnotations;

namespace minimal_api_net8_dataannotations.Features.Posts.DTOs;

public record CreatePostRequest
{
    [Required(ErrorMessage = "Título é obrigatório")]
    [MaxLength(199, ErrorMessage = "Título não pode exceder 199 caracteres")]
    public string Title { get; init; } = string.Empty;

    [Required(ErrorMessage = "Conteúdo é obrigatório")]
    public string Body { get; init; } = string.Empty;

    [Range(1, 100, ErrorMessage = "UserId deve estar entre 1 e 100")]
    public int UserId { get; init; }

    [Range(0, 10000, ErrorMessage = "Views deve estar entre 0 e 10000")]
    public int Views { get; init; }

    public List<string> Tags { get; init; } = new();
}
