using System;

namespace ExileLab.Extensions.Configuration
{
    public class VersionedConfig
    {
        public DateTime LoadedAt { get; }

        public GitItem Config;
        public Exception LastError { get; }

        public VersionedConfig(GitItem config, Exception error = null)
        {
            LoadedAt = DateTime.UtcNow;
            Config = config;
            LastError = error;
        }
    }

    public static class VersionConfig
    {
        public static VersionedConfig Fail(Exception error) =>
                new VersionedConfig(null, error);
    }
}