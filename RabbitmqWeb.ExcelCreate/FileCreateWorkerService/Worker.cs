using ClosedXML.Excel;
using FileCreateWorkerService.Models;
using FileCreateWorkerService.Services;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace FileCreateWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly RabbitMqClientService _rabbitMqClientService;
        private RabbitMQ.Client.IModel _channel;
        private readonly IServiceProvider _serviceProvider;
        public Worker(ILogger<Worker> logger, RabbitMqClientService RabbitMqClientService, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _rabbitMqClientService = RabbitMqClientService;
            _serviceProvider = serviceProvider;
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
           _channel= _rabbitMqClientService.Connect();
            _channel.BasicQos(0, 1, false);



            return base.StartAsync(cancellationToken);
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            _channel.BasicConsume(RabbitMqClientService.QueueName, false, consumer);
            consumer.Received += Consumer_Received;

            return Task.CompletedTask;
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
        {
            await Task.Delay(5000);

            var createexcel = JsonSerializer.Deserialize<CreateExcelMessage>(Encoding.UTF8.GetString(@event.Body.ToArray()));

            using var ms = new MemoryStream();

            var wb = new XLWorkbook();
            var ds = new DataSet();
            ds.Tables.Add(GetTable("products"));

            wb.Worksheets.Add(ds);
            wb.SaveAs(ms);

            MultipartFormDataContent multipartFormDataContent = new();
            multipartFormDataContent.Add(new ByteArrayContent(ms.ToArray()), "file", Guid.NewGuid().ToString() + ".xlsx");

            var baseurl = "https://localhost:44327/api/files";
            using (var httpclient=new HttpClient())
            {
                var response=await httpclient.PostAsync($"{baseurl}?fileid={createexcel.FileId}", multipartFormDataContent);
                if (response.IsSuccessStatusCode)
                {
                    _channel.BasicAck(@event.DeliveryTag, false);
                }
            }

        }

        private DataTable GetTable(string tablename)
        {
            List<FileCreateWorkerService.Models.Product> products;

            using (var scope=_serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AdventureWorks2017Context>();

                products = context.Products.ToList();
            }

            DataTable table= new DataTable() { TableName=tablename};
            table.Columns.Add("ProductId", typeof(int));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("ProductNumber", typeof(string));
            table.Columns.Add("Color", typeof(string));

            products.ForEach(p =>
            {
                table.Rows.Add(p.ProductId, p.Name, p.ProductNumber, p.Color);
            });
            return table;
        }
    }
}
