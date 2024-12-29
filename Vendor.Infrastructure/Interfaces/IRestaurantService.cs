namespace Vendor.Infrastructure
{
    public interface IRestaurantService
    {
        Task<RestaurantModel> GetRestaurant(int restaurantId);
        Task<List<CategorizedFoodItemModel>> GetFoodItems(int restaurantId, string? searchText = null);
        Task<List<CategoryModel>> GetCategories(int restaurantId);

        Task UpdateRestaurantStatus(int restaurantId, bool isAvailable);
        Task UpdatePreparationTime(int restaurantId, int preparationTime);
        Task<bool> UpdateFoodItem(FoodItemModel foodItem);
        Task<bool> UpdateCategory(CategoryModel category);
        Task<bool> ReorderCategory(int categoryId, bool increase);
    }
}
