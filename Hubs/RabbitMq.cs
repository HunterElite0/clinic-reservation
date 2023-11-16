﻿using System.Text;
using RabbitMQ.Client;
namespace clinic_reservation.Hubs;

public class RabbitMq{
    public RabbitMq(String message){

        ConnectionFactory factory = new();
        factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
        factory.ClientProvidedName = "RabbitMq";
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        var routingKey = "doctor.info";
        var exchangeName = "topic-doctor";
        var queueName = "doctor-messages";



        channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Topic);
        channel.QueueDeclare(queue: "doctor-messages",
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);                     
        channel.QueueBind(queueName, exchangeName, routingKey, null);

     

        byte[] bytes = Encoding.UTF8.GetBytes("Hello World");
        channel.BasicPublish(exchangeName, routingKey, null, bytes); 

        // channel.Close();
        // connection.Close();
    }

}