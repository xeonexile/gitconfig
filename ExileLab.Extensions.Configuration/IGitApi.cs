using System.Threading.Tasks;

namespace ExileLab.Extensions.Configuration
{
    public interface IGitApi
    {
        Task<GitItem> GetFile(GitQuery input);
    }
}