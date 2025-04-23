using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCreateWorkerService.Services
{
    public class RabbitMqClientService
    {
        private readonly IConnectionFactory _connectionFactory;
        private IModel _channel;
        private IConnection _connection;

        public static string QueueName = "queue-excel";

        public RabbitMqClientService(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public IModel Connect()
        {
            _connection= _connectionFactory.CreateConnection();
            if (_channel is { IsOpen:true})
            {
                return _channel;
            }
            _channel = _connection.CreateModel();
            return _channel;
        }
    }
}
