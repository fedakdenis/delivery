using DeliveryApp.Core.Domain.Model.OrderAggregate;
using DeliveryApp.Core.Domain.SharedKernel;
using DeliveryApp.Infrastructure.Adapters.Postgres.Repositories;
using FluentAssertions;
using Xunit;

namespace DeliveryApp.IntegrationTests.Repositories;

public class OrderRepositoryShould : RepositoryShouldBase
{
    [Fact]
    public async Task CanAddOrder()
    {
        var orderId = Guid.NewGuid();
        var location = Location.CreateRandom();
        var newOrder = Order.Create(orderId, location);
        
        var orderRepository = new OrderRepository(_applicationDbContext);
        await orderRepository.AddAsync(newOrder.Value);
        await _applicationDbContext.SaveChangesAsync();

        var order = await orderRepository.GetByIdAsync(orderId);
        newOrder.Value.Should().BeEquivalentTo(order.Value);
    }

    [Fact]
    public async Task CanUpdateOrder()
    {
        var orderId = Guid.NewGuid();
        var location = Location.CreateRandom();
        var newOrder = Order.Create(orderId, location);
        
        var orderRepository = new OrderRepository(_applicationDbContext);
        await orderRepository.AddAsync(newOrder.Value);
        await _applicationDbContext.SaveChangesAsync();

        var order = await orderRepository.GetByIdAsync(orderId);
        order.Value.Id.Should().Be(orderId);

        order.Value.Complete();
        orderRepository.Update(order.Value);
        await _applicationDbContext.SaveChangesAsync();

        var updatedOrder = await orderRepository.GetByIdAsync(orderId);
        updatedOrder.Value.Should().BeEquivalentTo(order.Value);
    }
}