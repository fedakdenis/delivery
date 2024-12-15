using DeliveryApp.Core.Application.SharedKernel;

namespace DeliveryApp.Core.Application.UseCases.Queries.GetCouriers;

public class Courier
{
    /// <summary>
    ///     Идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     Имя
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Геопозиция (X,Y)
    /// </summary>
    public Location Location { get; set; }

    /// <summary>
    ///     Вид транспорта
    /// </summary>
    public int TransportId { get; set; }
}
