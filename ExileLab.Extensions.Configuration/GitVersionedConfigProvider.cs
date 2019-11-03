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
            _current = TimedEntry.Create((VersionedConfig)VersionConfig.Fail(new InvalidOperationException()), TimeSpan.Zero);
        }

        public GitVersionedConfigProvider(IGitApi git, string repository, string path, string branch = "master")
            : this(git, repository, path, branch, TimeSpan.FromMinutes(15), TimeSpan.FromMinutes(1))
        { }


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