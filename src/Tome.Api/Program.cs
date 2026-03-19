using Tome.Api.Repositories;
using Tome.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<ISeriesRepository, SeriesRepository>();
builder.Services.AddSingleton<IChapterRepository, ChapterRepository>();
builder.Services.AddScoped<ISeriesService, SeriesService>();
builder.Services.AddScoped<IChapterService, ChapterService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

// Needed for integration test host
public partial class Program { }
