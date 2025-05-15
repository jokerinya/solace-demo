using SolaceSystems.Solclient.Messaging;

namespace SolaceDemo.Services;

public class SolaceSubscriberService : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Initialize the API
        ContextFactory.Instance.Init(new ContextFactoryProperties());

        // Create the context
        var context = ContextFactory.Instance.CreateContext(new ContextProperties(), null);

        // Create properties for context session
        var properties = new SessionProperties
        {
            Host = "tcp://localhost:55554",
            VPNName = "default",
            UserName = "default",
            Password = "default",
        };

        // Create and connect session, raises event if receives a message
        var session = context.CreateSession(properties, MessageHandler, SessionEventHandler);
        var result = session.Connect();
        if (result != ReturnCode.SOLCLIENT_OK)
        {
            Console.WriteLine("Failed to connect");
            return Task.CompletedTask;
        }

        // Subscribe to topics
        var topic = ContextFactory.Instance.CreateTopic("tryme/topic");
        var topic2 = ContextFactory.Instance.CreateTopic("tryme2/topic");
        session.Subscribe(topic, true);
        session.Subscribe(topic2, true);
        return Task.CompletedTask;
    }
    
    // Message handler delegate
    private static void MessageHandler(object? _, MessageEventArgs args)
    {
        var payload = args.Message.BinaryAttachment;
        var text = System.Text.Encoding.UTF8.GetString(payload);
        var topic = args.Message.Destination?.Name;

        switch (topic)
        {
            case "tryme/topic":
                HandleTryMeTopic(text);
                break;
            case "tryme2/topic":
                HandleTryMe2Topic(text);
                break;
            default:
                Console.WriteLine($"Unhandled topic: {topic}");
                break;
        }
    }
    private static void HandleTryMeTopic(string message)
    {
        Console.WriteLine($"[tryme/topic] Got: {message}");
    }

    private static void HandleTryMe2Topic(string message)
    {
        Console.WriteLine($"[tryme2/topic] Got: {message}");
    } 
    
    // Session event handler (can be null or basic logging)
    private static void SessionEventHandler(object? _, SessionEventArgs args)
    {
        Console.WriteLine($"Session event: {args.Event}");
    }
}