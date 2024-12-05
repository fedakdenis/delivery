using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using Primitives;

namespace DeliveryApp.Core.Ports;

public interface IOrderRepository: IRepository<Order>
{
    /// <summary>
    /// Добавить заказ
    /// </summary>
    UnitResult<Error> Add(Order order);
    /// <summary>
    /// Обновить заказ
    /// </summary>
    UnitResult<Error> Update(Order order);
    /// <summary>Order
    /// Получить заказ по идентификатору
    /// </summary>
    Result<Order, Error> GetById(Guid orderId);
    /// <summary>Order
    /// Получить все новые заказы (заказы со статусом "Created")
    /// </summary>
    Result<IReadOnlyList<Order>, Error> GetCreated(Guid orderId);
    /// <summary>Order
    /// Получить все назначенные заказы (заказы со статусом "Assigned")
    /// </summary>
    Result<IReadOnlyList<Order>, Error> GetAssigned(Guid orderId);
}