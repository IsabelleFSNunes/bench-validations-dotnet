using minimal_api_net7_dataannotations;
using minimal_api_net7_dataannotations.Features.Posts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<PostService>(client =>
{
    client.BaseAddress = new Uri("https://dummyjson.com/");
});

builder.Services.AddSingleton<PostService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPostEndpoints();

app.Run();
