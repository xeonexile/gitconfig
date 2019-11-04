using ExileLab.Extensions.Configuration.GitHub;
using System;
using System.Net.Http;
using Xunit;

namespace ExileLab.Extensions.Configuration.Tests
{
    public class GitHubApiTests
    {
        [Fact]
        public async void GetFileTest()
        {
            var http = new HttpClient()
            {
                BaseAddress = new Uri("https://api.github.com")
            };
            http.DefaultRequestHeaders.Add("Authorization", $"Bearer {Environment.GetEnvironmentVariable("INT_GITHUB_KEY")}");
            http.DefaultRequestHeaders.Add("User-Agent", "ExileLab");

            var api = new GitHubApi(http);

            var res = await api.GetFile(new GitQuery("xeonexile/gitconfig", "config/appsettings.json"));
            return;

        }
    }
}