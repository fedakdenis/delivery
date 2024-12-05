using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.Model.CourierAggregate;
using Primitives;

namespace DeliveryApp.Core.Ports;

public interface ICourierRepository: IRepository<Courier>
{
    /// <summary>
    /// Добавить курьера
    /// </summary>
    UnitResult<Error> Add(Courier courier);
    /// <summary>
    /// Обновить курьера
    /// </summary>
    UnitResult<Error> Update(Courier courier);
    /// <summary>
    /// Получить курьера по идентификатору
    /// </summary>
    Result<Courier, Error> GetById(Guid courierId);
    /// <summary>
    /// Получить всех свободных курьеров (курьеры со статусом "Ready")
    /// </summary>
    Result<IReadOnlyList<Courier>, Error> GetReady();
}