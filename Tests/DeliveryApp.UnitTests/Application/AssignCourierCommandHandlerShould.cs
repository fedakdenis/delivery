using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DeliveryApp.Core.Application.UseCases.Commands.AssignCourier;
using DeliveryApp.Core.Domain.Model.CourierAggregate;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using DeliveryApp.Core.Domain.SharedKernel;
using DeliveryApp.Core.Ports;
using FluentAssertions;
using NSubstitute;
using Primitives;
using Xunit;

namespace DeliveryApp.UnitTests.Application;


public class AssignCourierCommandHandlerShould
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICourierRepository _courierRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AssignCourierCommandHandlerShould()
    {
        _orderRepository = Substitute.For<IOrderRepository>();
        _courierRepository = Substitute.For<ICourierRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
    }

    [Fact]
    public async Task ReturnFalseWhenOrderDoesNotExist()
    {
        _orderRepository.GetCreated()
            .Returns(new List<Order> { });

        var handler = new AssignCourierCommandHandler(_orderRepository, _courierRepository, _unitOfWork);

        var result = await handler.Handle(new AssignCourierCommand(), new CancellationToken());
        result.Should().BeFalse();
    }

    [Fact]
    public async Task ReturnFalseWhenCourierDoesNotExist()
    {
        _orderRepository.GetCreated()
            .Returns(new List<Order> { Order.Create(Guid.NewGuid(), Location.CreateRandom()).Value });
        _courierRepository.GetFree()
            .Returns(new List<Courier> { });

        var handler = new AssignCourierCommandHandler(_orderRepository, _courierRepository, _unitOfWork);

        var result = await handler.Handle(new AssignCourierCommand(), new CancellationToken());
        result.Should().BeFalse();
    }

    [Fact]
    public async Task ReturnTrueWhenCourierIsAssigned()
    {
        var order = Order.Create(Guid.NewGuid(), Location.CreateRandom()).Value;
        _orderRepository.GetCreated()
            .Returns(new List<Order> { order });
        var courier = Courier.Create("Ivan", Transport.Bicycle, Location.CreateRandom()).Value;
        _courierRepository.GetFree()
            .Returns(new List<Courier> { courier });

        var handler = new AssignCourierCommandHandler(_orderRepository, _courierRepository, _unitOfWork);

        order.Status.Should().Be(OrderStatus.Created);
        courier.Status.Should().Be(CourierStatus.Free);
        var result = await handler.Handle(new AssignCourierCommand(), new CancellationToken());
        result.Should().BeTrue();
        order.Status.Should().Be(OrderStatus.Assigned);
        courier.Status.Should().Be(CourierStatus.Busy);
    }
}