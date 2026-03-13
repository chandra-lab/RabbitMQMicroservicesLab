using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("PaymentService starting...");

var factory = new ConnectionFactory()
{
    HostName = "localhost",
    UserName = "guest",
    Password = "guest"
};

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

// Declare queue (must match producer queue)
channel.QueueDeclare(
    queue: "orders-queue",
    durable: true,
    exclusive: false,
    autoDelete: false,
    arguments: null
);

Console.WriteLine("Waiting for orders...");

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);

    Console.WriteLine($"Order received: {message}");

    // acknowledge message
    channel.BasicAck(ea.DeliveryTag, false);
};

channel.BasicConsume(
    queue: "orders-queue",
    autoAck: false,
    consumer: consumer
);

Console.ReadLine();
