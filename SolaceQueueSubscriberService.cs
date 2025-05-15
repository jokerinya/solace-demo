using System.Text;
using SolaceSystems.Solclient.Messaging;

namespace SolaceDemo;

public class SolaceQueueSubscriberService : BackgroundService
{
    IFlow? flow = null;
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        ContextFactory.Instance.Init(new ContextFactoryProperties());
        var context = ContextFactory.Instance.CreateContext(new ContextProperties(), null);

        var sessionProps = new SessionProperties
        {
            Host = "tcp://localhost:55554",
            VPNName = "default",
            UserName = "default",
            Password = "default",
        };

        var session = context.CreateSession(sessionProps, null, SessionEventHandler);
        if (session.Connect() != ReturnCode.SOLCLIENT_OK)
        {
            Console.WriteLine("Queue session connect failed.");
            return Task.CompletedTask;
        }

        var queue = ContextFactory.Instance.CreateQueue("demo.queue");
        var flowProps = new FlowProperties
        {
            AckMode = MessageAckMode.ClientAck,
        };

        flow = session.CreateFlow(flowProps, queue, null, QueueMessageHandler, FlowEventHandler);
        flow.Start();

        Console.WriteLine("Bound to queue and waiting for messages.");
        return Task.CompletedTask;


    }
    
    private void QueueMessageHandler(object? _, MessageEventArgs args)
    {
        var message = args.Message;
        var text = Encoding.UTF8.GetString(message.BinaryAttachment);
        Console.WriteLine($"[QUEUE] Received: {text}");

        flow?.Ack(message.ADMessageId);
        Console.WriteLine("[QUEUE] Message acknowledged.");
    }
    
    private static void FlowEventHandler(object? sender, FlowEventArgs args)
    {
        Console.WriteLine($"[QUEUE] Flow event: {args.Info}");
    }

    private static void SessionEventHandler(object? sender, SessionEventArgs args)
    {
        Console.WriteLine($"[QUEUE] Session event: {args.Event}");
    }
}
