using CSharpFunctionalExtensions;

namespace DeliveryApp.Core.Domain.SharedKernel;

public class Location : ValueObject
{
    public const int MinValue = 1;
    public const int MaxValue = 10;

    public Location(int x, int y)
    {
        if (x < MinValue || x > MaxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(x));
        }
        
        if (y < MinValue || y > MaxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(y));
        }

        X = x;
        Y = y;
    }

    public int X { get; }
    public int Y { get; }

    public int DistanceTo(Location other)
    {
        return Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
    }

    public static Location CreateRandom()
    {
        var random = new Random();
        var x = random.Next(MinValue, MaxValue);
        var y = random.Next(MinValue, MaxValue);
        return new Location(x, y);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return X;
        yield return Y;
    }
}