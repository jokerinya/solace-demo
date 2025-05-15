using SolaceDemo.Services;

namespace SolaceDemo;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddHostedService<SolaceSubscriberService>();
        builder.Services.AddHostedService<SolaceQueueSubscriberService>();

        var app = builder.Build();

        app.MapGet("/", () => "Hello World!");

        app.Run();
    }
}

