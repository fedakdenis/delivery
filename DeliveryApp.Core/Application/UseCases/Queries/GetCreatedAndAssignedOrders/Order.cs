using DeliveryApp.Core.Application.SharedKernel;

namespace DeliveryApp.Core.Application.UseCases.Queries.GetCreatedAndAssignedOrders;

public class Order
{
    /// <summary>
    ///     Идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     Геопозиция (X,Y)
    /// </summary>
    public Location Location { get; set; }
}
