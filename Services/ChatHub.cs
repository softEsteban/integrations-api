using Microsoft.AspNetCore.SignalR;

namespace IntegrationsApi.Services
{
    public class ChatHub : Hub
    {
        private readonly ChatService _chatService;

        public ChatHub(ChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "MyChat");
            await Clients.Caller.SendAsync("UserConnected");
        }

        public async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "MyChat");
            await base.OnDisconnectedAsync(exception);
        }

        public async Task AddUserConnectionId(string name)
        {
            _chatService.AddUserConnectionId(name, Context.ConnectionId);
        }
    }
}
