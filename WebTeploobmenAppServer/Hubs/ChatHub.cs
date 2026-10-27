using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace WebTeploobmenAppServer.Hubs
{
    public class ChatHub : Hub
    {
        // TODO: online list
        public async Task SendMessage(string message)
        {
           await Clients.All.SendAsync("Send", Context.User.Identity.Name + ": " + message);
        }
    }
}
