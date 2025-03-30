using ChatAppBackend.Models;

namespace ChatApplication.Services.Service_Contracts
{
    public interface IMessageStatus
    {
        string AddMessageStatus(MessageStatus messageStatus);
        string UpdateMessageStatus(int messageStatusId, string status);
        string DeleteMessageStatus(int messageStatusId);
        MessageStatus GetMessageStatus(int messageStatusId);
        List<MessageStatus> GetAllMessageStatuses();
    }
}
