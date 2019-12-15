using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ExileLab.Extensions.Configuration.GitLab
{
    public class GitLabApi : IGitApi
    {
        private HttpClient _http;

        public GitLabApi(HttpClient http)
        {
            _http = http ?? throw new ArgumentNullException(nameof(http));
        }

        public async Task<GitItem> GetFile(GitQuery input)
        {
            var uri = $"/api/v4/projects/{input.Repository}/repository/files/{Uri.EscapeDataString(input.Path)}?ref={input.Branch}";

            var responseString = await _http.GetStringAsync(uri);

            var entry = JsonConvert.DeserializeObject<GitLabEntry>(responseString);

            return new GitItem(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(entry.Content)), entry.ContentSha256);
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