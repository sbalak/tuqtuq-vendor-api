namespace Vendor.Data
{
    public class Cart
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public int UserId { get; set; }
        public DateTime DateCreated { get; set; }
        public User User { get; set; }
        public Restaurant Restaurant { get; set; }
    }

    public class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int FoodItemId { get; set; }
        public int Quantity { get; set; }
        public Cart Cart { get; set; }
        public FoodItem? FoodItem { get; set; }
    }
}
