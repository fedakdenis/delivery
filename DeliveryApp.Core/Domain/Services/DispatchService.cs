using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.Model.CourierAggregate;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using Primitives;

namespace DeliveryApp.Core.Domain.Services;

public class DispatchService : IDispatchService
{
    public Result<Courier, Error> Dispatch(Order order, List<Courier> couriers)
    {
        if (order == null)
        {
            return GeneralErrors.ValueIsRequired(nameof(order));
        }

        if (couriers == null || couriers.Count == 0)
        {
            return GeneralErrors.ValueIsRequired(nameof(couriers));
        }

        var courierEnumerator = couriers.GetEnumerator();
        if (!courierEnumerator.MoveNext())
        {
            return GeneralErrors.CollectionIsTooSmall(1, 0);
        }

        var result = couriers
            .Select(courier => new
            {
                courier,
                stepCountResult = courier.GetStepCount(order.Location)
            })
            .Where(x => x.stepCountResult.IsSuccess)
            .OrderBy(x => x.stepCountResult.Value)
            .First().courier;
        return result;
    }
}