using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;
using social_media_api_filter_dotnet10.Filters;
using social_media_api_filter_dotnet10.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>(); // aplica o filtro globalmente
});

// Suprime a validação automática do [ApiController] para o ValidationFilter assumir o controle
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddOpenApi();
builder.Services.AddSingleton<IPostRepository, PostRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
