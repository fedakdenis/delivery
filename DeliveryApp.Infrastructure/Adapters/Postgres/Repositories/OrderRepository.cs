using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using DeliveryApp.Core.Ports;
using Primitives;

namespace DeliveryApp.Infrastructure.Adapters.Postgres.Repositories;

public class OrderRepository : IOrderRepository
{
    public UnitResult<Error> Add(Order order)
    {
        throw new NotImplementedException();
    }

    public Result<IReadOnlyList<Order>, Error> GetAssigned(Guid orderId)
    {
        throw new NotImplementedException();
    }

    public Result<Order, Error> GetById(Guid orderId)
    {
        throw new NotImplementedException();
    }

    public Result<IReadOnlyList<Order>, Error> GetCreated(Guid orderId)
    {
        throw new NotImplementedException();
    }

    public UnitResult<Error> Update(Order order)
    {
        throw new NotImplementedException();
    }
}