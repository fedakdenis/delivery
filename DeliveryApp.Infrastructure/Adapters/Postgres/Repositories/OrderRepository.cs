using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using DeliveryApp.Core.Ports;
using Microsoft.EntityFrameworkCore;
using Primitives;

namespace DeliveryApp.Infrastructure.Adapters.Postgres.Repositories;

public class OrderRepository : IOrderRepository
{
    private ApplicationDbContext _applicationDbContext;

    public OrderRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<UnitResult<Error>> AddAsync(Order order)
    {
        Attach(order);
        
        await _applicationDbContext.Orders.AddAsync(order);

        return UnitResult.Success<Error>();
    }

    public Result<IReadOnlyCollection<Order>, Error> GetAssigned()
    {
        var result = GetByStatus(OrderStatus.Assigned);
        return result;
    }

    public async Task<Result<Order, Error>> GetByIdAsync(Guid orderId)
    {
        if (orderId == Guid.Empty)
        {
            return GeneralErrors.ValueIsInvalid(nameof(orderId));
        }

        var result = await GetQuery()
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (result == null)
        {
            return GeneralErrors.NotFound();
        }

        return result;
    }

    public Result<IReadOnlyCollection<Order>, Error> GetCreated()
    {
        var result = GetByStatus(OrderStatus.Created);
        return result;
    }

    public UnitResult<Error> Update(Order order)
    {
        Attach(order);

        _applicationDbContext.Orders.Update(order);

        return UnitResult.Success<Error>();
    }

    private Order[] GetByStatus(OrderStatus status)
    {
        var result = GetQuery()
            .Where(o => o.Status.Id == status.Id)
            .ToArray();

        return result;
    }

    private IQueryable<Order> GetQuery()
    {
        return _applicationDbContext.Orders
            .Include(o => o.Status);
    }

    private void Attach(Order order)
    {
        if (order.Status != null)
        {
            _applicationDbContext.Attach(order.Status);
        }
    }
}