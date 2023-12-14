using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
namespace clinic_reservation.Hubs;

public class RabbitMq
{
    public RabbitMq(String message, String destination)
    {

        ConnectionFactory factory = new();
        var connectionString = Environment.GetEnvironmentVariable("RABBITMQ");
        factory.Uri = new Uri(connectionString!);
        factory.ClientProvidedName = "RabbitMq";
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

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

        var json = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(json);

        channel.BasicPublish(exchange: exchangeName,
                             routingKey: routingKey,
                             body: body);

        channel.Close();
        connection.Close();
    }


}