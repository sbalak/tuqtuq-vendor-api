namespace Vendor.Infrastructure
{
    public class OrderModel
    {
        public OrderModel()
        {
            OrderItems = new List<OrderItemModel>();
        }
        public int OrderId { get; set; }
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public string RestaurantLocality { get; set; }
        public string Status { get; set; }
        public string TotalPrimaryTaxAmount { get; set; }
        public string TotalSecondaryTaxAmount { get; set; }
        public string TotalTaxAmount { get; set; }
        public string TotalTaxableAmount { get; set; }
        public string TotalAmount { get; set; }
        public string DateOrdered { get; set; }
        public List<OrderItemModel> OrderItems { get; set; }
    }

    public class OrderItemModel
    {
        public string FoodName { get; set; }
        public string Type { get; set; }
        public int Quantity { get; set; }
        public string TaxablePrice { get; set; }
        public string Price { get; set; }
        public string TaxableAmount { get; set; }
        public string Amount { get; set; }
    }
}
