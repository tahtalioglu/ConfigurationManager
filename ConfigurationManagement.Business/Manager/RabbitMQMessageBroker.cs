using ConfigurationManagement.Business.Contract;
using ConfigurationManagement.Data;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ConfigurationManagement.Business.Manager
{
    public class RabbitMQMessageBroker : IMessageBroker
    {
        readonly string hostName = "localhost";
        readonly string queueName = "ConfigRecord";
        public void AddMessage(Record record)
        {
            var factory = new ConnectionFactory() { HostName = hostName };
            using (IConnection connection = factory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string message = JsonConvert.SerializeObject(record);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: queueName,
                                     basicProperties: null,
                                     body: body);
            }
        }
        public void ConsumeMessage()
        {
            var factory = new ConnectionFactory() { HostName = hostName };

            using (IConnection connection = factory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Record record = JsonConvert.DeserializeObject<Record>(message);

                };
                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);
            }
        }

    }
}
