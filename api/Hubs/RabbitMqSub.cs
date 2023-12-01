using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
namespace clinic_reservation.Hubs;

public class RabbitMqSub
{
    public string? _message = "";
    public RabbitMqSub(String destination)
    {

        ConnectionFactory factory = new();
        factory.Uri = new Uri("amqp://guest:guest@rabbit:5672");
        factory.ClientProvidedName = "RabbitMq";
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        var body = null as byte[];
        var messages = "";
        channel.BasicQos(prefetchSize: 0, prefetchCount: 10, global: false);

        var routingKey = destination.ToString();
        var exchangeName = "doctor-notification";

        channel.ExchangeDeclare(exchange: exchangeName,
                        type: ExchangeType.Direct);
        channel.QueueDeclare(queue: routingKey,
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);
        channel.QueueBind(queue: routingKey, exchangeName, routingKey);

        var messageCount = channel.MessageCount(destination);
        BasicGetResult? result = null;
        if (messageCount == 0)
        {
            _message = "You have no notifications.";
        }
        else
        {
            while (messageCount > 0)
            {
                result = channel.BasicGet(destination, true);
                var message = Encoding.UTF8.GetString(result.Body.ToArray());
                messages += message + ":";
                messageCount = channel.MessageCount(destination);
            }
        }

        _message = messages;
        channel.Close();
        connection.Close();
    }
}