using System;
using Microsoft.Extensions.Configuration;

namespace ExileLab.Extensions.Configuration
{
    public class GitConfigurationSource : IConfigurationSource
    {
        private readonly IVersionedConfigProvider _provider;

        public GitConfigurationSource(IVersionedConfigProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }
        public IConfigurationProvider Build(IConfigurationBuilder builder) =>
            new GitConfigurationProvider(_provider);
    }

    public static class ConfigurationExtensions
    {
        public static IConfigurationBuilder AddGitProvider(this IConfigurationBuilder configuration, IVersionedConfigProvider provider)
        {
            configuration.Add(new GitConfigurationSource(provider));
            return configuration;
        }
    }
}