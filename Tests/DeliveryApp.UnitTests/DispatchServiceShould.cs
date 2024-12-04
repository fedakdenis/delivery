using System;
using System.Collections;
using System.Collections.Generic;
using DeliveryApp.Core.Domain.Model.CourierAggregate;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using DeliveryApp.Core.Domain.Services;
using DeliveryApp.Core.Domain.SharedKernel;
using FluentAssertions;
using Xunit;

namespace DeliveryApp.UnitTests;

public class DispatchServiceShould
{
    static Courier Ivan = Courier.Create("Иван", Transport.Pedestrian, new Location(1, 1)).Value;
    static Courier Pyotr = Courier.Create("Пётр", Transport.Bicycle, new Location(1, 1)).Value;
    static Courier Fyodor = Courier.Create("Фёдор", Transport.Car, new Location(1, 1)).Value;

    [Theory]
    [ClassData(typeof(DispatchData))]
    public void DispatchBySpeed(List<Courier> couriers, Courier expectedFastestCourier)
    {
        var order = Order.Create(Guid.NewGuid(), new Location(10, 10));
        var dispatchService = new DispatchService();
        var fastestCourier =  dispatchService.Dispatch(order.Value, couriers);
        fastestCourier.Value.Should().Be(expectedFastestCourier);
    }

    [Fact]
    public void FailOnEmptyCourierList()
    {
        var order = Order.Create(Guid.NewGuid(), Location.CreateRandom());
        var dispatchService = new DispatchService();
        var fastestCourier =  dispatchService.Dispatch(order.Value, new List<Courier>());
        fastestCourier.IsFailure.Should().BeTrue();
    }


    class DispatchData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new List<Courier> { Ivan, Pyotr, Fyodor }, Fyodor };
            yield return new object[] { new List<Courier> { Ivan, Pyotr }, Pyotr };
            yield return new object[] { new List<Courier> { Ivan }, Ivan };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

}