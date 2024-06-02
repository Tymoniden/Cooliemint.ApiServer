namespace Cooliemint.ApiServer.Extensions
{
    public static class WebHostExtension
    {
        public static bool IsLocalDevelopment(this IWebHostEnvironment webHostEnvironment)
        {
            return webHostEnvironment.EnvironmentName.Equals("Development.local");
        }

        public static bool IsDevelopment(this IWebHostEnvironment webHostEnvironment)
        {
            return webHostEnvironment.EnvironmentName.StartsWith("Development");
        }
    }
}
