using System.Text;
using Application.User.Interfaces;
using Application.User.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Misc;
using Misc.Options;
using Presentation.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddTransient<TokenService>();
builder.Services.AddTransient<PasswordService>();

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
    
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi("/api/docs.json");
    app.MapScalarApiReference(options =>
    {
        options.OpenApiRoutePattern = "/api/docs.json";
        options.Title = "Todo app Api Reference";
        options.Theme = ScalarTheme.DeepSpace;
        options.DarkMode = true;
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();