using System;
using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using DeliveryApp.Core.Domain.SharedKernel;
using FluentAssertions;
using Primitives;
using Xunit;

namespace DeliveryApp.UnitTests;

public class OrderShould
{
    [Fact]
    public void BeCreated()
    {
        var basketId = Guid.NewGuid();
        var location = Location.CreateRandom();
        var newOrder = Order.Create(basketId, location);
        newOrder.IsSuccess.Should().BeTrue();
        newOrder.Value.Should().NotBeNull();
    }

    [Fact]
    public void NotBeCreatedWithEmptyId()
    {
        var emptyId = Guid.Empty;
        var location = Location.CreateRandom();
        var newOrder = Order.Create(emptyId, location);
        newOrder.IsFailure.Should().BeTrue();
        Assert.Throws<ResultFailureException<Error>>(() => newOrder.Value);
    }

    [Fact]
    public void NotBeCreatedWithoutLocation()
    {
        var basketId = Guid.NewGuid();
        var newOrder = Order.Create(basketId, null);
        newOrder.IsFailure.Should().BeTrue();
        Assert.Throws<ResultFailureException<Error>>(() => newOrder.Value);
    }

    [Fact]
    public void BeAssigned()
    {
        var courierId = Guid.NewGuid();
        var basketId = Guid.NewGuid();
        var location = Location.CreateRandom();
        var newOrder = Order.Create(basketId, location);
        var result = newOrder.Value.Assign(courierId);
        result.IsSuccess.Should().BeTrue();
        newOrder.Value.CourierId.Should().Be(courierId);
        newOrder.Value.Status.Should().Be(OrderStatus.Assigned);
    }

    [Fact]
    public void NotBeAssignedToEmptyCourierId()
    {
        var emptyId = Guid.Empty;
        var basketId = Guid.NewGuid();
        var location = Location.CreateRandom();
        var newOrder = Order.Create(basketId, location);
        var result = newOrder.Value.Assign(emptyId);
        result.IsFailure.Should().BeTrue();
        newOrder.Value.Status.Should().Be(OrderStatus.Created);
    }

    [Fact]
    public void BeCompleted()
    {
        var courierId = Guid.NewGuid();
        var basketId = Guid.NewGuid();
        var location = Location.CreateRandom();
        var newOrder = Order.Create(basketId, location);
        newOrder.Value.Assign(courierId);
        var result = newOrder.Value.Complete();
        result.IsSuccess.Should().BeTrue();
        newOrder.Value.Status.Should().Be(OrderStatus.Completed);
    }

    [Fact]
    public void NotBeCompletedIfIsNotAssigned()
    {
        var basketId = Guid.NewGuid();
        var location = Location.CreateRandom();
        var newOrder = Order.Create(basketId, location);
        var result = newOrder.Value.Complete();
        result.IsFailure.Should().BeTrue();
        newOrder.Value.Status.Should().NotBe(OrderStatus.Completed);
    }

}