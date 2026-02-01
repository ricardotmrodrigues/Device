using Domain.Contracts;
using Infrastructure.Database;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        //add dbcontext
        services.AddDbContext<DeviceDBContext>(options =>
            options.UseNpgsql(connectionString));

        //add repositoris
        services.AddScoped<IDeviceEntityRepository, DeviceEntityRepository>();

        return services;
    }
}
