using Microsoft.EntityFrameworkCore;
using Vendor.Data;

namespace Vendor.Infrastructure
{
    public class RestaurantService : IRestaurantService
    {
        private readonly VendorContext _context;

        public RestaurantService(VendorContext context)
        {
            _context = context;
        }

        public async Task<RestaurantModel> GetRestaurant(int restaurantId)
        {
            var restaurant = await _context.Restaurants
                                           .Where(x => x.Id == restaurantId)
                                           .Select(x => new RestaurantModel {
                                               Id = x.Id,
                                               Name = x.Name,
                                               Photo = x.Photo,
                                               Locality = x.Locality,
                                               City = x.City,
                                               Cuisine = x.Cuisine,
                                               Distance = 0
                                           }).FirstOrDefaultAsync();
            return restaurant;
        }

        public async Task<List<CategorizedFoodItemModel>> GetFoodItems(int restaurantId, string? searchText = null)
        {
            var categorizedFoodItems = new List<CategorizedFoodItemModel>();
            var foodItems = new List<FoodItemModel>();

            var categories = await _context.Categories.Where(x => x.RestaurantId == restaurantId).OrderBy(x => x.Order).ToListAsync();

            if (searchText != null)
            {
                foodItems = await _context.FoodItems.Where(x => x.RestaurantId == restaurantId && x.Name.Contains(searchText))
                                        .Select(x => new FoodItemModel
                                        {
                                            Id = x.Id,
                                            CategoryId = x.CategoryId,
                                            Name = x.Name,
                                            Description = x.Description,
                                            Type = x.Type,
                                            Photo = x.Photo,
                                            TaxablePrice = x.Price / (100 + x.Restaurant.PrimaryTaxRate + x.Restaurant.SecondaryTaxRate),
                                            Price = x.Price
                                        }).ToListAsync();
            }
            else
            {
                foodItems = await _context.FoodItems.Where(x => x.RestaurantId == restaurantId)
                                        .Select(x => new FoodItemModel
                                        {
                                            Id = x.Id,
                                            CategoryId = x.CategoryId,
                                            Name = x.Name,
                                            Description = x.Description,
                                            Type = x.Type,
                                            Photo = x.Photo,
                                            TaxablePrice = x.Price / (100 + x.Restaurant.PrimaryTaxRate + x.Restaurant.SecondaryTaxRate),
                                            Price = x.Price
                                        }).ToListAsync();
            }

            foreach (var category in categories)
            {
                var categorizedFoodItem = new CategorizedFoodItemModel();

                var foodItemsByCategory = foodItems.Where(x => x.CategoryId == category.Id).ToList();

                if (foodItemsByCategory.Count > 0)
                {
                    categorizedFoodItem.Title = category.Name;
                    categorizedFoodItem.Data = foodItemsByCategory;

                    categorizedFoodItems.Add(categorizedFoodItem);
                }
            }

            return categorizedFoodItems;
        }

        public async Task<List<CategoryModel>> GetCategories(int restaurantId)
        {
            var categories = await _context.Categories
                                     .Where(x => x.RestaurantId == restaurantId)
                                     .Select(x => new CategoryModel { 
                                         Id = x.Id,
                                         Name = x.Name,
                                         Order = x.Order
                                     })
                                     .ToListAsync();
            return categories;
        }

        public async Task UpdateRestaurantStatus(int restaurantId, bool isAvailable)
        {
            var item = await _context.Restaurants.Where(x => x.Id == restaurantId).FirstOrDefaultAsync();

            if (item != null)
            {
                item.IsAvailable = isAvailable;
            }

            _context.Restaurants.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePreparationTime(int restaurantId, int preparationTime)
        {
            var item = await _context.Restaurants.Where(x => x.Id == restaurantId).FirstOrDefaultAsync();

            if (item != null)
            {
                item.PreparationTime = preparationTime;
            }

            _context.Restaurants.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateFoodItem(FoodItemModel foodItem)
        {
            var item = await _context.FoodItems.Where(x => x.Id == foodItem.Id).FirstOrDefaultAsync();

            if (item != null) 
            {
                item.Name = foodItem.Name;
                item.Description = foodItem.Description;
                item.Type = foodItem.Type;
                item.Photo = foodItem.Photo;
                item.Price = foodItem.Price;
            }

            _context.FoodItems.Update(item);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateCategory(CategoryModel category)
        {
            var item = await _context.Categories.Where(x => x.Id == category.Id).FirstOrDefaultAsync();

            if (item != null)
            {
                item.Name = category.Name;
            }

            _context.Categories.Update(item);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ReorderCategory(int categoryId, bool increase)
        {
            Category swap = new Category();
            int itemOrder, swapOrder = 0;
            var item = await _context.Categories
                                     .Where(x => x.Id == categoryId)
                                     .FirstOrDefaultAsync();
            var list = await _context.Categories
                                     .Where(x => x.RestaurantId == item.RestaurantId)
                                     .OrderBy(x => x.Order)
                                     .ToListAsync();
            var count = list.Count;

            if (item != null && count > 1)
            {
                if ((item.Order == 1 && increase == true) || (item.Order != 1 && item.Order != count && increase == true))
                {
                    swap = list.FirstOrDefault(x => x.Order == (item.Order + 1));

                    swap.Order = item.Order;
                    item.Order = item.Order + 1;
                }
                else if ((item.Order == count && increase == false) || (item.Order != 1 && item.Order != count && increase == false))
                {
                    swap = list.FirstOrDefault(x => x.Order == (item.Order - 1));

                    swap.Order = item.Order;
                    item.Order = item.Order - 1;
                }
                else
                {
                    return false;
                }

                _context.Categories.Update(item);
                _context.Categories.Update(swap);
                await _context.SaveChangesAsync();
            }

            return true;
        }
    }
}
