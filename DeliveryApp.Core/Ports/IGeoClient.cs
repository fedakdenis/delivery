using DeliveryApp.Core.Domain.SharedKernel;

namespace DeliveryApp.Core.Ports;

public interface IGeoClient
{
    /// <summary>
    ///     Получить информацию о геолокации по улице
    /// </summary>
    Task<Location> GetGeolocationAsync(string street, CancellationToken cancellationToken);
}
