namespace Vendor.Infrastructure
{
    public interface IOrderService
    {
        Task<List<OrderModel>> GetOrders(int restaurantId, int? page = 1, int? pageSize = 10);
        Task<OrderModel> GetOrder(int orderId);

        Task Accept(int orderId);
        Task Reject(int orderId);
        Task Complete(int orderId);
    }
}
