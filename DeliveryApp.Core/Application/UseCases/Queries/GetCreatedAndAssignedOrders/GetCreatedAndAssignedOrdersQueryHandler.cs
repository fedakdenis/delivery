using DeliveryApp.Core.Application.SharedKernel;
using DeliveryApp.Core.Ports;
using MediatR;

namespace DeliveryApp.Core.Application.UseCases.Queries.GetCreatedAndAssignedOrders;

public class GetCreatedAndAssignedOrdersQueryHandler : IRequestHandler<GetCreatedAndAssignedOrdersQuery, GetCreatedAndAssignedOrdersResponse>
{
    private readonly IOrderRepository _orderRepository;

    public GetCreatedAndAssignedOrdersQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    
    public Task<GetCreatedAndAssignedOrdersResponse> Handle(GetCreatedAndAssignedOrdersQuery request, CancellationToken cancellationToken)
    {
        var created = _orderRepository.GetCreated();
        var assigned = _orderRepository.GetAssigned();
        var createAndAssigned = created.Value
            .Union(assigned.Value)
            .Select(o => new Order
            {
                Id = o.Id,
                Location = new Location
                {
                    X = o.Location.X,
                    Y = o.Location.Y
                }
            })
            .ToList();

        return Task.FromResult(new GetCreatedAndAssignedOrdersResponse(createAndAssigned));
    }
}
