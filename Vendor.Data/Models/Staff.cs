namespace Vendor.Data
{
    public class Staff
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
        public string? Otp { get; set; }
        public DateTime? OtpExpiry { get; set; }
    }

    public class Contract
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public int StaffId { get; set; }
        public Restaurant Restaurant { get; set; }
        public Staff Staff { get; set; }
    }
}
