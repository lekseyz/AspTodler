using Persistence;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Extensions;

public static class DbExtensions
{
    public static IServiceCollection AddTestingDb(this IServiceCollection service, IConfiguration configuration)
    {
        return service.AddDbContext<TodlerDbContext>(options =>
        {
            options.UseInMemoryDatabase("TodlerDb");
        });
    }

    public static IServiceCollection AddProdDb(this IServiceCollection service, IConfiguration configuration)
    {
        return service.AddDbContext<TodlerDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("TodlerDbContext"));
        });
    }
}