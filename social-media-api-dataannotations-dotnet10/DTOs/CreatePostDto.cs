using System.ComponentModel.DataAnnotations;

namespace social_media_api_dataannotations_dotnet10.DTOs;

public class CreatePostDto
{
    [Required(ErrorMessage = "Título é obrigatório")]
    [MaxLength(199, ErrorMessage = "Título não pode exceder 199 caracteres")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Conteúdo é obrigatório")]
    public string Body { get; set; } = string.Empty;

    [Range(1, 100, ErrorMessage = "UserId deve estar entre 1 e 100")]
    public int UserId { get; set; }

    [Range(0, 10000, ErrorMessage = "Views deve estar entre 0 e 10000")]
    public int Views { get; set; }

    public List<string> Tags { get; set; } = [];
}