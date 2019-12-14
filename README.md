# gitconfig
Add git based configuration source into netcore configuration pipeline

dotnet add package ExileLab.Extensions.Configuration --version 1.0.0-rc1

```csharp
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
 ```