using RabbitMQ.Client;

namespace RabbitmqWeb.ExcelCreate.Services
{
    public class RabbitMqClientService
    {
        private readonly IConnectionFactory _connectionFactory;
        private  IConnection _connection;
        private IModel _channel;

        public static string ExchangeName = "ExcelDirect";
        public static string RouteName = "excel-route";
        public static string QuequeName = "queue-excel";

        public RabbitMqClientService(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public IModel Connect()
        {
            _connection = _connectionFactory.CreateConnection();    
            if (_channel is { IsOpen:true})
            {
                return _channel;
            }
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: ExchangeName, type: "direct", true, false);
            _channel.QueueDeclare(queue: QuequeName, durable: true, false, false,null);

            _channel.QueueBind(queue: QuequeName, exchange: ExchangeName, routingKey: RouteName);

            return _channel;
        }
    }
}
