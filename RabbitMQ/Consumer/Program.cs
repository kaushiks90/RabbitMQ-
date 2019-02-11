using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Consumer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                ConnectionFactory factory = new ConnectionFactory
                {
                    //factory.UserName = "sukant";
                    //factory.Password = "sukant";
                    //factory.VirtualHost = "vhost";
                    //factory.HostName = "13.234.17.231";
                    //factory.Port = 15672;
                    Uri =
                    new Uri("amqp://azgrijmc:yIrf1Qb_q26isYJCMAvyVQgbDCouKped@spider.rmq.cloudamqp.com/azgrijmc")
                };
                IConnection conn = factory.CreateConnection();
                IModel channel = conn.CreateModel();
                channel.QueueDeclare(queue: "Queue2", durable: false, autoDelete: false, exclusive: false, arguments: null);
                EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    byte[] body = ea.Body;
                    Console.WriteLine(Encoding.UTF8.GetString(body));
                    System.Threading.Thread.Sleep(3000);
                    channel.BasicAck(ea.DeliveryTag, false);
                };
                channel.BasicConsume(queue: "Queue2", autoAck: false, consumer: consumer);
                Console.WriteLine("Waiting for messages");
                Console.ReadLine();
            }
            catch (Exception)
            {
            }
        }
    }
}