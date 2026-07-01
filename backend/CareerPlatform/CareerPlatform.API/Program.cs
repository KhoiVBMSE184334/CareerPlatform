using System.Text;
using CareerPlatform.Application.DTOs.Auth;
using CareerPlatform.Application.DTOs.CareerPaths;
using CareerPlatform.Application.DTOs.Chats;
using CareerPlatform.Application.DTOs.Dashboards;
using CareerPlatform.Application.DTOs.Portfolios;
using CareerPlatform.Application.DTOs.Roadmaps;
using CareerPlatform.Application.DTOs.SkillGaps;
using CareerPlatform.Application.Interfaces;
using CareerPlatform.Application.Interfaces.Repositories;
using CareerPlatform.Application.Interfaces.Services;
using CareerPlatform.Application.Services;
using CareerPlatform.Application.Validators.Auth;
using CareerPlatform.Application.Validators.CareerPaths;
using CareerPlatform.Application.Validators.Chats;
using CareerPlatform.Application.Validators.Roadmaps;
using CareerPlatform.Application.Validators.SkillGaps;
using CareerPlatform.Domain.Entities;
using CareerPlatform.Infrastructure;
using CareerPlatform.Infrastructure.Repositories;
using CareerPlatform.Infrastructure.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173", "https://careerplatform.netlify.app")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token in this format: Bearer {your token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserSkillRepository, UserSkillRepository>();
builder.Services.AddScoped<ICareerPathRepository, CareerPathRepository>();
builder.Services.AddScoped<ISkillNodeRepository, SkillNodeRepository>();
builder.Services.AddScoped<IUserCareerPathRepository, UserCareerPathRepository>();
builder.Services.AddScoped<IRoadmapProgressRepository, RoadmapProgressRepository>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<IPortfolioRepository, PortfolioRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICareerPathService, CareerPathService>();
builder.Services.AddScoped<IRoadmapService, RoadmapService>();
builder.Services.AddScoped<ISkillGapService, SkillGapService>();
builder.Services.AddScoped<IAIChatService, AIChatService>();
builder.Services.AddScoped<IPortfolioService, PortfolioService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISkillNodeService, SkillNodeService>();
builder.Services.AddHttpClient<IGitHubService, GitHubService>();
builder.Services.AddHttpClient<IGeminiService, GeminiService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IValidator<RegisterRequestDto>, RegisterRequestValidator>();
builder.Services.AddScoped<IValidator<LoginRequestDto>, LoginRequestValidator>();
builder.Services.AddScoped<IValidator<CreateCareerPathDto>, CreateCareerPathValidator>();
builder.Services.AddScoped<IValidator<UpdateCareerPathDto>, UpdateCareerPathValidator>();
builder.Services.AddScoped<IValidator<SelectCareerPathDto>, SelectCareerPathValidator>();
builder.Services.AddScoped<IValidator<ChatRequestDto>, ChatRequestValidator>();
builder.Services.AddScoped<IValidator<ProgressUpdateDto>, ProgressUpdateValidator>();
builder.Services.AddScoped<IValidator<SkillGapRequestDto>, SkillGapRequestValidator>();

var jwtSecret = builder.Configuration["Jwt:Secret"]
    ?? throw new InvalidOperationException("JWT secret is not configured.");

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            ClockSkew = TimeSpan.Zero
        };
    });

var app = builder.Build();
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Urls.Add($"http://0.0.0.0:{port}");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
