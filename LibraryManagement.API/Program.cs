using DotNetEnv;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Infrastructure.Data;
using LibraryManagement.Infrastructure.Repositories;
// using LibraryManagement.Infrastructure.BookRepository;
using LibraryManagement.Infrastructure.CategoryRepository;
//------------------------------------------------------
using LibraryManagement.Application.UserService;
using LibraryManagement.Application.AuthorService;
// using LibraryManagement.Application.BookService;
using LibraryManagement.Application.CategoryService;
//-----------------------------------------------------
using LibraryManagement.Application.UserInterface;
using LibraryManagement.Application.AuthorInterface;
// using LibraryManagement.Application.BookInterface;
using LibraryManagement.Application.CategoryInterface;
//----------------------------------------------------





Env.Load();

var builder = WebApplication.CreateBuilder(args);

var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter()
        );
    });

builder.Services.AddCors();  

builder.Services.AddOpenApi();

builder.Services.AddScoped<IUserRepository, UserRepository>();
// builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

// builder.Services.AddScoped<UserService>();
// builder.Services.AddScoped<BookService>();
// builder.Services.AddScoped<AuthorService>();


builder.Services.AddScoped<UserService>();
// builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<AuthorService>();
builder.Services.AddScoped<CategoryService>();



var app = builder.Build();

app.UseCors(builder=>builder.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader());


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();  

app.Run();