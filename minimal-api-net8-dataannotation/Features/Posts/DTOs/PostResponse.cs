namespace minimal_api_net8_dataannotations.Features.Posts.DTOs;

public record PostResponse(int Id, string Title, string Body, int UserId, int Views, List<string> Tags);
