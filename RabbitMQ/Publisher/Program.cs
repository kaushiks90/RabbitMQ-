using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace Publisher
{
    public class Message
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
    }

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
                    Uri = new Uri("amqp://azgrijmc:yIrf1Qb_q26isYJCMAvyVQgbDCouKped@spider.rmq.cloudamqp.com/azgrijmc")
                };
                IConnection conn = factory.CreateConnection();
                IModel channel = conn.CreateModel();
                //Delivery Mode 2 indicates persistent and 1 indicates non persistent default is 1
                IBasicProperties properties = channel.CreateBasicProperties();
                properties.DeliveryMode = 2;

                channel.ExchangeDeclare("Exchange2", ExchangeType.Direct);
                channel.QueueDeclare("Queue2", true, false, false, null);
                channel.QueueBind("Queue2", "Exchange2", "routing key2");

                for (int i = 0; i < 10; i++)
                {
                    //System.Threading.Thread.Sleep(5000);
                    Message message = new Message() { Name = "Name" + i.ToString(), Age = i, Gender = "Male" };
                    string messageString = JsonConvert.SerializeObject(message);

                    byte[] messagebodytobytes = Encoding.UTF8.GetBytes(messageString);
                    // properties can be passed in place of null
                    channel.BasicPublish("Exchange2", "routing key2", properties, messagebodytobytes);
                    Console.WriteLine(messageString);
                }

                channel.Dispose();
                conn.Dispose();
                Console.ReadLine();
            }
            catch (Exception)
            {
            }
        }
    }
}