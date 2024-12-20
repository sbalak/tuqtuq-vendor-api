namespace Vendor.Data
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RestaurantId { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal Amount { get; set; }
        public decimal PrimaryTaxAmount { get; set; }
        public decimal SecondaryTaxAmount { get; set; }
        public DateTime DateOrdered { get; set; }
        public DateTime? DateAccepted { get; set; }
        public DateTime? DateCompleted { get; set; }
        public DateTime? DateCancelled { get; set; }
        public User User { get; set; }
        public Restaurant Restaurant { get; set; }
    }
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int FoodItemId { get; set; }
        public int Quantity { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal Amount { get; set; }
        public Order Order { get; set; }
        public FoodItem? FoodItem { get; set; } 
    }
    public class Rating
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int Score { get; set; }
        public string Comment { get; set; }
        public DateTime DateCreated { get; set; }
        public Order Order { get; set; }
    }
}
