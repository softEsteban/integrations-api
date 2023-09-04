using IntegrationsApi.Models;
using OpenAI_API;
using OpenAI_API.Completions;

namespace IntegrationsApi.Services
{
    public class ChatGptService
    {
        private readonly IConfiguration _configuration;

        private readonly string ApiKey;

        public ChatGptService(IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            IConfiguration config = builder.Build();
            _configuration = configuration;
            ApiKey = config["ApiKey"];
        }

        public async Task<ChatReponseDto> SendMessage(string prompt)
        {
            string outputResult = "";
            var openai = new OpenAIAPI(ApiKey);
            CompletionRequest completionRequest = new CompletionRequest();
            completionRequest.Prompt = prompt;
            completionRequest.Model = OpenAI_API.Models.Model.DavinciText;
            completionRequest.MaxTokens = 1024;
            completionRequest.Temperature= 0.7;

            var completions = await openai.Completions.CreateCompletionAsync(completionRequest);

            foreach (var completion in completions.Completions)
            {
                outputResult += completion.Text;
            }

            var response = new ChatReponseDto{
                Message= outputResult.ToString()
            };

            return response;
        }

    }
}
