using Confluent.Kafka;
using DeliveryApp.Core.Ports;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OrderStatusChanged;
using Primitives.Extensions;

namespace DeliveryApp.Infrastructure.Adapters.Kafka.OrderStatusChanged1;

public sealed class MessageBusProducer : IMessageBusProducer
{
    private readonly ProducerConfig _config;
    private readonly string _topicName;

    public MessageBusProducer(string kafkaHost, string orderStatusChangedTopic)
    {
        _config = new ProducerConfig
        {
            BootstrapServers = kafkaHost
        };
        _topicName = orderStatusChangedTopic;
    }

    public async Task Publish(OrderStatusChangedDomainEvent notification, CancellationToken cancellationToken)
    {
        // Перекладываем данные из Domain Event в Integration Event
        var orderStatusChangedIntegrationEvent = new OrderStatusChangedIntegrationEvent
        {
            OrderId = notification.OrderId.ToString(),
            OrderStatus = (OrderStatus)notification.Status.Id
        };

        // Создаем сообщение для Kafka
        var message = new Message<string, string>
        {
            Key = notification.EventId.ToString(),
            Value = JsonConvert.SerializeObject(orderStatusChangedIntegrationEvent)
        };

        // Отправляем сообщение в Kafka
        using var producer = new ProducerBuilder<string, string>(_config).Build();
        await producer.ProduceAsync(_topicName, message);
    }
}