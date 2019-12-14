using System;
using Microsoft.Extensions.Configuration;

namespace ExileLab.Extensions.Configuration
{
    public static class ConfigurationExtensions
    {
        public static IConfigurationBuilder AddGitProvider(this IConfigurationBuilder configuration, IVersionedConfigProvider provider)
        {
            configuration.Add(new GitConfigurationSource(provider));
            return configuration;
        }

        public static IConfigurationBuilder TryAddGitProvider(this IConfigurationBuilder configuration, GitConfigOptions options)
        {
            if (options != null)
            {
                configuration.Add(new GitConfigurationSource(GitVersionedConfigProvider.Create(options.Host,
                    options.Token,
                    options.Repository,
                    options.Path,
                    options.Branch,
                    options.ReloadInterval,
                    options.InvalidReloadInterval)));
            }
            return configuration;
        }
    }

    public class GitConfigOptions
    {
        public string Host { get; set; }
        public string Repository { get; set; }

        public string Branch { get; set; } = "master";
        public string Path { get; set; }
        public string Token { get; set; }

        public TimeSpan ReloadInterval { get; set; } = TimeSpan.FromMinutes(15);

        public TimeSpan InvalidReloadInterval { get; set; } = TimeSpan.FromMinutes(1);
    }
}