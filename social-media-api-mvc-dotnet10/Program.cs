using FluentValidation;
using Scalar.AspNetCore;
using social_media_api_fluentvalidation_dotnet10.Repository;
using social_media_api_fluentvalidation_dotnet10.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddHttpClient<PostServices>(client =>
{
    client.BaseAddress = new Uri("https://dummyjson.com/");
});

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddSingleton<PostRepository>(); // singleton = persiste em memória

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(); // UI disponível em /scalar/v1
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
