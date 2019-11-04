using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using ExileLab.Extensions.Configuration;

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
            .ConfigureAppConfiguration(config =>
            {
                config.AddGitProvider(GitVersionedConfigProvider.Create("https://api.github.com",
                    "9030bc330e4bab95d391ed3423603a336e0acefa", 
                    "xeonexile/gitconfig", 
                    "config/appsettings.json", 
                    "master", TimeSpan.FromMinutes(15), TimeSpan.FromMinutes(1)));
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
