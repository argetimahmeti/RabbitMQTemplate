using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Lisening the Queue...");

        ConnectionFactory factory = new ConnectionFactory() { HostName = "rabbitmq", Port = 5672 };
        factory.UserName = "guest";
        factory.Password = "guest";
        IConnection conn = factory.CreateConnection();
        IModel channel = conn.CreateModel();
        channel.QueueDeclare(queue: "hello",
                                durable: false,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            Console.WriteLine("Sss");
            var body = ea.Body;
            var message = Encoding.UTF8.GetString(body.ToArray());
            Console.WriteLine(" [x] Received from Rabbit: {0}", message);
        };
        channel.BasicConsume(queue: "hello",
                                autoAck: true,
                                consumer: consumer);
    }
}