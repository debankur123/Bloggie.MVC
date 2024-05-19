namespace Bloggie.Web.Services
{
    public static class Commonservice
    {
        public static string getConnectionString()
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
    }
}