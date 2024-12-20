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

        #region Query for GetRestaurants()

        /*    
    @query nvarchar(MAX) = '',
    @latpoint float = 0,
	@longpoint float = 0,
	@offset int = 0,
	@fetch int = 10

            SELECT [Id], [Name], [Photo], [LegalName], [AddressLine1], [AddressLine2], [Locality], [City], [Postcode], [Cuisine], [Latitude], [Longitude], ROUND([Distance], 2) AS [Distance]
            FROM (
	            SELECT z.[Id], z.[Name], z.[Photo], z.[LegalName], z.[AddressLine1], z.[AddressLine2], z.[Locality], z.[City], z.[Postcode], z.[Cuisine], z.[Latitude], z.[Longitude], p.[Radius],
		                p.[DistanceUnit]
				            * DEGREES(ACOS(LEAST(1.0, COS(RADIANS(p.[LatPoint]))
                            * COS(RADIANS(z.[Latitude]))
                            * COS(RADIANS(p.[LongPoint] - z.[Longitude]))
                            + SIN(RADIANS(p.[LatPoint]))
                            * SIN(RADIANS(z.[Latitude]))))) AS [Distance]
	            FROM [Restaurants] AS z
	            JOIN (
		            SELECT @latpoint AS [LatPoint], 
		                   @longpoint AS [LongPoint],
			               50.0 AS [Radius],
			               111.045 AS [DistanceUnit]
                ) AS p ON 1=1
	            WHERE z.[Latitude] BETWEEN p.[LatPoint] - (p.[Radius] / p.[DistanceUnit]) AND 
	                                        p.[LatPoint] + (p.[Radius] / p.[DistanceUnit]) AND
		                z.[Longitude] BETWEEN p.[LongPoint] - (p.[Radius] / (p.[DistanceUnit] * COS(RADIANS(p.[LatPoint])))) AND 
							                p.[LongPoint] + (p.[Radius] / (p.[DistanceUnit] * COS(RADIANS(p.[LatPoint]))))
            ) AS d
            WHERE [Distance] <= [Radius] AND [NAME] LIKE '%' + @query + '%'
            ORDER BY [Distance]
            OFFSET @offset ROWS 
            FETCH NEXT @fetch ROWS ONLY
        */

        #endregion

        public async Task<List<RestaurantModel>> GetRestaurants(double latitude, double longitude, string? query = "", int? page = 1, int? pageSize = 10)
        {
            int offset = (Convert.ToInt32(page) - 1) * Convert.ToInt32(pageSize);
            int fetch = Convert.ToInt32(page) * Convert.ToInt32(pageSize);

            var restaurants = await _context.Database.SqlQuery<RestaurantModel>($"EXEC [dbo].[GetRestaurants] @latpoint={latitude}, @longpoint = {longitude}, @query = {query}, @offset = {offset}, @fetch = {fetch};").ToListAsync();
            return restaurants;
        }

        #region Query for GetRestaurantsRecentlylVisited

        /*
    @user int = null,
    @latpoint float = 0,
	@longpoint float = 0,
    @offset int = 0,
    @fetch int = 10

	        SELECT [Id], [Name], [Photo], [LegalName], [AddressLine1], [AddressLine2], [Locality], [City], [Postcode], [Cuisine], [Latitude], [Longitude], ROUND([Distance], 2) AS [Distance], [DateOrdered], FORMAT([DateOrdered], 'dd MMM yy, hh:mm tt', 'en-gb') AS [FormattedDateOrdered]
	        FROM (
		        SELECT z.[Id], z.[Name], z.[Photo], z.[LegalName], z.[AddressLine1], z.[AddressLine2], z.[Locality], z.[City], z.[Postcode], z.[Cuisine], z.[Latitude], z.[Longitude], p.[Radius], w.[DateOrdered],
					        p.[DistanceUnit]
						        * DEGREES(ACOS(LEAST(1.0, COS(RADIANS(p.[LatPoint]))
						        * COS(RADIANS(z.[Latitude]))
						        * COS(RADIANS(p.[LongPoint] - z.[Longitude]))
						        + SIN(RADIANS(p.[LatPoint]))
						        * SIN(RADIANS(z.[Latitude]))))) AS [Distance]
		        FROM Restaurants AS z
		        INNER JOIN (
			        SELECT t.[RestaurantId], MAX(t.[DateOrdered]) AS [DateOrdered]
			        FROM [Orders] AS t
			        WHERE UserId = @user
			        GROUP BY t.[RestaurantId]
		        ) AS w ON w.RestaurantId = z.Id
		        JOIN (
			        SELECT @latpoint AS [LatPoint], 
				           @longpoint AS [LongPoint],
				           50.0 AS [Radius],
				           111.045 AS [DistanceUnit]
		        ) AS p ON 1=1
		        WHERE z.[Latitude] BETWEEN p.[LatPoint] - (p.[Radius] / p.[DistanceUnit]) AND 
									        p.[LatPoint] + (p.[Radius] / p.[DistanceUnit]) AND
				        z.[Longitude] BETWEEN p.[LongPoint] - (p.[Radius] / (p.[DistanceUnit] * COS(RADIANS(p.[LatPoint])))) AND 
									        p.[LongPoint] + (p.[Radius] / (p.[DistanceUnit] * COS(RADIANS(p.[LatPoint]))))
	        ) AS d
	        WHERE [Distance] <= [Radius]
	        ORDER BY [DateOrdered] DESC
	        OFFSET @offset ROWS 
	        FETCH NEXT @fetch ROWS ONLY 
         */

        #endregion

        public async Task<List<RestaurantRecentlyVisitedModel>> GetRestaurantsRecentlyVisited(int userId, double latitude, double longitude, int? page = 1, int? pageSize = 10)
        {
            int offset = (Convert.ToInt32(page) - 1) * Convert.ToInt32(pageSize);
            int fetch = Convert.ToInt32(page) * Convert.ToInt32(pageSize);

            var restaurants = await _context.Database.SqlQuery<RestaurantRecentlyVisitedModel>($"EXEC [dbo].[GetRestaurantsRecentlyVisited] @user={userId}, @latpoint={latitude}, @longpoint = {longitude}, @offset = {offset}, @fetch = {fetch};").ToListAsync();
            return restaurants;
        }

        #region Query for GetRestaurant

        /*
	    @restaurant int = null,
        @latpoint float = 0,
	    @longpoint float = 0

	        SELECT z.[Id], z.[Name], z.[Photo], z.[Locality], z.[City], z.[Postcode], z.[Cuisine], 
                ROUND(p.[DistanceUnit]
                    * DEGREES(ACOS(LEAST(1.0, COS(RADIANS(p.[LatPoint]))
                    * COS(RADIANS(z.[Latitude]))
                    * COS(RADIANS(p.[LongPoint] - z.[Longitude]))
                    + SIN(RADIANS(p.[LatPoint]))
                    * SIN(RADIANS(z.[Latitude]))))), 2) AS [Distance]
	        FROM [Restaurants] AS z
	        JOIN (
		        SELECT @latpoint AS [LatPoint], 
				        @longpoint AS [LongPoint],
				        50.0 AS [Radius],
				        111.045 AS [DistanceUnit]
	        ) AS p ON 1=1
	        WHERE z.Id = @restaurant
         */

        #endregion

        public async Task<RestaurantModel> GetRestaurant(int restaurantId, double latitude, double longitude)
        {
            var restaurant = _context.Database.SqlQuery<RestaurantModel>($"EXEC [dbo].[GetRestaurant] @restaurant={restaurantId}, @latpoint={latitude}, @longpoint = {longitude};").AsEnumerable().FirstOrDefault();
            return restaurant;
        }

        public async Task<List<CategorizedFoodItemModel>> GetFoodItems(int userId, int restaurantId, string? searchText = null)
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

            var cartItems = await (from m in _context.Carts
                             join n in _context.CartItems on m.Id equals n.CartId
                             join o in _context.FoodItems on n.FoodItemId equals o.Id
                             where m.UserId == userId
                             select new
                             {
                                 FoodItemId = n.FoodItemId,
                                 Quantity = n.Quantity
                             }).ToListAsync();

            foreach (var foodItem in foodItems)
            {
                var cartItem = cartItems.Where(x => x.FoodItemId == foodItem.Id && x.Quantity > 0).FirstOrDefault();

                if (cartItem != null)
                {
                    foodItem.Quantity = cartItem.Quantity;
                }
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
    }
}
