using BasketConfirmed;
using Confluent.Kafka;
using DeliveryApp.Core.Application.UseCases.Commands.CreateOrder;
using DeliveryApp.Infrastructure;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace DeliveryApp.Api.Adapters.Kafka.BasketConfirmed;

public class ConsumerService : BackgroundService
{
    private readonly IMediator _mediator;
    private readonly IServiceScope _scope;
    private readonly IConsumer<Ignore, string> _consumer;
    private readonly string _topic;

    public ConsumerService(IServiceScopeFactory serviceScopeFactory, IOptions<Settings> settings)
    {
        _scope = serviceScopeFactory.CreateScope();
        _mediator = _scope.ServiceProvider.GetRequiredService<IMediator>();
        if (string.IsNullOrWhiteSpace(settings.Value.MessageBrokerHost)) throw new ArgumentException(nameof(settings.Value.MessageBrokerHost));
        if (string.IsNullOrWhiteSpace(settings.Value.BasketConfirmedTopic)) throw new ArgumentException(nameof(settings.Value.BasketConfirmedTopic));

        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = settings.Value.MessageBrokerHost,
            GroupId = "DeliveryConsumerGroup",
            EnableAutoOffsetStore = false,
            EnableAutoCommit = true,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnablePartitionEof = true
        };
        _consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
        _topic = settings.Value.BasketConfirmedTopic;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _consumer.Subscribe(_topic);
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
                var consumeResult = _consumer.Consume(cancellationToken);

                if (consumeResult.IsPartitionEOF) continue;

                Console.WriteLine(
                    $"Received message at {consumeResult.TopicPartitionOffset}\n Key:{consumeResult.Message.Key}\n Value:{consumeResult.Message.Value}");
                var basketConfirmedIntegrationEvent =
                    JsonConvert.DeserializeObject<BasketConfirmedIntegrationEvent>(consumeResult.Message.Value);

                var createOrderCommand = new CreateOrderCommand(
                    Guid.Parse(basketConfirmedIntegrationEvent.BasketId),
                    basketConfirmedIntegrationEvent.Address.Street);

                var response = await _mediator.Send(createOrderCommand, cancellationToken);
                if (!response) Console.WriteLine("Error");

                try
                {
                    _consumer.StoreOffset(consumeResult);
                }
                catch (KafkaException e)
                {
                    Console.WriteLine($"Store Offset error: {e.Error.Reason}");
                }
            }
        }
        catch (OperationCanceledException)
        {
            _consumer.Close();
        }
    }

    public override void Dispose()
    {
        base.Dispose();
        _scope.Dispose();
    }
}