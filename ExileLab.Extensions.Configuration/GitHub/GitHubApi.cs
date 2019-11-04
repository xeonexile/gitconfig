using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ExileLab.Extensions.Configuration.GitHub
{
    public class GitHubApi : IGitApi
    {
        private HttpClient _http;

        public GitHubApi(HttpClient http)
        {
            _http = http ?? throw new ArgumentNullException(nameof(http));
        }

        public async Task<GitItem> GetFile(GitQuery input)
        {
            var uri = $"/repos/{input.Repository}/contents/{input.Path}?ref={input.Branch}";

            var rs = await _http.GetAsync(uri);


            var responseString = await rs.Content.ReadAsStringAsync();

            var entry = JsonConvert.DeserializeObject<GitHubEntry>(responseString);

            return new GitItem(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(entry.Content)), entry.Sha);
        }

                public static HttpClient CreateHttp(string host, string token)
        {
            var http = new HttpClient()
            {
                BaseAddress = new Uri(host)
            };
            http.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            http.DefaultRequestHeaders.Add("User-Agent", "ExileLab");
            return http;
        }
    }
}