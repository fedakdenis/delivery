using DeliveryApp.Core.Domain.Model.CourierAggregate;
using DeliveryApp.Core.Domain.SharedKernel;
using DeliveryApp.Infrastructure.Adapters.Postgres.Repositories;
using FluentAssertions;
using Xunit;

namespace DeliveryApp.IntegrationTests.Repositories;

public class CourierRepositoryShould : RepositoryShouldBase
{
    [Fact]
    public async Task CanAddCourier()
    {
        var location = Location.CreateRandom();
        var newCourier = Courier.Create("Ivan", Transport.Bicycle, location);
        
        var courierRepository = new CourierRepository(_applicationDbContext);
        await courierRepository.AddAsync(newCourier.Value);
        await _applicationDbContext.SaveChangesAsync();

        var courier = await courierRepository.GetByIdAsync(newCourier.Value.Id);
        newCourier.Value.Should().BeEquivalentTo(courier.Value);
    }

    [Fact]
    public async Task CanUpdateCourier()
    {
        var location = Location.CreateRandom();
        var newCourier = Courier.Create("Ivan", Transport.Bicycle, location);
        
        var courierRepository = new CourierRepository(_applicationDbContext);
        await courierRepository.AddAsync(newCourier.Value);
        await _applicationDbContext.SaveChangesAsync();

        var courier = await courierRepository.GetByIdAsync(newCourier.Value.Id);
        courier.Value.Should().BeEquivalentTo(newCourier.Value);

        courier.Value.Busy();
        courierRepository.Update(courier.Value);
        await _applicationDbContext.SaveChangesAsync();

        var updatedCourier = await courierRepository.GetByIdAsync(newCourier.Value.Id);
        updatedCourier.Value.Should().BeEquivalentTo(courier.Value);
    }
}
