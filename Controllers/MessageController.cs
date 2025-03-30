using Microsoft.AspNetCore.Mvc;
using ChatAppBackend.Models;
using ChatApplication.Services.Service_Contracts;
using Microsoft.AspNetCore.SignalR;
using ChatAppBackend.Hubs;

namespace ChatAppBackend.Controllers
{
    [Route("api/messages")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IHubContext<ChatHub> _hubContext;

        public MessageController(IMessageService messageService, IHubContext<ChatHub> hubContext)
        {
            _messageService = messageService;
            _hubContext = hubContext;
        }

        // 1️⃣ Send Message (SignalR & HTTP)
        [HttpPost("send")]
        public string SendMessage([FromBody] Message message)
        {
            return _messageService.SendMessage(message);
        }

        // 2️⃣ Fetch Chat History (HTTP)
        [HttpGet("history")]
        public IActionResult GetAllMessages(int userId, int receiverId)
        {
            // Fetch messages from both directions
            var messagesSentToReceiver = _messageService.GetAllMessages(userId, receiverId);
            var messagesSentFromReceiver = _messageService.GetAllMessages(receiverId, userId);

            // Combine the messages from both directions
            var allMessages = messagesSentToReceiver.Concat(messagesSentFromReceiver)
                                                   .OrderBy(m => m.created_at)  // Sort by created_at timestamp
                                                   .ToList();

            // Return the sorted list of messages
            return Ok(allMessages);
        }


        // 3️⃣ Update Message Status (HTTP)
        [HttpPatch("status/{messageId}")]
        public IActionResult UpdateMessageStatus(int messageId, [FromBody] string status)
        {
            var result = _messageService.UpdateMessageStatus(messageId, status);
            if (result == "Success")
            {
                return Ok(new { success = true, message = "Message status updated." });
            }
            return BadRequest(new { success = false, message = result });
        }

        // 4️⃣ Get Unread Message Count (HTTP)
        [HttpGet("unread/{userId}")]
        public IActionResult GetUnreadMessageCount(int userId)
        {
            var unreadCount = _messageService.GetUnreadMessageCount(userId);
            return Ok(new { unreadMessages = unreadCount });
        }

        // 5️⃣ Delete Message (HTTP)
        [HttpDelete("delete/{messageId}")]
        public IActionResult DeleteMessage(int messageId)
        {
            var result = _messageService.DeleteMessage(messageId);
            if (result == "Success")
            {
                return Ok(new { success = true, message = "Message deleted successfully." });
            }
            return BadRequest(new { success = false, message = result });
        }

        // 6️⃣ Edit Message (HTTP)
        [HttpPut("edit/{messageId}")]
        public IActionResult EditMessage(int messageId, [FromBody] string newContent)
        {
            var result = _messageService.EditMessage(messageId, newContent);
            if (result == "Success")
            {
                return Ok(new { success = true, message = "Message edited successfully." });
            }
            return BadRequest(new { success = false, message = result });
        }

        // 7️⃣ Soft Delete Message (HTTP)
        [HttpPatch("soft-delete/{messageId}")]
        public IActionResult SoftDeleteMessage(int messageId)
        {
            var result = _messageService.SoftDeleteMessage(messageId);
            if (result == "Success")
            {
                return Ok(new { success = true, message = "Message soft-deleted successfully." });
            }
            return BadRequest(new { success = false, message = result });
        }

        // 8️⃣ Search Messages (HTTP)
        [HttpGet("search")]
        public IActionResult SearchMessages(int userId, [FromQuery] string query)
        {
            var messages = _messageService.SearchMessages(userId, query);
            return Ok(messages);
        }
        [HttpGet("GetContacts")]
        public List<int> GetContacts(int id)
        {
            List<Message> sentMessages = _messageService.GetMessages(id);    // Messages sent by the user
            List<Message> receivedMessages = _messageService.GetInverseMessages(id); // Messages received by the user

            // Collect all unique contacts (both senders and receivers, regardless of message content)
            List<int> contacts = sentMessages.Concat(receivedMessages)
                                             .SelectMany(m => new List<int> { m.sender_id, m.receiver_id }) // Get both sender and receiver
                                             .Where(contactId => contactId != id) // Exclude the current user
                                             .Distinct()
                                             .ToList();

            return contacts;
        }






    }
}
