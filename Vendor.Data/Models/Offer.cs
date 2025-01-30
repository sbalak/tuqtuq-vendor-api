namespace Vendor.Data
{
    public class Offer
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int Value { get; set; }
        public int? Threshold { get; set; }
        public int? UpperLimit { get; set; }
        public bool IsDisabled { get; set; }
    }
}
