using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using WebTeploobmenAppServer.Data;

namespace WebTeploobmenAppServer.Hubs
{
    public class ChatHub : Hub
    {
        // TODO: private chats
        // TODO: save messages in the db
        // TODO: allow admins to delete messages
        // TODO: error checking

        private static Dictionary<string, User> _users = new();
        private WebServicesContext _context;

        public ChatHub(WebServicesContext context)
        {
            _context = context;
        }

        public async Task SendMessage(string message)
        {
           await Clients.All.SendAsync("Send", Context.User.Identity.Name + ": " + message);
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            int userId = Convert.ToInt32(Context.User.FindFirst("UserId").Value);
            User? user = _context.Users.Find(userId);
            _users.Add(Context.ConnectionId, user);
            await Clients.All.SendAsync("GetUserList", _users.Values.Select(u => u.Login).ToList());
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
            _users.Remove(Context.ConnectionId);
            await Clients.All.SendAsync("GetUserList", _users.Values.Select(u => u.Login).ToList());
        }
    }
}
