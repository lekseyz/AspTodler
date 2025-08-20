using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
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

app.MapControllers();

app.Run();
