using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vendor.Infrastructure;

namespace Vendor.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RestaurantController : ControllerBase
    {
        private IRestaurantService _restaurant;

        public RestaurantController(IRestaurantService restaurant)
        {
            _restaurant = restaurant;
        }

        [HttpGet("Details")]
        public async Task<RestaurantModel> Details(int restaurantId)
        {
            var restaurant = await _restaurant.GetRestaurant(restaurantId);
            return restaurant;
        }

        [HttpGet("Status/Update")]
        public async Task UpdateRestaurantStatus(int restaurantId, bool isAvailable)
        {
            await _restaurant.UpdateRestaurantStatus(restaurantId, isAvailable);
        }

        [HttpGet("PreparationTime/Update")]
        public async Task UpdatePreparationTime(int restaurantId, int preparationTime)
        {
            await _restaurant.UpdatePreparationTime(restaurantId, preparationTime);
        }

        [HttpGet("FoodItems")]
        public async Task<List<CategorizedFoodItemModel>> FoodItems(int restaurantId, string? searchText = null)
        {
            var foodItems = await _restaurant.GetFoodItems(restaurantId, searchText);
            return foodItems;
        }

        [HttpPost("FoodItems/Update")]
        public async Task<bool> UpdateFoodItem(FoodItemModel foodItem)
        {
            var updated = await _restaurant.UpdateFoodItem(foodItem);
            return updated;
        }

        [HttpGet("Categories")]
        public async Task<List<CategoryModel>> Categories(int restaurantId)
        {
            var categories = await _restaurant.GetCategories(restaurantId);
            return categories;
        }

        [HttpPost("Categories/Update")]
        public async Task<bool> UpdateCategory(CategoryModel category)
        {
            var updated = await _restaurant.UpdateCategory(category);
            return updated;
        }

        [HttpPost("Categories/Reorder")]
        public async Task<bool> Reorder(int categoryId, bool increase)
        {
            var reordered = await _restaurant.ReorderCategory(categoryId, increase);
            return reordered;
        }
    }
}
