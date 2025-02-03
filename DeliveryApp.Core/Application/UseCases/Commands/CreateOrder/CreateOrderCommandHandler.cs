using DeliveryApp.Core.Domain.SharedKernel;
using DeliveryApp.Core.Ports;
using MediatR;
using Primitives;

namespace DeliveryApp.Core.Application.UseCases.Commands.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, bool>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGeoClient _geoClient;

    public CreateOrderCommandHandler(
        IOrderRepository orderRepository, 
        IUnitOfWork unitOfWork,
        IGeoClient geoClient)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _geoClient = geoClient;
    }

    public async Task<bool> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var location = await GetLocationByStreetAsync(request.Street, cancellationToken);
        var newOrder = Domain.Model.OrderAggregate.Order.Create(request.BasketId, location);
        await _orderRepository.AddAsync(newOrder.Value);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    private async Task<Location> GetLocationByStreetAsync(string street, CancellationToken cancellationToken)
    {        
        return await _geoClient.GetGeolocationAsync(street, cancellationToken);
    }
}
