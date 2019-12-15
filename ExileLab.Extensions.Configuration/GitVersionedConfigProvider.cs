using System;
using System.Threading;

namespace ExileLab.Extensions.Configuration
{
    public class GitVersionedConfigProvider : IVersionedConfigProvider
    {
        private TimedEntry<VersionedConfig> _current;
        private volatile int _loading = 0;

        private readonly IGitApi _git;
        private readonly GitQuery _gitQuery;
        private readonly TimeSpan _ttl;
        private readonly TimeSpan _invalidTtl;

        public GitVersionedConfigProvider(IGitApi git, string repository, string path, string branch, TimeSpan ttl, TimeSpan invalidTtl)
        {
            _git = git ?? throw new NullReferenceException(nameof(git));
            _ttl = ttl;
            _invalidTtl = invalidTtl;

            _gitQuery = new GitQuery(repository, path, branch);
            _current = TimedEntry.Create(VersionConfig.Fail(new InvalidOperationException()), TimeSpan.Zero);
        }

        /// <summary>
        /// Creates new git provider
        /// </summary>
        /// <param name="git"></param>
        /// <param name="repository">ProjectId for gitlab
        /// username/repository for github </param>
        /// <param name="path">path to a json config file</param>
        public GitVersionedConfigProvider(IGitApi git, string repository, string path, string branch = "master")
            : this(git, repository, path, branch, TimeSpan.FromMinutes(15), TimeSpan.FromMinutes(1))
        { }

        public static IVersionedConfigProvider CreateGitLab(string host, string token, string repository, string path, string branch, TimeSpan reloadInterval, TimeSpan invalidInterval) =>
            new GitVersionedConfigProvider(new GitLab.GitLabApi(GitLab.GitLabApi.CreateHttp(host, token)), repository, path, branch, reloadInterval, invalidInterval);

        public static IVersionedConfigProvider CreateGitHub(string host, string token, string repository, string path, string branch, TimeSpan reloadInterval, TimeSpan invalidInterval) =>
            new GitVersionedConfigProvider(new GitHub.GitHubApi(GitHub.GitHubApi.CreateHttp(host, token)), repository, path, branch, reloadInterval, invalidInterval);

        public static IVersionedConfigProvider Create(string host, string token, string repository, string path, string branch, TimeSpan reloadInterval, TimeSpan invalidInterval) =>
            host == "https://api.github.com"
                ? CreateGitHub(host, token, repository, path, branch, reloadInterval, invalidInterval)
                : CreateGitLab(host, token, repository, path, branch, reloadInterval, invalidInterval);


        public TimeSpan ReloadInterval => _ttl;

        public VersionedConfig GetConfig()
        {
            if (_current.IsValid)
                return _current.Value;

            if (Interlocked.CompareExchange(ref _loading, 1, 0) == 0)
            {
                var updated = ReadData();
                if (updated.Value.LastError == null)
                {
                    _current = updated;
                    Interlocked.Decrement(ref _loading);
                    return _current.Value;
                }
                Interlocked.Decrement(ref _loading);
            }

            return _current.Value;
        }

        private TimedEntry<VersionedConfig> ReadData()
        {
            VersionedConfig vc;
            try
            {
                //as all infrastructure is sync
                var gitItem = _git.GetFile(_gitQuery).ConfigureAwait(false).GetAwaiter().GetResult();
                vc = new VersionedConfig(gitItem);
            }
            catch (Exception ex)
            {
                return TimedEntry.Create(VersionConfig.Fail(ex), _invalidTtl);
            }
            return TimedEntry.Create(vc, _ttl);
        }
    }
}