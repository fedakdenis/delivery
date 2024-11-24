using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using Primitives;

namespace DeliveryApp.Core.Domain.Model.CourierAggregate;

public class Transport : Entity<int>
{
    public static readonly Transport Pedestrian = Create(1, "Pedestrian", 1).Value;
    public static readonly Transport Bicycle = Create(2, "Bicycle", 2).Value;
    public static readonly Transport Car = Create(3, "Car", 3).Value;

    [ExcludeFromCodeCoverage]
    private Transport()
    {
    }

    private Transport(int id, string name, int speed) : this()
    {
        Id = id;
        Name = name;
        Speed = speed;
    }

    public string Name { get; private set; }
    public int Speed { get; private set; }

    public static Result<Transport, Error> Create(int id, string name, int speed)
    {
        if (id < 1)
        {
            return Errors.OutOfRange(nameof(id));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            return Errors.OutOfRange(nameof(name));
        }

        if (speed < 1)
        {
            return Errors.OutOfRange(nameof(speed));
        }

        return new Transport(id, name, speed);
    }

    public static Result<Transport, Error> FromId(int id)
    {
        if (id < 1)
        {
            return Errors.OutOfRange(nameof(id));
        }

        var result = List().SingleOrDefault(x => x.Id == id);
        if (result == null)
        {
            return Errors.NotFound(id);
        }

        return result;
    }

    public static Result<Transport, Error> FromName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Errors.OutOfRange(nameof(name));
        }

        var result = List().SingleOrDefault(x => x.Name == name);
        if (result == null)
        {
            return Errors.NotFound(name);
        }

        return result;
    }

    public static IEnumerable<Transport> List() 
    {
        yield return Pedestrian;
        yield return Bicycle;
        yield return Car;
    }

    public static class Errors
    {
        private const string TransportNotFountCode = "transport.not-found";
        private const string TransportOutOfRangeCode = "transport.out-of-range";

        public static Error NotFound(int id)
        {
            return new(TransportNotFountCode, $"Can not find transport with id {id}");
        }

        public static Error NotFound(string name)
        {
            return new(TransportNotFountCode, $"Can not find transport with name {name}");
        }

        public static Error OutOfRange(string parameter)
        {
            return new(TransportOutOfRangeCode, $"Parameter {parameter} is out of range");
        }
    }
}