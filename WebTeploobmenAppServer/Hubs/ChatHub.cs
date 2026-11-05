using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore;
using WebTeploobmenAppServer.Data;
using WebTeploobmenAppServer.Models;

namespace WebTeploobmenAppServer.Hubs
{
    public class ChatHub : Hub
    {
        // TODO: error checking

        private static Dictionary<string, User> _users = new();
        private WebServicesContext _context;
        private int _savedChatId = -1;

        public ChatHub(WebServicesContext context)
        {
            _context = context;
        }

        private int GetChatId()
        {
            if (_savedChatId >= 0)
            {
                return _savedChatId;
            }
            if (int.TryParse(Context.GetHttpContext().Request.Query["chatId"], out int chatId) && chatId > 0)
            {
                PrivateChat? chat = _context.PrivateChats.Find(chatId);
                User user = _users[Context.ConnectionId];
                if (chat is not null && (chat.Person1Id == user.Id || chat.Person2Id == user.Id))
                {
                    _savedChatId = chatId;
                    return chatId;
                }
                // If the user doesn't have access to the private chat, send them to the global one instead
            }
            _savedChatId = 0;
            return 0;
        }

        private string GetCurrentGroup() => "chat" + GetChatId();

        private MessageViewModel MessageToViewModel(Message message)
            => new()
            {
                Id = message.Id,
                UserName = message.Author.Login,
                DateTime = message.DateTime,
                Message = message.MessageText,
            };

        public async Task SendMessage(string message)
        {
            User user = _users[Context.ConnectionId];
            Message dbMessage = new()
            {
                ChatId = GetChatId(),
                AuthorId = user.Id,
                DateTime = DateTime.UtcNow,
                MessageText = message,
            };
            _context.Messages.Add(dbMessage);
            await _context.SaveChangesAsync();
            dbMessage.Author = user;
            await Clients.Group(GetCurrentGroup()).SendAsync("Send", MessageToViewModel(dbMessage));
        }

        [Authorize(Roles = "admin")]
        public async Task RemoveMessage(int messageId)
        {
            Message? message = _context.Messages.Find(messageId);
            if (message is null)
            {
                return;
            }
            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
            await Clients.Group(GetCurrentGroup()).SendAsync("Remove", messageId);
        }

        private async Task SendLastMessages()
        {
            int chatId = GetChatId();
            List<MessageViewModel> messages = _context.Messages
                .Include(m => m.Author)
                .Where(m => m.ChatId == chatId)
                .Select(MessageToViewModel)
                .TakeLast(50)
                .ToList();
            foreach (MessageViewModel message in messages)
            {
                await Clients.Caller.SendAsync("Send", message);
            }
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            int userId = Convert.ToInt32(Context.User.FindFirst("UserId").Value);
            User? user = _context.Users.Find(userId);
            _users.Add(Context.ConnectionId, user);
            await Groups.AddToGroupAsync(Context.ConnectionId, GetCurrentGroup());
            Console.WriteLine(GetCurrentGroup());
            await SendUserList();
            await SendLastMessages();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
            _users.Remove(Context.ConnectionId);
            await SendUserList();
        }

        private async Task SendUserList()
        {
            await Clients.Group(GetCurrentGroup()).SendAsync("GetUserList", _users.Values.Select(u => new UserListViewModel()
            {
                Login = u.Login,
                UserId = u.Id,
            }).ToList());
        }
    }
}
