using IntegrationsApi.Services;
using IntegrationsApi.Models;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Version = "v1",
            Title = "Integrations API",
            Description =
                "An ASP.NET Core Web API for managing user authentication, Github and Chat GPT integrations"
        }
    );
    
    // Add XML comments to Swagger
    var filePath = Path.Combine(System.AppContext.BaseDirectory, "IntegrationsApi.xml");
    options.IncludeXmlComments(filePath);

});

// Inject service with singleton
builder.Services.AddSingleton<AuthService>();
builder.Services.AddSingleton<ChatGptService>();
builder.Services.AddSingleton<GithubService>();
builder.Services.AddSingleton<ChatService>();
builder.Services.AddScoped<SworkzService>();

// Add services to the container.
builder.Services.AddHttpClient();

builder.Services.Configure<GptApiDatabaseSettings>(
    builder.Configuration.GetSection("GptApiDatabase")
);

builder.Services.AddSignalR();
builder.Services.AddCors();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:4200"));

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();
app.MapControllers();

app.MapHub<ChatHub>("hubs/chat");

app.Run();
