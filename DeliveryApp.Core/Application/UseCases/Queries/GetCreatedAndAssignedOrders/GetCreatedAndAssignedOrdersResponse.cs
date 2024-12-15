namespace DeliveryApp.Core.Application.UseCases.Queries.GetCreatedAndAssignedOrders;

public class GetCreatedAndAssignedOrdersResponse
{
    public GetCreatedAndAssignedOrdersResponse(List<Order> orders)
    {
        Orders.AddRange(orders);
    }

    public List<Order> Orders { get; set; } = new();
}
