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
                var staff = await _context.Staffs.Where(x => x.Phone == username).FirstOrDefaultAsync();

                if (staff == null)
                {
                    staff = new Staff();

                    staff.Phone = username;

                    await _context.Staffs.AddAsync(staff);
                    await _context.SaveChangesAsync();
                }

                staff.RefreshToken = null;
                staff.RefreshTokenExpiry = null;
                staff.Otp = otp;
                staff.OtpExpiry = DateTime.Now.AddSeconds(120);

                _context.Staffs.Update(staff);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<AccessTokenModel> Refresh(AccessTokenModel model)
        {
            AccessTokenModel response = new AccessTokenModel();
            var tokenPrincipal = _privilege.GetTokenPrincipal(model.AccessToken);

            if (tokenPrincipal?.Identity?.IsAuthenticated == true)
            {
                var staffId = tokenPrincipal.Claims.Where(x => x.Type == "Id").Select(x => x.Value).FirstOrDefault();

                if (staffId != null)
                {
                    var staff = await _context.Staffs.Where(x => x.Id == Convert.ToInt32(staffId)).FirstOrDefaultAsync();

                    if (staff == null || staff.RefreshToken != model.RefreshToken || staff.RefreshTokenExpiry < DateTime.Now)
                    {
                        return response;
                    }

                    response = _privilege.GenerateToken(staff);

                    response.AccessToken = response.AccessToken;
                    response.RefreshToken = response.RefreshToken;

                    staff.RefreshToken = response.RefreshToken;
                    staff.RefreshTokenExpiry = DateTime.Now.AddDays(90);

                    _context.Staffs.Update(staff);
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
                var staff = await _context.Staffs.Where(x => x.Phone == username && x.Otp == code && x.OtpExpiry > DateTime.Now).FirstOrDefaultAsync();

                if (staff != null)
                {
                    response = _privilege.GenerateToken(staff);

                    response.AccessToken = response.AccessToken;
                    response.RefreshToken = response.RefreshToken;

                    staff.RefreshToken = response.RefreshToken;
                    staff.RefreshTokenExpiry = DateTime.Now.AddDays(90);
                    staff.Otp = null;
                    staff.OtpExpiry = null;

                    _context.Staffs.Update(staff);
                    await _context.SaveChangesAsync();
                }
            }

            return response;
        }
    }
}
