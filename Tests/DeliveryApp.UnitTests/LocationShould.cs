using System;
using DeliveryApp.Core.Domain.SharedKernel;
using FluentAssertions;
using Xunit;

namespace DeliveryApp.UnitTests;

public class LocationShould
{
    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, 8)]
    [InlineData(8, 0)]
    [InlineData(11, 11)]
    [InlineData(11, 3)]
    [InlineData(5, 11)]
    [InlineData(-1, -1)]
    [InlineData(-1, 8)]
    [InlineData(8, -1)]
    public void ThrowsArgumentOutOfRangeException(int x, int y)
    {    
        Assert.Throws<ArgumentOutOfRangeException>(() => new Location(x, y));
    }

    [Theory]
    [InlineData(2, 6, 4, 9, 5)]
    [InlineData(1, 1, 1, 1, 0)]
    [InlineData(1, 1, 10, 10, 18)]
    [InlineData(10, 1, 1, 10, 18)]
    [InlineData(1, 1, 1, 10, 9)]
    [InlineData(1, 1, 10, 1, 9)]
    [InlineData(5, 5, 5, 5, 0)]
    [InlineData(5, 5, 6, 6, 2)]
    [InlineData(5, 5, 5, 6, 1)]
    [InlineData(10, 10, 10, 10, 0)]
    public void CalculateDistance(int x1, int y1, int x2, int y2, int d)
    {
        var l1 = new Location(x1, y1);
        var l2 = new Location(x2, y2);
        var l1l2Distance = l1.DistanceTo(l2);
        var l2l1Distance = l2.DistanceTo(l1);
        l1l2Distance.Should().Be(d);
        l2l1Distance.Should().Be(d);
    }

    [Theory]
    [InlineData(1, 1, 1, 1)]
    [InlineData(10, 10, 10, 10)]
    [InlineData(1, 10, 1, 10)]
    [InlineData(10, 1, 10, 1)]
    [InlineData(2, 6, 2, 6)]
    public void BeEquals(int x1, int y1, int x2, int y2)
    {
        var l1 = new Location(x1, y1);
        var l2 = new Location(x2, y2);
        l1.Should().Be(l2);
        l2.Should().Be(l1);
    }

    [Theory]
    [InlineData(1, 10, 10, 1)]
    [InlineData(10, 1, 1, 10)]
    [InlineData(2, 6, 6, 2)]
    public void NotBeEquals(int x1, int y1, int x2, int y2)
    {
        var l1 = new Location(x1, y1);
        var l2 = new Location(x2, y2);
        l1.Should().NotBe(l2);
        l2.Should().NotBe(l1);
    }

    [Fact]
    public void CreateRandomLocation()
    {
        var l = Location.CreateRandom();
        l.X.Should().BeGreaterThanOrEqualTo(1).And.BeLessThanOrEqualTo(10);
        l.Y.Should().BeGreaterThanOrEqualTo(1).And.BeLessThanOrEqualTo(10);
    }

}