using System;
using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.Model.CourierAggregate;
using DeliveryApp.Core.Domain.SharedKernel;
using FluentAssertions;
using Primitives;
using Xunit;

namespace DeliveryApp.UnitTests;

public class TransportShould
{
    [Fact]
    public void BeCorrectWhenParamsAreCorrectOnCreated()
    {
        var result = Transport.Create(4, "Rocket", 10);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Id.Should().Be(4);
        result.Value.Name.Should().Be("Rocket");
        result.Value.Speed.Should().Be(10);
    }

    [Theory]
    [InlineData(0, "Rocket", 10)]
    [InlineData(-1, "Rocket", 10)]
    [InlineData(4, null, 10)]
    [InlineData(4, "", 10)]
    [InlineData(4, " ", 10)]
    [InlineData(4, "\t", 10)]
    [InlineData(4, "\n", 10)]
    [InlineData(4, "Rocket", 0)]
    [InlineData(4, "Rocket", -1)]
    public void ReturnErrorWhenParamsAreNotCorrectOnCreated(int id, string name, int speed)
    {
        var result = Transport.Create(id, name, speed);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
    }

    [Fact]
    public void DoFromIdCorrect()
    {
        var result = Transport.FromId(1);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Id.Should().Be(1);
    }

    [Fact]
    public void DoFromIdFail()
    {
        var result = Transport.FromId(10);

        result.IsFailure.Should().BeTrue();
        Assert.Throws<ResultFailureException<Error>>(() => result.Value);
    }

    [Fact]
    public void DoFromNameCorrect()
    {
        var result = Transport.FromName("Car");

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Name.Should().Be("Car");
    }

    [Fact]
    public void DoFromNameFail()
    {
        var result = Transport.FromName("Train");

        result.IsFailure.Should().BeTrue();
        Assert.Throws<ResultFailureException<Error>>(() => result.Value);
    }

    [Fact]
    public void BeEqualsIfIdIsEquals()
    {
        var rocket1 = Transport.Create(4, "Rocket1", 12);
        var rocket2 = Transport.Create(4, "Rocket2", 15);
        var train1 = Transport.Create(5, "Train", 5);
        var train2 = Transport.Create(6, "Train", 5);

        rocket1.IsSuccess.Should().BeTrue();
        rocket2.IsSuccess.Should().BeTrue();
        rocket1.Value.Should().NotBeNull();
        rocket2.Value.Should().NotBeNull();
        rocket1.Value.Should().Be(rocket2.Value);

        train1.IsSuccess.Should().BeTrue();
        train2.IsSuccess.Should().BeTrue();
        train1.Value.Should().NotBeNull();
        train2.Value.Should().NotBeNull();
        train1.Value.Should().NotBe(train2.Value);
    }
}