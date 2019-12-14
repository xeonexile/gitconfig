using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using ExileLab.Extensions.Configuration;
using Microsoft.Extensions.Configuration;

namespace ExileLab.SampleWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration( (hostContext, config) =>
            {
                config.TryAddGitProvider(hostContext.Configuration.GetSection("Git").Get<GitConfigOptions>());
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
