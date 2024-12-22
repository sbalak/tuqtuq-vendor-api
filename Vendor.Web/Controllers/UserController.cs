using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vendor.Infrastructure;

namespace Vendor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StaffController : ControllerBase
    {
        private IStaffService _staff;

        public StaffController(IStaffService staff)
        {
            _staff = staff;
        }

        [HttpGet("Details")]
        public async Task<StaffModel> Details(int staffId)
        {
            var staff = await _staff.GetStaff(staffId);
            return staff;
        }

        [HttpPost("Update")]
        public async Task<bool> Update(int staffId, string firstName, string lastName)
        {
            await _staff.Update(staffId, firstName, lastName);
            return true;
        }

        [HttpGet("Coordinates")]
        public async Task<StaffCoordinatesModel> Coordinates(int staffId)
        {
            var coordinates = await _staff.GetCoordinates(staffId);
            return coordinates;
        }

        [HttpPost("SetCoordinates")]
        public async Task<bool> SetCoordinates(int staffId, double latitude, double longitude)
        {
            await _staff.SetCoordinates(staffId, latitude, longitude);
            return true;
        }
    }
}
