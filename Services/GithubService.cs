using System.Text.Json;
using IntegrationsApi.Models;

namespace IntegrationsApi.Services
{
    public class GithubService
    {
        private readonly HttpClient _httpClient;

        public GithubService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("IntegrationsApi/7.32.3");

        }

        public async Task<GithubUserReponseDto> GetGitHubUserDataAsync(string username)
        {
            string url = "https://api.github.com/users/" + username;
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var user = JsonSerializer.Deserialize<GitHubUser>(responseBody);
                    var githubRes = new GithubUserReponseDto
                    {
                        Status = response.StatusCode.ToString(),
                        Data = user
                    };
                    return githubRes;
                }
                else
                {
                    var githubRes = new GithubUserReponseDto
                    {
                        Status = response.StatusCode.ToString(),
                        Data = new GitHubUser() { }
                    };
                    return githubRes;
                }
            }
            catch (HttpRequestException ex)
            {
                var githubRes = new GithubUserReponseDto
                {
                    Data = { },
                    Status = "An exception has occurred: " + ex.Message
                };
                return githubRes;
            }
        }

        public async Task<GithubCommitReponseDto> GetUserAndRepoCommitsAsync(string username, string repo)
        {
            string url = "https://api.github.com/repos/" + username + "/" + repo + "/commits";
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Deserialize the JSON into a Commit object
                    var commit = JsonSerializer.Deserialize<List<Commit>>(responseBody);

                    var githubRes = new GithubCommitReponseDto
                    {
                        Data = commit,
                        Status = response.StatusCode.ToString()
                    };
                    return githubRes;
                }
                else
                {
                    Console.WriteLine($"Request failed with status code: {response.StatusCode}");
                    var githubRes = new GithubCommitReponseDto
                    {
                        Data = new List<Commit>(),
                        Status = response.StatusCode.ToString()
                    };
                    return githubRes;

                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request exception: {ex.Message}");
            }
            return null;
        }

    }
}
