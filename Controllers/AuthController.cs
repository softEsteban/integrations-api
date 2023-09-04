using Microsoft.AspNetCore.Mvc;
using IntegrationsApi.Models;
using IntegrationsApi.Services;

namespace IntegrationsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration, AuthService authService)
        {
            _configuration = configuration;
            _authService = authService;
        }

        /// <summary>
        /// Validate user credentials and data to register and access the token
        /// </summary>
        /// <param name="userDto">User data DTO to validate and register</param>
        /// <returns></returns>
        [HttpPost("Register")]
        public async Task<ActionResult<User>> Register(UserDto userDto)
        {
            User user = await _authService.Register(userDto);
            return Ok(user);
        }

        /// <summary>
        /// Validate user credentials to access the token
        /// </summary>
        /// <param name="userDto">User data DTO to validate and login></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public async Task<ActionResult<LoginReponseDto>> Login(UserDto userDto)
        {
            LoginReponseDto response = await _authService.Login(userDto);

            if (response.Message == "User not found.")
            {
                return BadRequest("Invalid credentials.");
            }

            if (response.Message == "Wrong password.")
            {
                return BadRequest("Invalid credentials.");
            }

            return response;
        }

        /// <summary>
        /// Gets all registered users in database
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpGet("GetUsers")]
        public async Task<List<User>> Get() => await _authService.GetAsync();

        /// <summary>
        /// Creates a new user into database
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPost("CreateUser")]
        public async Task<IActionResult> Post(User newUser)
        {
            await _authService.CreateAsync(newUser);
            return CreatedAtAction(nameof(Get), new { id = newUser.Id }, newUser);
        }
    }
}
