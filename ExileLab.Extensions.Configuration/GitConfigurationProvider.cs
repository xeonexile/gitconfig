using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace ExileLab.Extensions.Configuration
{
    public class GitConfigurationProvider : ConfigurationProvider
    {
        private readonly Func<VersionedConfig> _provider;

        public GitConfigurationProvider(Func<VersionedConfig> provider, TimeSpan reloadInterval)
        {
            _provider = provider;
            _ = new System.Threading.Timer(_ => Reload(), null, reloadInterval, reloadInterval);
        }

        public override void Load()
        {
            Reload();
        }

        private void Reload()
        {
            var content = _provider().Config;
            if (string.IsNullOrEmpty(content.Content))
                return;

            using (var sr = new MemoryStream(Encoding.UTF8.GetBytes(content.Content)))
            {
                Data = Microsoft.Extensions.Configuration.Json.JsonConfigurationFileParser.Parse(sr);
            }
        }
    }
}