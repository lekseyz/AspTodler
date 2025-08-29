using Application.NoteLogic.Interfaces;
using Application.NoteLogic.Services;
using Application.UserLogic.Interfaces;
using Application.UserLogic.Services;
using Infrastructure;
using Persistence.Repositories;
using Presentation.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTestingDb(builder.Configuration);
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<NoteRepository>();
builder.Services.AddScoped<RefreshTokenRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<INoteService, NoteService>();
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