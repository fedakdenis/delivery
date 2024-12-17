using DeliveryApp.Core.Application.SharedKernel;
using DeliveryApp.Core.Ports;
using MediatR;

namespace DeliveryApp.Core.Application.UseCases.Queries.GetCouriers;

public class GetCouriersQueryHandler : IRequestHandler<GetCouriersQuery, GetCouriersResponse>
{
    private readonly ICourierRepository _courierRepository;

    public GetCouriersQueryHandler(ICourierRepository courierRepository)
    {
        _courierRepository = courierRepository;
    }

    public Task<GetCouriersResponse> Handle(GetCouriersQuery request, CancellationToken cancellationToken)
    {
        var free = _courierRepository.GetFree();
        var freeCouriers = free.Value
            .Select(c => new Courier
            {
                Id = c.Id,
                Name = c.Name,
                Location = new Location
                {
                    X = c.Location.X,
                    Y = c.Location.Y
                }
            })
            .ToList();

        return Task.FromResult(new GetCouriersResponse(freeCouriers));
    }
}