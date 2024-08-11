using System.Net;
using System.Security.Cryptography;
using System.Text;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Bloggie.Web.Services
{
    public static class Commonservice
    {
        public static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        public static string GetConnectionString()
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            var connString = configuration.GetConnectionString("dbConnection");
            return connString;
        }
        public static DateTime GetIndianDatetime()
        {
            var indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            return indianTime;
        }
        public static async Task<string> UploadToCloudinaryAsync(IFormFile files)
        {
            if (files == null)
            {
                throw new ArgumentNullException(nameof(files), "File is null");
            }
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            var cloudName = config.GetSection("Cloudinary")["CloudName"];
            var apiKey = config.GetSection("Cloudinary")["ApiKey"];
            var apiSecret = config.GetSection("Cloudinary")["ApiSecret"];
            if (string.IsNullOrEmpty(cloudName))
            {
                throw new InvalidOperationException("Cloudinary CloudName is not configured.");
            }
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new InvalidOperationException("Cloudinary ApiKey is not configured.");
            }
            if (string.IsNullOrEmpty(apiSecret))
            {
                throw new InvalidOperationException("Cloudinary ApiSecret is not configured.");
            }
            var cloudinaryAccount = new Account(cloudName, apiKey, apiSecret);
            var client = new Cloudinary(cloudinaryAccount);
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(files.FileName, files.OpenReadStream()),
                DisplayName = files.FileName
            };
            var uploadResult = await client.UploadAsync(uploadParams);
            if (uploadResult is { StatusCode: HttpStatusCode.OK })
                return uploadResult.SecureUrl.ToString();
            var errorMsg = uploadResult?.Error?.Message ?? "Unknown error occurred.";
            throw new InvalidOperationException($"Cloudinary upload failed: {errorMsg}");
        }
        // --------------------------- Password Hashing-----------------------------------
        public static (string Hash, string Salt) HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var salt = GenerateSalt();
            var saltedPassword = password + salt;
            var hash = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword)));
            return (Hash: hash, Salt: salt);
        }

        private static string GenerateSalt()
        {
            var saltBytes = new byte[16];
            RandomNumberGenerator.Fill(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }
        public static bool VerifyPasswordHash(string password, string storedHash, string storedSalt)
        {
            // Recompute hash using provided password and stored salt
            using var sha256 = SHA256.Create();
            var saltedPassword = password + storedSalt;
            var computedHash = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword)));
            // Compare computed hash with stored hash
            return computedHash.Equals(storedHash, StringComparison.Ordinal);
        }


    }
}