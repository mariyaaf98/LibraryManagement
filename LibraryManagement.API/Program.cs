using DotNetEnv;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using LibraryManagement.Application.Common;

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
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<TokenService>();

// CORS (Angular)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

// JWT CONFIG
var key = Environment.GetEnvironmentVariable("JWT_KEY");

if (string.IsNullOrEmpty(key))
{
    throw new Exception("JWT_KEY is missing in environment variables");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
            ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key)
            ),

            ClockSkew = TimeSpan.Zero
        };


        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.Request.Cookies["accessToken"];

                if (!string.IsNullOrEmpty(token))
                {
                    context.Token = token;
                }

                return Task.CompletedTask;
            }
        };
    });


builder.Services.AddAuthorization();

var app = builder.Build();

// MIDDLEWARE PIPELINE
// app.UseHttpsRedirection();

app.UseCors("AllowAngular");

// SWAGGER
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// GLOBAL EXCEPTION HANDLER
app.UseMiddleware<ExceptionMiddleware>();

// AUTH (ORDER IS IMPORTANT)
app.UseAuthentication();
app.UseAuthorization();

// CONTROLLERS
app.MapControllers();

app.Run();