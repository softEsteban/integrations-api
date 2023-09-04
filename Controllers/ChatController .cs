using Microsoft.AspNetCore.Mvc;
using IntegrationsApi.Models;
using IntegrationsApi.Services;
using Microsoft.Extensions.Configuration;

namespace IntegrationsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly ChatService _chatService;

        public ChatController(ChatService chatService)
        {
            _chatService = chatService;
        }

        // [HttpPost("register-user")]
        // public async Task<IActionResult> RegisterUser(UserDto user)
        // {
        //     if (_chatService.AddUser(user.Username))
        //     {
        //         return NoContent();
        //     }
        //     return BadRequest("This name is taken");
        // }
    }
}
