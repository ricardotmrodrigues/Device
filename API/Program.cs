using Application.Extensions;
using Infrastructure.Extensions;
using FluentValidation.AspNetCore;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

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
        builder.Services.AddControllers();
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Apply migrations automatically on startup
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<Infrastructure.Database.DeviceDBContext>();
            context.Database.Migrate();
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
