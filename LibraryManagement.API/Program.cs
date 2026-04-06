using DotNetEnv;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Infrastructure.Data;
using LibraryManagement.Infrastructure.Repositories;
using LibraryManagement.Infrastructure.CategoryRepository;

//------------------------------------------------------
using LibraryManagement.Application.UserService;
using LibraryManagement.Application.AuthorService;
using LibraryManagement.Application.CategoryService;
//-----------------------------------------------------
using LibraryManagement.Application.UserInterface;
using LibraryManagement.Application.AuthorRepository;
using LibraryManagement.Application.CategoryInterface;
using LibraryManagement.Infrastructure.SubCategoryRepository;
using LibraryManagement.Application.SubCategoryService;
using LibraryManagement.Application.BookRepository;
using LibraryManagement.Infrastructure.BookRepository;
using LibraryManagement.Application.CopyInterface;
//----------------------------------------------------

Env.Load();

var builder = WebApplication.CreateBuilder(args);

// DB CONNECTION
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// CONTROLLERS + ENUM STRING
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter()
        );
    });


// SWAGGER
builder.Services.AddOpenApi();


// REPOSITORIES
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();
builder.Services.AddScoped<ICopyRepository, CopyRepository>();


// SERVICES
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<AuthorService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<SubCategoryService>();
builder.Services.AddScoped<CopyService>();



// CORS (Angular)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();


app.UseHttpsRedirection();     

app.UseCors("AllowAngular"); 

//  SWAGGER
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// CONTROLLERS
app.MapControllers();

app.Run();