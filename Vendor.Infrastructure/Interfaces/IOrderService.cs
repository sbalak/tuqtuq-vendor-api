namespace Vendor.Infrastructure
{
    public interface IOrderService
    {
        Task<List<OrderModel>> GetOrders(int userId, int? page = 1, int? pageSize = 10);
        Task<OrderModel> GetOrder(int id);
        Task Confirm(int userId);
    }
}
