namespace Vendor.Infrastructure
{
    public class CartModel
    {
        public CartModel()
        {
            CartItems = new List<CartItemModel>();
        }
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public string RestaurantLocality { get; set; }
        public int TotalQuantity { get; set; }
        public string TotalPrimaryTaxAmount { get; set; }
        public string TotalSecondaryTaxAmount { get; set; }
        public string TotalTaxAmount { get; set; }
        public string TotalTaxableAmount { get; set; }
        public string TotalAmount { get; set; }
        public List<CartItemModel> CartItems { get; set; }
    }
    
    public class CartItemModel
    {
        public int FoodItemId { get; set; }
        public string FoodName { get; set; }
        public int Quantity { get; set; }
        public string Price { get; set; }
        public string Amount { get; set; }
    }

    public class CartValueModel
    {
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
    }
}
