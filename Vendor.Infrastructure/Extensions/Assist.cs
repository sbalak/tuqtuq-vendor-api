using System.Security.Cryptography;

namespace Vendor.Infrastructure
{
    public class Assist
    {
        public static string GenerateOTP()
        {
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            var bytes = new byte[6];
            rng.GetBytes(bytes);
            return BitConverter.ToUInt32(bytes, 0).ToString().Substring(0, 6);
        }
    }
}
