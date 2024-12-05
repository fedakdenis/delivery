using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.Model.CourierAggregate;
using DeliveryApp.Core.Ports;
using Primitives;

namespace DeliveryApp.Infrastructure.Adapters.Postgres.Repositories;

public class CourierRepository : ICourierRepository
{
    public UnitResult<Error> Add(Courier courier)
    {
        throw new NotImplementedException();
    }

    public Result<Courier, Error> GetById(Guid courierId)
    {
        throw new NotImplementedException();
    }

    public Result<IReadOnlyList<Courier>, Error> GetReady()
    {
        throw new NotImplementedException();
    }

    public UnitResult<Error> Update(Courier courier)
    {
        throw new NotImplementedException();
    }
}