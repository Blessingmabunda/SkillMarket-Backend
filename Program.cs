using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();

                // Change the port to avoid conflicts
                webBuilder.UseUrls("http://localhost:5050");

                // Optionally, add exception handling for startup errors
                webBuilder.CaptureStartupErrors(true);
            });
}
