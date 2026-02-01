using Application.Extensions;
using Infrastructure.Extensions;
using FluentValidation.AspNetCore;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

namespace API;

public class Program
{
    public static void Main(string[] args)
    {
        Env.Load(Path.Combine(Directory.GetCurrentDirectory(), "..", ".env"));
        var builder = WebApplication.CreateBuilder(args);

        var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")
            ?? throw new InvalidOperationException("CONNECTION_STRING environment variable is not set.");
        builder.Services.AddInfrastructure(connectionString);
        builder.Services.AddApplication();
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Device API",
                Version = "v1",
                Description = "API for managing devices"
            });
            c.UseInlineDefinitionsForEnums();
        });

        var app = builder.Build();

        // Apply migrations automatically on startup
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<Infrastructure.Database.DeviceDBContext>();
            context.Database.Migrate();
        }

        // if (app.Environment.IsDevelopment())
        // {
            app.UseSwagger();
            app.UseSwaggerUI();
        // }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
