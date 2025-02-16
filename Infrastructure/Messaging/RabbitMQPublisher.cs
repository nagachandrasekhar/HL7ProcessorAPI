using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Infrastructure.Messaging
{
    public class RabbitMQPublisher : IDisposable
    {
        private readonly IConnection _connection; // Now it refers to RabbitMQ.Client.IConnection
        private readonly IModel _channel;

        public RabbitMQPublisher(string hostname)
        {
            var factory = new ConnectionFactory() { HostName = hostname };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Declare the queue with the correct method signature
            _channel.QueueDeclare(
                queue: "hl7_messages",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }

        public void Publish(Guid id, string message)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            try
            {
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    var properties = channel.CreateBasicProperties();
                    properties.Headers = new System.Collections.Generic.Dictionary<string, object>
                {
                    { "MessageId", id.ToString() }
                };

                    byte[] body = Encoding.UTF8.GetBytes(message);

                    // Ensure the queue exists before publishing
                    channel.QueueDeclare(queue: "hl7_queue",
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    // Publish to RabbitMQ
                    channel.BasicPublish(
                        exchange: "",
                        routingKey: "hl7_queue",
                        basicProperties: properties,
                        body: body
                    );
                }
            }
            catch (Exception ex)
            {


            }


        }

        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
        }
    }
}
