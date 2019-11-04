using System;

namespace ExileLab.Extensions.Configuration
{
    public interface IVersionedConfigProvider
    {
        VersionedConfig GetConfig();

        TimeSpan ReloadInterval { get; }
    }
}