using System;

namespace ExileLab.Extensions.Configuration
{
    public class GitConfigOptions
    {
        public string Host { get; set; }
        /// <summary>
        /// Project Id for GitLab, or repository path for GitHub
        /// </summary>
        public string Repository { get; set; }

        public string Branch { get; set; } = "master";
        
        /// <summary>
        /// Path to json file within the Repository
        /// </summary>
        public string Path { get; set; }
        
        /// <summary>
        /// Access token with read permission
        /// </summary>
        /// <value></value>
        public string Token { get; set; }

        public TimeSpan ReloadInterval { get; set; } = TimeSpan.FromMinutes(15);

        public TimeSpan InvalidReloadInterval { get; set; } = TimeSpan.FromMinutes(1);
    }
}