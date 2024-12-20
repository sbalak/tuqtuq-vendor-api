using Microsoft.EntityFrameworkCore;
using Vendor.Data;

namespace Vendor.Infrastructure
{
    public class AuthService : IAuthService
    {
        private readonly VendorContext _context;
        private readonly PrivilegeAssist _privilege;

        public AuthService(VendorContext context, PrivilegeAssist privilege)
        {
            _context = context;
            _privilege = privilege;
        }

        public async Task Login(string username)
        {
            var phoneNumber = RegexValidator.IsValidPhone(username);

            if (phoneNumber)
            {
                var otp = "123456";//Assist.GenerateOTP();
                var user = await _context.Users.Where(x => x.Phone == username).FirstOrDefaultAsync();

                if (user == null)
                {
                    user = new User();

                    user.Phone = username;

                    await _context.Users.AddAsync(user);
                    await _context.SaveChangesAsync();
                }

                user.RefreshToken = null;
                user.RefreshTokenExpiry = null;
                user.Otp = otp;
                user.OtpExpiry = DateTime.Now.AddSeconds(120);

                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<AccessTokenModel> Refresh(AccessTokenModel model)
        {
            AccessTokenModel response = new AccessTokenModel();
            var tokenPrincipal = _privilege.GetTokenPrincipal(model.AccessToken);

            if (tokenPrincipal?.Identity?.IsAuthenticated == true)
            {
                var userId = tokenPrincipal.Claims.Where(x => x.Type == "Id").Select(x => x.Value).FirstOrDefault();

                if (userId != null)
                {
                    var user = await _context.Users.Where(x => x.Id == Convert.ToInt32(userId)).FirstOrDefaultAsync();

                    if (user == null || user.RefreshToken != model.RefreshToken || user.RefreshTokenExpiry < DateTime.Now)
                    {
                        return response;
                    }

                    response = _privilege.GenerateToken(user);

                    response.AccessToken = response.AccessToken;
                    response.RefreshToken = response.RefreshToken;

                    user.RefreshToken = response.RefreshToken;
                    user.RefreshTokenExpiry = DateTime.Now.AddDays(90);

                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                }
            }

            return response;
        }

        public async Task<AccessTokenModel> Verify(string username, string code)
        {
            AccessTokenModel response = new AccessTokenModel();
            var phoneNumber = RegexValidator.IsValidPhone(username);

            if (phoneNumber)
            {
                var user = await _context.Users.Where(x => x.Phone == username && x.Otp == code && x.OtpExpiry > DateTime.Now).FirstOrDefaultAsync();

                if (user != null)
                {
                    response = _privilege.GenerateToken(user);

                    response.AccessToken = response.AccessToken;
                    response.RefreshToken = response.RefreshToken;

                    user.RefreshToken = response.RefreshToken;
                    user.RefreshTokenExpiry = DateTime.Now.AddDays(90);
                    user.Otp = null;
                    user.OtpExpiry = null;

                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                }
            }

            return response;
        }
    }
}
