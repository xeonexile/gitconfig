using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ExileLab.Extensions.Configuration.GitHub
{
    public class GitHubApi : IGitApi
    {
        private HttpClient _http;
        private readonly string _token;


        public GitHubApi(HttpClient http, string token)
        {
            _http = http ?? throw new ArgumentNullException(nameof(http));
            _token = token ?? throw new ArgumentNullException(nameof(token));
        }

        public async Task<GitItem> GetFile(GitQuery input)
        {
            //https://api.github.com/repos/lastexile/studies/contents/NCT01146990.md
            var uri = $"/repos/{input.Repository}/contents/{input.Path}?ref={input.Branch}";

            var responseString = await _http.GetStringAsync(uri);

            var entry = JsonConvert.DeserializeObject<GitHubEntry>(responseString);

            return new GitItem(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(entry.Content)), entry.Sha);
        }
    }
}