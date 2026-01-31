using Application.CQRS;
using FluentValidation;
using Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class ServiceCollectionExtensions
{
    private const string ConnectionStringEnvVar = "CONNECTION_STRING";

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var connectionString = Environment.GetEnvironmentVariable(ConnectionStringEnvVar)
            ?? throw new InvalidOperationException($"Environment variable '{ConnectionStringEnvVar}' is not set.");

        services.AddInfrastructure(connectionString);

        var assembly = typeof(ServiceCollectionExtensions).Assembly;

        services.AddValidatorsFromAssembly(assembly);

        services.Scan(scan => scan
            .FromAssemblies(assembly)
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.Scan(scan => scan
            .FromAssemblies(assembly)
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.Scan(scan => scan
            .FromAssemblies(assembly)
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.AddScoped<IDispatcher, Dispatcher>();

        return services;
    }
}
