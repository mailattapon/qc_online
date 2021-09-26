using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace QcSupplier {
  public class Program {
    public static void Main(string[] args) {
      CreateWebHostBuilder(args).Build().Run();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
      WebHost.CreateDefaultBuilder(args)
      .ConfigureKestrel(options => {
        options.Limits.MaxRequestBodySize = 52428800; //50MB
      })
      .ConfigureLogging((hostingContext, logging) => {
        if (hostingContext.HostingEnvironment.IsDevelopment()) {
          logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
          logging.AddConsole();
          logging.AddDebug();
          logging.AddEventSourceLogger();
        } else {
          logging.ClearProviders();
          logging.AddNLog();
        }
      })
      .UseStartup<Startup>();
  }
}