using BookStore.Controllers;
using BookStore.Data;
using BookStore.Filters;
using BookStore.Helpers;
using BookStore.Models;
using BookStore.Repository;
using BookStore.Seeds;
using BookStore.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(
    options =>
    {
        /*options.Filters.Add<BookPublisherAttribute>();*/ // can add global filter for all endpoints here
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// ------------------------- Scopes Conf ----------------------//
builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("JWT"));
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

// Permissions
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

// ------------------------------------------------------- //




// ------------------------- Sql Conf ----------------------//

builder.Services.AddDbContext<ApplicationDbContext>(
    options =>
        {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
/*        options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
*/        });


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
builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.Zero;
});

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

// ------------------------- Other Confs ----------------------//

// Logger
var logger =new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();

builder.Logging.AddSerilog(logger);

// auto mapper
builder.Services.AddAutoMapper(typeof(Program));

// ------------------------------------------------------- //


var app = builder.Build();



// ---------------------------  Data Seeding    ---------------------------- //
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var loggerAccounts = services.GetRequiredService<ILogger<AccountsController>>();

var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();


try
{
    if (roleManager.Roles.Any())
    {
        var authService = services.GetRequiredService<IAuthService>();
        var roleService = services.GetRequiredService<IRoleService>();


        await DefaultRoles.SeedAsync(roleService);
        await DefaultUsers.SeedAdminAsync(authService, roleService, roleManager);
        await DefaultUsers.SeedBasicAsync(authService, roleService, roleManager);
        await DefaultUsers.SeedSuperAdminAsync(authService, roleService, roleManager);
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
