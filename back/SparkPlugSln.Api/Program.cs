using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SparkPlugSln.Application.Services.Implementations;
using SparkPlugSln.Application.Services.Interfaces;
using SparkPlugSln.Domain.Enums;
using SparkPlugSln.Domain.IRepositories;
using SparkPlugSln.Domain.Models.kavenegar;
using SparkPlugSln.Persistence;
using SparkPlugSln.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    var xmlFile = "SparkPlugDocumentation.xml";
    var xmlPath = Path.Combine(Directory.GetCurrentDirectory(), xmlFile);
    config.IncludeXmlComments(xmlPath);
});

// Database Config
builder.Services.AddDbContext<PersistenceDbContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration.GetConnectionString("PersistenceDb"));
});

#region JWT Authentication

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];
var issuer = jwtSettings["Issuer"];
var audience = jwtSettings["Audience"];

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
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
    {
        policy.RequireAuthenticatedUser(); // Ensure the user is authenticated
        policy.RequireClaim(ClaimTypes.Role, ((int)UserRoles.Admin).ToString()); // Require the role claim with value "0" (Admin)
    });
});

#endregion

#region Kavenegar

builder.Services.Configure<KavenegarInfoVm>(builder.Configuration.GetSection("KavenegarInfo"));

#endregion

#region DI

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISmsService, SmsService>();

#endregion

builder.Services.AddCors(opts =>
{
    opts.AddPolicy(name: "AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();