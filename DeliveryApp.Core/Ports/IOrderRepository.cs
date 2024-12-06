using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using Primitives;

namespace DeliveryApp.Core.Ports;

public interface IOrderRepository: IRepository<Order>
{
    /// <summary>
    /// Добавить заказ
    /// </summary>
    Task<UnitResult<Error>> AddAsync(Order order);
    /// <summary>
    /// Обновить заказ
    /// </summary>
    UnitResult<Error> Update(Order order);
    /// <summary>Order
    /// Получить заказ по идентификатору
    /// </summary>
    Task<Result<Order, Error>> GetByIdAsync(Guid orderId);
    /// <summary>Order
    /// Получить все новые заказы (заказы со статусом "Created")
    /// </summary>
    Result<IReadOnlyCollection<Order>, Error> GetCreated();
    /// <summary>Order
    /// Получить все назначенные заказы (заказы со статусом "Assigned")
    /// </summary>
    Result<IReadOnlyCollection<Order>, Error> GetAssigned();
}