using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using failure_api.Data;
using failure_api.Models;
using failure_api.Services;
using failure_api.Filters;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.LoginPath = "/api/auth/login"; // Change as needed
    options.AccessDeniedPath = "/api/auth/access-denied"; // Change as needed
    options.SlidingExpiration = true;
});

Env.Load();

var originUrl = Environment.GetEnvironmentVariable("ORIGIN_URL") ?? "http://localhost:3000";

builder.Services.AddCors(options => {
    options.AddPolicy("AllowSpecificOrigins",
        builder => builder
            .WithOrigins(originUrl)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

// Add application services.
builder.Services.AddScoped<IBadgeService, BadgeService>();
builder.Services.AddScoped<PrivateProfileFilter>();

var app = builder.Build();

app.UseCors("AllowSpecificOrigins");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();