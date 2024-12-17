using DeliveryApp.Core.Domain.SharedKernel;
using DeliveryApp.Core.Ports;
using MediatR;
using Primitives;

namespace DeliveryApp.Core.Application.UseCases.Commands.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, bool>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateOrderCommandHandler(
        IOrderRepository orderRepository, 
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var location = GetLocationByStreet(request.Street);
        var newOrder = Domain.Model.OrderAggregate.Order.Create(request.BasketId, location);
        await _orderRepository.AddAsync(newOrder.Value);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    private Location GetLocationByStreet(string street)
    {
        return Location.CreateRandom();
    }
}
