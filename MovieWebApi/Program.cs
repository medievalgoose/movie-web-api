using Microsoft.EntityFrameworkCore;
using MovieWebApi.Data;
using MovieWebApi.Interfaces;
using MovieWebApi.Repositories;
using MovieWebApi.Models;
using MovieWebApi.DTO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Dependency injection for repository pattern.
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IRatingRepository, RatingRepository>();

// Auto mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MovieWebApiContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
