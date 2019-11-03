using System;
using Microsoft.Extensions.Configuration;

namespace ExileLab.Extensions.Configuration
{
    public class GitConfigurationSource : IConfigurationSource
    {
        private readonly Func<VersionedConfig> _provider;
        private readonly TimeSpan _reloadInterval;
        public GitConfigurationSource(Func<VersionedConfig> provider, TimeSpan reloadInterval)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _reloadInterval = reloadInterval;
        }
        public Microsoft.Extensions.Configuration.IConfigurationProvider Build(IConfigurationBuilder builder) =>
            new GitConfigurationProvider(_provider, _reloadInterval);
    }

    public static class ConfigurationExtensions
    {
        public static IConfigurationBuilder AddGitProvider(this IConfigurationBuilder configuration, Func<VersionedConfig> provider, TimeSpan reloadInterval)
        {
            configuration.Add(new GitConfigurationSource(provider, reloadInterval));
            return configuration;
        }
    }
}