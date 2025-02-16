using DeliveryApp.Core.Ports;
using MediatR;
using Primitives;

namespace DeliveryApp.Core.Application.UseCases.Commands.AssignCourier;

public class AssignCourierCommandHandler : IRequestHandler<AssignCourierCommand, bool>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICourierRepository _courierRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AssignCourierCommandHandler(
        IOrderRepository orderRepository, 
        ICourierRepository courierRepository,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _courierRepository = courierRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(AssignCourierCommand request, CancellationToken cancellationToken)
    {
        var ordersToAssign = _orderRepository.GetCreated();
        var orderToAssign = ordersToAssign.Value.FirstOrDefault();
        if (orderToAssign == null)
        {
            return false;
        }

        var freeCouriers = _courierRepository.GetFree();
        var fastestCourier = freeCouriers.Value
            .OrderBy(c => c.GetStepCount(orderToAssign.Location).Value)
            .FirstOrDefault();
        if (fastestCourier == null)
        {
            return false;
        }

        var assignResult = orderToAssign.Assign(fastestCourier.Id);
        if (assignResult.IsFailure)
        {
            throw new InvalidOperationException(assignResult.Error.Message);
        }
        _orderRepository.Update(orderToAssign);

        var busyResult = fastestCourier.Busy();
        if (busyResult.IsFailure)
        {
            throw new InvalidOperationException(busyResult.Error.Message);
        }
        _courierRepository.Update(fastestCourier);

        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}
