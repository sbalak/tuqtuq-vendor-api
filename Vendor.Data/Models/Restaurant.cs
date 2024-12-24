namespace Vendor.Data
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PreparationTime { get; set; }
        public string Photo { get; set; }
        public string LegalName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Locality { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string Cuisine { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public decimal PrimaryTaxRate { get; set; }
        public decimal SecondaryTaxRate { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsDisabled { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public DateTime? DateDisabled { get; set; }
    }
    public class Category
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public Restaurant Restaurant { get; set; }
    }
    public class FoodItem
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string? Photo { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsDisabled { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public DateTime? DateDisabled { get; set; }
        public Restaurant Restaurant { get; set; }
        public Category Category { get; set; }
    }
}
