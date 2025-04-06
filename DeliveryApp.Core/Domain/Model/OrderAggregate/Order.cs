using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.SharedKernel;
using Primitives;

namespace DeliveryApp.Core.Domain.Model.OrderAggregate;

public class Order : Aggregate
{
    private Order() { }

    private Order(Guid orderId, Location location)
    {
        Id = orderId;
        Location = location;
        Status = OrderStatus.Created;
    }

    public static Result<Order, Error> Create(Guid orderId, Location location)
    {
        if (orderId == Guid.Empty)
        {
            return GeneralErrors.ValueIsInvalid(nameof(orderId));
        }

        if (location == null)
        {
            return GeneralErrors.ValueIsInvalid(nameof(location));
        }
        
        return new Order(orderId, location);
    }

    public Location Location{ get; private set; }

    public OrderStatus Status { get; private set; }

    public Guid? CourierId { get; private set; }

    public Result<object, Error> Assign(Guid courierId)
    {
        if (courierId == Guid.Empty)
        {
            return GeneralErrors.ValueIsInvalid(nameof(courierId));
        }

        CourierId = courierId;
        Status = OrderStatus.Assigned;
        RaiseDomainEvent(new OrderStatusChangedDomainEvent(Id, Status));
        return new object();
    }

    public Result<object, Error> Complete()
    {
        if (Status != OrderStatus.Assigned)
        {
            return Errors.OrderIsNotAssigned();
        }

        Status = OrderStatus.Completed;
        RaiseDomainEvent(new OrderStatusChangedDomainEvent(Id, Status));
        return new object();
    }

    static class Errors
    {
        public static Error OrderIsNotAssigned()
            => new Error("order.is-not-assigned", "Can not complete not assigned order"); 
    }
}