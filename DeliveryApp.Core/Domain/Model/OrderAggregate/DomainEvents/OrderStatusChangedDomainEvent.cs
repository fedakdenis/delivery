using DeliveryApp.Core.Domain.Model.OrderAggregate;
using Primitives;

public record OrderStatusChangedDomainEvent(Guid OrderId, OrderStatus Status) : DomainEvent; 