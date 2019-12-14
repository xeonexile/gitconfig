using Microsoft.Extensions.Configuration;

namespace ExileLab.Extensions.Configuration
{
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Add GitConfigurationSource based on provider
        /// </summary>
        public static IConfigurationBuilder AddGitProvider(this IConfigurationBuilder configuration, IVersionedConfigProvider provider)
        {
            configuration.Add(new GitConfigurationSource(provider));
            return configuration;
        }

        /// <summary>
        /// Try to add GitConfigurationSource if options are valid
        /// </summary>
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
}