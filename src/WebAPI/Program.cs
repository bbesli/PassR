using System.Reflection;
using PassR.Mediator;
using PassR.Utilities.Endpoints;
using PassR.Utilities.Extensions;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Logging.AddConsole();

        builder.Services.AddPresentation();
        builder.Services.AddPassR();
        builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());


        var app = builder.Build();

        app.UsePassRPresentation(version: 1, endpointAssembly: Assembly.GetExecutingAssembly());

        app.UseHttpsRedirection();

        app.Run();
    }
}

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
