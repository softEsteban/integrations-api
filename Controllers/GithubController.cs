using Microsoft.AspNetCore.Mvc;
using IntegrationsApi.Models;
using IntegrationsApi.Services;

namespace IntegrationsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GithubController : ControllerBase
    {
        private readonly GithubService _githubService;

        public GithubController(GithubService githubService)
        {
            _githubService = githubService;
        }

        /// <summary>
        /// Get user data from Github by its username
        /// </summary>
        /// <param name="username">The Github username</param>
        [HttpGet("User/{username}")]
        public async Task<IActionResult> GetUser(string username)
        {
            try
            {
                var userData = await _githubService.GetGitHubUserDataAsync(username);
                return Ok(userData);
            }
            catch (HttpRequestException ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Get user and repo commits from Github by its username and repo
        /// </summary>
        /// <param name="username">The Github username</param>
        /// <param name="repo">The Github repo</param>
        [HttpGet("Commits/{username}/{repo}")]
        public async Task<IActionResult> GetUserAndRepoCommitsAsync(string username, string repo)
        {
            try
            {
                var userData = await _githubService.GetUserAndRepoCommitsAsync(username, repo);
                return Ok(userData);
            }
            catch (HttpRequestException ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
