using minimal_api_net10_dataannotation.Features.Posts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddValidation(); // .NET 10 built-in DataAnnotations validation service
builder.Services.AddSingleton<PostService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPostEndpoints();

app.Run();

public partial class Program { }
