namespace Vendor.Infrastructure
{
    public interface ICartService
    {
        Task<CartModel> GetCart(int userId);
        Task<CartValueModel> GetCartValue(int userId, int restaurantId);
        Task Add(int userId, int restaurantId, int foodId);
        Task Remove(int userId, int restaurantId, int foodId);
    }
}
