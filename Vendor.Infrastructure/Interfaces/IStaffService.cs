namespace Vendor.Infrastructure
{
    public interface IStaffService
    {
        Task<StaffModel> GetStaff(int staffId);
        Task<bool> Update(int staffId, string firstName, string lastName);
        Task<StaffCoordinatesModel> GetCoordinates(int staffId);
        Task<bool> SetCoordinates(int staffId, double latitude, double longitude);
    }
}
