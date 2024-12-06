using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.Model.CourierAggregate;
using Primitives;

namespace DeliveryApp.Core.Ports;

public interface ICourierRepository: IRepository<Courier>
{
    /// <summary>
    /// Добавить курьера
    /// </summary>
    Task<UnitResult<Error>> AddAsync(Courier courier);
    /// <summary>
    /// Обновить курьера
    /// </summary>
    UnitResult<Error> Update(Courier courier);
    /// <summary>
    /// Получить курьера по идентификатору
    /// </summary>
    Task<Result<Courier, Error>> GetByIdAsync(Guid courierId);
    /// <summary>
    /// Получить всех свободных курьеров (курьеры со статусом "Ready")
    /// </summary>
    Result<IReadOnlyCollection<Courier>, Error> GetFree();
}