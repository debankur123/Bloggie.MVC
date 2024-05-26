using System.Net;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Bloggie.Web.Services
{
    public static class Commonservice
    {
        private static readonly Account account;
        public static string GetConnectionString()
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            var connString = configuration.GetConnectionString("dbConnection");
            return connString;
        }
        public static DateTime getIndianDatetime()
        {
            var indianTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            var indianDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, indianTimeZone);
            return indianDateTime;
        }
        public static async Task<string> UploadToCloudinaryAsync(IFormFile files)
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            var cloudinaryAccount = new Account(
                config.GetSection("Cloudinary")["CloudName"],
                config.GetSection("Cloudinary")["ApiKey"],
                config.GetSection("Cloudinary")["ApiSecret"]
            );
            var client = new Cloudinary(cloudinaryAccount);
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(files.FileName,files.OpenReadStream()),
                UseFilename = true,
                UniqueFilename = false,
                Overwrite = true,
                DisplayName = files.FileName
            };
            var uploadResult = await client.UploadAsync(uploadParams);
            return uploadResult is { StatusCode: HttpStatusCode.OK } ? uploadResult.SecureUrl.ToString() : "Error occured while Uploading!";
        }
    }
}