using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebTeploobmenAppServer.Data;

namespace WebTeploobmenAppServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ManageChatController : Controller
    {
        private readonly ILogger<NewsController> _logger;
        private readonly WebServicesContext _context;

        public ManageChatController(ILogger<NewsController> logger, WebServicesContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("GetPrivateChat")]
        public async Task<IActionResult> GetPrivateChat(int other)
        {
            int user1Id = Convert.ToInt32(User?.FindFirst("UserId")?.Value ?? "0");
            User? user1 = _context.Users.Find(user1Id);
            User? user2 = _context.Users.Find(other);
            if (user1 is null || user2 is null)
            {
                return NotFound();
            }
            if (user1Id == other)
            {
                return Forbid();
            }
            PrivateChat? chat = _context.PrivateChats.FirstOrDefault(c =>
                (c.Person1Id == user1Id && c.Person2Id == other)
                || (c.Person1Id == other && c.Person2Id == user1Id));
            if (chat is null)
            {
                chat = new()
                {
                    Person1Id = user1Id,
                    Person2Id = other,
                };
                _context.PrivateChats.Add(chat);
                await _context.SaveChangesAsync();
            }
            return Ok(chat.Id);
        }
    }
}
