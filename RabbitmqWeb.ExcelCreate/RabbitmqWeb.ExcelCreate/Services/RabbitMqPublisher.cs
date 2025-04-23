using RabbitMQ.Client;
using Shared;
using System.Text;
using System.Text.Json;
using System.Text.Unicode;

namespace RabbitmqWeb.ExcelCreate.Services
{
    public class RabbitMqPublisher
    {
        private readonly RabbitMqClientService _rabbitMqClientService;

        public RabbitMqPublisher(RabbitMqClientService rabbitMqClientService)
        {
            _rabbitMqClientService = rabbitMqClientService;
        }

        public void Publish(CreateExcelMessage createExcelMessage)
        {
            var channel=_rabbitMqClientService.Connect();

            var bodystring=JsonSerializer.Serialize(createExcelMessage);

            var getbyte = Encoding.UTF8.GetBytes(bodystring);

            var properties=channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange: RabbitMqClientService.ExchangeName, routingKey: RabbitMqClientService.RouteName, basicProperties: properties, body: getbyte);
        }
    }
}
