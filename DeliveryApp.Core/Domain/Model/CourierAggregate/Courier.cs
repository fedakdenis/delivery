using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.SharedKernel;
using Primitives;

namespace DeliveryApp.Core.Domain.Model.CourierAggregate;

public class Courier : Aggregate
{
    private Courier() { }

    private Courier(string name, Transport transport, Location location)
    {
        Id = Guid.NewGuid();
        Name = name;
        Transport = transport;
        Location = location;
        Status = CourierStatus.Free;
    }

    public static Result<Courier, Error> Create(string name, Transport transport, Location location)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return GeneralErrors.ValueIsRequired(nameof(name));
        }

        if (transport == null)
        {
            return GeneralErrors.ValueIsRequired(nameof(transport));
        }

        if (location == null)
        {
            return GeneralErrors.ValueIsInvalid(nameof(location));
        }

        return new Courier(name, transport, location);
    }

    public string Name { get; private set; }
    public Transport Transport { get; private set; }
    public Location Location { get; private set; }
    public CourierStatus Status { get; private set; }

    public Result<object, Error> Busy()
    {
        if (Status == CourierStatus.Busy)
        {
            return Errors.CourierIsBusy();
        }

        Status = CourierStatus.Busy;
        return new object();
    }

    public Result<object, Error> Free()
    {
        if (Status == CourierStatus.Free)
        {
            return Errors.CourierIsFree();
        }

        Status = CourierStatus.Free;
        return new object();
    }

    public Result<int, Error> GetStepCount(Location destination)
    {
        var distanceToTarget = Location.DistanceTo(destination);
        var stepCount = Math.Ceiling((double)distanceToTarget / Transport.Speed);
        return Convert.ToInt32(stepCount);
    }

    public Result<object, Error> DoStep(Location destination)
    {
        var stepSize = Transport.Speed;
        var horizontalDeleta = destination.X - Location.X;
        if (Math.Abs(horizontalDeleta) >= stepSize)
        {
            Location = new(Location.X + stepSize * Math.Sign(horizontalDeleta), Location.Y);
            return new object();
        }

        stepSize -= Math.Abs(horizontalDeleta);
        var verticalDeleta = destination.Y - Location.Y;
        if (Math.Abs(verticalDeleta) >= stepSize)
        {
            Location = new(destination.X, Location.Y + stepSize * Math.Sign(verticalDeleta));
            return new object();
        }

        Location = destination;
        return new object();
    }

    static class Errors
    {
        public static Error CourierIsBusy()
            => new Error("courier.is-busy", "Courier is busy");

        public static Error CourierIsFree()
            => new Error("courier.is-free", "Courier is free");
    }
}