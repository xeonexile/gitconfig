using ExileLab.Extensions.Configuration.GitLab;
using System;
using System.Net.Http;
using Xunit;

namespace ExileLab.Extensions.Configuration.Tests
{
    public class GitLabApiTests
    {
        [Fact]
        public async void GetFileTest()
        {
            var http = new HttpClient()
            {
                BaseAddress = new Uri("https://gitlab.com")
            };
            http.DefaultRequestHeaders.Add("Authorization", $"Bearer {Environment.GetEnvironmentVariable("INT_GITLAB_KEY")}");
            http.DefaultRequestHeaders.Add("User-Agent", "ExileLab");


            var api = new GitLabApi(http);

            var res = await api.GetFile(new GitQuery("15173360", "config/appsettings.json"));
            return;
        }
    }
}