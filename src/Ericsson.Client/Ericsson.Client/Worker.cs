using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ericsson.Client.Dtos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Ericsson.Client
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IOptions<RabbitMqOptions> _rabbitMqOptions;
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;

        public Worker(ILogger<Worker> logger, IOptions<RabbitMqOptions> rabbitMqOptions)
        {
            _logger = logger;
            _rabbitMqOptions = rabbitMqOptions;
            _connectionFactory = new ConnectionFactory()
            {
                Endpoint = new AmqpTcpEndpoint(rabbitMqOptions.Value.Host),
                Port = rabbitMqOptions.Value.Port,
                UserName = rabbitMqOptions.Value.Username,
                Password = rabbitMqOptions.Value.Password
            };
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Throw Exception if cancel request happened
            stoppingToken.ThrowIfCancellationRequested();

            // Create Connection
            _connection = _connectionFactory.CreateConnection();

            // Create Model
            _channel = _connection.CreateModel();
            
            // Create Exchange Declare If Not Exist
            _channel.ExchangeDeclare(exchange: _rabbitMqOptions.Value.Exchange, type: ExchangeType.Fanout);

            // Create Queue With Random Name
            var queueName = _channel.QueueDeclare().QueueName;
            
            // Bind Queue to Exchange
            _channel.QueueBind(queue: queueName,
                exchange: _rabbitMqOptions.Value.Exchange,
                routingKey: "");

            // Create Basic Event Consumer
            var consumer = new EventingBasicConsumer(_channel);
            
            consumer.Received += (_, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] {0}", message);
                _logger.LogInformation($" [x] {message}");
            };
            
            // Consume
            _channel.BasicConsume(
                queue: queueName,
                autoAck: true,
                consumer: consumer);
            
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}