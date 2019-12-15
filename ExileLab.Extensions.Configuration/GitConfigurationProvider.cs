using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace ExileLab.Extensions.Configuration
{
    public class GitConfigurationProvider : ConfigurationProvider
    {
        private readonly IVersionedConfigProvider _provider;

        public GitConfigurationProvider(IVersionedConfigProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _ = new System.Threading.Timer(_ => Load(), null, provider.ReloadInterval, provider.ReloadInterval);
        }

        public override void Load()
        {
            var content = _provider.GetConfig().Config;
            if (string.IsNullOrEmpty(content.Content))
                return;

            using (var sr = new MemoryStream(Encoding.UTF8.GetBytes(content.Content)))
            {
                Data = Microsoft.Extensions.Configuration.Json.JsonConfigurationFileParser.Parse(sr);
            }
        }
    }
}