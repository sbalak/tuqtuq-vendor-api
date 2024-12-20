using Microsoft.EntityFrameworkCore;
using Vendor.Data;

namespace Vendor.Infrastructure
{
    public class UserService : IUserService
    {
        private readonly VendorContext _context;

        public UserService(VendorContext context)
        {
            _context = context;
        }

        public async Task<UserModel> GetUser(int userId)
        {
            var user = await _context.Users.Where(x => x.Id == userId).Select(x => new UserModel 
                { 
                    Id = x.Id,
                    Email = x.Email,
                    Phone = x.Phone,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    RefreshToken = x.RefreshToken,
                    RefreshTokenExpiry = x.RefreshTokenExpiry,
                    Otp = x.Otp,
                    OtpExpiry = x.OtpExpiry
                }).FirstOrDefaultAsync();
            return user;
        }

        public async Task<bool> Update(int userId, string firstName, string lastName)
        {
            var user = await _context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();

            user.FirstName = firstName;
            user.LastName = lastName;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<UserCoordinatesModel> GetCoordinates(int userId)
        {
            var coordinates = await _context.Users.Where(x => x.Id == userId).Select(x => new UserCoordinatesModel 
                { 
                    Latitude = x.Latitude, 
                    Longitude = x.Longitude 
                }).FirstOrDefaultAsync();
            return coordinates;
        }

        public async Task<bool> SetCoordinates(int userId, double latitude, double longitude)
        {
            var user = await _context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();

            user.Latitude = latitude;
            user.Longitude = longitude;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
