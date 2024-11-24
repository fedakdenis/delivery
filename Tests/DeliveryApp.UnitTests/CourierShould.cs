using System;
using System.Collections;
using System.Collections.Generic;
using DeliveryApp.Core.Domain.Model.CourierAggregate;
using DeliveryApp.Core.Domain.SharedKernel;
using FluentAssertions;
using Xunit;

namespace DeliveryApp.UnitTests;

public class CourierShould
{
    [Fact]
    public void BeCreated()
    {
        var location = Location.CreateRandom();
        var newOrder = Courier.Create("Denis", Transport.Bicycle, location);
        newOrder.IsSuccess.Should().BeTrue();
        newOrder.Value.Should().NotBeNull();
        newOrder.Value.Status.Should().Be(CourierStatus.Free);
    }
    
    [Fact]
    public void SetStatus()
    {
        var location = Location.CreateRandom();
        var newOrder = Courier.Create("Denis", Transport.Bicycle, location);
        newOrder.IsSuccess.Should().BeTrue();
        newOrder.Value.Should().NotBeNull();
        newOrder.Value.Status.Should().Be(CourierStatus.Free);
        var busyResult = newOrder.Value.Busy();
        busyResult.IsSuccess.Should().BeTrue();
        newOrder.Value.Status.Should().Be(CourierStatus.Busy);
        var freeResult = newOrder.Value.Free();
        freeResult.IsSuccess.Should().BeTrue();
        newOrder.Value.Status.Should().Be(CourierStatus.Free);
    }
    
    [Fact]
    public void SetStatusFail()
    {
        var location = Location.CreateRandom();
        var newOrder = Courier.Create("Denis", Transport.Bicycle, location);
        newOrder.IsSuccess.Should().BeTrue();
        newOrder.Value.Should().NotBeNull();
        newOrder.Value.Status.Should().Be(CourierStatus.Free);
        var freeResult = newOrder.Value.Free();
        freeResult.IsFailure.Should().BeTrue();
        newOrder.Value.Status.Should().Be(CourierStatus.Free);
        var busyResult = newOrder.Value.Busy();
        busyResult.IsSuccess.Should().BeTrue();
        newOrder.Value.Status.Should().Be(CourierStatus.Busy);
        var busyResultFail = newOrder.Value.Busy();
        busyResultFail.IsFailure.Should().BeTrue();
        newOrder.Value.Status.Should().Be(CourierStatus.Busy);
    }

    [Theory]
    [InlineData(1,1, 1,1, "Car", 0)]
    [InlineData(1,1, 1,2, "Car", 1)]
    [InlineData(1,1, 5,5, "Bicycle", 4)]
    public void ClculateStepCount(int fromX, int fromY, int toX, int toY, string transpotName, int expectedStepCount)
    {
        var from = new Location(fromX, fromY);
        var to = new Location(toX, toY);
        var transport = Transport.FromName(transpotName);
        var newCourier = Courier.Create("Denis", transport.Value, from);
        var stepCount = newCourier.Value.GetStepCount(to);
        stepCount.IsSuccess.Should().BeTrue();
        stepCount.Value.Should().Be(expectedStepCount);
    }

    [Theory]
    [ClassData(typeof(DoStepData))]
    public void DoStep(Location from, Location to, string transpotName, Location[] checkPoints)
    {
        var transport = Transport.FromName(transpotName);
        var newCourier = Courier.Create("Denis", transport.Value, from);
        newCourier.Value.Location.Should().Be(from);
        foreach (var checkPoint in checkPoints)
        {
            newCourier.Value.DoStep(to);
            newCourier.Value.Location.Should().Be(checkPoint);
        }
    }

    class DoStepData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new Location(1,1), new Location(5,5), "Bicycle", new Location[] { new(3, 1), new(5, 1), new(5, 3), new(5, 5)} };
            yield return new object[] { new Location(1,1), new Location(2,2), "Bicycle", new Location[] { new(2, 2)} };
            yield return new object[] { new Location(1,1), new Location(2,2), "Car", new Location[] { new(2, 2)} };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

}