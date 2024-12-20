namespace Vendor.Infrastructure
{
    public class UserModel
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

    public class UserCoordinatesModel
    {
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
