using Microsoft.EntityFrameworkCore;
using Vendor.Data;

namespace Vendor.Infrastructure
{
    public class StaffService : IStaffService
    {
        private readonly VendorContext _context;

        public StaffService(VendorContext context)
        {
            _context = context;
        }

        public async Task<StaffModel> GetStaff(int staffId)
        {
            var staff = await _context.Staffs.Where(x => x.Id == staffId).Select(x => new StaffModel 
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
            return staff;
        }

        public async Task<bool> Update(int staffId, string firstName, string lastName)
        {
            var staff = await _context.Staffs.Where(x => x.Id == staffId).FirstOrDefaultAsync();

            staff.FirstName = firstName;
            staff.LastName = lastName;

            _context.Staffs.Update(staff);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<StaffCoordinatesModel> GetCoordinates(int staffId)
        {
            var coordinates = await _context.Staffs.Where(x => x.Id == staffId).Select(x => new StaffCoordinatesModel 
                { 
                    Latitude = x.Latitude, 
                    Longitude = x.Longitude 
                }).FirstOrDefaultAsync();
            return coordinates;
        }

        public async Task<bool> SetCoordinates(int staffId, double latitude, double longitude)
        {
            var staff = await _context.Staffs.Where(x => x.Id == staffId).FirstOrDefaultAsync();

            staff.Latitude = latitude;
            staff.Longitude = longitude;

            _context.Staffs.Update(staff);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
