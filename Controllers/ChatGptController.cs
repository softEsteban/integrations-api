using Microsoft.AspNetCore.Mvc;
using IntegrationsApi.Services;
using IntegrationsApi.Models;

namespace IntegrationsApi.Controllers
{
    [ApiController]
    [Route("api/Chatgpt")]
    public class ChatGptController : ControllerBase
    {
        private readonly ChatGptService _chatgptService;
        private readonly IConfiguration _configuration;

        public ChatGptController(IConfiguration configuration, ChatGptService chatgptService)
        {
            _configuration = configuration;
            _chatgptService = chatgptService;
        }

        /// <summary>
        /// Send a prompt to ChatGPT API
        /// </summary>
        /// <param name="prompt">The prompt message to send</param>
        [HttpGet]
        [Route("SendMessage")]
        public async Task<ChatReponseDto> SendMessage(string prompt)
        {
            return await _chatgptService.SendMessage(prompt);
        }
    }
}
