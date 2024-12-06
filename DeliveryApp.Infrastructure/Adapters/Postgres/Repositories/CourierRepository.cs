using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.Model.CourierAggregate;
using DeliveryApp.Core.Ports;
using Microsoft.EntityFrameworkCore;
using Primitives;

namespace DeliveryApp.Infrastructure.Adapters.Postgres.Repositories;

public class CourierRepository : ICourierRepository
{
    private ApplicationDbContext _applicationDbContext;

    public CourierRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<UnitResult<Error>> AddAsync(Courier courier)
    {
        Attach(courier);

        await _applicationDbContext.Couriers.AddAsync(courier);

        return UnitResult.Success<Error>();
    }

    public async Task<Result<Courier, Error>> GetByIdAsync(Guid courierId)
    {
        if (courierId == Guid.Empty)
        {
            return GeneralErrors.ValueIsInvalid(nameof(courierId));
        }

        var result = await GetQuery()
            .FirstOrDefaultAsync(o => o.Id == courierId);

        if (result == null)
        {
            return GeneralErrors.NotFound();
        }

        return result;
    }

    public Result<IReadOnlyCollection<Courier>, Error> GetFree()
    {
        var free = CourierStatus.Free;
        var result = GetQuery()
            .Where(o => o.Status.Id == free.Id)
            .ToArray();

        return result;
    }

    public UnitResult<Error> Update(Courier courier)
    {
        Attach(courier);

        _applicationDbContext.Couriers.Update(courier);

        return UnitResult.Success<Error>();
    }

    private IQueryable<Courier> GetQuery()
    {
        return _applicationDbContext.Couriers
            .Include(o => o.Status)
            .Include(o => o.Transport);
    }

    private void Attach(Courier courier)
    {
        if (courier.Status != null)
        {
            _applicationDbContext.Attach(courier.Status);
        }
        if (courier.Transport != null)
        {
            _applicationDbContext.Attach(courier.Transport);
        }
    }
}