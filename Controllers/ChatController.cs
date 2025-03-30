using Microsoft.AspNetCore.Mvc;
using ChatAppBackend.Models;
using ChatApplication.Services.Service_Contracts;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using System;
using ChatAppBackend.Hubs;

namespace ChatAppBackend.Controllers
{
    [Route("api/chat")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatController(IChatService chatService, IHubContext<ChatHub> hubContext)
        {
            _chatService = chatService;
            _hubContext = hubContext;
        }

        [HttpPost]
        public async Task<IActionResult> AddChat([FromBody] Chat chat)
        {
            var result = _chatService.AddChat(chat);
            if (result == "Success")
            {
                // Notify the sender and receiver about the new chat
                await _hubContext.Clients.User(chat.SenderId.ToString()).SendAsync("ReceiveNewChat", chat);
                await _hubContext.Clients.User(chat.receiver_id.ToString()).SendAsync("ReceiveNewChat", chat);

                return CreatedAtAction(nameof(GetChat), new { chatId = chat.ChatId }, chat);
            }
            return BadRequest(new { success = false, message = "Failed to add chat." });
        }

        [HttpDelete("{chatId}")]
        public async Task<IActionResult> DeleteChat(int chatId)
        {
            var chat = _chatService.GetChat(chatId);  // Get the chat details based on chatId

            if (chat == null)
            {
                return NotFound(new { success = false, message = "Chat not found." });
            }

            var result = _chatService.DeleteChat(chatId);  // Proceed with the deletion
            if (result == "Success")
            {
                // Notify the sender and receiver about the chat deletion
                await _hubContext.Clients.User(chat.SenderId.ToString()).SendAsync("NotifyChatDeleted", chatId);
                await _hubContext.Clients.User(chat.receiver_id.ToString()).SendAsync("NotifyChatDeleted", chatId);

                return NoContent();  // Successfully deleted the chat
            }
            return BadRequest(new { success = false, message = "Failed to delete chat." });
        }

        // 3️⃣ Get all chats for a specific user (sender or receiver)
        [HttpGet("user/{userId}")]
        public IActionResult GetChatsByUser(int userId)
        {
            var chats = _chatService.GetChatsByUser(userId);
            return Ok(new { success = true, chats });
        }

        // 4️⃣ Get a specific chat by ID
        [HttpGet("{chatId}")]
        public IActionResult GetChat(int chatId)
        {
            var chat = _chatService.GetChat(chatId);
            if (chat == null)
            {
                return NotFound(new { success = false, message = "Chat not found." });
            }
            return Ok(new { success = true, chat });
        }
    }
}
