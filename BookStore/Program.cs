using BookStore.Controllers;
using BookStore.Data;
using BookStore.Helpers;
using BookStore.Models;
using BookStore.Repository;
using BookStore.Seeds;
using BookStore.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// ------------------------- Scopes Conf ----------------------//
builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("JWT"));
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

// ------------------------------------------------------- //




// ------------------------- Sql Conf ----------------------//

builder.Services.AddDbContext<ApplicationDbContext>(
    options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
        ));

// ------------------------------------------------------- //


// ------------------------- Authentication Conf ----------------------//
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
    options =>
    {
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireDigit = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 8;
    }
    ).AddEntityFrameworkStores<ApplicationDbContext>();

// ------------------------------------------------------- //

// ------------------------- JwtBearer Conf ----------------------//

builder.Services.AddAuthentication(
    options=>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(op =>
    {
        op.RequireHttpsMetadata = true;
        op.SaveToken = false;
        op.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,

            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SECRETKEY"]))
        };
    });

// ------------------------------------------------------- //

// ------------------------- logger Conf ----------------------//

var logger =
    new LoggerConfiguration()
    .ReadFrom
    .Configuration(builder.Configuration)
    .CreateLogger();

builder.Logging.AddSerilog(logger);





var app = builder.Build();

// ---------------------------  Data Seeding    ---------------------------- //
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var authService = services.GetRequiredService<IAuthService>();
var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
var loggerAccounts = services.GetRequiredService<ILogger<AccountsController>>();


try
{
    if (roleManager.Roles.Any())
    {
        await DefaultRoles.SeedAsync(authService);
        await DefaultUsers.SeedAdminAsync(authService, roleManager);
        await DefaultUsers.SeedBasicAsync(authService, roleManager);
        await DefaultUsers.SeedSuperAdminAsync(authService, roleManager);
    }
}
catch(Exception ex)
{
    // todo: add to logger
    loggerAccounts.LogError(ex, "an Error ocurred while seeding initial data");
}

// ------------------------------------------------------- //


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();







app.Run();
