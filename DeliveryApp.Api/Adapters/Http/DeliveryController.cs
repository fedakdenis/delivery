using DeliveryApp.Core.Application.UseCases.Commands.CreateOrder;
using DeliveryApp.Core.Application.UseCases.Queries.GetCouriers;
using DeliveryApp.Core.Application.UseCases.Queries.GetCreatedAndAssignedOrders;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OpenApi.Controllers;
using Courier = OpenApi.Models.Courier;
using Location = OpenApi.Models.Location;
using Order = OpenApi.Models.Order;


namespace DeliveryApp.Api.Adapters.Http;

public class DeliveryController : DefaultApiController
{
    private readonly IMediator _mediator;

    public DeliveryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async override Task<IActionResult> CreateOrder()
    {
        var orderId = Guid.NewGuid();
        var street = "Несуществующая";
        var createOrderCommand = new CreateOrderCommand(orderId, street);
        var response = await _mediator.Send(createOrderCommand);
        if (response) 
        {
            return Ok();
        }
        
        return Conflict();
    }

    public async override Task<IActionResult> GetCouriers()
    {
        var result = await _mediator.Send(new GetCouriersQuery());
        var model = result.Couriers.Select(c => new Courier
        {
            Id = c.Id,
            Name = c.Name,
            Location = new Location 
            { 
                X = c.Location.X, 
                Y = c.Location.Y
            }
        });
        
        return Ok(model);
    }

    public async override Task<IActionResult> GetOrders()
    {
        var result = await _mediator.Send(new GetCreatedAndAssignedOrdersQuery());
        var model = result.Orders.Select(o => new Order
        {
            Id = o.Id,
            Location = new Location 
            { 
                X = o.Location.X, 
                Y = o.Location.Y
            }
        });

        return Ok(result);
    }
}