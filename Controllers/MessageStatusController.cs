using Microsoft.AspNetCore.Mvc;
using ChatAppBackend.Models;
using ChatApplication.Services.Service_Contracts;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using ChatAppBackend.Hubs;

namespace ChatAppBackend.Controllers
{
    [Route("api/message/status")]
    [ApiController]
    public class MessageStatusController : ControllerBase
    {
        private readonly IMessageStatus _messageStatusService;
        private readonly IHubContext<ChatHub> _hubContext;


        public MessageStatusController(IMessageStatus messageStatusService, IHubContext<ChatHub> hubContext)
        {
            _messageStatusService = messageStatusService;
            _hubContext = hubContext;
        }

        // 1️⃣ Get Message Status
        [HttpGet("{messageStatusId}")]
        public IActionResult GetMessageStatus(int messageStatusId)
        {
            var messageStatus = _messageStatusService.GetMessageStatus(messageStatusId);
            if (messageStatus == null)
            {
                return NotFound(new { success = false, message = "Message status not found." });
            }
            return Ok(new { success = true, messageStatus });
        }

        [HttpPatch("{messageStatusId}")]
        public async Task<IActionResult> UpdateMessageStatus(int messageStatusId, [FromBody] string status)
        {
            var result = _messageStatusService.UpdateMessageStatus(messageStatusId, status);
            if (result == "Success")
            {
                // Retrieve the MessageId and the relevant user(s) to notify
                var messageStatus = _messageStatusService.GetMessageStatus(messageStatusId);
                if (messageStatus != null)
                {
                    // Notify the sender or other user about the status update
                    await _hubContext.Clients.User(messageStatus.MessageId.ToString()).SendAsync("ReceiveMessageStatus", messageStatus);
                }

                return Ok(new { success = true, message = "Message status updated." });
            }
            return BadRequest(new { success = false, message = "Failed to update status." });
        }


        // 3️⃣ Get All Message Statuses
        [HttpGet]
        public IActionResult GetAllMessageStatuses()
        {
            var messageStatuses = _messageStatusService.GetAllMessageStatuses();
            return Ok(new { success = true, messageStatuses });
        }
    }
}
