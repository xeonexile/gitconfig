using System;

namespace ExileLab.Extensions.Configuration
{
    public class GitItem
    {
        public GitItem(string content, string hash)
        {
            Content = content ?? throw new ArgumentNullException(nameof(content));
            Hash = hash ?? throw new ArgumentNullException(nameof(hash));
        }

        public string Content { get; set; }
        public string Hash { get; set; }

    }

    public class GitQuery
    {
        /// <summary>
        /// Could be github username/repository or gitlab ProjectId
        /// </summary>
        public string Repository { get; set; }
        public string Path { get; set; }
        public string Branch { get; set; }

        public GitQuery(string projectId, string path, string branch = "master")
        {
            Repository = projectId ?? throw new ArgumentNullException(nameof(projectId));
            Path = path ?? throw new ArgumentNullException(nameof(path));
            Branch = branch ?? throw new ArgumentNullException(nameof(branch));
        }
    }
}