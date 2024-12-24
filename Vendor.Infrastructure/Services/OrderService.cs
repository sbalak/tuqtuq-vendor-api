using Microsoft.EntityFrameworkCore;
using Vendor.Data;
using System.Globalization;

namespace Vendor.Infrastructure
{
    public class OrderService : IOrderService
    {
        private readonly VendorContext _context;

        public OrderService(VendorContext context)
        {
            _context = context;
        }

        public async Task<List<OrderModel>> GetOrders(int restaurantId, int? page = 1, int? pageSize = 10)
        {
            List<OrderModel> orders = new List<OrderModel>();

            int offset = (Convert.ToInt32(page) - 1) * Convert.ToInt32(pageSize);
            int fetch = Convert.ToInt32(page) * Convert.ToInt32(pageSize);

            var ordersList = await (from m in _context.Orders
                                    join n in _context.Restaurants on m.RestaurantId equals n.Id
                                    where n.Id == restaurantId
                                    select new
                                    {
                                        OrderId = m.Id,
                                        RestaurantId = n.Id,
                                        RestaurantName = n.Name,
                                        RestaurantLocality = n.Locality,
                                        Status = m.Status,
                                        TotalTaxableAmount = m.TaxableAmount,
                                        TotalAmount = m.Amount,
                                        PrimaryTaxAmount = m.PrimaryTaxAmount,
                                        SecondaryTaxAmount = m.SecondaryTaxAmount,
                                        DateOrdered = m.DateOrdered,
                                        FormattedDateOrdered = m.DateOrdered.ToString("dd MMM yy, hh:mm tt")
                                    }).OrderByDescending(m => m.DateOrdered).Skip(offset).Take(fetch).ToListAsync();

            foreach (var ordersItem in ordersList)
            {
                OrderModel order = new OrderModel()
                {
                    OrderId = ordersItem.OrderId,
                    RestaurantId = ordersItem.RestaurantId,
                    RestaurantName = ordersItem.RestaurantName,
                    RestaurantLocality = ordersItem.RestaurantLocality,
                    Status = ordersItem.Status,
                    TotalPrimaryTaxAmount = Assist.Rupee(ordersItem.PrimaryTaxAmount),
                    TotalSecondaryTaxAmount = Assist.Rupee(ordersItem.SecondaryTaxAmount),
                    TotalTaxAmount = Assist.Rupee(ordersItem.PrimaryTaxAmount + ordersItem.SecondaryTaxAmount),
                    TotalTaxableAmount = Assist.Rupee(ordersItem.TotalTaxableAmount),
                    TotalAmount = Assist.Rupee(ordersItem.TotalAmount),
                    DateOrdered = ordersItem.FormattedDateOrdered
                };

                var foodItems = await (from m in _context.Orders
                                       join n in _context.OrderItems on m.Id equals n.OrderId
                                       join o in _context.FoodItems on n.FoodItemId equals o.Id
                                       where m.Id == order.OrderId
                                       select new
                                       {
                                           FoodName = o.Name,
                                           Type = o.Type,
                                           Quantity = n.Quantity,
                                           TaxableAmount = n.TaxableAmount,
                                           Amount = n.Amount
                                       }).ToListAsync();

                if (foodItems.Count > 0)
                {
                    List<OrderItemModel> orderItems = new List<OrderItemModel>();

                    foreach (var foodItem in foodItems)
                    {
                        OrderItemModel orderItem = new OrderItemModel()
                        {
                            FoodName = foodItem.FoodName,
                            Quantity = foodItem.Quantity,
                            Type = foodItem.Type,
                            TaxablePrice = Assist.Rupee(foodItem.TaxableAmount / foodItem.Quantity),
                            Price = Assist.Rupee(foodItem.Amount / foodItem.Quantity),
                            TaxableAmount = Assist.Rupee(foodItem.TaxableAmount),
                            Amount = Assist.Rupee(foodItem.Amount)

                        };

                        orderItems.Add(orderItem);
                    }

                    order.OrderItems = orderItems;
                }

                orders.Add(order);
            }

            return orders;
        }

        public async Task<OrderModel> GetOrder(int orderId)
        {
            OrderModel order = new OrderModel();
            List<OrderItemModel> orderItems = new List<OrderItemModel>();

            var orderDetails = await (from m in _context.Orders
                                      join n in _context.Restaurants on m.RestaurantId equals n.Id
                                      where m.Id == orderId
                                      select new
                                      {
                                          OrderId = m.Id,
                                          RestaurantId = n.Id,
                                          RestaurantName = n.Name,
                                          RestaurantLocality = n.Locality,
                                          Status = m.Status,
                                          TotalTaxableAmount = m.TaxableAmount,
                                          TotalAmount = m.Amount,
                                          PrimaryTaxAmount = m.PrimaryTaxAmount,
                                          SecondaryTaxAmount = m.SecondaryTaxAmount,
                                          DateOrdered = m.DateOrdered,
                                          FormattedDateOrdered = m.DateOrdered.ToString("dd MMM yy, hh:mm tt")
                                      }).FirstOrDefaultAsync();

            var foodItems = await (from m in _context.Orders
                                   join n in _context.OrderItems on m.Id equals n.OrderId
                                   join o in _context.FoodItems on n.FoodItemId equals o.Id
                                   where m.Id == orderId
                                   select new
                                   {
                                       FoodName = o.Name,
                                       Type = o.Type,
                                       Quantity = n.Quantity,
                                       TaxableAmount = n.TaxableAmount,
                                       Amount = n.Amount
                                   }).ToListAsync();

            if (orderDetails != null && foodItems.Count > 0)
            {
                order.OrderId = orderDetails.OrderId;
                order.RestaurantId = orderDetails.RestaurantId;
                order.RestaurantName = orderDetails.RestaurantName;
                order.RestaurantLocality = orderDetails.RestaurantLocality;
                order.Status = orderDetails.Status;
                order.TotalPrimaryTaxAmount = Assist.Rupee(orderDetails.PrimaryTaxAmount);
                order.TotalSecondaryTaxAmount = Assist.Rupee(orderDetails.SecondaryTaxAmount);
                order.TotalTaxAmount = Assist.Rupee(orderDetails.PrimaryTaxAmount + orderDetails.SecondaryTaxAmount);
                order.TotalTaxableAmount = Assist.Rupee(orderDetails.TotalTaxableAmount);
                order.TotalAmount = Assist.Rupee(orderDetails.TotalAmount);
                order.DateOrdered = orderDetails.FormattedDateOrdered;

                foreach (var foodItem in foodItems)
                {
                    OrderItemModel orderItem = new OrderItemModel()
                    {
                        FoodName = foodItem.FoodName,
                        Quantity = foodItem.Quantity,
                        Type = foodItem.Type,
                        TaxablePrice = Assist.Rupee(foodItem.TaxableAmount / foodItem.Quantity),
                        Price = Assist.Rupee(foodItem.Amount / foodItem.Quantity),
                        TaxableAmount = Assist.Rupee(foodItem.TaxableAmount),
                        Amount = Assist.Rupee(foodItem.Amount)

                    };

                    orderItems.Add(orderItem);
                }

                order.OrderItems = orderItems;

            }

            return order;
        }

        public async Task Accept(int orderId)
        {

        }

        public async Task Reject(int orderId)
        {

        }

        public async Task Complete(int orderId)
        {

        }
    }
}
