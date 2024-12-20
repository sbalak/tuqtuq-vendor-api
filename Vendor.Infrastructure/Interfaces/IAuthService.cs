namespace Vendor.Infrastructure
{
    public interface IAuthService
    {
        Task Login(string username);
        Task<AccessTokenModel> Verify(string username, string code);
        Task<AccessTokenModel> Refresh(AccessTokenModel model);
    }
}
