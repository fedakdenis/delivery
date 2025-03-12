
public interface IMessageBusProducer
{
    Task Publish(OrderStatusChangedDomainEvent notification, CancellationToken cancellationToken);
}