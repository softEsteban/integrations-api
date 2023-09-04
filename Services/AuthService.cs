using IntegrationsApi.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System;
using MongoDB.Driver;

namespace IntegrationsApi.Services
{
    public class AuthService
    {
        private readonly IMongoCollection<User> _usersCollection;
        private readonly IConfiguration _configuration;
        private string Token;

        public AuthService(
            IOptions<GptApiDatabaseSettings> gptApiDatabaseSettings,
            IConfiguration configuration
        )
        {
            var mongoClient = new MongoClient(gptApiDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(gptApiDatabaseSettings.Value.DatabaseName);

            _usersCollection = mongoDatabase.GetCollection<User>(
                gptApiDatabaseSettings.Value.UsersCollectionName
            );

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            IConfiguration config = builder.Build();

            _configuration = configuration;
            Token = config["Token"];
        }

        public async Task<User> Register(UserDto userDto)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

            User user = new User { username = userDto.Username, passwordHash = hashedPassword };

            await _usersCollection.InsertOneAsync(user);

            return user;
        }

        public async Task<LoginReponseDto> Login(UserDto userDto)
        {
            LoginReponseDto loginResponse = new LoginReponseDto();

            User user = await _usersCollection
                .Find(x => x.username == userDto.Username)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                loginResponse.Message = "User not found.";
                return loginResponse;
            }

            if (!BCrypt.Net.BCrypt.Verify(userDto.Password, user.passwordHash))
            {
                loginResponse.Message = "Wrong password";
                return loginResponse;
            }

            string token = CreateToken(user.username);
            loginResponse.Message = "Authenticated successfully";
            loginResponse.Token = token;
            return loginResponse;
        }

        public string CreateToken(string username)
        {
            List<Claim> claims = new List<Claim> { new Claim(ClaimTypes.Name, username) };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!)
            );

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public async Task<List<User>> GetAsync()
        {
            return await _usersCollection.Find(_ => true).ToListAsync();
        }

        public async Task CreateAsync(User createUser)
        {
            await _usersCollection.InsertOneAsync(createUser);
        }

        public async Task UpdateAsync(string id, User updateUser)
        {
            await _usersCollection.ReplaceOneAsync(x => x.Id == id, updateUser);
        }
    }
}
