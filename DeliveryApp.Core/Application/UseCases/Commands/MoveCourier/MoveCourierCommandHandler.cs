using DeliveryApp.Core.Ports;
using MediatR;
using Primitives;

namespace DeliveryApp.Core.Application.UseCases.Commands.MoveCourier;

public class MoveCourierCommandHandler : IRequestHandler<MoveCourierCommand, bool>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICourierRepository _courierRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MoveCourierCommandHandler(
        IOrderRepository orderRepository, 
        ICourierRepository courierRepository,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _courierRepository = courierRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(MoveCourierCommand request, CancellationToken cancellationToken)
    {
        var assigned = _orderRepository.GetAssigned();
        if (assigned.Value.Count() > 0)
        {
            foreach (var order in assigned.Value)
            {
                var courierId = order.CourierId.Value;
                var courier = await _courierRepository.GetByIdAsync(courierId);
                courier.Value.DoStep(order.Location);

                if (order.Location == courier.Value.Location)
                {
                    courier.Value.Free();
                    order.Complete();
                }
            }

            await _unitOfWork.SaveChangesAsync();
        }

        return true;
    }
}
